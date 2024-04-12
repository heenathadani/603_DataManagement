using UnityEngine;

public class TargetButton : MonoBehaviour
{
    public int targetIndex;
    public CombatActionTargets type;
    
    public void DoAction()
    {
        if(type == CombatActionTargets.SingleEnemyBodyPart)
        {
            GetComponentInParent<CombatManager>().SetPartIndex(targetIndex);
        } else
        {
            GetComponentInParent<CombatManager>().SetTargetUnit(targetIndex);
        }
        
    }
}