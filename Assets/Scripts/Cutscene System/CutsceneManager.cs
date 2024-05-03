using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class CutsceneManager : MonoBehaviour
{
    public List<Cutscene> loadedCutscenes;
    public Canvas canvas;
    public float horizontalMargins;
    public CanvasGroup canvasGroup;
    public bool waitingForInput = false;
    public TextMeshProUGUI cutsceneEndText;
    public AudioSource audioSource;

    private bool isPlayingMusic = false;
    private float opacityChangeSpeed;
    private float changeDuration;
    private float currentTransitionDuration;
    private float newOpacityTarget;
    private int actionIndex = 0;
    private Cutscene activeCutscene;
    private CutsceneActionData activeAction;


    private IEnumerator OpacityChange()
    {
        float direction = newOpacityTarget == 1.0 ? 1 : -1;
        
        while(currentTransitionDuration <= changeDuration)
        {
            canvasGroup.alpha += opacityChangeSpeed * direction;
            currentTransitionDuration += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        MoveToNextAction();
    }

    private IEnumerator FadeInText(TextMeshProUGUI text, Color c)
    {
        while(currentTransitionDuration <= changeDuration)
        {
            text.color = new Color(c.r, c.g, c.b, currentTransitionDuration / changeDuration);
            currentTransitionDuration += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        MoveToNextAction();
    }
    
    private IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
        MoveToNextAction();
    }

    private void Update()
    {
        if (!waitingForInput)
        {
            return;
        }


        // This could stand to be generalized. Right now I'm just moving to the next scene.
        if (Input.GetMouseButtonDown(0))
        {
            int currentScene = SceneManagers.GetCurrentScene();
            SceneManagers.StaticLoad(currentScene + 1);
        }
    }

    public void StartWait(float duration) {
        StartCoroutine(Wait(duration));
    }

    public void ToggleMusic()
    {
        if (!isPlayingMusic)
        {
            audioSource.Play();
            isPlayingMusic = true;
        } else
        {
            audioSource.Stop();
            isPlayingMusic = false;
        }
        MoveToNextAction();
    }

    public void AddTextToScreen(string content, Vector2 location, float duration, bool canBeFaded)
    {
        GameObject go = new GameObject();
        RectTransform rt = go.AddComponent<RectTransform>();
        TextMeshProUGUI text = go.AddComponent<TextMeshProUGUI>();
        text.text = content;
        Color c;
        changeDuration = duration;
        if (canBeFaded)
        {
            rt.SetParent(canvasGroup.transform);
            c = Color.white;
            c.a = 0;
            text.color = c;
        } else
        {
            rt.SetParent(canvas.transform);
            c = Color.red;
            c.a = 0;
            text.color = Color.red;
        }
        rt.localPosition = location;
        rt.sizeDelta = new Vector2(Screen.width - horizontalMargins, 50);

        StartCoroutine(FadeInText(text, c));
    }

    public void StartOpacityChange(float duration, float opacityTarget)
    {
        changeDuration = duration;
        newOpacityTarget = opacityTarget;
        opacityChangeSpeed = 1 / Mathf.Abs(duration);
        StartCoroutine(OpacityChange());
    }

    private void MoveToNextAction()
    {
        if (actionIndex == activeCutscene.CutsceneData.Length)
        {
            waitingForInput = true;
            cutsceneEndText.gameObject.SetActive(true);
        } else {
            currentTransitionDuration = 0.0f;
            activeAction = activeCutscene.CutsceneData[actionIndex++];
            activeAction.SetBehavior();
            activeAction.behavior.PerformAction(this);
        }
    }


    private void OnEnable()
    {
        activeCutscene = loadedCutscenes[0];
        MoveToNextAction();
    }
}