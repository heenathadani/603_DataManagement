using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlot : MonoBehaviour
{
    public GameObject actionOne;
    public GameObject actionTwo;
    public GameObject actionThree;
    public GameObject actionFour;
    public GameObject FillStrengthOne;
    public GameObject FillStrengthTwo;
    public GameObject FillStrengthThree;
    public GameObject FillStrengthFour;
    BodyPartType bodyPartType;
    BodyPartStats stats;
    public void SetCombatant(Combatant.aCombatant player)
    {
        // Set the name of the player in the slot
        transform.Find("PlayerName").GetComponent<TMPro.TextMeshProUGUI>().text = player.Name;
        actionOne.GetComponent<TMPro.TextMeshProUGUI>().text = BodyPartType.Head.ToString();
        actionTwo.GetComponent<TMPro.TextMeshProUGUI>().text = BodyPartType.Arm.ToString();
        actionThree.GetComponent<TMPro.TextMeshProUGUI>().text = BodyPartType.Leg.ToString();
        actionFour.GetComponent<TMPro.TextMeshProUGUI>().text = BodyPartType.Body.ToString();
        FillStrengthOne.GetComponent<Slider>().value = player._equipment[BodyPartType.Head].bodyPartStats.effectValue;
        FillStrengthTwo.GetComponent<Slider>().value = player._equipment[BodyPartType.Arm].bodyPartStats.effectValue;
        FillStrengthThree.GetComponent<Slider>().value = player._equipment[BodyPartType.Leg].bodyPartStats.effectValue;
        FillStrengthFour.GetComponent<Slider>().value = player._equipment[BodyPartType.Body].bodyPartStats.effectValue;
    }
}
