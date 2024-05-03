using Combatant;
using System.Collections.Generic;
using UnityEngine;

public static class CombatantData
{

    public static List<Combatant.aCombatant> partyCharacters = new List<Combatant.aCombatant>();
    public static EnemyFormation enemyCombatFormation = null;
    public static List<Combatant.aCombatant> enemies = new List<Combatant.aCombatant>();

    public static Dictionary<BodyPartType, List<BodyPart>> playerInventory = new Dictionary<BodyPartType, List<BodyPart>>();

    //Check player index
    public static int currentPlayerIndex = 0;

    //public static List<BodyPart> bodyPartsList = new List<BodyPart>();

    static CombatantData()
    {
        InitializeInventory();
    }


    public static void InitializeCharacters(List<BodyPartData> _bodyPartDataList)
    {
        CombatantData.partyCharacters.Add(new Protagonist("Test Protagonist 1", 0));
        CombatantData.partyCharacters.Add(new Protagonist("Test Protagonist 2", 1));
        CombatantData.partyCharacters.Add(new Protagonist("Test Protagonist 3", 2));

        // Temp, Check status update -- Rin
        Protagonist protagonist1 = CombatantData.partyCharacters[0] as Protagonist;
        if (protagonist1 != null && _bodyPartDataList.Count > 0)
        {
            protagonist1.Equip(new BodyPart(_bodyPartDataList[0]));
            protagonist1.Equip(new BodyPart(_bodyPartDataList[1]));
            protagonist1.Equip(new BodyPart(_bodyPartDataList[2]));
            protagonist1.Equip(new BodyPart(_bodyPartDataList[3]));
        }

        Protagonist protagonist2 = CombatantData.partyCharacters[1] as Protagonist;
        if (protagonist2 != null && _bodyPartDataList.Count > 0)
        {
            protagonist2.Equip(new BodyPart(_bodyPartDataList[0]));
            protagonist2.Equip(new BodyPart(_bodyPartDataList[1]));
            protagonist2.Equip(new BodyPart(_bodyPartDataList[2]));
            protagonist2.Equip(new BodyPart(_bodyPartDataList[3]));
        }

        Protagonist protagonist3 = CombatantData.partyCharacters[2] as Protagonist;
        if (protagonist3 != null && _bodyPartDataList.Count > 0)
        {
            protagonist3.Equip(new BodyPart(_bodyPartDataList[0]));
            protagonist3.Equip(new BodyPart(_bodyPartDataList[1]));
            protagonist3.Equip(new BodyPart(_bodyPartDataList[2]));
            protagonist3.Equip(new BodyPart(_bodyPartDataList[3]));
        }
    }
    public static void InitializeInventory()
    {
        playerInventory.Add(BodyPartType.Head, new List<BodyPart>());
        playerInventory.Add(BodyPartType.Arm, new List<BodyPart>());
        playerInventory.Add(BodyPartType.Body, new List<BodyPart>());
        playerInventory.Add(BodyPartType.Leg, new List<BodyPart>());
    }

    public static List<Combatant.aCombatant> GetGroup(Combatant.CombatantType whichOne)
    {
        switch (whichOne)
        {
            case Combatant.CombatantType.ALLIES:
                return partyCharacters;
            default:
                return enemies;
        }
    }

    public static Combatant.CombatantType GetNext(Combatant.CombatantType whosOnNow)
    {
        switch (whosOnNow)
        {
            case Combatant.CombatantType.ALLIES:
                return Combatant.CombatantType.ENEMIES;
            default:
                return Combatant.CombatantType.ALLIES;
        }
    }

    public static void SetFormation(EnemyFormation formation)
    {
        enemyCombatFormation = formation;
    }

    public static void Reset()
    {
        partyCharacters.Clear();
        enemies.Clear();
        enemyCombatFormation = null;
    }


    public static void AddBodyPartToPlayerInventory(BodyPart _bodyPart)
    {
        //Temp
        _bodyPart.SetbodyPartStats();


        playerInventory[_bodyPart.bodyPartStats.type].Add(_bodyPart);
    }
}