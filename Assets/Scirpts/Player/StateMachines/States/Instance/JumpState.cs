using static MoveController;

public class JumpState : IState
{
    public override int ID => 1;
    public JumpState(MoveController moveController, string[] _animation, float _crossfade):base(moveController , _animation , _crossfade) { }

    public override void OnEnter()
    {
        base.OnEnter();
        moveController.State = MovementState.jumping;
        if (animationSize == 1)
        {
            animator.CrossFade(animation[0], crossfade);
        }
        moveController.Speed = 3f;
        moveController.isJump = true;
        moveController.jumpForce = 10f;
        moveController.isGround = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        moveController.isJump = false;
    }

    public override bool OnChange(IState other)
    {
        if (other.ID == 0) return true;
        return base.OnChange(other);
    }
}