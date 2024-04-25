using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntityUI : CombatEntityUI
{
    public GameObject actionButtons;
    public override void HideButtons()
    {
        actionButtons.gameObject.SetActive(false);
    }

    public override void ShowButtons()
    {
        actionButtons.gameObject.SetActive(true);
    }

    public override void Disable()
    {
        foreach (Transform child in actionButtons.transform)
        {
            Button b = child.GetComponent<Button>();
            b.enabled = false;
        }
    }

    public override void SetUp(int i)
    {
        base.SetUp(i);
    }
}

