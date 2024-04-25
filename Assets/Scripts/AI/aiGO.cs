using Combatant;
using UnityEngine;

public class EnemyGameObject : aCombatObject
{
    CombatManager mng;
    ICombatStrategy strategy;
    public int enemyIndex;

    public void TakeTurn()
    {
        // An enemy turn should be

        strategy.SetCombatant(_combatantData);
        // Pick who I want to target
        CombatTarget targetInformation = new();
        aCombatant target = strategy.PickTarget();

        targetInformation.actingUnit = _combatantData;
        targetInformation.targetUnit = target;
        targetInformation.sideBeingTargeted = target.GetSide();

        // Pick what I want to do
        aCombatAction action = strategy.PickAction(target);
        targetInformation.typeOfTarget = action.GetActionTarget();
        if (action.type == CombatActionTypes.Power)
        {
            targetInformation.selectedPower = strategy.PickPower(target, targetInformation.typeOfTarget);
        }

        // Pick a part if necessary
        if (targetInformation.typeOfTarget == CombatActionTargets.SingleEnemyBodyPart || targetInformation.typeOfTarget == CombatActionTargets.SingleAllyBodyPart)
        {
            targetInformation.partType = strategy.PickPart(target);
        }
        
        // Do the thing
        mng.SetAIAction(action);
        mng.SetAICombatTarget(targetInformation);
        mng.ExecuteCombatAction();
        mng.AITurnEnd();
    }

    public void SetUpStrategy()
    {
        Enemy e = (Enemy)_combatantData;
        strategy = AIFactory.MakeCombatStrategy(e.aiType);
    }

    public void SetManager(CombatManager mg)
    {
        mng = mg;
    }

    public override void ReceiveHit()
    {

    }
}