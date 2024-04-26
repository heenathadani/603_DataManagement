using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{

    InventoryUIManager inventoryUIManager;

    public int itemId;

    public void Start()
    {
        inventoryUIManager = FindAnyObjectByType<InventoryUIManager>();
    }

    public void OnClick()
    {
        inventoryUIManager.ChangeBodyPart(itemId);
    }
}
