using UnityEngine;

// Interim logic for UI. This will likely become deprecated or adapted once Annie and Heena unify their UI.
public class TargetButton : MonoBehaviour
{
    public int targetIndex;
    public CombatActionTargets type;
    
    public void DoAction()
    {
        if(type == CombatActionTargets.SingleEnemyBodyPart)
        {
            Debug.Log("On Click Index" + targetIndex.ToString());
            GetComponentInParent<CombatManager>().SetPartIndex(targetIndex);
        } else
        {
            GetComponentInParent<CombatManager>().SetTargetUnit(targetIndex);
        }
        
    }
}