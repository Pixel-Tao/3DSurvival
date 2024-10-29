using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakePhysicalDamage(int damage);
}

public class PlayerCondition : MonoBehaviour, IDamageable
{
    public event Action onTakeDamage;

    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }
    Condition mana { get { return uiCondition.mana; } }

    public float noHungerDamage = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);
        mana.Add(mana.passiveValue * Time.deltaTime);

        if (hunger.curValue <= 0)
        {
            health.Subtract(noHungerDamage * Time.deltaTime);
        }
        if (health.curValue <= 0)
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

    public void ManaRegen(float amount)
    {
        mana.Add(amount);
    }

    public void StaminaRegen(float amount)
    {
        stamina.Add(amount);
    }

    private void Die()
    {
        Debug.Log("Player is dead");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
        if (health.curValue <= 0)
        {
            Die();
        }
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

    public bool UseMana(float amount)
    {
        if (mana.curValue - amount < 0)
        {
            return false;
        }

        mana.Subtract(amount);
        return true;
    }
}
