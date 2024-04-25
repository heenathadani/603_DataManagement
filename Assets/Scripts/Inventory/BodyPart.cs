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
    public BodyPartStats bodyPartStats;

    public BodyPart(BodyPartData bodyPart)
    {
        bodyPartData = bodyPart;
        bodyPartStats = bodyPart.Clone();
    }
    
    public BodyPart Clone()
    {
        BodyPart bp = new BodyPart(bodyPartData);
        return bp;
    }
}
