using UnityEngine;
using UnityEngine.UI;

public class ManaPresenter : MonoBehaviour
{
    private Mana _mana;
    private Slider _slider;
    public CharacterData character;

    private void Start()
    {
        _mana = GetComponent<Mana>();
        _mana.MaxMana = character.Mana;
        _mana.CurrentMana = 0;

        _slider.maxValue = character.Mana;

        _mana.OnManaChanged += UpdateManaView;
        UpdateManaView();
    }

    public void SetSlider(Slider slider)
    {
        _slider = slider;
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
