using UnityEngine;
using UnityEngine.UI;

public class ViewChoseCharacter : View
{
    [SerializeField] private Button _backButton;
    public override void Initialize()
    {
        _backButton.onClick.AddListener(() =>
        {
            UiManager.ShowLast();
        });
    }
}
