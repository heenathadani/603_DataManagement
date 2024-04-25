// This class is an adaptation of Taode Ogden's Ability class
// Original code can be found here https://github.com/NoyaCai1110/603Game4/blob/main/Assets/Scripts/Abilities/Ability.cs

using Combatant;
using UnityEngine;

public enum PowerCondition
{
    isDead,
    isAlive,
    bodyPartIsActive
}

[CreateAssetMenu(menuName = "ScriptableObjects/Power")]
public class Power : ScriptableObject
{
    // Target information
    [Tooltip("Who can this power target")]
    public CombatActionTargets targetType;

    [Tooltip("If this affects a bodyPart, which one")]
    public BodyPartType bodyPart;


    // Text Information
    [Tooltip("What name will be displayed to the player")]
    public string powerName;

    [Tooltip("More information that will be shown to the player")]
    public string description;


    // Effect variables
    [Tooltip("How powerful is the effect of this power")]
    public float effectModifier;

    [Tooltip("What gives this power a bigger effect")]
    public StatType statAugmentingThisPower;

    [Tooltip("What does this power affect")]
    public StatType statAffectedByPower;


    // What needs to be available
    [Tooltip("How much does this power cost to use")]
    public float cost;

    [Tooltip("What conditions must the target meet for this power to be able to target them")]
    public PowerCondition targetRequirement;
    
    public float GetTotalEffect(aCombatant who)
    {
        return 0.0f;
    }
}