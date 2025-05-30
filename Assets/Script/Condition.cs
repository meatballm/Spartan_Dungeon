using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float maxValue;
    public float startValue;
    public float passiveValue;
    public Image uiBar;

    private void Start()
    {
        curValue = startValue;
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage();
        curValue = Mathf.Max(Mathf.Min(curValue + passiveValue*Time.deltaTime, maxValue),0.0f);
    }

    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }
    public void SlowSubtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount * Time.deltaTime, 0.0f);
    }

    public float GetPercentage()
    {
        return curValue / maxValue;
    }
}