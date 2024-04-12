using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Store fixed body part data -- Rin
[CreateAssetMenu]
public class BodyPartData : ScriptableObject
{

    public string bodyPartName;
    //Image show in inventory
    public Sprite bodyPartImage;

    //Stats
    public float maxHp;

    public float attackPoint;

    public float shieldPoint;


}
