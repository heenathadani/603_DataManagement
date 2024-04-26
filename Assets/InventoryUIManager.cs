using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    // Start is called before the first frame update


    public GameObject inventoryViewportContent;

    public GameObject inventoryItemPrefab;

    public BodyPartType currentView;

    private void OnEnable()
    {
        
    }



    public void ChangeBodyPart(int changeBodyPartId)
    {
        switch(currentView)
        {
            case BodyPartType.Head:
                CombatantData.playerHeadInventory.RemoveAt(changeBodyPartId);

                break;
            case BodyPartType.Arm:
                CombatantData.playerArmInventory.RemoveAt(changeBodyPartId);
                break;
            case BodyPartType.Body:
                CombatantData.playerBodyInventory.RemoveAt(changeBodyPartId);
                break;
            case BodyPartType.Leg:
                CombatantData.playerLegInventory.RemoveAt(changeBodyPartId);
                break;
        }

        UpdateInventoryScollView();
    }


    // For buttons
    public void UpdateHeadScollView()
    {
        currentView = BodyPartType.Head;
        UpdateInventoryScollView();
    }
    public void UpdateArmScollView()
    {
        currentView = BodyPartType.Arm;
        UpdateInventoryScollView();
    }
    public void UpdateBodyScollView()
    {
        currentView = BodyPartType.Body;
        UpdateInventoryScollView();
    }
    public void UpdateLegScollView()
    {
        currentView = BodyPartType.Leg;
        UpdateInventoryScollView();
    }

    public void UpdateInventoryScollView()
    {

        ClearInventoryScollView();
        int i = 0;
        //Set all the items in the inventory view
        switch (currentView)
        {
            
            case BodyPartType.Head:    
                foreach (BodyPart bp in CombatantData.playerHeadInventory)
                {
                    GameObject newBodyPart = Instantiate(inventoryItemPrefab);
                    newBodyPart.transform.SetParent(inventoryViewportContent.transform);
                    newBodyPart.GetComponentInChildren<Image>().sprite = bp.bodyPartData.bodyPartImage;
                    newBodyPart.GetComponentInChildren<TextMeshProUGUI>().text = bp.bodyPartData.bodyPartName;
                    newBodyPart.GetComponent<InventoryItem>().ItemId = i;
                    i++;
                }
                break;
            case BodyPartType.Arm:
                foreach (BodyPart bp in CombatantData.playerArmInventory)
                {
                    GameObject newBodyPart = Instantiate(inventoryItemPrefab);
                    newBodyPart.transform.SetParent(inventoryViewportContent.transform);
                    newBodyPart.GetComponentInChildren<Image>().sprite = bp.bodyPartData.bodyPartImage;
                    newBodyPart.GetComponentInChildren<TextMeshProUGUI>().text = bp.bodyPartData.bodyPartName;
                    newBodyPart.GetComponent<InventoryItem>().ItemId = i;
                    i++;
                }
                break;
            case BodyPartType.Body:
                foreach (BodyPart bp in CombatantData.playerBodyInventory)
                {
                    GameObject newBodyPart = Instantiate(inventoryItemPrefab);
                    newBodyPart.transform.SetParent(inventoryViewportContent.transform);
                    newBodyPart.GetComponentInChildren<Image>().sprite = bp.bodyPartData.bodyPartImage;
                    newBodyPart.GetComponentInChildren<TextMeshProUGUI>().text = bp.bodyPartData.bodyPartName;
                    newBodyPart.GetComponent<InventoryItem>().ItemId = i;
                    i++;
                }
                break;
            case BodyPartType.Leg:
                foreach (BodyPart bp in CombatantData.playerLegInventory)
                {
                    GameObject newBodyPart = Instantiate(inventoryItemPrefab);
                    newBodyPart.transform.SetParent(inventoryViewportContent.transform);
                    newBodyPart.GetComponentInChildren<Image>().sprite = bp.bodyPartData.bodyPartImage;
                    newBodyPart.GetComponentInChildren<TextMeshProUGUI>().text = bp.bodyPartData.bodyPartName;
                    newBodyPart.GetComponent<InventoryItem>().ItemId = i;
                    i++;
                }
                break;
        }
    }

    public void ClearInventoryScollView()
    {
        foreach(Transform t in inventoryViewportContent.transform)
        {
            Destroy(t.gameObject);
        }

    }
}
