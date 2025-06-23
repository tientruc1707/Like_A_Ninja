using System;
using UnityEngine;

public class Health : MonoBehaviour
{

    public event Action OnHealthChanged;
    private float maxHealth;
    private float currentHealth;

    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
    
    public void IncreamentHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealth();
    }

    public void DecreamentHealth(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealth();
    }

    public void RegenHealth(float amount)
    {
        currentHealth = maxHealth;
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        OnHealthChanged?.Invoke();
    }

}
