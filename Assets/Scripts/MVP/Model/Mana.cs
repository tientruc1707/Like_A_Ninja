using System;
using UnityEngine;

public class Mana : MonoBehaviour
{
    public event Action OnManaChanged;

    private float _currentMana;
    private float _maxMana;

    public float CurrentMana { get => _currentMana; set => _currentMana = value; }
    public float MaxMana { get => _maxMana; set => _maxMana = value; }

    public void IncreamentMana(float value)
    {
        _currentMana += value;
        _currentMana = Math.Clamp(_currentMana, 0, _maxMana);
        UpdateMana();
    }

    public void DecreamentMana(float value)
    {
        _currentMana -= value;
        _currentMana = Math.Clamp(_currentMana, 0, _maxMana);
        UpdateMana();
    }

    public void UpdateMana()
    {
        OnManaChanged?.Invoke();
    }

}
