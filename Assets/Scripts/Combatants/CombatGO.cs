
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
        public override void ReceiveHit()
        {
            
        }
    }
}


