using UnityEngine;

public static class ExplorationData
{
    static Vector3 playerLocation;
    static int lastSavedExplorationScene;

    // Save player location
    public static void SavePlayerLocation(Vector3 location)
    {
        playerLocation = location;
    }

    public static Vector3 LoadPlayerLocation()
    {
        Vector3 loc = playerLocation;
        playerLocation = Vector3.zero;
        return loc;
    }

    
    // Exploration scene index methods
    public static void SaveExplorationSceneIndex(int index)
    {
        lastSavedExplorationScene = index;
    }

    public static int GetLastExploredScene()
    {
        return lastSavedExplorationScene;
    }

    // Reset this static object
    public static void Reset()
    {
        playerLocation = Vector3.zero;
        lastSavedExplorationScene = 0;
    }
}