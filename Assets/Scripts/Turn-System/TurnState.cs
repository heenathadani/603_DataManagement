using UnityEngine;

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

// A static factory for creating new states
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

// The abstract class for the States for the turn state machine
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

// Entry point to the state machine. Resets everything.
public class TurnStartState : aTurnState
{
    public TurnStartState(TurnStateMachine sm)
    {
        stateMachine = sm;
    }

    protected override void OnEnter(CombatManager manager)
    {
        manager.ClearTarget();
        // Show player action buttons
        CombatUIManager uiManager = manager.gameObject.GetComponent<CombatUIManager>();
        if (manager._activeType == Combatant.CombatantType.ALLIES)
        {
            switch (manager._currentTurn)
            {
                case 0:
                    uiManager.ShowPlayerActionButtons(0);
                    break;
                case 1:
                    uiManager.ShowPlayerActionButtons(1);
                    break;
                case 2:
                    uiManager.ShowPlayerActionButtons(2);
                    break;
            }
        }
        else if (manager._activeType == Combatant.CombatantType.ENEMIES)
        {
            Debug.Log("????");
            uiManager.ShowPlayerActionButtons(10);
        }


    }
    protected override void OnCancel(CombatManager manager)
    {
        stateMachine.Next(TurnStateType.ACTION_CANCELLED);
    }
    protected override void OnExit(CombatManager manager)
    {
        CombatTarget targetData = manager.GetCombatTargetInformation();
        if (targetData.typeOfTarget == CombatActionTargets.Self || targetData.typeOfTarget == CombatActionTargets.AllEnemies || targetData.typeOfTarget == CombatActionTargets.AllAllies)
        {
            stateMachine.Next(TurnStateType.TARGETING_COMPLETE);
        } else if (targetData.typeOfTarget == CombatActionTargets.SelfBodyPart)
        {
            stateMachine.Next(TurnStateType.SELECT_PART);
        }  else
        {
            stateMachine.Next(TurnStateType.SELECT_TARGET);
        }
    }
}

// If the player is targeting something other than the active combatant, a specific target needs to be set
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
        //Hide all player action buttons
        uiManager.ShowPlayerActionButtons(10);
    }
    protected override void OnCancel(CombatManager manager)
    {
        stateMachine.Next(TurnStateType.ACTION_CANCELLED);
    }
    protected override void OnExit(CombatManager manager)
    {
        CombatTarget targetInformation = manager.GetCombatTargetInformation();
        if (targetInformation.typeOfTarget == CombatActionTargets.SingleEnemyBodyPart || targetInformation.typeOfTarget == CombatActionTargets.SingleAllyBodyPart)
        {
            stateMachine.Next(TurnStateType.SELECT_PART);
        } else
        {
            stateMachine.Next(TurnStateType.TARGETING_COMPLETE);
        }
    }
}

// Most actions target a specific body part. This is that targeting part of the process.
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

        uiManager.ShowPlayerActionButtons(10);
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

// Once a target is selected, the internal processes of the turn begin.
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

// The actual execution of the action selected by the player happens here.
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

// Once the action has been executed, we transition into a cleanup state. This also marks the end of the turn.
public class ActionDoneState : aTurnState
{
    public ActionDoneState(TurnStateMachine sm)
    {

        stateMachine = sm;
    }

    protected override void OnEnter(CombatManager manager)
    {
        //Debug.Log("One turn over");
        CombatUIManager uiManager = manager.gameObject.GetComponent<CombatUIManager>();

        switch (manager._currentTurn)
        {
            case 0:
                uiManager.ShowPlayerActionButtons(1);
                break;
            case 1:
                uiManager.ShowPlayerActionButtons(2);
                break;
            case 2:
                uiManager.ShowPlayerActionButtons(0);
                break;

        }
        Exit(manager);
    }
    protected override void OnCancel(CombatManager manager)
    {
        stateMachine.Next(TurnStateType.ACTION_CANCELLED);
    }

    protected override void OnExit(CombatManager manager)
    {
        manager.DoAction();
    }
}

// Actions can be cancelled (think of when the player might choose to stop attacking altogether).
// This resets the state machine.
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