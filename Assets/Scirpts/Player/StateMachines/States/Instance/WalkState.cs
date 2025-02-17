public class WalkState : IState
{
    public override int ID => 2;
    public float walkSpeed;
    public WalkState(MoveController moveController, string[] _animation, float _crossfade):base(moveController , _animation , _crossfade) { walkSpeed = 4f; }

    public override void OnEnter()
    {
        base.OnEnter();
        moveController.State = MovementState.walking;
        animator.CrossFade(animation[moveController.fx], crossfade);
        moveController.Speed = walkSpeed;
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