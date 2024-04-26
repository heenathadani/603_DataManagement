using UnityEngine;

public class TargetButton : MonoBehaviour
{
    public BodyPartType partType;
    public CombatActionTargets actionTarget;
    
    public void DoAction()
    {
        EnemyEntityUI entityUI = GetComponentInParent<EnemyEntityUI>();
        CombatManager cm = GetComponentInParent<CombatManager>();
        aCombatAction action = CombatActionFactory.MakeAction(CombatActionTypes.Attack, actionTarget);
        
        // Set the target information
        CombatTarget targetInformation = new();
        targetInformation.typeOfTarget = actionTarget;
        targetInformation.sideBeingTargeted = Combatant.CombatantType.ENEMIES;
        targetInformation.targetIndex = entityUI.enemyIndex;
        targetInformation.targetUnit = CombatantData.enemies[entityUI.enemyIndex];
        targetInformation.partType = partType;

        int playerIndex = cm._currentTurn;
        cm.BeginTurn(playerIndex, action, targetInformation);
    }
}