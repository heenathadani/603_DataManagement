using System;
using System.Collections.Generic;

namespace Combatant
{
    public enum AITypes
    {
        Basic,
        Bruiser
    }

    public static class AIFactory
    {
        public static ICombatStrategy MakeCombatStrategy(AITypes type)
        {
            switch (type)
            {
                case AITypes.Bruiser:
                    return new BruiserStrategy();
                default:
                    return new BasicStrategy();
            }
        }
    }

    public interface ICombatStrategy
    {
        public aCombatAction PickAction(aCombatant target);
        public aCombatant PickTarget();
        public BodyPartType PickPart(aCombatant target);
        public void SetCombatant(aCombatant me);
        public Power PickPower(aCombatant targt, CombatActionTargets targetType);
    }

    public abstract class aAIStrategy : ICombatStrategy {
        protected aCombatant myself;
        protected abstract float CalculateThreat(aCombatant candidate);
        protected abstract float CalculatePartThreat(BodyPart part);

        protected abstract CombatActionTypes PickPreferredAction(aCombatant target);
        protected abstract CombatActionTargets PickActionTarget( CombatActionTypes type);
        protected abstract float EvaluatePower(aCombatant target, Power p);
        public Power PickPower(aCombatant target, CombatActionTargets targetType)
        {
            Power bestPower = null;
            float maxPowerValue = -1;
            List<Power> candidatePowers = myself.FilterPowerByTargetType(targetType);
            foreach (Power candidate in candidatePowers)
            {
                float candidateValue = EvaluatePower(target, candidate);
                if (candidateValue > maxPowerValue)
                {
                    maxPowerValue = candidateValue;
                    bestPower = candidate;
                }
            }
            return bestPower;
        }

        public aCombatAction PickAction(aCombatant target)
        {
            CombatActionTypes bestAction = PickPreferredAction(target);
            CombatActionTargets bestTarget = PickActionTarget(bestAction);
            aCombatAction chosenAction = CombatActionFactory.MakeAction(bestAction, bestTarget);
            chosenAction.SetActingAgent(myself);
            return chosenAction;
        }

        public BodyPartType PickPart(aCombatant target)
        {
            float maxThreat = -1;
            BodyPartType finalType = BodyPartType.Body;

            foreach(KeyValuePair<BodyPartType, BodyPart> kvp  in target._equipment)
            {
                float currentThreat = CalculatePartThreat(kvp.Value);
                if (currentThreat > maxThreat)
                {
                    maxThreat = currentThreat;
                    finalType = kvp.Key;
                }
            }
            return finalType;
        }
        public aCombatant PickTarget()
        {
            float maxThreat = -1;
            aCombatant finalTarget = null;
            List<aCombatant> possibleTargets = GetPossibleTargets();

            foreach (aCombatant target in possibleTargets)
            {
                float currentThreat = CalculateThreat(target);
                if (currentThreat > maxThreat)
                {
                    maxThreat = currentThreat;
                    finalTarget = target;
                }
            }

            return finalTarget;
        }
        protected List<aCombatant> GetPossibleTargets()
        {
            List<aCombatant> possibleTargets = new List<aCombatant>();
            foreach (aCombatant possibleTarget in CombatantData.enemies)
            {
                if (possibleTarget.isAlive())
                {
                    possibleTargets.Add(possibleTarget);
                }
            }
            foreach (aCombatant possibleTarget in CombatantData.partyCharacters)
            {
                if (possibleTarget.isAlive())
                {
                    possibleTargets.Add(possibleTarget);
                }
            }
            return possibleTargets;
        }
        public void SetCombatant(aCombatant me)
        {
            myself = me;
        }
    }

    public class BasicStrategy : aAIStrategy
    {
        protected override CombatActionTypes PickPreferredAction(aCombatant target)
        {
            if (target == null)
            {
                return CombatActionTypes.Defend;
            }
            return CombatActionTypes.Attack;
        }
        protected override float EvaluatePower(aCombatant target, Power p)
        {
            float value = 0;
            return value;
        }
        protected override CombatActionTargets PickActionTarget(CombatActionTypes type)
        {
            if (type == CombatActionTypes.Defend)
            {
                return CombatActionTargets.Self;
            }
            return CombatActionTargets.SingleAllyBodyPart;
        }
        protected override float CalculatePartThreat(BodyPart part)
        {
            // Pick a random body part
            return UnityEngine.Random.Range(0.0f, 1.0f) * 10;
        }

        protected override float CalculateThreat(aCombatant candidate)
        {
            // Pick a random target
            if (candidate.GetSide() == CombatantType.ENEMIES)
            {
                // Ignore other enemies
                return -1;
            }
            return UnityEngine.Random.Range(0.0f, 1.0f) * 10;

        }
    }

    // The bruiser looks for either easy kills or dudes it can hurt the most badly.
    // It also gives preference to powers that hurt the entire party
    public class BruiserStrategy : aAIStrategy
    {
        protected override float EvaluatePower(aCombatant target, Power p)
        {
            float value = 0;
            return value;
        }
        protected override CombatActionTypes PickPreferredAction(aCombatant target)
        {
            if (target == null)
            {
                return CombatActionTypes.Defend;
            }
            return CombatActionTypes.Attack;
        }
        protected override CombatActionTargets PickActionTarget(CombatActionTypes type)
        {
            if (type == CombatActionTypes.Defend)
            {
                return CombatActionTargets.Self;
            }
            if (type == CombatActionTypes.Power)
            {
                CombatActionTargets mostLikelyTarget = CombatActionTargets.SingleAllyBodyPart;
                foreach(Power p in myself._powers)
                {
                    if (p.targetType == CombatActionTargets.AllAllies)
                    {
                        mostLikelyTarget = CombatActionTargets.AllAllies;
                    } else if (p.targetType == CombatActionTargets.SingleAlly && mostLikelyTarget != CombatActionTargets.AllAllies)
                    {
                        mostLikelyTarget = CombatActionTargets.SingleAlly;
                    }
                }
                return mostLikelyTarget;
            }
            return CombatActionTargets.SingleAllyBodyPart;
        }
        protected override float CalculatePartThreat(BodyPart part)
        {
            return 10 * (1 - (part.bodyPartStats.remainingHealth));
        }
        protected override float CalculateThreat(aCombatant candidate)
        {
            float result = 0;
            if (candidate.GetSide() == CombatantType.ENEMIES)
            {
                return -200;
            }

            result += 10 * (1 - candidate.GetStatValue(StatType.Armor));
            return result;
        }
    }
}
