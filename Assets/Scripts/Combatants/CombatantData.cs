using System.Collections.Generic;
using UnityEngine;

public static class CombatantData
{

    public static List<Combatant.aCombatant> partyCharacters = new List<Combatant.aCombatant>();
    public static EnemyFormation enemyCombatFormation = null;
    public static List<Combatant.aCombatant> enemies = new List<Combatant.aCombatant>();


    // Store player inventory -- Rin
/*    public static List<BodyPart> playerHeadInventory = new List<BodyPart>();
    public static List<BodyPart> playerArmInventory = new List<BodyPart>();
    public static List<BodyPart> playerBodyInventory = new List<BodyPart>();
    public static List<BodyPart> playerLegInventory = new List<BodyPart>();*/

    public static Dictionary<BodyPartType, List<BodyPart>> playerInventory = new Dictionary<BodyPartType, List<BodyPart>>();

    //Check player index
    public static int currentPlayerIndex = 0;

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