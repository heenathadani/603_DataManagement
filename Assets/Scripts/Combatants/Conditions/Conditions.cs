using Combatant;

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

public abstract class aCondition
{
    public abstract void ApplyConditionEffect(aCombatant who);
    public abstract void CleanUp(aCombatant who);
}

public class DefendCondition : aCondition
{
    public override void ApplyConditionEffect(aCombatant who)
    {
        who.AffectStatByType(StatType.Shield, 10);
    }

    public override void CleanUp(aCombatant who)
    {
        who.AffectStatByType(StatType.Shield, -10);
    }
}