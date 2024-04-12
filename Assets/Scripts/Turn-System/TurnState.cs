public enum TurnStateType
{
    TURN_START,
    SELECT_TARGET,
    SELECT_PART,
    TARGETING_COMPLETE,
    EXECUTING_ACTION,
    ACTION_DONE,
    ACTION_CANCELLED
}

public static class StateFactory
{
    public static aTurnState MakeState(TurnStateType type, TurnStateMachine sm)
    {
        switch (type)
        {
            case TurnStateType.SELECT_TARGET:
                return new SelectTargetState(sm);
            case TurnStateType.SELECT_PART:
                return new SelectPartState(sm);
            case TurnStateType.ACTION_CANCELLED:
                return new CancelAction(sm);
            case TurnStateType.TARGETING_COMPLETE:
                return new TargetingCompleteState(sm);
            case TurnStateType.EXECUTING_ACTION:
                return new ExecutingActionState(sm);
            case TurnStateType.ACTION_DONE:
                return new ActionDoneState(sm);
            default:
                return new TurnStartState(sm);
        }
    }
}

public abstract class aTurnState
{
    protected TurnStateMachine stateMachine;
    public void Enter(CombatManager manager)
    {
        OnEnter(manager);
    }

    public void Exit(CombatManager manager)
    {
        OnExit(manager);
    }

    public void Cancel(CombatManager manager)
    {
        OnCancel(manager);
    }

    protected abstract void OnEnter(CombatManager manager);
    protected abstract void OnExit(CombatManager manager);
    protected abstract void OnCancel(CombatManager manager);
}

public class TurnStartState : aTurnState
{
    public TurnStartState(TurnStateMachine sm)
    {
        stateMachine = sm;
    }

    protected override void OnEnter(CombatManager manager)
    {
        manager.ClearTarget();
    }
    protected override void OnCancel(CombatManager manager)
    {
        stateMachine.Next(TurnStateType.ACTION_CANCELLED);
    }
    protected override void OnExit(CombatManager manager)
    {
        CombatTarget targetData = manager.GetCombatTargetInformation();
        if (targetData.typeOfTarget == CombatActionTargets.Self)
        {
            stateMachine.Next(TurnStateType.TARGETING_COMPLETE);
        } else if (targetData.typeOfTarget == CombatActionTargets.SelfBodyPart)
        {
            stateMachine.Next(TurnStateType.SELECT_PART);
        } else
        {
            stateMachine.Next(TurnStateType.SELECT_TARGET);
        }
    }
}

public class SelectTargetState : aTurnState
{
    public SelectTargetState(TurnStateMachine sm)
    {
        stateMachine = sm;
    }

    protected override void OnEnter(CombatManager manager)
    {
        // Show available targets
        CombatUIManager uiManager = manager.gameObject.GetComponent<CombatUIManager>();
        uiManager.ShowEnemies();
    }
    protected override void OnCancel(CombatManager manager)
    {
        stateMachine.Next(TurnStateType.ACTION_CANCELLED);
    }
    protected override void OnExit(CombatManager manager)
    {
        CombatTarget targetInformation = manager.GetCombatTargetInformation();
        if (targetInformation.typeOfTarget == CombatActionTargets.SingleEnemyBodyPart || targetInformation.typeOfTarget == CombatActionTargets.SingeAllyBodyPart)
        {
            stateMachine.Next(TurnStateType.SELECT_PART);
        } else
        {
            stateMachine.Next(TurnStateType.TARGETING_COMPLETE);
        }
    }
}

public class SelectPartState : aTurnState
{
    public SelectPartState(TurnStateMachine sm)
    {
        stateMachine = sm;
    }

    protected override void OnEnter(CombatManager manager)
    {
        // Show available targets
        CombatUIManager uiManager = manager.gameObject.GetComponent<CombatUIManager>();
        CombatTarget targetInformation = manager.GetCombatTargetInformation();
        if (targetInformation.sideBeingTargeted == Combatant.CombatantType.ENEMIES)
        {
            uiManager.ShowPartButtons(manager.GetCombatTargetInformation().targetIndex);
        }
    }
    protected override void OnCancel(CombatManager manager)
    {
        stateMachine.Next(TurnStateType.ACTION_CANCELLED);
    }
    protected override void OnExit(CombatManager manager)
    {
        stateMachine.Next(TurnStateType.TARGETING_COMPLETE);

    }
}

public class TargetingCompleteState : aTurnState
{
    public TargetingCompleteState(TurnStateMachine sm)
    {
        stateMachine = sm;
    }

    protected override void OnEnter(CombatManager manager)
    {
        // Hide the UI information
        CombatUIManager uiManager = manager.gameObject.GetComponent<CombatUIManager>();
        uiManager.HideAll();
        Exit(manager);
    }
    protected override void OnCancel(CombatManager manager)
    {
        stateMachine.Next(TurnStateType.ACTION_CANCELLED);
    }
    protected override void OnExit(CombatManager manager)
    {
        stateMachine.Next(TurnStateType.EXECUTING_ACTION);
    }
}

public class ExecutingActionState : aTurnState
{
    public ExecutingActionState(TurnStateMachine sm)
    {
        stateMachine = sm;
    }

    protected override void OnEnter(CombatManager manager)
    {
        manager.ExecuteCombatAction();
        Exit(manager);
    }
    protected override void OnCancel(CombatManager manager)
    {
        stateMachine.Next(TurnStateType.ACTION_CANCELLED);
    }
    protected override void OnExit(CombatManager manager)
    {
        stateMachine.Next(TurnStateType.ACTION_DONE);
    }
}

public class ActionDoneState : aTurnState
{
    public ActionDoneState(TurnStateMachine sm)
    {
        stateMachine = sm;
    }

    protected override void OnEnter(CombatManager manager)
    {
        Exit(manager);
    }
    protected override void OnCancel(CombatManager manager)
    {
        stateMachine.Next(TurnStateType.ACTION_CANCELLED);
    }
    protected override void OnExit(CombatManager manager)
    {
        stateMachine.Next(TurnStateType.TURN_START);
    }
}

public class CancelAction : aTurnState
{
    public CancelAction(TurnStateMachine sm)
    {
        stateMachine = sm;
    }

    protected override void OnEnter(CombatManager manager)
    {
        manager.ClearTarget();
        Exit(manager);
    }
    protected override void OnCancel(CombatManager manager)
    {

    }
    protected override void OnExit(CombatManager manager)
    {
        stateMachine.Next(TurnStateType.TURN_START);
    }
}