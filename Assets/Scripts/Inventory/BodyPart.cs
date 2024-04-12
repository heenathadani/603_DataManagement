using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Body Part -- Rin
public class BodyPart
{

    public BodyPartData bodyPartData;

    public float currentHp;

    public BodyPart(BodyPartData _bodypartdata)
    {
        bodyPartData = _bodypartdata;
        currentHp = _bodypartdata.maxHp;
    }

    public void UpdateCurrentHP(float value)
    {
        currentHp -= value;
    }

}