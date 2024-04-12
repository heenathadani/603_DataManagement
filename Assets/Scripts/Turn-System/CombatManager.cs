using Combatant;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private CombatUIManager uiManager;
    private List<aCombatant> _activeCombatants;
    private int _currentTurn;
    private CombatantType _activeType;
    private CombatTarget targetInformation;
    private aCombatAction currentAction;
    private TurnStateMachine stateMachine;

    public void DoAction()
    {
        if (_currentTurn == _activeCombatants.Count)
        {
            _activeType = CombatantData.GetNext(_activeType);
            _activeCombatants = CombatantData.GetGroup(_activeType);
            _currentTurn = 0;
            Debug.Log("Switching sides!");
        }
        Debug.Log("Current Turn: " + _activeCombatants[_currentTurn++].Name);
    }

    public void ExecuteCombatAction()
    {
        currentAction.DoAction(targetInformation);
    }

    public void BeginTurn(aCombatAction action, CombatActionTargets targetType, CombatantType side)
    {
        if (stateMachine.currentStateType != TurnStateType.TURN_START)
        {
            // We need to figure out a way of making sure people can't input other actions once they start
            // or we need to be flexible enough to handle people changing the type of action they want to do.

            // A cursor, rather than relying on the mouse, would go a long way here.
            throw new CombatRuntimeException("The turn was already started. This action should not have happened.");
        }
        targetInformation.sideBeingTargeted = side;
        targetInformation.typeOfTarget = targetType;
        currentAction = action;
        uiManager.ShowByTarget(targetType);
        stateMachine.Transition();
    }

    // This function is temporary and should go away once we transition
    // to a better system to setup combat initiation
    public void DummySetup()
    {
        // Set the data
        CombatantData.enemies.Add(new Enemy("Test Enemy 1"));
        CombatantData.enemies.Add(new Enemy("Test Enemy 2"));

        CombatantData.partyCharacters.Add(new Protagonist("Test Protagonist 1"));
        CombatantData.partyCharacters.Add(new Protagonist("Test Protagonist 2"));

        // Set up this controller
        _currentTurn = 0;
        _activeType = CombatantType.ALLIES;
        _activeCombatants = CombatantData.GetGroup(_activeType);

        Debug.Log("Data set up!");
    }

    private void OnEnable()
    {
        DummySetup();
        uiManager = GetComponent<CombatUIManager>();
        stateMachine = new TurnStateMachine(this);
    }


    public CombatTarget GetCombatTargetInformation()
    {
        return targetInformation;
    }

    public void SetTargetUnit(int index)
    {
        targetInformation.targetIndex = index;
        targetInformation.targetUnit = CombatantData.enemies[index];
        stateMachine.Next(TurnStateType.SELECT_PART);
    }

    public void SetPartIndex(int index)
    {
        targetInformation.partIndex = index;
        stateMachine.Transition();
    }

    public void ClearTarget()
    {
        targetInformation = new();
        uiManager.HideAll();
    }
}
