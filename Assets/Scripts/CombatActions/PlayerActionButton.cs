
using Combatant;
using UnityEngine;

public class PlayerActionButton : MonoBehaviour
{
    public CombatActionTypes actionType;
    public CombatActionTargets actionTarget;
    public CombatantType sideThisAffects;

    public void DoTheThing()
    {
        aCombatAction action = CombatActionFactory.MakeAction(actionType, actionTarget);
        GetComponentInParent<CombatManager>().BeginTurn(action, actionTarget, sideThisAffects);
    }
}