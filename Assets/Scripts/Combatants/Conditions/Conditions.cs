using Combatant;
using System;

public enum ConditionType
{
    Normal,
    Defend
}

[System.Serializable]
public class ConditionData
{
    public int maxDuration;
    public int currentDuration;
    public ConditionType type;
    public aCondition conditionBehavior;
}

// A static class that handles everything related to conditions.
// This keeps code execution encapsulated within this domain
public static class ConditionManager
{
    public static void ApplyConditionEffect(aCombatant who, ConditionData data)
    {
        if (data.conditionBehavior == null) return;
        data.conditionBehavior.ApplyConditionEffect(who);
    }

    public static void CleanUpCondition(aCombatant who, ConditionData data)
    {
        if (data.conditionBehavior == null) return;
        data.conditionBehavior.CleanUp(who);
    }

    public static ConditionData CreateConditionData(ConditionType type)
    {
        ConditionData data = new();
        data.type = type;
        switch (type)
        {
            case ConditionType.Defend:
                data.maxDuration = 1;
                data.currentDuration = 0;
                data.conditionBehavior = new DefendCondition();
                return data;
            default:
                return data;
        }
    }
}

// The abstract representation of a condition
public abstract class aCondition
{
    
    public abstract ConditionType Condition { get; }
    public abstract void ApplyConditionEffect(aCombatant who);
    public abstract void CleanUp(aCombatant who);

    
}

// Example condition - Defend.
// Adds a bonus to armor while active
// Removes that bonus on clean up
public class DefendCondition : aCondition
{
    public override ConditionType Condition
    {
        get{
            return ConditionType.Defend;
        }
    }

    public override void ApplyConditionEffect(aCombatant who)
    {
        
    }

    public override void CleanUp(aCombatant who)
    {
       
    }
}