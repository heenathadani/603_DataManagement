using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cutscene")]
public class Cutscene : ScriptableObject
{
    public CutsceneActionData[] CutsceneData;
}