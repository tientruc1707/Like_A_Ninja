using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FailureView : View
{
    [SerializeField] private Button _restartLevelButton;
    [SerializeField] private Button _mainMenuButton;



    public override void Initialize()
    {
        AudioManager.Instance.PlaySFX(StringConstant.SoundName.SFX.GAME_LOSE);

        _restartLevelButton.onClick.AddListener(() =>
        {
            UiManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
        });
        _mainMenuButton.onClick.AddListener(() =>
        {
            UiManager.Instance.LoadScene("MainMenu");
        });

    }

}
