
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyEntityUI : CombatEntityUI
{
    public GameObject enemySelectButton;
    public GameObject targetButtons;
    public override void HideButtons()
    {
        enemySelectButton.SetActive(false);
    }

    public override void ShowButtons()
    {
        enemySelectButton.SetActive(true);
    }

    public override void ShowOptions()
    {
        targetButtons.SetActive(true);
    }

    public override void HideOptions()
    {
        targetButtons.SetActive(false);
    }

    public override void Disable()
    {
        base.Disable();
    }

    public override void SetUp(int i)
    {
        enemySelectButton.GetComponent<ChooseEnemyButton>().unitIndex = i;
    }

    public Dictionary<BodyPartType, Button> GetButtons()
    {
        Dictionary<BodyPartType, Button> buttons = new Dictionary<BodyPartType, Button>();
        foreach (Transform child in targetButtons.transform)
        {
            TargetButton targetButton = child.gameObject.GetComponent<TargetButton>();
            buttons.Add(targetButton.partType,child.gameObject.GetComponent<Button>());
        }
        return buttons;
    }
}