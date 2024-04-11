using Combatant;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private List<aCombatant> _activeCombatants;
    private int _currentTurn;
    private CombatantType _activeType;

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
    }

}
