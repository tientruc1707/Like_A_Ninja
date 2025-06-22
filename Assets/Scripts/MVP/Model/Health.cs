using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public CharacterData characterData;
    public event Action OnHealthChanged;
    public float MaxHealth => characterData.Health;
    public float CurrentHealth { get; set; }

    public void IncreamentHealth(float amount)
    {
        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        UpdateHealth();
    }

    public void DecreamentHealth(float amount)
    {
        CurrentHealth -= amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        UpdateHealth();
    }

    public void RegenHealth(float amount)
    {
        CurrentHealth = MaxHealth;
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        OnHealthChanged?.Invoke();
    }

}
