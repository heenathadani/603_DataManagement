using Combatant;
using System.Collections.Generic;
using UnityEngine;

public enum TurnStateType
{
    UPDATE_CONDITIONS,
    TURN_START,
    TARGETING_COMPLETE,
    EXECUTING_ACTION,
    ACTION_DONE
}

// A static factory for creating new states
public static class StateFactory
{
    public static aTurnState MakeState(TurnStateType type, TurnStateMachine sm)
    {
        switch (type)
        {
            case TurnStateType.TARGETING_COMPLETE:
                return new TargetingCompleteState(sm);
            case TurnStateType.EXECUTING_ACTION:
                return new ExecutingActionState(sm);
            case TurnStateType.ACTION_DONE:
                return new ActionDoneState(sm);
            case TurnStateType.UPDATE_CONDITIONS:
                return new UpdateConditionState(sm);
            default:
                return new TurnStartState(sm);
        }
    }
}

// The abstract class for the States for the turn state machine
public abstract class aTurnState
{
    public bool isTargetValidState;
    protected TurnStateMachine stateMachine;
    public void Enter(CombatManager manager)
    {
        OnEnter(manager);
    }

    public void Exit(CombatManager manager)
    {
        OnExit(manager);
    }

    protected abstract void OnEnter(CombatManager manager);
    protected abstract void OnExit(CombatManager manager);
}

// Entry point to the state machine. Resets everything.
public class TurnStartState : aTurnState
{
    public TurnStartState(TurnStateMachine sm)
    {
        stateMachine = sm;
        isTargetValidState = true;
    }

    protected override void OnEnter(CombatManager manager)
    {
        manager.ClearTarget();
        // Show player action buttons
        CombatUIManager uiManager = manager.gameObject.GetComponent<CombatUIManager>();
        if (manager._activeType == Combatant.CombatantType.ALLIES)
        {
            //Check if character dies, if dies, pass
            if(!CombatantData.partyCharacters[manager._currentTurn].isAlive())
            {
                manager._currentTurn += 1;
                if (manager._currentTurn > CombatantData.partyCharacters.Count - 1)
                {
                    manager.SwitchSides();
                }

                OnEnter(manager);
                return;
                
            }

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
            uiManager.ShowPlayerActionButtons(10);
            manager.StartAITurn();
        }


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
        isTargetValidState = false;
    }

    protected override void OnEnter(CombatManager manager)
    {
        // Hide the UI information
        CombatUIManager uiManager = manager.gameObject.GetComponent<CombatUIManager>();
        uiManager.HideAll();
        Exit(manager);
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
        isTargetValidState = false;
    }

    protected override void OnEnter(CombatManager manager)
    {
        manager.ExecuteCombatAction();
        Exit(manager);

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
        isTargetValidState = false;
    }

    protected override void OnEnter(CombatManager manager)
    {
        // Apply the effects of any active conditions
        aCombatant who = CombatantData.GetGroup(manager._activeType)[manager._currentTurn];
        who.ApplyConditions();

        CombatUIManager uiManager = manager.gameObject.GetComponent<CombatUIManager>();
        if (manager._activeType == Combatant.CombatantType.ALLIES)
        {
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
        }
        
        Exit(manager);
    }

    protected override void OnExit(CombatManager manager)
    {
        manager.EndTurn();
    }
}

public class UpdateConditionState : aTurnState
{
    public UpdateConditionState(TurnStateMachine sm)
    {
        stateMachine = sm;
        isTargetValidState = false;
    }

    protected override void OnEnter(CombatManager manager)
    {
        List<aCombatant> who = CombatantData.GetGroup(manager._activeType);
        who[manager._currentTurn].UpdateActiveConditions();
        Exit(manager);
    }

    protected override void OnExit(CombatManager manager)
    {
        stateMachine.Next(TurnStateType.TURN_START);
    }
}