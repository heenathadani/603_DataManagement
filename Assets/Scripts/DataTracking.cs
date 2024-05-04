using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTracking : MonoBehaviour
{
    private Dictionary<string, int> damageByPlayer = new Dictionary<string, int>();

    public void DealDamage(Combatant.aCombatant player, int damageAmount)
    {
        Debug.Log("Tracking damage");
        // Track damage by player
        if (damageByPlayer.ContainsKey(player.Name))
        {
            damageByPlayer[player.Name] += damageAmount;
        }
        else
        {
            damageByPlayer[player.Name] = damageAmount;
        }
    }

    public void Save()
    {
        Debug.Log("Saving to file");
        DataSaver.SaveData(damageByPlayer);
    }

    public void DisplayDamageStats()
    {
        foreach (var entry in damageByPlayer)
        {
            Debug.Log(entry.Key + " dealt " + entry.Value + " damage.");
        }
    }
}
