using UnityEngine;
using UnityEngine.UI;

public class HealthPresenter : MonoBehaviour
{
    private Health _health;
    private Slider _healthSlider;

    public CharacterData character;
    private void Start()
    {
        _health = GetComponent<Health>();

        _health.MaxHealth = character.Health;
        _health.CurrentHealth = character.Health;

        _healthSlider.maxValue = _health.MaxHealth;

        _health.OnHealthChanged += UpdateHealthView;
        UpdateHealthView();
    }

    public float GetCurrentHealth()
    {
        return _health.CurrentHealth;
    }
    
    public void SetSlider(Slider slider)
    {
        _healthSlider = slider;
    }

    public void IncreaseHealth(float amount)
    {
        _health.IncreamentHealth(amount);
    }

    public void DecreaseHealth(float amount)
    {
        _health.DecreamentHealth(amount);
    }

    public void RegenerateHealth()
    {
        _health.RegenHealth(_health.MaxHealth);
    }

    private void UpdateHealthView()
    {
        if (_healthSlider != null)
        {
            _healthSlider.value = _health.CurrentHealth;
        }
    }

}
