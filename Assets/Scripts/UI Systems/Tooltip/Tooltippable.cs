using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltippable : MonoBehaviour, ITooltippable
{
    EventSystem eventSys;
    public string tooltipText;
    private Tooltip tooltip;
    int UILayer;

    private void OnEnable()
    {
        eventSys = EventSystem.current;
        UILayer = LayerMask.NameToLayer("UI");
    }

    public void Interact(Vector2 screenPos)
    {
        CombatUIManager uiManager = GetComponentInParent<CombatUIManager>();
        tooltip = uiManager.tooltip;
        tooltip.Show(screenPos, tooltipText);
    }

    public void Update()
    {
        if (eventSys.IsPointerOverGameObject())
        {
            List<RaycastResult> results = GetEventSystemRaycastResults();
            for (int index = 0; index < results.Count; index++)
            {
                RaycastResult curResult = results[index];
                if (curResult.gameObject.layer == UILayer && curResult.gameObject == gameObject)
                {
                    Interact(curResult.screenPosition);
                }
            }
        } else
        {
            Hide();
        }
    }

    public void Hide()
    {
        if (tooltip != null)
        {
            tooltip.Hide();
        }

    }

    private List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventdata = new PointerEventData(EventSystem.current);
        eventdata.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventdata, results);
        return results;
    }
}