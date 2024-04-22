using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    private static int _currentLevel = 1; // Start at 1 to avoid the main menu
    public void LoadNextLevel()
    {
        _currentLevel++;
        SceneManager.LoadScene(_currentLevel);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(_currentLevel);
    }
    public void ClearData()
    {
        _currentLevel = 0;
    }
    public void LoadScene(int sceneIndex)
    {
        _currentLevel = sceneIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadMainMenu()
    {
        CombatantData.Reset();
        ExplorationData.Reset();
        SceneManager.LoadScene(0);
    }

    public void ReturnToExploration()
    {
        SceneManager.LoadScene(ExplorationData.GetLastExploredScene());
    }

    public static void StaticLoad(int sceneIndex)
    {
        _currentLevel = sceneIndex;
        // Same as Load Scene but can be done statically to avoid having to instance this object
        SceneManager.LoadScene(sceneIndex);
    }
    public static int GetCurrentScene()
    {
        // Returns the current scene as an index for marking where to return to.
        Scene scene = SceneManager.GetActiveScene();
        return scene.buildIndex;
    }
}
