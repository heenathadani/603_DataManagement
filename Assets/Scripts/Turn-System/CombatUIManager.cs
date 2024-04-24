using Combatant;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class CombatUIManager : MonoBehaviour
{
    public float playerUIOffset;
    public float enemyOffset;
    // This is dummy data - a better UI approach must be developed for this
    private List<CombatEntityUI> playerUIs;
    private List<CombatEntityUI> enemyUIs;

    //Store all character Hp Sliders
    public List<Slider> characterHpSliderList;
    public List<Slider> enemyHpSliderList;

    //Temp need to have ui system after playtest1 -- Rin
    public GameObject gameOverScreen;
    public GameObject victoryScreen;
    public Tooltip tooltip;

    public void Setup()
    {
        playerUIs = new List<CombatEntityUI>();
        enemyUIs = new List<CombatEntityUI>();
    }

    public void AddCombatEntityUI(CombatantType whichSide, CombatEntityUI whatToAdd)
    {
        if (whichSide == CombatantType.ALLIES)
        {
            RectTransform rt = whatToAdd.GetComponent<RectTransform>();
            float x = rt.localPosition.x + playerUIs.Count * playerUIOffset;
            playerUIs.Add(whatToAdd);
            // Place the UI thingy
            rt.localPosition = new Vector3(x, rt.localPosition.y, rt.localPosition.z);

        } else
        {
            RectTransform rt = whatToAdd.GetComponent<RectTransform>();
            whatToAdd.SetUp(enemyUIs.Count);
            float x = rt.localPosition.x + enemyUIs.Count * enemyOffset;
            enemyUIs.Add(whatToAdd);
            // Place the UI thingy
            rt.localPosition = new Vector3(x, rt.localPosition.y, rt.localPosition.z);
        }
    }

    // Temp for end screen in playtest 1
    public void ShowGameOver()
    {
        gameOverScreen.SetActive(true);
    }

    public void ShowVictory()
    {
        victoryScreen.SetActive(true);
    }

    public void ShowPartButtons(int enemy, aCombatant unit)
    {
        foreach(EnemyEntityUI enemyEntity in enemyUIs) { enemyEntity.HideOptions(); }
        EnemyEntityUI entityUI = (EnemyEntityUI)enemyUIs[enemy];
        entityUI.ShowOptions();
        Dictionary<BodyPartType, Button> buttonOptions = entityUI.GetButtons();
        foreach(KeyValuePair<BodyPartType, Button> kvp in buttonOptions)
        {
            if (!unit._equipment[kvp.Key].bodyPartStats.alive)
            {
                kvp.Value.gameObject.SetActive(false);
            }
        }
    }

    public void HidePartButtons()
    {
        foreach(CombatEntityUI ui in enemyUIs)
        {
            ui.HideOptions();
        }
    }
    
    public void ShowEnemies()
    {
        List<aCombatant> enemies = CombatantData.GetGroup(CombatantType.ENEMIES);
        foreach(Enemy enemy in enemies)
        {
            if (enemy.isAlive())
            {
                enemy.combatantUI.ShowButtons();
            }
        }
    }

    public void HideAll()
    {
        foreach(CombatEntityUI ui in enemyUIs)
        {
            ui.HideButtons();
            ui.HideOptions();
        }
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
                playerUIs[0].ShowButtons();
                playerUIs[1].HideButtons();
                playerUIs[2].HideButtons();
                break;

            case 1:
                playerUIs[0].HideButtons();
                playerUIs[1].ShowButtons();
                playerUIs[2].HideButtons();
                break;

            case 2:
                playerUIs[0].HideButtons();
                playerUIs[1].HideButtons();
                playerUIs[2].ShowButtons();
                break;

            default:
                playerUIs[0].HideButtons();
                playerUIs[1].HideButtons();
                playerUIs[2].HideButtons();

                break;
        }
    }
}