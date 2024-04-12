public class TurnStateMachine
{
    private CombatManager cm;
    private aTurnState currentState;
    public TurnStateType currentStateType;

    public TurnStateMachine(CombatManager manager)
    {
        cm = manager;
        Next(TurnStateType.TURN_START);
    }

    public void Next(TurnStateType nextState)
    {
        currentStateType = nextState;
        currentState = StateFactory.MakeState(nextState, this);
        currentState.Enter(cm);
    }

    public void Transition()
    {
        currentState.Exit(cm);
    }
}



