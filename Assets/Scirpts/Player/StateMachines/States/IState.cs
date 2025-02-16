using UnityEngine;

public class IState
{
    public virtual int ID { get; private set; }
    string[] StateName =
    {
        "IdleState",
        "JumpState",
        "WalkState",
        "DuckState",
        "RunState",
        "SingleShotState",
        "BurstShotState",
        "AutoShotState",
        "SendMessageState",
    };
    public string name
    {
        get
        {
            return StateName[ID];
        }
    }
    protected Animator animator;
    protected int animationSize;
    protected string[] animation;
    protected MoveController moveController;
    protected float crossfade;

    public IState(MoveController moveController , string[] _animation , float _crossfade)
    {
        this.moveController = moveController;
        animator = moveController.transform.Find("Player").GetComponent<Animator>();
        animation = _animation;
        animationSize = animation.Length;
        crossfade = _crossfade;
    }

    public virtual void OnEnter()
    {
        Debug.Log("OnEnter " + name);
        //animator.CrossFade(animation, crossfade);
    }

    public virtual void OnUpdate()
    {
        Debug.Log("OnUpdate " + name);
    }

    public virtual void OnExit()
    {
        Debug.Log("OnExit " + name);
    }

    public virtual bool OnChange(IState other)
    {
        if(ID == other.ID)
        {
            return false;
        }
        return true;
    }

}
