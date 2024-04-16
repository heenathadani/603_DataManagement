using System;

public enum BodyPartType
{
    Body,
    Arm,
    Leg,
    Head,
    Back
}

//Body Part -- Rin
[Serializable]
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

    public float GetMaxHp()
    {
        return bodyPartData.maxHp;
    }

}
