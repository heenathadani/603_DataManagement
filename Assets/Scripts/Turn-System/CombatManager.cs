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

    // Have the combat manager treat combat like a state-based process
    private TurnStateMachine stateMachine;

    //Store the body part data list
    public List<BodyPartData> _bodyPartDataList;

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
        stateMachine.Next(TurnStateType.TURN_START);
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

        // Set up the target information for the action selected by the player
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

        // Temp, Check status update -- Rin
        Protagonist protagonist = CombatantData.partyCharacters[0] as Protagonist;
        if (protagonist != null && _bodyPartDataList.Count > 0)
        {
            protagonist.AddBodyPart(new BodyPart(_bodyPartDataList[0]));
            protagonist.AddBodyPart(new BodyPart(_bodyPartDataList[0]));
            protagonist.UpdateStatus();
        }

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

    // Most of the state machine logic relies on having access to what the player is trying to do.
    public CombatTarget GetCombatTargetInformation()
    {
        return targetInformation;
    }

    public void SetTargetUnit(int index)
    {
        targetInformation.targetIndex = index;
        targetInformation.targetUnit = CombatantData.enemies[index];

        // We hack the state machine to allow us to transition back into this stage if the player changes targets
        stateMachine.Next(TurnStateType.SELECT_PART);
    }

    public void SetPartIndex(int index)
    {
        // This will likely need updating once Rin's code is done
        targetInformation.partIndex = index;
        stateMachine.Transition();
    }

    public void ClearTarget()
    {
        targetInformation = new();
        uiManager.HideAll();
    }
}
