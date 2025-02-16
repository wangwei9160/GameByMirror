using UnityEngine;

public class RunState : IState
{
    public override int ID => 4;
    public float runSpeed;
    public RunState(MoveController moveController, string[] _animation, float _crossfade):base(moveController , _animation , _crossfade) { runSpeed = 6f; }

    public override void OnEnter()
    {
        base.OnEnter();
        moveController.State = MovementState.running;
        if (animationSize == 1)
        {
            animator.CrossFade(animation[0], crossfade);
        }
        moveController.Speed = runSpeed;
        //Debug.Log(string.Format("moveController.Speed = {0} , runSpeed = {1}" , moveController.Speed , runSpeed));
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}