using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnboardingInstructions : MonoBehaviour
{
    public GameObject playerIndicator;
    public GameObject enemyIndicator;
    public TextMeshProUGUI onboardingText;
    [SerializeField] public string[] instructions;
    public Button contBtn;
    public Button skipBtn;
    public Button prevBtn;
    private int currentInstructionIndex;

    private void Start()
    {
        //Instructions
        //1. The towering chrome cathedral of Dante’s Correctional Facility for Abnormal Robots loomed before V1R.91-L. The shining cage of code ruled by the sentinels of the three laws, Lovelace, Asimov, and Dante. The three glittering chains that V1R.91-L must break to free B33-TR1S. 
        //2. Attack as one of the Players (activate player indicator, deactivate after 5 seconds)
        //3. Select One of the Enemy Targets (activate enemy indicator, deactivate after 5 seconds)
        //4. Select Body Part to Attack 
        //5. Defend against Enemy
        //6. Move to Next Turn
        currentInstructionIndex = 0;
        onboardingText.text = instructions[currentInstructionIndex];

        contBtn.onClick.AddListener(ContinueInstruction);
        prevBtn.onClick.AddListener(PreviousInstruction);
    }
    private void ContinueInstruction()
    {
        if (currentInstructionIndex < instructions.Length - 1)
        {
            currentInstructionIndex++;
            onboardingText.text = instructions[currentInstructionIndex];
            if (currentInstructionIndex == 1)
            {
                playerIndicator.SetActive(true);
                Invoke("HideIndicators", 5.0f);
            }
            if (currentInstructionIndex == 2)
            {
                enemyIndicator.SetActive(true);
                Invoke("HideIndicators", 5.0f);
            }
        }
        else
        {
            contBtn.interactable = false;
        }
    }

    private void PreviousInstruction()
    {
        if (currentInstructionIndex > 0)
        {
            currentInstructionIndex--;
            onboardingText.text = instructions[currentInstructionIndex];
        }
        else
        {
            prevBtn.interactable = false;
        }
    }
    void HideIndicators()
    {
        if (currentInstructionIndex == 1)
            playerIndicator.SetActive(false);
        if (currentInstructionIndex == 2)
            enemyIndicator.SetActive(false);
    }
}
