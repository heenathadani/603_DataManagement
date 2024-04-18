using Combatant;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public EnemyFormation dummyFormation;

    private List<EnemyGameObject> activeEnemies;
    public EnemySpawnManager spawnManager;
    private CombatUIManager uiManager;
    private List<aCombatant> _activeCombatants;
    public GameObject CharacterUIPrefab;
    public GameObject EnemyUIPrefab;
    
    public int _currentTurn;
    public CombatantType _activeType;
    private CombatTarget targetInformation;
    private aCombatAction currentAction;

    private IEnumerator SlowEnemiesDown()
    {
        yield return new WaitForSeconds(1.0f);
        if (activeEnemies[_currentTurn]._combatantData.isAlive())
        {
            activeEnemies[_currentTurn].TakeTurn();
        }
        else
        {
            AITurnEnd();
        }
    }

    // Have the combat manager treat combat like a state-based process
    private TurnStateMachine stateMachine;

    //Store the body part data list
    public List<BodyPartData> _bodyPartDataList;

    public void SwitchSides()
    {
        _activeType = CombatantData.GetNext(_activeType);
        _activeCombatants = CombatantData.GetGroup(_activeType);
        _currentTurn = 0;
    }

    public void DoAction()
    {

        _currentTurn++;
        if (_currentTurn == _activeCombatants.Count)
        {
            SwitchSides();
        }
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

    private void SpawnEnemies()
    {
        spawnManager.SetEnemiesToSpawn(dummyFormation.enemies.Count);
        for (int i = 0; i < dummyFormation.enemies.Count; i++)
        {

            Enemy e = dummyFormation.enemies[i].Clone(i);
            e.SetSlider(uiManager.enemyHpSliderList[i]);
            e.UpdateStatus();
            CombatantData.enemies.Add(e);
            GameObject spawnedEnemy = spawnManager.SpawnEnemy(e);
            EnemyGameObject eGO = spawnedEnemy.GetComponent<EnemyGameObject>();
            eGO.SetManager(this);
            activeEnemies.Add(eGO);
        }
    }

    private void SetUPUI()
    {
        foreach(EnemyGameObject enemy in activeEnemies)
        {
            GameObject enemyUICanvasObject = Instantiate(EnemyUIPrefab, transform);
            CombatEntityUI entityUI = enemyUICanvasObject.GetComponent<CombatEntityUI>();
            enemy._combatantData.combatantUI = entityUI;
            uiManager.AddCombatEntityUI(CombatantType.ENEMIES, entityUI);
            // Relocate the canvas object
        }

        for (int i = 0; i < CombatantData.partyCharacters.Count; i++)
        {
            GameObject playerUICanvasObject = Instantiate(CharacterUIPrefab, transform);
            CombatEntityUI entityUI = playerUICanvasObject.GetComponent<CombatEntityUI>();
            CombatantData.partyCharacters[i].combatantUI = entityUI;
            uiManager.AddCombatEntityUI(CombatantType.ALLIES, entityUI);
            // Relocate this canvas object
        }
    }

    public void SpawnCharacters()
    {
        CombatantData.partyCharacters.Add(new Protagonist("Test Protagonist 1", 0));
        CombatantData.partyCharacters.Add(new Protagonist("Test Protagonist 2", 1));
        CombatantData.partyCharacters.Add(new Protagonist("Test Protagonist 3", 2));

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
    }

    private void OnEnable()
    {
        uiManager = GetComponent<CombatUIManager>();
        stateMachine = new TurnStateMachine(this);
        activeEnemies = new List<EnemyGameObject>();
        SpawnCharacters();
        SpawnEnemies();
        SetUPUI();

        // Set up this controller
        _currentTurn = 0;
        _activeType = CombatantType.ALLIES;
        _activeCombatants = CombatantData.GetGroup(_activeType);

        stateMachine.Next(TurnStateType.TURN_START);
    }

    // Most of the state machine logic relies on having access to what the player is trying to do.
    public CombatTarget GetCombatTargetInformation()
    {
        return targetInformation;
    }


    //Pass the turn
    public void StartAITurn()
    {
        StartCoroutine(SlowEnemiesDown());
        
        
    }
    
    public void SetAIAction(aCombatAction action)
    {
        currentAction = action;
    }

    public void AITurnEnd()
    {
        // Check if all players are dead
        bool allPlayersDead = true;
        foreach(aCombatant protagonist in CombatantData.partyCharacters)
        {
            if (protagonist.isAlive())
            {
                allPlayersDead = false;
            }
        }
        if (allPlayersDead)
        {
            Debug.Log("All players are dead");
        } else
        {
            stateMachine.Next(TurnStateType.ACTION_DONE);
        }
        
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
