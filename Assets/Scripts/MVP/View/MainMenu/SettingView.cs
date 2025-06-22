using UnityEngine;
using UnityEngine.UI;

public class ViewSetting : View
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;


    public override void Initialize()
    {
        _backButton.onClick.AddListener(() =>
        {
            UiManager.ShowLast();
        });
    }

    private void Update()
    {
        if (_musicVolumeSlider != null)
        {
            AudioManager.Instance.SetBackgroundVolume(_musicVolumeSlider.value);
        }

        if (_sfxVolumeSlider != null)
        {
            AudioManager.Instance.SetSFXVolume(_sfxVolumeSlider.value);
        }
        
    }
}
