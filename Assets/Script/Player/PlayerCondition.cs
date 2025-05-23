using System;
using UnityEngine;
public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount);
}
public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecay;
    public float noHungerStaminaDecay;
    public event Action onTakeDamage;

    private void Update()
    {
        if (hunger.curValue <= 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
            stamina.Subtract(noHungerStaminaDecay * Time.deltaTime);
        }

        if (health.curValue <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void Die()
    {
        Debug.Log("죽어버리다니 한심하구나.");
    }
    public void TakePhysicalDamage(int damageAmount)
    {
        health.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }
    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0)
        {
            return false;
        }
        stamina.Subtract(amount);
        return true;
    }
    public bool UseSprint(float amount)
    {
        if (stamina.curValue <= amount/10)
        {
            return false;
        }
        stamina.SlowSubtract(amount);
        return true;
    }
}