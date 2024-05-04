using System.Collections.Generic;
using Combatant;
using Unity.VisualScripting;
using UnityEngine;

public enum CombatActionTypes
{
    Attack,
    Defend,
    Power,
    Item,
    Escape
}

public enum CombatActionTargets
{
    Self,
    SelfBodyPart,
    SingleEnemyBodyPart,
    SingleAllyBodyPart,
    SingleEnemy,
    SingleAlly,
    AllEnemies,
    AllAllies
}

public static class CombatActionFactory
{
    public static aCombatAction MakeAction(CombatActionTypes type, CombatActionTargets target)
    {
        aCombatAction result;
        switch (type)
        {
            case CombatActionTypes.Attack:
                result = new AttackAction();
                break;
            case CombatActionTypes.Defend:
                result = new DefendAction();
                break;
            case CombatActionTypes.Power:
                result = new PowerAction();
                break;
            case CombatActionTypes.Item:
                result = new ItemAction();
                break;
            default:
                result = new EscapeAction();
                break;
        }
        result.SetTargetType(target);
        result.type = type;
        return result;
    }
}

public interface ICombatAction
{
    public CombatActionTargets GetActionTarget();
    public void SetTargetType(CombatActionTargets targetType);
    public void DoAction(CombatTarget targetInformation);
}

public abstract class aCombatAction : ICombatAction
{
    protected CombatActionTargets _targetType;
    public CombatActionTypes type;
    protected UI_BattleFeedback battleLog;//Carrie: reference to the BattleFeedback script

    public CombatActionTargets GetActionTarget()
    {
        return _targetType;
    }

    protected aCombatant actingAgent;

    protected abstract void DoSingleTarget(CombatTarget targetInformation);
    protected abstract void DoMultiTarget(CombatTarget targetInformation);
    protected abstract void DoSelf(CombatTarget targetInformation);
    protected abstract void DoSingleBodyPart(CombatTarget targetInformation);

    public void DoAction(CombatTarget targetInformation)
    {
        if (_targetType == CombatActionTargets.Self)
        {
            DoSelf(targetInformation);
        } else if (_targetType == CombatActionTargets.SingleEnemy || _targetType == CombatActionTargets.SingleAlly)
        {
            DoSingleTarget(targetInformation);
        } else if (_targetType == CombatActionTargets.SingleEnemyBodyPart || _targetType == CombatActionTargets.SingleAllyBodyPart || _targetType == CombatActionTargets.SelfBodyPart) {
            DoSingleBodyPart(targetInformation);
        } else
        {
            DoMultiTarget(targetInformation);
        }
    }
    public void SetTargetType(CombatActionTargets targetType)
    {
        _targetType = targetType;
    }

    public void SetActingAgent(aCombatant whoIsDoingThisAction)
    {
        actingAgent = whoIsDoingThisAction;
    }

    protected void ShowActionFeedback(CombatTarget targetInformation, bool isDamage, int actionEffectValue)
    {
        if(battleLog.IsUnityNull())//Carrie: gets a reference to the battle log if it doesn't already have one
        {
            battleLog = GameObject.FindGameObjectWithTag("BattleLog").GetComponent<UI_BattleFeedback>();
        }

        if (isDamage)
        {
            targetInformation.targetUnit.combatantUI.DisplayDamage(actionEffectValue);

            battleLog.updateBattleLog(actionEffectValue, type.ToString(), actingAgent.Name, targetInformation.targetUnit.Name);//Carrie: updates the battlelog
        }
    }
}

public class AttackAction : aCombatAction
{
    protected override void DoSelf(CombatTarget targetInformation)
    {

    }
    protected override void DoMultiTarget(CombatTarget targetInformation)
    {
        
    }

    protected override void DoSingleTarget(CombatTarget targetInformation)
    {

    }
        
    protected override void DoSingleBodyPart(CombatTarget targetInformation)
    {
        // Roll for attack
        float chance = targetInformation.actingUnit.GetStatValue(StatType.HitRate) - targetInformation.targetUnit.GetStatValue(StatType.Evasion);
        bool attackSuccess = Random.Range(0.0f, 1.0f) <= chance;
        
        // Handle miss
        if (!attackSuccess)
        {
            targetInformation.targetUnit.combatantUI.DisplayMiss();
            return;
        }

        // Deal Damage
        float damage = targetInformation.actingUnit.GetStatValue(StatType.Damage) * (1 - targetInformation.targetUnit.GetStatValue(StatType.Armor));
        targetInformation.targetUnit.DamageBodyPart(targetInformation.partType, damage);
        targetInformation.targetUnit.combatantUI.DisplayDamage((int)(damage * 100));
        ShowActionFeedback(targetInformation, true, (int)(damage * 100));
        if (targetInformation.sideBeingTargeted != CombatantType.ALLIES)
        {
            DataTracking dataTracker = (DataTracking) GameObject.FindAnyObjectByType(typeof(DataTracking));
            dataTracker.DealDamage(targetInformation.actingUnit, (int)(damage * 100));
        }
    }
}

public class DefendAction : aCombatAction
{
    protected override void DoSelf(CombatTarget targetInformation)
    {
        // The defend action gives the defend condition
        targetInformation.targetUnit.AddCondition(ConditionType.Defend);
    }
    protected override void DoMultiTarget(CombatTarget targetInformation)
    {

    }

    protected override void DoSingleTarget(CombatTarget targetInformation)
    {
        
    }

    protected override void DoSingleBodyPart(CombatTarget targetInformation)
    {
        
    }
}

public class PowerAction : aCombatAction
{
    protected override void DoSelf(CombatTarget targetInformation)
    {
        aCombatant target = targetInformation.targetUnit;
    }
    protected override void DoMultiTarget(CombatTarget targetInformation)
    {
        CombatantType sideBeingTargeted = targetInformation.sideBeingTargeted;
        List<aCombatant> whoThough;
        if (sideBeingTargeted == CombatantType.ALLIES)
        {
            whoThough = CombatantData.partyCharacters;
        }
        else
        {
            whoThough = CombatantData.enemies;
        }
        foreach(aCombatant target in whoThough)
        {
            int summedTotal = 0;
            int perPartDamage = (int) targetInformation.selectedPower.GetTotalEffect(actingAgent) / target._equipment.Count;
            Dictionary<BodyPartType, BodyPart>.KeyCollection keys = target._equipment.Keys;
            for (int i = 0; i < keys.Count; i++)
            {
                // Calculate the damage
            }
            target.combatantUI.DisplayDamage(summedTotal);
        }
    }

    protected override void DoSingleTarget(CombatTarget targetInformation)
    {
        aCombatant target = targetInformation.targetUnit;
        int damage = (int) targetInformation.selectedPower.GetTotalEffect(actingAgent);
        ShowActionFeedback(targetInformation, true, damage);
    }

    protected override void DoSingleBodyPart(CombatTarget targetInformation)
    {
        aCombatant target = targetInformation.targetUnit;
        int totalEffect = (int) targetInformation.selectedPower.GetTotalEffect(actingAgent);
        ShowActionFeedback(targetInformation, true, totalEffect);
    }
}

public class ItemAction : aCombatAction
{
    protected override void DoSelf(CombatTarget targetInformation)
    {

    }
    protected override void DoMultiTarget(CombatTarget targetInformation)
    {

    }

    protected override void DoSingleTarget(CombatTarget targetInformation)
    {
        
    }

    protected override void DoSingleBodyPart(CombatTarget targetInformation)
    {
        
    }
}

public class EscapeAction : aCombatAction
{
    protected override void DoSelf(CombatTarget targetInformation)
    {
        // Have the acting combatant leave the battlefield

    }
    protected override void DoMultiTarget(CombatTarget targetInformation)
    {
        // Have the entire party escape
    }
        
    protected override void DoSingleTarget(CombatTarget targetInformation)
    {
        // Remove target from combat
    }

    protected override void DoSingleBodyPart(CombatTarget targetInformation)
    {
        // N/A, this shouldn't happen.
        throw new System.NotImplementedException();
    }
}