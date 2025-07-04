using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class GameView : View
{
    [SerializeField] private Button _pauseButton;

    public override void Initialize()
    {
        _pauseButton.onClick.AddListener(() =>
        {
            Time.timeScale = 0;
            UiManager.Show<PausingView>();
        });

    }
    private void Start()
    {
        UiManager.Instance.OnSceneLoaded();
        UiManager.Instance.RegisterStartingView(this);
    }

}