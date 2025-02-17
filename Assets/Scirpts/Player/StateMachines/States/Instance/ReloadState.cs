using UnityEngine;

public class ReloadState : IState
{
    public override int ID => 8;
    public float BufferTime = 1.2f;
    public float Timer = 0f;
    public bool isReload = false;
    public ReloadState(MoveController moveController, string[] _animation, float _crossfade):base(moveController , _animation , _crossfade) { }

    public override void OnEnter()
    {
        base.OnEnter();
        Timer = 0f;
        moveController.isShoot = true;
        isReload = false;
        if (animationSize == 1)
        {
            animator.CrossFade(animation[0], crossfade);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Timer += Time.deltaTime;
        if(Timer > BufferTime && !isReload)
        {
            isReload = true;
            moveController.PlayerInfo.CurrentBullet = moveController.PlayerInfo.MoreBullet;
            EventCenter.Broadcast(EventDefine.OnPlayerInfoChange , moveController.PlayerInfo);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        moveController.isShoot = false;
    }
}