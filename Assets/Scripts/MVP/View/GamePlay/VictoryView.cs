using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryView : View
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _nextLevelButton;

    private void Start()
    {
        SaveSystem.Instance.SetLevel(SaveSystem.Instance.GetLevel() + 1);
    }
    public override void Initialize()
    {
        _mainMenuButton.onClick.AddListener(() =>
        {
            UiManager.Instance.LoadScene("MainMenu");
        });
        _nextLevelButton.onClick.AddListener(() =>
        {
            UiManager.Instance.OnSceneUnloaded();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
    }
}
