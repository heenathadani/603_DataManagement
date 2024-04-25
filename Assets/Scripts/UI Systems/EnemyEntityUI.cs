
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Combatant;

public class EnemyEntityUI : CombatEntityUI
{
    public int enemyIndex;
    public GameObject targetButtons;
    private Dictionary<BodyPartType, Slider> hpSliders;
    public override void HideButtons()
    {
        targetButtons.SetActive(false);
    }

    public override void ShowButtons()
    {
        targetButtons.SetActive(true);
    }


    public override void Disable()
    {
        base.Disable();
    }

    public override void SetUp(int i)
    {
        hpSliders = new Dictionary<BodyPartType, Slider>();
        enemyIndex = i;
        aCombatant combatant = CombatantData.enemies[i];
        float totalHealth = 0.0f;
        foreach(Transform child in targetButtons.transform)
        {
            MovableButton targetButton = child.gameObject.GetComponent<MovableButton>();
            float remainingHealth = combatant._equipment[targetButton.button.partType].bodyPartStats.remainingHealth;
            totalHealth += remainingHealth;
            if (remainingHealth > 0)
            {
                targetButton.slider.value = remainingHealth;
                hpSliders.Add(targetButton.button.partType, targetButton.slider);
            } else
            {
                targetButton.gameObject.SetActive(false);
            }
        }
        hpBar.value = totalHealth / 4.0f;
    }
}