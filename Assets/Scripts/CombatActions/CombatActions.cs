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
    SingeAllyBodyPart,
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
        return result;
    }
}

public interface ICombatAction
{
    public void SetTargetType(CombatActionTargets targetType);
    public void DoAction(CombatTarget targetInformation);
}

public abstract class aCombatAction : ICombatAction
{
    protected CombatActionTargets _targetType;
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
        } else if (_targetType == CombatActionTargets.SingleEnemyBodyPart || _targetType == CombatActionTargets.SingeAllyBodyPart || _targetType == CombatActionTargets.SelfBodyPart) {
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
    }
        
    protected override void DoSingleBodyPart(CombatTarget targetInformation)
    {
        Debug.Log("I am attacking " + targetInformation.targetUnit.Name + " in one of his parts");
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
    }
    protected override void DoMultiTarget(CombatTarget targetInformation)
    {
        CombatantType sideBeingTargeted = targetInformation.sideBeingTargeted;
        List<aCombatant> whoThough;
        if (actingAgent.GetSide() == sideBeingTargeted)
        {
            // This means Allies targeting their own allies or Enemies targeting their enemies. Either way, the party is the target.
            whoThough = CombatantData.partyCharacters;
        }
        else
        {
            // This means allies targeting their enemies or enemies targeting their allies. Either way, the enemies are the target
            whoThough = CombatantData.enemies;
        }
        foreach(aCombatant target in whoThough)
        {
            target.AffectStatByType(targetInformation.selectedPower.statAffectedByPower, targetInformation.selectedPower.GetTotalEffect(actingAgent));
        }
    }

    protected override void DoSingleTarget(CombatTarget targetInformation)
    {
        aCombatant target = targetInformation.targetUnit;
        target.AffectStatByType(targetInformation.selectedPower.statAffectedByPower, targetInformation.selectedPower.GetTotalEffect(actingAgent));
    }

    protected override void DoSingleBodyPart(CombatTarget targetInformation)
    {
        aCombatant target = targetInformation.targetUnit;
        target.AffectBodyPartByType(targetInformation.selectedPower.bodyPart, targetInformation.selectedPower.GetTotalEffect(actingAgent));
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