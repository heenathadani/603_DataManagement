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
        targetInformation.targetUnit.AffectBodyPartByIndex(targetInformation.targetIndex, -CombatantData.partyCharacters[CombatantData.currentPlayerIndex]._attackPoint);
        targetInformation.targetUnit.UpdateStatus();

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
            int perPartDamage = (int) targetInformation.selectedPower.GetTotalEffect(actingAgent) / target._bodyPartsInventory.Count;
            for (int i = 0; i < target._bodyPartsInventory.Count; i++)
            {
                int finalDamage = perPartDamage - (int)target._bodyPartsInventory[i].bodyPartData.shieldPoint;
                target.AffectBodyPartByIndex(i, -finalDamage);
            }
            target.UpdateStatus();
        }
        actingAgent._currentEnergy -= targetInformation.selectedPower.cost;
    }

    protected override void DoSingleTarget(CombatTarget targetInformation)
    {
        aCombatant target = targetInformation.targetUnit;
        target.AffectStatByType(targetInformation.selectedPower.statAffectedByPower, targetInformation.selectedPower.GetTotalEffect(actingAgent));
        target.UpdateStatus();
        actingAgent._currentEnergy -= targetInformation.selectedPower.cost;
    }

    protected override void DoSingleBodyPart(CombatTarget targetInformation)
    {
        aCombatant target = targetInformation.targetUnit;
        target.AffectBodyPartByType(targetInformation.selectedPower.bodyPart, targetInformation.selectedPower.GetTotalEffect(actingAgent));
        target.UpdateStatus();
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