using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausingView : View
{
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    public override void Initialize()
    {
        _restartButton.onClick.AddListener(() =>
        {
            UiManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
        });
        _resumeButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            UiManager.ShowLast();
        });
        _mainMenuButton.onClick.AddListener(() =>
        {
            UiManager.Instance.LoadScene("MainMenu");
        });
    }
}
