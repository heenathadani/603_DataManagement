using Combatant;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    public void EndTurn()
    {

        // Check for game over
        bool isGameOver = false;
        for (int i = 0; i < CombatantData.partyCharacters.Count; i++)
        {
            if (CombatantData.partyCharacters[i].isAlive())
            {
                break;
            } else
            {
                uiManager.ShowGameOver();
                isGameOver = true;
            }
        }

        for (int i = 0; i < CombatantData.enemies.Count; i++)
        {
            if (CombatantData.enemies[i].isAlive())
            {
                break;
            }
            else
            {
                uiManager.ShowVictory();
                isGameOver = true;
            }
        }

        if (isGameOver)
        {
            // Quit out of this loop
            return;
        }


        // Otherwise, continue with battle
        _currentTurn++;
        if (_currentTurn == _activeCombatants.Count)
        {
            SwitchSides();
        }
        stateMachine.Next(TurnStateType.UPDATE_CONDITIONS);
    }

    public void ExecuteCombatAction()
    {
        currentAction.DoAction(targetInformation);
    }

    public void BeginTurn(int playerIndex, aCombatAction action, CombatTarget targetInfo)
    {
        // Set up the target information for the action selected by the player
        targetInformation = targetInfo;
        currentAction = action;
        targetInformation.actingUnit = _activeCombatants[_currentTurn];
        action.SetActingAgent(CombatantData.GetGroup(_activeType)[playerIndex]);
        if (action.GetActionTarget() == CombatActionTargets.Self)
        {
            targetInformation.targetUnit = _activeCombatants[_currentTurn];
        }
        stateMachine.Transition();
    }

    private void SpawnEnemies()
    {
        EnemyFormation spawnFormation = dummyFormation;
        if (CombatantData.enemyCombatFormation != null)
        {
            spawnFormation = CombatantData.enemyCombatFormation;
        }
        spawnManager.SetEnemiesToSpawn(spawnFormation.enemies.Count);
        for (int i = 0; i < spawnFormation.enemies.Count; i++)
        {

            Enemy e = spawnFormation.enemies[i].Clone(i);
            for (int j = 0; j < spawnFormation.enemyInventory[i].data.Length; j++)
            {
                e.Equip(new BodyPart(spawnFormation.enemyInventory[i].data[j]));
            }
            CombatantData.enemies.Add(e);
            GameObject spawnedEnemy = spawnManager.SpawnEnemy(e);
            EnemyGameObject eGO = spawnedEnemy.GetComponent<EnemyGameObject>();
            eGO.SetManager(this);
            eGO.enemyIndex = i;
            activeEnemies.Add(eGO);
        }
    }

    private void SetUpUI()
    {
        for (int i = 0; i < CombatantData.partyCharacters.Count; i++)
        {
            GameObject playerUICanvasObject = Instantiate(CharacterUIPrefab, transform);
            CombatEntityUI entityUI = playerUICanvasObject.GetComponent<CombatEntityUI>();
            entityUI.hpBar.value = 1.0f;
            CombatantData.partyCharacters[i].combatantUI = entityUI;

            uiManager.AddCombatEntityUI(CombatantType.ALLIES, entityUI);
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
            protagonist1.Equip(new BodyPart(_bodyPartDataList[0]));
            protagonist1.Equip(new BodyPart(_bodyPartDataList[1]));
            protagonist1.Equip(new BodyPart(_bodyPartDataList[2]));
            protagonist1.Equip(new BodyPart(_bodyPartDataList[3]));
        }

        Protagonist protagonist2 = CombatantData.partyCharacters[1] as Protagonist;
        if (protagonist2 != null && _bodyPartDataList.Count > 0)
        {
            protagonist2.Equip(new BodyPart(_bodyPartDataList[0]));
            protagonist2.Equip(new BodyPart(_bodyPartDataList[1]));
            protagonist2.Equip(new BodyPart(_bodyPartDataList[2]));
            protagonist2.Equip(new BodyPart(_bodyPartDataList[3]));
        }

        Protagonist protagonist3 = CombatantData.partyCharacters[2] as Protagonist;
        if (protagonist3 != null && _bodyPartDataList.Count > 0)
        {
            protagonist3.Equip(new BodyPart(_bodyPartDataList[0]));
            protagonist3.Equip(new BodyPart(_bodyPartDataList[1]));
            protagonist3.Equip(new BodyPart(_bodyPartDataList[2]));
            protagonist3.Equip(new BodyPart(_bodyPartDataList[3]));
        }
    }

    private void OnEnable()
    {
        uiManager = GetComponent<CombatUIManager>();
        uiManager.Setup(this);
        stateMachine = new TurnStateMachine(this);
        activeEnemies = new List<EnemyGameObject>();
        SpawnCharacters();
        SpawnEnemies();
        SetUpUI();

        // Set up this controller
        _currentTurn = 0;
        _activeType = CombatantType.ALLIES;
        _activeCombatants = CombatantData.GetGroup(_activeType);

        stateMachine.Next(TurnStateType.TURN_START);
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

    public bool InTargetState()
    {
        return stateMachine.isActiveTargetState();
    }

    public void ClearTarget()
    {
        targetInformation = new();
        uiManager.HideAll();
    }
}
