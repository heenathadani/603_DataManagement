
using Combatant;
using UnityEngine;

// Interim logic for UI. This will likely become deprecated or adapted once Annie and Heena unify their UI.
public class PlayerActionButton : MonoBehaviour
{
    public CombatActionTypes actionType;
    public CombatActionTargets actionTarget;
    public CombatantType sideThisAffects;

    public void DoTheThing()
    {
        aCombatAction action = CombatActionFactory.MakeAction(actionType, actionTarget);
        GetComponentInParent<CombatManager>().BeginTurn(action, actionTarget, sideThisAffects);
        Debug.Log("good");
    }
}