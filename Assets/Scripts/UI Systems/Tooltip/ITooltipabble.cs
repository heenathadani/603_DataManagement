using UnityEngine;

public interface ITooltippable
{
    public void Interact(Vector2 screenPos);
    public void Hide();
}