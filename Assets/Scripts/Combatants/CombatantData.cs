using System.Collections.Generic;

public static class CombatantData
{

    public static List<Combatant.aCombatant> partyCharacters = new List<Combatant.aCombatant>();
    public static EnemyFormation enemyCombatFormation = null;
    public static List<Combatant.aCombatant> enemies = new List<Combatant.aCombatant>();


    // Store player inventory -- Rin
    public static List<BodyPart> playerHeadInventory = new List<BodyPart>();
    public static List<BodyPart> playerArmInventory = new List<BodyPart>();
    public static List<BodyPart> playerBodyInventory = new List<BodyPart>();
    public static List<BodyPart> playerLegInventory = new List<BodyPart>();

    //Check player index
    public static int currentPlayerIndex = 0;

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
}