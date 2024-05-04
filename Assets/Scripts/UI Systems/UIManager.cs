using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject interactionUI;
    public GameObject dialogueUI;

    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueContent;
    public TextMeshProUGUI option1Content;
    public TextMeshProUGUI option2Content;
    public TextMeshProUGUI option3Content;
    public GameObject option1Button;
    public GameObject option2Button;
    public GameObject option3Button;
    public GameObject continueButton;
    public GameObject inventoryUI;
    


    PlayerMovement playerMovement;

    //Manage dialogue
    [HideInInspector]
    public List<string> currentDialogueList;
    private int currentDialogueIndex;
    [HideInInspector]
    public List<string> currentOptionList;
    [HideInInspector]
    public List<string> currentDialogueAfterOptionList;
    [HideInInspector]
    public EnemyFormation currentEnemyFormation;
    [HideInInspector]
    public int combatOptionId;
    [HideInInspector]
    public int combatOptionId2;
    [HideInInspector]
    public GameObject currentTalkingNPC;
    [HideInInspector]
    public bool ifFinishDialogue;
    [HideInInspector]
    public bool ifCombat;




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
        if(currentDialogueIndex != currentDialogueList.Count - 1 && !ifFinishDialogue)
        {
            continueButton.SetActive(true);
            option1Button.SetActive(false);
            option2Button.SetActive(false);
            option3Button.SetActive(false);
            dialogueContent.text = currentDialogueList[currentDialogueIndex];
            currentDialogueIndex++;
        }
        else if(!ifFinishDialogue)
        {
            continueButton.SetActive(false);
            option1Button.SetActive(true);
            option2Button.SetActive(true);
            option3Button.SetActive(true);
            dialogueContent.text = currentDialogueList[currentDialogueIndex];
            option1Content.text = currentOptionList[0];
            option2Content.text = currentOptionList[1];
            option3Content.text = currentOptionList[2];
            ifFinishDialogue = true;
        }
        else
        {
            if(ifCombat)
            {
                Debug.Log(currentEnemyFormation);
                CombatantData.SetFormation(currentEnemyFormation);

                ExplorationData.SavePlayerLocation(playerMovement.gameObject.transform.position);


                SceneManager.LoadScene("Level-1");
                DestroyCurrentNPC();
            }
            else
            {
                HideDialogue();
                DestroyCurrentNPC();
            }
        }
    }


    public void GoToEndDialogue(int buttonId)
    {
        // Combat! -- Rin
        if (buttonId == combatOptionId || buttonId == combatOptionId2)
        {
            ifCombat = true;

        }
        // The player is good to go -- Rin
        else
        {
            ifCombat = false;
        }

        dialogueContent.text = currentDialogueAfterOptionList[buttonId];
        option1Button.SetActive(false);
        option2Button.SetActive(false);
        option3Button.SetActive(false);
        continueButton.SetActive(true);
    }


    public void DestroyCurrentNPC()
    {
        ExplorationDataManager explorationDataManager = FindAnyObjectByType<ExplorationDataManager>();
        ExplorationData.aliveEnemy[explorationDataManager.npcList.IndexOf(currentTalkingNPC)] = false;
        Destroy(currentTalkingNPC);
    }


    public void ShowInventory()
    {
        inventoryUI.SetActive(true);
    }


}
