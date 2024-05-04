using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataSaver
{
    // Data Saver class adapted from Rob Reddick's work on Apply To The Industry Simulator

    public static void SaveData(Dictionary<string, int> damageByPlayer)
    {
        List<string> damageStats = new();
        foreach(KeyValuePair<string, int> kvp in damageByPlayer)
        {
            DamageByPlayer dbp = new();
            dbp.name = kvp.Key;
            dbp.damage = kvp.Value;
            string jsonData = JsonUtility.ToJson(dbp, true);
            damageStats.Add(jsonData);
        }
        
        string hashId = GenerateHash();

        // Get file path and push all content to json file
        string filePath = Application.dataPath + "/Data/" + hashId + "-damage-by-player.json";

        // Get the file directory
        string dirPath = System.IO.Path.GetDirectoryName(filePath);

        // Create the file if it doesn't exist
        if (!System.IO.Directory.Exists(dirPath))
        {
            System.IO.Directory.CreateDirectory(dirPath);
        }

        System.IO.File.WriteAllLines(filePath, damageStats);
        Debug.Log("Everything is cool");
    }

    private static string GenerateHash()
    {
        Guid hash = Guid.NewGuid();
        return hash.ToString();
    }
}

[System.Serializable]
public struct CombatStatsTracker
{
    public int dealtDamage;
    public int receivedDamage;
    public int missedHits;
}

[System.Serializable]
public struct DamageByPlayer
{
    public string name;
    public int damage;
}
