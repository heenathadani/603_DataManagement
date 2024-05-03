using Combatant;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class CombatUIManager : MonoBehaviour
{
    public float playerUIOffset;
    public float enemyOffset;
    CombatManager mng;

    // This is dummy data - a better UI approach must be developed for this
    private List<CombatEntityUI> playerUIs;

    //Temp need to have ui system after playtest1 -- Rin
    public GameObject gameOverScreen;
    public GameObject victoryScreen;
    public GameObject inventoryScreen;
    public Tooltip tooltip;
    private bool uiVisibleLastFrame = false;
    private bool uiVisible = false;
    private Vector3 hoveredEnemyLocation;
    private int hoveredEnemyIndex;
    private GameObject createdUI;
    public GameObject EnemyUIPrefab;
    Coroutine buttonCoroutine;
    public TextMeshProUGUI lootNameList;
    

    // Animations for the buttons. Work is underway. - Ed
    private IEnumerator ExpandButtons()
    {
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator CollapseButtons()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(createdUI);
        createdUI = null;
    }


    public void Setup(CombatManager mng)
    {
        // Avoid race conditions
        playerUIs = new List<CombatEntityUI>();
        this.mng = mng;
    }

    private void Update()
    {
        if (uiVisible != uiVisibleLastFrame)
        {
            TriggerEnemyUIStateChange(uiVisible);
        }

        uiVisibleLastFrame = uiVisible;
    }

    private void FixedUpdate()
    {
        if (mng == null || (mng._activeType != CombatantType.ALLIES || !mng.InTargetState()))
        {
            uiVisible = false;
            return;
        }

        // Check to see if the player is mousing over an enemy
        Vector3 eyePosition = Camera.main.transform.position;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("Enemy");
        if (Physics.Raycast(eyePosition, ray.direction, out hit, 15, layerMask))
        {
            // Show the UI if they are
            uiVisible = true;
            hoveredEnemyLocation = hit.collider.gameObject.transform.position;
            EnemyGameObject aiGO = hit.collider.gameObject.GetComponent<EnemyGameObject>();
            hoveredEnemyIndex = aiGO.enemyIndex;
        } else
        {
            uiVisible = false;
        }
    }

    private void TriggerEnemyUIStateChange(bool isVisible)
    {
        // This function triggers the appearance of the Enemy UI. 
        // Rather than having to constantly repurpose a UI element, it is being created on the fly and set up.
        // Performance impacts are minimal.

        if (!isVisible)
        {
            if (buttonCoroutine != null)
            {
                StopCoroutine(buttonCoroutine);
                buttonCoroutine = StartCoroutine(CollapseButtons());
            } else
            {
                Destroy(createdUI);

            }
            return;
        }

        // Emergency stop if the player is moving too fast
        if (createdUI != null && buttonCoroutine != null)
        {
            StopCoroutine(buttonCoroutine);
            Destroy(createdUI);
            createdUI = null;
        }

        // Create the EnemyEntityUI and set it up
        createdUI = Instantiate(EnemyUIPrefab, transform);
        RectTransform rt = createdUI.GetComponent<RectTransform>();
        Vector2 screenPos = Camera.main.WorldToScreenPoint(hoveredEnemyLocation);
        rt.localPosition = new Vector3(screenPos.x-(Screen.width*0.5f)+enemyOffset, screenPos.y-(Screen.height*0.5f), 0);
        EnemyEntityUI enemyEntityUI = createdUI.GetComponent<EnemyEntityUI>();
        enemyEntityUI.SetUp(hoveredEnemyIndex);
        CombatantData.enemies[hoveredEnemyIndex].combatantUI = enemyEntityUI;
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

    public void HideAll()
    {
        // This would be a good place to refresh the UI and do other shenanigans
        // Keeping it empty until my next PR. - Ed
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


    public void ShowInventory()
    {
        inventoryScreen.SetActive(true);
    }

}