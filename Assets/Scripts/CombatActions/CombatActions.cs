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
    public void DoAction();
}

public abstract class aCombatAction : ICombatAction
{
    protected CombatActionTargets _targetType;

    protected abstract void DoSingleTarget();
    protected abstract void DoMultiTarget();
    protected abstract void DoSelf();
    public void DoAction()
    {
        if (_targetType == CombatActionTargets.Self)
        {
            DoSelf();
        } else if (_targetType == CombatActionTargets.SingleEnemy || _targetType == CombatActionTargets.SingleAlly)
        {
            DoSingleTarget();
        } else
        {
            DoMultiTarget();
        }
    }
    public void SetTargetType(CombatActionTargets targetType)
    {
        _targetType = targetType;
    }
}

public class AttackAction : aCombatAction
{
    protected override void DoSelf()
    {

    }
    protected override void DoMultiTarget()
    {
        
    }

    protected override void DoSingleTarget()
    {
        
    }
}

public class DefendAction : aCombatAction
{
    protected override void DoSelf()
    {

    }
    protected override void DoMultiTarget()
    {
        
    }

    protected override void DoSingleTarget()
    {
        
    }
}

public class PowerAction : aCombatAction
{
    protected override void DoSelf()
    {

    }
    protected override void DoMultiTarget()
    {
        
    }

    protected override void DoSingleTarget()
    {
        
    }
}

public class ItemAction : aCombatAction
{
    protected override void DoSelf()
    {

    }
    protected override void DoMultiTarget()
    {
        
    }

    protected override void DoSingleTarget()
    {
        
    }
}

public class EscapeAction : aCombatAction
{
    protected override void DoSelf()
    {

    }
    protected override void DoMultiTarget()
    {
        
    }

    protected override void DoSingleTarget()
    {
        
    }
}