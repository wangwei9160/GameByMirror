public class DuckState : IState
{
    public override int ID => 3;
    public float duckSpeed;
    public DuckState(MoveController moveController, string[] _animation, float _crossfade):base(moveController , _animation , _crossfade)
    {
        duckSpeed = 2f;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        moveController.State = MovementState.ducking;
        if (animationSize == 1)
        {
            animator.CrossFade(animation[0], crossfade);
        }
        moveController.Speed = duckSpeed;
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