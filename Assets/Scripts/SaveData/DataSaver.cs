using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataSaver
{
    // Data Saver class adapted from Rob Reddick's work on Apply To The Industry Simulator
    
    /// <summary>
    /// Saves incoming data to a JSON file
    /// </summary>
    public static void SaveData(CombatStatsTracker combatData)
    {
        // Convert data into json string and push to list
        string jsonData = JsonUtility.ToJson(combatData, true);
        string hashId = GenerateHash();

        // Get file path and push all content to json file
        string filePath = Application.dataPath + "/Data/" + hashId + ".json";

        // Get the file directory
        string dirPath = System.IO.Path.GetDirectoryName(filePath);

        // Create the file if it doesn't exist
        if (!System.IO.Directory.Exists(dirPath))
        {
            System.IO.Directory.CreateDirectory(dirPath);
        }

        System.IO.File.WriteAllLines(filePath, jsonData);
    }

    private static string GenerateHash()
    {
        Guid test = Guid.NewGuid();
        return test.ToString();
    }
}

[System.Serializable]
public struct CombatStatsTracker
{
    public int dealtDamage;
    public int receivedDamage;
    public int missedHits;
}
