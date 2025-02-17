using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Xml;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 角色移动模块、动画模块
/// </summary>
public enum MovementState
{
    idle,
    walking,
    running,
    ducking,
    jumping,
}

public class MoveController : NetworkBehaviour
{
    private CharacterController characterController;
    //private Animator animator;
    [SerializeField]private string currentAnimation = "";

    public float Speed;

    public MovementState State;
    public CollisionFlags CollisionFlags;

    private PlayerInfo playerInfo;
    public PlayerInfo PlayerInfo
    {
        get { return playerInfo; }
        set { playerInfo = value; EventCenter.Broadcast(EventDefine.OnPlayerInfoChange , playerInfo); }
    }

    /// <summary>
    /// 跳跃相关
    /// </summary>
    /// 用于手感调优，部分情况下检测条件过于苛刻，使用定时器完成延迟跳跃。
    [Header("跳跃")]
    [Tooltip("跳跃缓冲时间")] public float jumpBufferTime = 0.2f;
    [Tooltip("定时器")] public float jumpBufferTimer = 0f;
    [Tooltip("检测跳跃缓冲存在")] public bool isJump;     // 是否已经处于跳跃状态

    public bool isShoot;
    public bool isGround;    // 当前停留在地面上
    public float jumpForce;    // 跳跃上升
    public float falllForce;    // 跳跃上升
    public int fx = 0;

    public StateMachine stateMachine;
    public IdleState idleState;
    public WalkState walkState;
    public RunState runState;
    public JumpState jumpState;
    public DuckState duckState;
    public SingleShootState singleShootState;
    public ReloadState reloadState;

    [Header("键位")]
    [Tooltip("静步")] public KeyCode walkInputName = KeyCode.LeftShift;
    [Tooltip("下蹲")] public KeyCode duckInputName = KeyCode.LeftControl;
    [Tooltip("跳跃")] public KeyCode JumpInputName = KeyCode.Space;
    [Tooltip("换弹")] public KeyCode reloadInputName = KeyCode.R;

    //public Transform MuzzleFlash;
    public ParticleSystem[] VfXs;


    private void Awake()
    {
        transform.SetParent(GameObject.Find("PlayerContainer").transform);
        characterController = GetComponent<CharacterController>();
        VfXs = transform.Find("MuzzleFlash").GetComponentsInChildren<ParticleSystem>();

        //animator = transform.Find("Player").GetComponent<Animator>();
        
        idleState = new IdleState(this , new string[] { "Idle_gunMiddle_AR" } , 0.2f);
        walkState = new WalkState(this , new string[] { "WalkFront_Shoot_AR" , "WalkBack_Shoot_AR" , "WalkRight_Shoot_AR" , "WalkLeft_Shoot_AR" }, 0.2f);
        runState = new RunState(this, new string[] { "Run_guard_AR" }, 0.2f);
        jumpState = new JumpState(this, new string[] { "Jump" }, 0.2f);
        duckState = new DuckState(this, new string[] { "Idle_Ducking_AR" }, 0.2f);
        singleShootState = new SingleShootState(this , new string[] {"Shoot_SingleShot_AR"}, 0.2f);
        reloadState = new ReloadState(this , new string[] { "Reload" } , 0.2f);
        stateMachine = new StateMachine(idleState);
    }

    private void Start()
    {
        isShoot = false;
        isJump = false;
        isGround = true;
        CollisionFlags = CollisionFlags.Below;
        PlayerInfo = new PlayerInfo();
    }

    void Update()
    {
        if (!isLocalPlayer) { return; }

        // [-1,1] 
        //float moveForward = Input.GetAxis("Vertical") ;
        //float moveRight = Input.GetAxis("Horizontal") ;
        // -1 0 1
        float moveForward = Input.GetAxisRaw("Vertical");
        float moveRight = Input.GetAxisRaw("Horizontal");
        PlayerShoot();
        PlayerJump();
        PlayerMove( moveForward, moveRight);
        stateMachine.OnUpdate();
    }

    public void PlayerShoot()
    {
        if (Input.GetKeyDown(reloadInputName))
        {
            stateMachine.ChangeState(reloadState);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("按下了左键");
            if(playerInfo.CurrentBullet > 0) stateMachine.ChangeState(singleShootState);
            else stateMachine.ChangeState(reloadState);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("按下了右键");
            playerInfo.FireMode = 1 - playerInfo.FireMode;
            EventCenter.Broadcast(EventDefine.OnPlayerInfoChange,playerInfo);
        }


        //if(Input.GetMouseButton(0))
        //{
        //    Debug.Log("持续按下鼠标左键");
        //}
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("抬起了鼠标左键");
            isShoot = false;
        }

        if (!isShoot && (stateMachine.IsState(singleShootState) || stateMachine.IsState(reloadState)))
        {
            stateMachine.ChangeState(idleState);
        }
    }

    public void PlayerJump()
    {
        if (Input.GetKeyDown(JumpInputName))
        {
            jumpBufferTimer = jumpBufferTime;
            isJump = true;
        }
        
        if (isJump )
        {
            jumpBufferTimer -= Time.deltaTime;

            if(jumpBufferTimer <= 0.0f || isGround)
            {
                if(isGround)
                {
                    stateMachine.ChangeState(jumpState);
                }

                isJump = false;
            }

        }

        if(!isGround)
        {
            jumpForce = jumpForce - falllForce * Time.deltaTime;
            Vector3 jump = new Vector3(0 , jumpForce * Time.deltaTime , 0);
            //transform.position += jump;
            //Debug.Log(string.Format("Before-> {0}" , transform.position));
            CollisionFlags = characterController.Move(jump);
            //Debug.Log(string.Format("After-> {0}", transform.position));
            //Debug.Log(string.Format("CollisionFlags = {0}" , CollisionFlags));

            // 脚底下有碰撞，意味着在地面
            if (CollisionFlags == CollisionFlags.Below)
            {
                //Debug.Log("CollisionFlags.Below");
                isGround = true;
            }

            //没有任何碰撞，浮空状态
            if (isGround && CollisionFlags == CollisionFlags.None)
            {
                //Debug.Log("CollisionFlags.None");
                isGround = false;
            }
        }

        if(stateMachine.IsState(jumpState) && isGround)
        {
            stateMachine.ChangeState(idleState);
            isJump = false;
        }
    }

    public void PlayerMove(float up , float right)
    {
        if (isGround && (Math.Abs(up) > 0 || Math.Abs(right) > 0))
        {
            if (Input.GetKey(walkInputName))
            {
                if (up > 0) fx = 0;
                else if (up < 0) fx = 1;
                else if (right > 0) fx = 2;
                else if (right < 0) fx = 3;
                stateMachine.ChangeState(walkState);
            }
            else if (Input.GetKey(duckInputName))
            {
                stateMachine.ChangeState(duckState);
            }
            else {
                stateMachine.ChangeState(runState);
            }
        }
        else if(isGround)
        {
            if (Input.GetKey(duckInputName))
            {
                stateMachine.ChangeState(duckState);
            }else
            {   
                if(!isJump && !isShoot) stateMachine.ChangeState(idleState);
            }
        }

        float yAngle = transform.eulerAngles.y;
        float yawAngle1 = yAngle * Mathf.Deg2Rad;
        float yawAngle2 = (yAngle - 90f) * Mathf.Deg2Rad;
        // 角色旋转角度为0时，正前方是z轴的正方向
        float moveZ = up * Mathf.Cos(yawAngle1) - right * Mathf.Cos(yawAngle2);
        float moveX = up * Mathf.Sin(yawAngle1) - right * Mathf.Sin(yawAngle2);
        Vector3 moveDirection = new(moveX, 0, moveZ);
        moveDirection.Normalize();
        moveDirection *= Time.deltaTime * Speed;
        //transform.position += moveDirection;
        //Debug.Log(string.Format("Before-> {0}", transform.position));
        CollisionFlags = characterController.Move(new Vector3(moveDirection.x , 0 , moveDirection.z));
        //Debug.Log(string.Format("After-> {0}", transform.position));
        //if (CollisionFlags == CollisionFlags.None)
        //{
        //    isGround = false;
        //}else if(CollisionFlags == CollisionFlags.Below)
        //{
        //    isGround = true;
        //}
        CollisionFlags = characterController.Move(new Vector3(0, -Time.deltaTime, 0));
        if (CollisionFlags == CollisionFlags.Below)
        {
            //Debug.Log(string.Format("Go {0} -> CollisionFlags.Below", transform.position));
            isGround = true;
            characterController.Move(new Vector3(0, Time.deltaTime, 0));
        }
        else if(isGround && !isShoot && CollisionFlags == CollisionFlags.None)
        {
            isGround = false;
            jumpForce = 0;
        }
    }

    //public bool ChangeAnimation(string animation, float crossfade = 0.2f)
    //{
    //    if (currentAnimation == "Die")
    //    {
    //        return false;
    //    }
    //    if (currentAnimation != animation)
    //    {
    //        currentAnimation = animation;
    //        animator.CrossFade(animation, crossfade);
    //        return true;
    //    }
    //    return false;
    //}

}