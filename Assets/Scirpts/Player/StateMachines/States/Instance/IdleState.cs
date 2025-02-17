public class IdleState : IState
{
    public override int ID => 0;
    public IdleState(MoveController moveController, string[] _animation, float _crossfade):base(moveController , _animation , _crossfade) { }

    public override void OnEnter()
    {
        base.OnEnter();
        moveController.State = MovementState.idle;
        if(animationSize == 1)
        {
            animator.CrossFade(animation[0], crossfade);
        }
        moveController.Speed = 0f;
        moveController.falllForce = 20f; // œ¬¬‰ÀŸ∂»
        //moveController.jumpForce = 10f;
        //moveController.isGround = true;
    }

    public override void OnUpdate()
    {
        //base.OnUpdate();
    }

    public override void OnExit()
    {
        //base.OnExit();
    }
}