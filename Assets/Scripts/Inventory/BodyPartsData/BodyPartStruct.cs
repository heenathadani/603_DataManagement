using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BodyPartStats
{
    public float remainingHealth;
    public float effectValue;
    public Sprite image;
    public string partName;
    public BodyPartType type;
    public bool alive;

    public BodyPartStats(float effect, Sprite sprite, string name, BodyPartType partType)
    {
        remainingHealth = 1.0f;
        effectValue = effect;
        image = sprite;
        partName = name;
        type = partType;
        alive = true;
    }
}