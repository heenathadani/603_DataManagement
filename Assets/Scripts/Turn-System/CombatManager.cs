using Combatant;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public EnemyFormation dummyFormation;
    public GameObject enemyPrefab;
    private List<GameObject> activeEnemies;

    private CombatUIManager uiManager;
    private List<aCombatant> _activeCombatants;
    public int _currentTurn;
    public CombatantType _activeType;
    private CombatTarget targetInformation;
    private aCombatAction currentAction;

    // Have the combat manager treat combat like a state-based process
    private TurnStateMachine stateMachine;

    //Store the body part data list
    public List<BodyPartData> _bodyPartDataList;
    //Store all enemy HP bar
    public List<Slider> _enemySliderList;

    public void DoAction()
    {
        if (_currentTurn == _activeCombatants.Count - 1)
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

    public void BeginTurn(int playerIndex, aCombatAction action, CombatTarget targetInfo)
    {
        if (stateMachine.currentStateType != TurnStateType.TURN_START)
        {
            // We need to figure out a way of making sure people can't input other actions once they start
            // or we need to be flexible enough to handle people changing the type of action they want to do.

            // A cursor, rather than relying on the mouse, would go a long way here.
            throw new CombatRuntimeException("The turn was already started. This action should not have happened.");
        }

        // Set up the target information for the action selected by the player
        targetInformation = targetInfo;
        currentAction = action;
        action.SetActingAgent(CombatantData.GetGroup(_activeType)[playerIndex]);
        uiManager.ShowByTarget(targetInformation.typeOfTarget);
        stateMachine.Transition();
    }

    // This function is temporary and should go away once we transition
    // to a better system to setup combat initiation
    public void DummySetup()
    {

        // Set the data
        foreach(Enemy e in dummyFormation.enemies)
        {
            CombatantData.enemies.Add(e);
            GameObject enemyObject = Instantiate(enemyPrefab, transform);

        }

        CombatantData.partyCharacters.Add(new Protagonist("Test Protagonist 1",0));
        CombatantData.partyCharacters.Add(new Protagonist("Test Protagonist 2",1));
        CombatantData.partyCharacters.Add(new Protagonist("Test Protagonist 3",2));

        // Temp, Check status update -- Rin
        Protagonist protagonist1 = CombatantData.partyCharacters[0] as Protagonist;
        if (protagonist1 != null && _bodyPartDataList.Count > 0)
        {
            protagonist1.AddBodyPart(new BodyPart(_bodyPartDataList[0]));
            protagonist1.AddBodyPart(new BodyPart(_bodyPartDataList[0]));
            protagonist1.UpdateStatus();
        }
        Protagonist protagonist2 = CombatantData.partyCharacters[1] as Protagonist;
        if (protagonist2 != null && _bodyPartDataList.Count > 0)
        {
            protagonist2.AddBodyPart(new BodyPart(_bodyPartDataList[1]));
            protagonist2.AddBodyPart(new BodyPart(_bodyPartDataList[1]));
            protagonist2.UpdateStatus();
        }
        Protagonist protagonist3 = CombatantData.partyCharacters[2] as Protagonist;
        if (protagonist3 != null && _bodyPartDataList.Count > 0)
        {
            protagonist3.AddBodyPart(new BodyPart(_bodyPartDataList[2]));
            protagonist3.AddBodyPart(new BodyPart(_bodyPartDataList[2]));
            protagonist3.UpdateStatus();
        }

        Enemy enemy1 = CombatantData.enemies[0] as Enemy;
        if (enemy1 != null && _bodyPartDataList.Count > 0)
        {
            enemy1.AddBodyPart(new BodyPart(_bodyPartDataList[0]));
            enemy1.AddBodyPart(new BodyPart(_bodyPartDataList[0]));
            enemy1.AddBodyPart(new BodyPart(_bodyPartDataList[0]));
            enemy1.UpdateStatus();
        }
        Enemy enemy2 = CombatantData.enemies[1] as Enemy;
        if (enemy2 != null && _bodyPartDataList.Count > 0)
        {
            enemy2.AddBodyPart(new BodyPart(_bodyPartDataList[0]));
            enemy2.AddBodyPart(new BodyPart(_bodyPartDataList[0]));
            enemy2.AddBodyPart(new BodyPart(_bodyPartDataList[0]));
            enemy2.UpdateStatus();
        }
        Enemy enemy3 = CombatantData.enemies[2] as Enemy;
        if (enemy3 != null && _bodyPartDataList.Count > 0)
        {
            enemy3.AddBodyPart(new BodyPart(_bodyPartDataList[0]));
            enemy3.AddBodyPart(new BodyPart(_bodyPartDataList[0]));
            enemy3.AddBodyPart(new BodyPart(_bodyPartDataList[0]));
            enemy3.UpdateStatus();
        }
        //End

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

    public void AITurnEnd()
    {
        stateMachine.Next(TurnStateType.ACTION_DONE);
    }

    public void SetCombatTarget(CombatTarget targetInfo)
    {
        targetInformation = targetInfo;
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
