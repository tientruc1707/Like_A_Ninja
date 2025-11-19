using UnityEngine;
using UnityEngine.UI;

public class PvPMode : MonoBehaviour
{
    [SerializeField] private Button _backButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _backButton.onClick.AddListener(() =>
        {
            UiManager.Instance.LoadScene(StringConstant.SCENE.MAIN_MENU);
        });
    }

}
