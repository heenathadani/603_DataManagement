using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LoadStatScene : MonoBehaviour
{
    public GameObject statScene;
    public GameObject playerSlotPrefab; // Assign this in Unity Editor
    public Transform playerSlotContainer; // The UI panel where player slots should be shown
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            statScene.SetActive(true);
            LoadStatScreen();
        }
    }
    public void LoadStatScreen()
    {
        InstantiatePlayerSlots();
    }

    private void InstantiatePlayerSlots()
    {
        foreach (var player in CombatantData.GetGroup(Combatant.CombatantType.ALLIES))
        {
            var slotInstance = Instantiate(playerSlotPrefab, playerSlotContainer);
            slotInstance.GetComponent<PlayerSlot>().SetCombatant(player);
        }
    }
}
