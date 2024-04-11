public enum ActionTypes
{
    Attack,
    Defend,
    Power,
    Item,
    Escape
}

public enum ActionTargets
{
    Self,
    SingleEnemy,
    SingleAlly,
    AllEnemies,
    AllAllies
}

public static class ActionFactory
{
    public static aAction MakeAction(ActionTypes type, ActionTargets target)
    {
        aAction result;
        switch (type)
        {
            case ActionTypes.Attack:
                result = new AttackAction();
                break;
            case ActionTypes.Defend:
                result = new DefendAction();
                break;
            case ActionTypes.Power:
                result = new PowerAction();
                break;
            case ActionTypes.Item:
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

public interface IAction
{
    public void SetTargetType(ActionTargets targetType);
    public void DoAction();
}

public abstract class aAction : IAction
{
    protected ActionTargets _targetType;

    protected abstract void DoSingleTarget();
    protected abstract void DoMultiTarget();
    protected abstract void DoSelf();
    public void DoAction()
    {
        if (_targetType == ActionTargets.Self)
        {
            DoSelf();
        } else if (_targetType == ActionTargets.SingleEnemy || _targetType == ActionTargets.SingleAlly)
        {
            DoSingleTarget();
        } else
        {
            DoMultiTarget();
        }
    }
    public void SetTargetType(ActionTargets targetType)
    {
        _targetType = targetType;
    }
}

public class AttackAction : aAction
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

public class DefendAction : aAction
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

public class PowerAction : aAction
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

public class ItemAction : aAction
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

public class EscapeAction : aAction
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