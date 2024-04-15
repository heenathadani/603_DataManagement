
using Combatant;
using UnityEngine;

// Interim logic for UI. This will likely become deprecated or adapted once Annie and Heena unify their UI.
public class PlayerActionButton : MonoBehaviour
{
    public int playerIndex;
    public CombatActionTypes actionType;
    public CombatActionTargets actionTarget;
    public CombatantType sideThisAffects;

    public void DoTheThing()
    {
        aCombatAction action = CombatActionFactory.MakeAction(actionType, actionTarget);
        CombatTarget combatTarget = new();
        combatTarget.typeOfTarget = actionTarget;
        combatTarget.sideBeingTargeted = sideThisAffects;
        GetComponentInParent<CombatManager>().BeginTurn(playerIndex, action, combatTarget);

        //Set player index -- Rin
        CombatantData.currentPlayerIndex = playerIndex;
    }
}