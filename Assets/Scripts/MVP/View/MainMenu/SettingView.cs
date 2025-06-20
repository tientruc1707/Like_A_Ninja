using UnityEngine;
using UnityEngine.UI;

public class ViewSetting : View
{
    [SerializeField] private Button _backButton;
    public override void Initialize()
    {
        _backButton.onClick.AddListener(() =>
        {
            UiManager.Show<MainMenuView>();
        });
    }
}
