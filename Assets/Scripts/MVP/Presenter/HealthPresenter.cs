using UnityEngine;
using UnityEngine.UI;

public class HealthPresenter : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Slider healthSlider;

    public CharacterData character;
    private void Start()
    {
        health.MaxHealth = character.Health;
        health.CurrentHealth = character.Health;

        healthSlider.maxValue = health.MaxHealth;
        
        health.OnHealthChanged += UpdateHealthView;
        UpdateHealthView();
    }

    private void OnDestroy()
    {
        health.OnHealthChanged -= UpdateHealthView;
    }

    public void IncreaseHealth(float amount)
    {
        health.IncreamentHealth(amount);
    }

    public void DecreaseHealth(float amount)
    {
        health.DecreamentHealth(amount);
    }

    public void RegenerateHealth()
    {
        health.RegenHealth(health.MaxHealth);
    }

    private void UpdateHealthView()
    {
        if (healthSlider != null)
        {
            healthSlider.value = health.CurrentHealth;
        }
    }

}
