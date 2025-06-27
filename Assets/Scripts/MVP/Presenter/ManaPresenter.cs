using UnityEngine;
using UnityEngine.UI;

public class ManaPresenter : MonoBehaviour
{
    [SerializeField] private Mana _mana;
    [SerializeField] private Slider _slider;
    public CharacterData character;

    private void Start()
    {
        _mana.MaxMana = character.Mana;
        _mana.CurrentMana = character.Mana;

        _slider.maxValue = character.Mana;

        _mana.OnManaChanged += UpdateManaView;
        UpdateManaView();
    }

    private void OnDisable()
    {
        _mana.OnManaChanged -= UpdateManaView;
    }

    public void IncreaseMana(float value)
    {
        _mana.IncreamentMana(value);
    }

    public void DecreaseMana(float value)
    {
        _mana.IncreamentMana(value);
    }

    private void UpdateManaView()
    {
        if (_slider != null)
            _slider.value = _mana.CurrentMana;
    }
}
