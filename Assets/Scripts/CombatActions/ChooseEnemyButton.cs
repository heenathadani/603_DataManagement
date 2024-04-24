using UnityEngine;

public class ChooseEnemyButton : MonoBehaviour
{
    public int unitIndex;

    public void DoAction()
    {
        GetComponentInParent<CombatManager>().SetTargetUnit(unitIndex);
    }
}