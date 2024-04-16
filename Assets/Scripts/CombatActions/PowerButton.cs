public class PowerButton : PlayerActionButton
{
    public Power power;

    public void DoPower()
    {
        aCombatAction action = CombatActionFactory.MakeAction(actionType, actionTarget);
        CombatTarget combatTarget = new();
        combatTarget.sideBeingTargeted = sideThisAffects;
        combatTarget.typeOfTarget = actionTarget;
        combatTarget.selectedPower = power;
        GetComponentInParent<CombatManager>().BeginTurn(playerIndex, action, combatTarget);
    }
}