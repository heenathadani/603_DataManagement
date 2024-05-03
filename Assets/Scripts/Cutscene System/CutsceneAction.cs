using UnityEngine;

[System.Serializable]
public struct CutsceneActionData
{
    // A data class that specifies what needs to happen when this part of a cutscene is invoked
    public CutsceneActionType type;
    public float duration;
    public string textContent;
    public Vector2 location;
    public aCutsceneActionBehavior behavior;

    public void SetBehavior()
    {
        behavior = CutsceneActionFactory.MakeCutsceneBehavior(this);
    }
}

public enum CutsceneActionType
{
    Wait,
    ToggleMusic,
    FadeIn,
    FadeOut,
    ShowFadeableText,
    ShowUnfadeableText
}

public static class CutsceneActionFactory
{
    // This produces the behavior class that informs the CutsceneData what it needs to do when called
    public static aCutsceneActionBehavior MakeCutsceneBehavior(CutsceneActionData data)
    {
        switch (data.type)
        {
            case CutsceneActionType.ToggleMusic:
                return new ToggleMusic(data);
            case CutsceneActionType.ShowFadeableText:
                return new ShowFadeableText(data);
            case CutsceneActionType.ShowUnfadeableText:
                return new ShowUnfadeableText(data);
            case CutsceneActionType.FadeIn:
                return new FadeInBehavior(data);
            case CutsceneActionType.FadeOut:
                return new FadeOutBehavior(data);
            default:
                return new WaitCutsceneBehavior(data);
        }
    }
}

public abstract class aCutsceneActionBehavior
{
    public abstract void PerformAction(CutsceneManager manager);
}

public class WaitCutsceneBehavior : aCutsceneActionBehavior
{
    private CutsceneActionData dataObject;

    public WaitCutsceneBehavior(CutsceneActionData data)
    {
        dataObject = data;
    }

    public override void PerformAction(CutsceneManager manager)
    {
        manager.StartWait(dataObject.duration);
    }
}

public class ShowFadeableText : aCutsceneActionBehavior
{
    private CutsceneActionData dataObject;
    public ShowFadeableText(CutsceneActionData data)
    {
        dataObject = data;
    }
    public override void PerformAction(CutsceneManager manager)
    {
        manager.AddTextToScreen(dataObject.textContent, dataObject.location, dataObject.duration, true);
    }
}

public class ShowUnfadeableText : aCutsceneActionBehavior
{
    private CutsceneActionData dataObject;
    public ShowUnfadeableText(CutsceneActionData data)
    {
        dataObject = data;
    }
    public override void PerformAction(CutsceneManager manager)
    {
        manager.AddTextToScreen(dataObject.textContent, dataObject.location, dataObject.duration, false);
    }
}

public class FadeInBehavior : aCutsceneActionBehavior
{
    private CutsceneActionData dataObject;
    public FadeInBehavior(CutsceneActionData data)
    {
        dataObject = data;
    }

    public override void PerformAction(CutsceneManager manager)
    {
        manager.StartOpacityChange(dataObject.duration, 1.0f);
    }
}

public class FadeOutBehavior : aCutsceneActionBehavior
{
    private CutsceneActionData dataObject;
    public FadeOutBehavior(CutsceneActionData data)
    {
        dataObject = data;
    }

    public override void PerformAction(CutsceneManager manager)
    {
        manager.StartOpacityChange(dataObject.duration, 0.0f);
    }
}

public class ToggleMusic : aCutsceneActionBehavior
{
    private CutsceneActionData dataObject;
    public ToggleMusic(CutsceneActionData data)
    {
        dataObject = data;
    }

    public override void PerformAction(CutsceneManager manager)
    {
        manager.ToggleMusic();
    }
}