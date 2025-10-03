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
        GameObject board = GameObject.Find("Board");
        _restartButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            UiManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
        });
        _resumeButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            board.SetActive(true);
            UiManager.ShowLast();
        });
        _mainMenuButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            UiManager.Instance.LoadScene(StringConstant.SCENE.PVE_MODE);
        });
    }
}
