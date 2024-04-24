using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Store fixed body part data -- Rin
[CreateAssetMenu]
public class BodyPartData : ScriptableObject
{
    public BodyPartStats stats;

    public BodyPartStats Clone()
    {
        return new BodyPartStats(
            stats.effectValue, stats.image, stats.partName, stats.type
            );
    }
}
