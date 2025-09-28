using UnityEngine;
using UnityEngine.UI;


public class MainMenuView : View
{
    [Header("Buttons")]
    [SerializeField] private Button _chosePlayingTypeButton;
    [SerializeField] private Button _helpButton;
    [SerializeField] private Button _choseCharacterButton;
    [SerializeField] private Button _settingButton;


    public override void Initialize()
    {
        _chosePlayingTypeButton.onClick.AddListener(() =>
        {
            UiManager.Show<ChosingPlayingTypeView>();
        });

        _helpButton.onClick.AddListener(() =>
        {
            UiManager.Show<HelpingView>();
        });

        _choseCharacterButton.onClick.AddListener(() =>
        {
            UiManager.Show<CharacterSelectingView>();
        });

        _settingButton.onClick.AddListener(() =>
        {
            UiManager.Show<ViewSetting>();
        });

    }

    public void Start()
    {
        AudioManager.Instance.PlayBackgroundSound(StringConstant.SoundName.BACKGROUND.MAIN_THEME);
        UiManager.Instance.OnSceneLoaded();
        UiManager.Instance.RegisterStartingView(this);
        GameManager.Instance.SetCharacterKey("Uchiha Itachi");
        GameManager.Instance.SetPlayer(SaveSystem.Instance.GetCharacterKey(), new Vector3(0, 3, 0));
    }
}
