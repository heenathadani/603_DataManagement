using System.Collections.Generic;
using Combatant;
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
        if (isDamage)
        {
            targetInformation.targetUnit.combatantUI.DisplayDamage(actionEffectValue);
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
        Debug.Log("I am attacking " + targetInformation.targetUnit.Name);
        targetInformation.targetUnit.UpdateStatus();
    }
        
    protected override void DoSingleBodyPart(CombatTarget targetInformation)
    {
        Debug.Log("I "+actingAgent.Name+" am attacking " + targetInformation.targetUnit.Name + " in one of his parts");

        //Take Damage -- Rin
        int attackDamage = (int)-CombatantData.partyCharacters[CombatantData.currentPlayerIndex]._attackPoint;
        targetInformation.targetUnit.AffectBodyPartByIndex(targetInformation.targetIndex, attackDamage);
        targetInformation.targetUnit.UpdateStatus();
        ShowActionFeedback(targetInformation, true, -attackDamage);
    }
}

public class DefendAction : aCombatAction
{
    protected override void DoSelf(CombatTarget targetInformation)
    {
        Debug.Log("I have defended");
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
        target.AffectStatByType(targetInformation.selectedPower.statAffectedByPower, targetInformation.selectedPower.GetTotalEffect(actingAgent));
        target.UpdateStatus();
        actingAgent._currentEnergy -= targetInformation.selectedPower.cost;
    }
    protected override void DoMultiTarget(CombatTarget targetInformation)
    {
        Debug.Log("Doing multitarget power");
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
            int perPartDamage = (int) targetInformation.selectedPower.GetTotalEffect(actingAgent) / target._bodyPartsInventory.Count;
            for (int i = 0; i < target._bodyPartsInventory.Count; i++)
            {
                int finalDamage = Mathf.Max(perPartDamage - (int)target._bodyPartsInventory[i].bodyPartData.shieldPoint,0);
                summedTotal += finalDamage;
                target.AffectBodyPartByIndex(i, -finalDamage);
            }
            target.combatantUI.DisplayDamage(summedTotal);
            target.UpdateStatus();
        }
        actingAgent._currentEnergy -= targetInformation.selectedPower.cost;
    }

    protected override void DoSingleTarget(CombatTarget targetInformation)
    {
        aCombatant target = targetInformation.targetUnit;
        int damage = (int) targetInformation.selectedPower.GetTotalEffect(actingAgent);
        target.AffectStatByType(targetInformation.selectedPower.statAffectedByPower, -damage);
        target.UpdateStatus();
        ShowActionFeedback(targetInformation, true, damage);
        actingAgent._currentEnergy -= targetInformation.selectedPower.cost;
    }

    protected override void DoSingleBodyPart(CombatTarget targetInformation)
    {
        aCombatant target = targetInformation.targetUnit;
        int totalEffect = (int) targetInformation.selectedPower.GetTotalEffect(actingAgent);
        target.AffectBodyPartByType(targetInformation.selectedPower.bodyPart, -totalEffect);
        target.UpdateStatus();
        ShowActionFeedback(targetInformation, true, totalEffect);
        actingAgent._currentEnergy -= targetInformation.selectedPower.cost;
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