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

    public Image headEquipImage;
    public Image armEquipImage;
    public Image bodyEquipImage;
    public Image legEquipImage;

    public TextMeshProUGUI headEquipText;
    public TextMeshProUGUI armEquipText;
    public TextMeshProUGUI bodyEquipText;
    public TextMeshProUGUI legEquipText;




    [HideInInspector]
    public BodyPartType currentViewBodyType;
    [HideInInspector]
    public int currentCharacterId = 0;


    public BodyPart tempBodyPartHead;
    public BodyPart tempBodyPartHead2;
    public BodyPart tempBodyPartArm;
    public BodyPart tempBodyPartBody;
    public BodyPart tempBodyPartLeg;

    private void OnEnable()
    {
        //Reset bar to head
        currentViewBodyType = BodyPartType.Head;
        UpdateInventoryScollView();
        UpdateCharacterEquipment();
    }

    private void Awake()
    {
        CombatantData.InitializeInventory();


        //Temp-test: Add body part to the inventory
        CombatantData.AddBodyPartToPlayerInventory(tempBodyPartHead);
        CombatantData.AddBodyPartToPlayerInventory(tempBodyPartHead2);
        CombatantData.AddBodyPartToPlayerInventory(tempBodyPartArm);
        CombatantData.AddBodyPartToPlayerInventory(tempBodyPartBody);
        CombatantData.AddBodyPartToPlayerInventory(tempBodyPartLeg);
        CombatantData.AddBodyPartToPlayerInventory(tempBodyPartHead);
        CombatantData.AddBodyPartToPlayerInventory(tempBodyPartArm);
        CombatantData.AddBodyPartToPlayerInventory(tempBodyPartBody);
        CombatantData.AddBodyPartToPlayerInventory(tempBodyPartLeg);

/*        tempBodyPartArm.bodyPartData.Clone();
        tempBodyPartBody.bodyPartData.Clone();
        tempBodyPartHead.bodyPartData.Clone();
        tempBodyPartLeg.bodyPartData.Clone();*/
    }

    //Temp test function
    private void Start()
    {
        UpdateInventoryScollView();
        UpdateCharacterEquipment();
        Debug.Log(CombatantData.partyCharacters[currentCharacterId]._equipment.Count);
    }


    public void ChangeBodyPart(int changeBodyPartId)
    {

        CombatantData.partyCharacters[currentCharacterId].Equip(CombatantData.playerInventory[currentViewBodyType][changeBodyPartId]);


        //Debug.Log("CurrentBodyPart" + CombatantData.partyCharacters[currentCharacterId]._equipment[BodyPartType.Head].bodyPartStats.partName);

        Debug.Log(CombatantData.partyCharacters[currentCharacterId]._equipment.Count);
        if(CombatantData.partyCharacters[currentCharacterId]._equipment[BodyPartType.Head].bodyPartData)
        {
            //Debug.Log("null!");
        }
        CombatantData.playerInventory[currentViewBodyType].RemoveAt(changeBodyPartId);


        UpdateInventoryScollView();
        UpdateCharacterEquipment();
    }


    // For buttons
    public void UpdateHeadScollView()
    {
        currentViewBodyType = BodyPartType.Head;
        UpdateInventoryScollView();
    }
    public void UpdateArmScollView()
    {
        currentViewBodyType = BodyPartType.Arm;
        UpdateInventoryScollView();
    }
    public void UpdateBodyScollView()
    {
        currentViewBodyType = BodyPartType.Body;
        UpdateInventoryScollView();
    }
    public void UpdateLegScollView()
    {
        currentViewBodyType = BodyPartType.Leg;
        UpdateInventoryScollView();
    }

    public void SwitchCharacter(int id)
    {
        currentCharacterId = id;
        UpdateCharacterEquipment();
    }

    public void UpdateInventoryScollView()
    {
        ClearInventoryScollView();
        int i = 0;
        //Debug.Log(CombatantData.playerInventory[currentViewBodyType].Count);

        foreach (BodyPart bp in CombatantData.playerInventory[currentViewBodyType])
        {
            GameObject newBodyPart = Instantiate(inventoryItemPrefab);
            newBodyPart.transform.SetParent(inventoryViewportContent.transform);
            newBodyPart.GetComponentInChildren<Image>().sprite = bp.bodyPartData.stats.image;
            newBodyPart.GetComponentInChildren<TextMeshProUGUI>().text = bp.bodyPartData.stats.partName;
            newBodyPart.GetComponent<InventoryItem>().itemId = i;
            i++;
        }
    }

    public void ClearInventoryScollView()
    {
        foreach(Transform t in inventoryViewportContent.transform)
        {
            Destroy(t.gameObject);
        }

    }

    public void UpdateCharacterEquipment()
    {
        if(currentCharacterId <= CombatantData.partyCharacters.Count - 1 && currentCharacterId >=0)
        {
            headEquipImage.sprite = CombatantData.partyCharacters[currentCharacterId]._equipment[BodyPartType.Head].bodyPartStats.image;
            armEquipImage.sprite = CombatantData.partyCharacters[currentCharacterId]._equipment[BodyPartType.Arm].bodyPartStats.image;
            bodyEquipImage.sprite = CombatantData.partyCharacters[currentCharacterId]._equipment[BodyPartType.Body].bodyPartStats.image;
            legEquipImage.sprite = CombatantData.partyCharacters[currentCharacterId]._equipment[BodyPartType.Leg].bodyPartStats.image;

            headEquipText.text = CombatantData.partyCharacters[currentCharacterId]._equipment[BodyPartType.Head].bodyPartStats.partName;
            armEquipText.text = CombatantData.partyCharacters[currentCharacterId]._equipment[BodyPartType.Arm].bodyPartStats.partName;
            bodyEquipText.text = CombatantData.partyCharacters[currentCharacterId]._equipment[BodyPartType.Body].bodyPartStats.partName;
            legEquipText.text = CombatantData.partyCharacters[currentCharacterId]._equipment[BodyPartType.Leg].bodyPartStats.partName;
        }

    }

}
