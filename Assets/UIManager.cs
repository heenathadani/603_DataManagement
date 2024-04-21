using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject interactionUI;
    public GameObject dialogueUI;

    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueContent;
    public TextMeshProUGUI option1Content;
    public TextMeshProUGUI option2Content;
    public GameObject option1Button;
    public GameObject option2Button;
    public GameObject continueButton;
    


    PlayerMovement playerMovement;

    //Manage dialogue
    [HideInInspector]
    public List<string> currentDialogueList;
    private int currentDialogueIndex;
    [HideInInspector]
    public List<string> currentOptionList;



    private void Start()
    {
        interactionUI.SetActive(false);
        dialogueUI.SetActive(false);

        playerMovement = GetComponent<PlayerMovement>();
    }
    public void ShowInteractionHint()
    {
        interactionUI.SetActive(true);
    }

    public void HideInteractionHint()
    {
        interactionUI.SetActive(false);
    }

    public void ShowDialogue(string _characterName)
    {
        interactionUI.SetActive(false);
        playerMovement.moveSpeed = 0f;
        currentDialogueIndex = 0;
        dialogueUI.SetActive(true);
        characterName.text = _characterName;
        UpdateDialogue();
    }

    public void HideDialogue()
    {
        dialogueUI.SetActive(false);
        playerMovement.moveSpeed = 5f;
    }

    public void UpdateDialogue()
    {
        if(currentDialogueIndex != currentDialogueList.Count - 1)
        {
            continueButton.SetActive(true);
            option1Button.SetActive(false);
            option2Button.SetActive(false);
            dialogueContent.text = currentDialogueList[currentDialogueIndex];
            currentDialogueIndex++;
        }
        else
        {
            continueButton.SetActive(false);
            option1Button.SetActive(true);
            option2Button.SetActive(true);
            dialogueContent.text = currentDialogueList[currentDialogueIndex];
            option1Content.text = currentOptionList[0];
            option2Content.text = currentOptionList[1];
        }
    }

}
