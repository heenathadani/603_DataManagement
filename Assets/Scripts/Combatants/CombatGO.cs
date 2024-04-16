
using UnityEngine;

namespace Combatant
{
    public abstract class aCombatObject : MonoBehaviour
    {
        protected aCombatant _combatantData;
        protected bool _isActive = false;

        // Animation Handle
        public abstract void ReceiveHit();
        public void SetCombatant(aCombatant data)
        {
            _combatantData = data;
        }

        public void SetAsActive(bool active)
        {
            _isActive = active;
        }
    }

    public class PlayerCombatObject : aCombatObject
    {
        public override void ReceiveHit()
        {
            throw new System.NotImplementedException();
        }
    }

    public class EnemyGameObject : aCombatObject
    {
        AITypes AIType;
        ICombatStrategy strategy;

        public void TakeTurn()
        {
            // An enemy turn should be

            strategy.SetCombatant(_combatantData);
            // Pick who I want to target
            CombatTarget targetInformation = new();
            aCombatant target = strategy.PickTarget();
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
                targetInformation.targetIndex = strategy.PickPartIndex(target);
            }

            // Do the thing
            CombatManager cm = GetComponentInParent<CombatManager>();
            cm.SetCombatTarget(targetInformation);
            cm.ExecuteCombatAction();
            cm.AITurnEnd();
        }

        private void OnEnable()
        {
            strategy = AIFactory.MakeCombatStrategy(AIType);
        }

        public override void ReceiveHit()
        {
            
        }
    }
}


