using System.Collections.Generic;
using Unity.VisualScripting;

namespace Combatant
{
    public enum AITypes
    {
        Support,
        Disabler,
        Bruiser,
        MageHunter
    }

    public static class AIFactory
    {
        public static ICombatStrategy MakeCombatStrategy(AITypes type)
        {
            switch (type)
            {
                case AITypes.Disabler:
                    break;
                case AITypes.Bruiser:
                    break;
                case AITypes.MageHunter:
                    break;
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
        public abstract aCombatant PickTarget();
    }

    public class SupportStrategy : aAIStrategy
    {

        public override aCombatant PickTarget()
        {
            aCombatant finalTarget;
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
}
