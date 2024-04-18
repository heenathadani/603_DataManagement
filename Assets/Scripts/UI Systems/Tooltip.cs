
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public Vector3 offSet;
    public TextMeshProUGUI tooltipText;
    public void SetLocation(RectTransform dependentObject)
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.localPosition = dependentObject.localPosition + offSet;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(RectTransform dependentObject, string text)
    {
        SetLocation(dependentObject);
        tooltipText.text = text;
    }
}