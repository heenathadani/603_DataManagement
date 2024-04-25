
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public Vector3 offSet;
    public TextMeshProUGUI tooltipText;
    public void SetLocation(Vector3 location)
    {
        RectTransform rt = GetComponent<RectTransform>();
        Vector3 offsetTrick = offSet;
        if (location.y > 0)
        {
            offsetTrick *= -1;
        }
        rt.localPosition = location + offsetTrick;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(Vector2 screenPos, string text)
    {
        gameObject.SetActive(true);
        Resolution res = Screen.currentResolution;
        Vector3 loc = new Vector3(screenPos.x - res.width * 0.5f, screenPos.y - res.height * 0.5f, 1);
        transform.SetAsLastSibling();
        SetLocation(loc);
        tooltipText.text = text;
    }
}