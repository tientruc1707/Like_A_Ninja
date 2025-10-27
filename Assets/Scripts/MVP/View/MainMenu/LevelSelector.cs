using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Button[] buttons;
    [SerializeField] private Button _backButton;
    public int unlockedLevel;

    void Start()
    {
        _backButton.onClick.AddListener(() => { UiManager.Instance.LoadScene(StringConstant.SCENE.MAIN_MENU); });
        unlockedLevel = SaveSystem.Instance.GetLevel(); // Highest unlocked level
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i + 1;
            // If the button index + 1 is greater than the unlocked level, lock the UI
            if (index > unlockedLevel)
            {
                //Lock's UI
                buttons[i].transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                buttons[i].onClick.AddListener(() => LoadLevel($"Level {index}"));
                if (index == unlockedLevel)
                {
                    //Unlock's UI
                    buttons[i].transform.GetChild(1).gameObject.SetActive(true);
                    buttons[i].GetComponentInChildren<Text>().text = $"{index}";
                }
                else
                {
                    //Passing's UI
                    buttons[i].transform.GetChild(2).gameObject.SetActive(true);
                    buttons[i].GetComponentInChildren<Text>().text = $"{index}";
                }
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].name = $"Level {i + 1}";
        }
    }

    private void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }


}
