using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    UIManager uiManager;
    bool ifPlayerInRange;

    public string characterName;
    public List<string> dialogueList;
    public List<string> optionList;
    

    private void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && ifPlayerInRange)
        {
            //Starts dialogue
            uiManager.currentDialogueList = dialogueList;
            uiManager.currentOptionList = optionList;
            uiManager.ShowDialogue(characterName);             
        }
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            uiManager.ShowInteractionHint();
            ifPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            uiManager.HideInteractionHint();
            ifPlayerInRange = false;
        }
    }
}
