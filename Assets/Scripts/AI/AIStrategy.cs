using System.Collections.Generic;
using Unity.VisualScripting;

namespace Combatant
{
    public enum AITypes
    {
        Support,
        Disabler,
        Bruiser,
        ThreatHunter
    }

    public static class AIFactory
    {
        public static ICombatStrategy MakeCombatStrategy(AITypes type)
        {
            switch (type)
            {
                case AITypes.Disabler:
                    return new DisablerStrategy();
                case AITypes.Bruiser:
                    return new BruiserStrategy();
                case AITypes.ThreatHunter:
                    return new ThreatHunterStrategy();
                default:
                    return new SupportStrategy();
            }
        }
    }

    public interface ICombatStrategy
    {
        public aCombatant PickTarget();
    }

    public abstract class aAIStrategy : ICombatStrategy {
        protected abstract int CalculateThreat();
        public abstract aCombatant PickTarget();
    }

    public class SupportStrategy : aAIStrategy
    {
        protected override int CalculateThreat()
        {
            
        }

        public override aCombatant PickTarget()
        {
            aCombatant finalTarget = null;
            List<aCombatant> sameSideTargets = new List<aCombatant>();
            foreach(aCombatant possibleTarget in CombatantData.enemies)
            {
                if (possibleTarget.isAlive())
                {
                    sameSideTargets.Add(possibleTarget);
                }
            }

            List<aCombatant> otherSideTargets = new List<aCombatant>();
            foreach(aCombatant possibleTarget in CombatantData.partyCharacters)
            {
                if (possibleTarget.isAlive())
                {
                    otherSideTargets.Add(possibleTarget);
                }
            }



            return finalTarget;
        }
    }

    public class BruiserStrategy : aAIStrategy
    {
        public override aCombatant PickTarget()
        {
            aCombatant target = null;
            return target;
        }
    }

    public class ThreatHunterStrategy : aAIStrategy
    {
        public override aCombatant PickTarget()
        {
            aCombatant target = null;
            return target;
        }
    }

    public class DisablerStrategy : aAIStrategy
    {
        public override aCombatant PickTarget()
        {
            aCombatant target = null;

            return target;
        }
    }
}
