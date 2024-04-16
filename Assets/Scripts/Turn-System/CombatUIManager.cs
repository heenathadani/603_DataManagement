using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// All of this will probably need to be reworked. TBH, this is all a hack while the UI is being designed by Heena and Annie.
public class CombatUIManager : MonoBehaviour
{
    // This is dummy data - a better UI approach must be developed for this
    public List<Button> enemy1PartButtons;
    public List<Button> enemy2PartButtons;
    public List<Button> enemy3PartButtons;
    public List<Button> enemies;

    public List<Button> character1ActionButtons;
    public List<Button> character2ActionButtons;
    public List<Button> character3ActionButtons;

    //Store all character Hp Sliders
    public List<Slider> characterHpSliderList;
    public List<Slider> enemyHpSliderList;

    public void ShowPartButtons(int enemy)
    {
        if (enemy == 0)
        {
            foreach(Button b in enemy1PartButtons)
            {
                b.gameObject.SetActive(true);
            }
            foreach(Button b in enemy2PartButtons)
            {
                b.gameObject.SetActive(false);
            }
            foreach (Button b in enemy3PartButtons)
            {
                b.gameObject.SetActive(false);
            }
        } 
        else if(enemy == 1)
        {
            foreach (Button b in enemy1PartButtons)
            {
                b.gameObject.SetActive(false);
            }
            foreach (Button b in enemy2PartButtons)
            {
                b.gameObject.SetActive(true);
            }
            foreach (Button b in enemy3PartButtons)
            {
                b.gameObject.SetActive(false);
            }
        }
        else if (enemy == 2)
        {
            foreach (Button b in enemy1PartButtons)
            {
                b.gameObject.SetActive(false);
            }
            foreach (Button b in enemy2PartButtons)
            {
                b.gameObject.SetActive(false);
            }
            foreach (Button b in enemy3PartButtons)
            {
                b.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("NO!!!! Enemy Index is not available");
        }
    }

    public void HidePartButtons()
    {
        foreach (Button b in enemy1PartButtons)
        {
            b.gameObject.SetActive(false);
        }
        foreach (Button b in enemy2PartButtons)
        {
            b.gameObject.SetActive(false);
        }
        foreach (Button b in enemy3PartButtons)
        {
            b.gameObject.SetActive(false);
        }
    }
    
    public void ShowEnemies()
    {
        foreach(Button b in enemies)
        {
            b.gameObject.SetActive(true);
        }
    }

    public void HideAll()
    {
        foreach(Button b in enemies)
        {
            b.gameObject.SetActive(false);
        }
        HidePartButtons();
    }

    public void ShowByTarget(CombatActionTargets targetType)
    {
        switch (targetType)
        {
            case CombatActionTargets.SingleEnemy:
                ShowEnemies();
                return;
            case CombatActionTargets.SingleEnemyBodyPart:
                ShowEnemies();
                return;
            default:
                return;
        }
    }

    public void ShowPlayerActionButtons(int playerIndex)
    {
        switch(playerIndex)
        {
            case 0:
                foreach (Button b in character1ActionButtons)
                {
                    b.gameObject.SetActive(true);
                }
                foreach (Button b in character2ActionButtons)
                {
                    b.gameObject.SetActive(false);
                }
                foreach (Button b in character3ActionButtons)
                {
                    b.gameObject.SetActive(false);
                }
                break;

            case 1:
                foreach (Button b in character1ActionButtons)
                {
                    b.gameObject.SetActive(false);
                }
                foreach (Button b in character2ActionButtons)
                {
                    b.gameObject.SetActive(true);
                }
                foreach (Button b in character3ActionButtons)
                {
                    b.gameObject.SetActive(false);
                }
                break;

            case 2:
                foreach (Button b in character1ActionButtons)
                {
                    b.gameObject.SetActive(false);
                }
                foreach (Button b in character2ActionButtons)
                {
                    b.gameObject.SetActive(false);
                }
                foreach (Button b in character3ActionButtons)
                {
                    b.gameObject.SetActive(true);
                }
                break;

            default:
                // Just hide all buttons -- Rin
                foreach (Button b in character1ActionButtons)
                {
                    b.gameObject.SetActive(false);
                }
                foreach (Button b in character2ActionButtons)
                {
                    b.gameObject.SetActive(false);
                }
                foreach (Button b in character3ActionButtons)
                {
                    b.gameObject.SetActive(false);
                }

                break;
        }
    }
}