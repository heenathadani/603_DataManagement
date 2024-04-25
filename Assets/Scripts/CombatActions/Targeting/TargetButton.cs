using UnityEngine;

// Interim logic for UI. This will likely become deprecated or adapted once Annie and Heena unify their UI.
public class TargetButton : MonoBehaviour
{
    public BodyPartType partType;
    public CombatActionTargets type;
    
    public void DoAction()
    {
        EnemyEntityUI entityUI = GetComponentInParent<EnemyEntityUI>();

        if(type == CombatActionTargets.SingleEnemyBodyPart)
        {
            GetComponentInParent<CombatManager>().SetTarget(entityUI.enemyIndex, partType);
        }
    }
}