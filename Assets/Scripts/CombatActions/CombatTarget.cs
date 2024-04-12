using Combatant;

//Simple struct to hold the data of what's being targeted by the player. Useful for the state machine.
public struct CombatTarget
{
    public aCombatant targetUnit;
    public CombatantType sideBeingTargeted;
    public CombatActionTargets typeOfTarget;
    public int targetIndex;
    public int partIndex;
}