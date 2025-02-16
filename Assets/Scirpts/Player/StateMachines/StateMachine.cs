public class StateMachine 
{
    public IState CurrentState { get; set; }

    public StateMachine(IState currentState)
    {
        CurrentState = currentState;
        CurrentState.OnEnter();
    }

    public bool IsState(IState state)
    {
        return CurrentState == state;
    }

    public void InitState(IState initState)
    {
        CurrentState = initState;
    }

    public void OnUpdate()
    {
        CurrentState.OnUpdate();
    }

    public void ChangeState(IState newState)
    {
        if (!CurrentState.OnChange(newState)) return;
        CurrentState.OnExit();
        CurrentState = newState;
        CurrentState.OnEnter();
    }

}