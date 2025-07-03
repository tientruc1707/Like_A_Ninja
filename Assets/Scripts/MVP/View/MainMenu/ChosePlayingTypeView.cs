using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChosingPlayingTypeView : View
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _singlePlayerButton;
    [SerializeField] private Button _multiPlayerButton;


    public override void Initialize()
    {
        _backButton.onClick.AddListener(() =>
        {
            UiManager.ShowLast();
        });
        _singlePlayerButton.onClick.AddListener(() =>
        {
            UiManager.Instance.LoadScene(StringConstant.SCENE.PVE_MODE);
        });
        _multiPlayerButton.onClick.AddListener(() =>
        {
            UiManager.Instance.LoadScene(StringConstant.SCENE.PVP_MODE);
        });
    }
}
