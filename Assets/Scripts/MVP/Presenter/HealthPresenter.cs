using UnityEngine;
using UnityEngine.UI;

public class HealthPresenter : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Slider healthSlider;

    private void Start()
    {
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
