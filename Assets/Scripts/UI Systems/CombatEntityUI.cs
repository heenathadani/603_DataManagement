using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatEntityUI : MonoBehaviour
{
    public float moveSpeed;
    public Slider hpBar;
    public int damageNumberIterationCycles;
    public TextMeshProUGUI DamageNumbers;
    private Vector3 damageNumbersOriginalPosition;

    private void OnEnable()
    {
        damageNumbersOriginalPosition = DamageNumbers.rectTransform.localPosition;
    }

    public void DisplayDamage(int value)
    {
        DamageNumbers.gameObject.SetActive(true);
        if (value > 0)
        {
            DamageNumbers.color = Color.red;
        } else
        {
            DamageNumbers.color = Color.green;
        }
        DamageNumbers.text = Mathf.Abs(value).ToString();
        StartCoroutine(FloatDamageNumber());
    }

    protected IEnumerator FloatDamageNumber()
    {
        for(int i = 0; i < damageNumberIterationCycles; i++)
        {
            float x = DamageNumbers.rectTransform.localPosition.x;
            float z = DamageNumbers.rectTransform.localPosition.z;
            float y = damageNumbersOriginalPosition.y + moveSpeed * i;
            DamageNumbers.rectTransform.localPosition = new Vector3(x, y, z);
            yield return new WaitForSeconds(0.1f);
        }
        // Reset after animation is over
        DamageNumbers.rectTransform.localPosition = damageNumbersOriginalPosition;
        DamageNumbers.gameObject.SetActive(false);
    }

    public void UpdateHPBar(float value)
    {
        hpBar.value = value;
    }

    public virtual void ShowButtons() { }
    public virtual void HideButtons() { }
    public virtual void ShowOptions() { }
    public virtual void HideOptions() { }
    public virtual void Disable(){}
    public virtual void SetUp(int i) { }
}
