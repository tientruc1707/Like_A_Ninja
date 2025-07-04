using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectingView : View
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _selectButton;
    [SerializeField] private Transform _characterContainer;
    private GameObject[] characters;


    public override void Initialize()
    {
        _backButton.onClick.AddListener(() =>
        {
            UiManager.ShowLast();
        });
    }

    void Start()
    {
        characters = GameManager.Instance.GetCharacters();

        foreach (var character in characters)
        {
            Button option = Instantiate(_selectButton);
            option.transform.SetParent(_characterContainer);
            option.name = character.GetComponent<CharacterPresenter>().CharacterName;
            option.GetComponent<Image>().sprite = character.GetComponent<CharacterPresenter>().CharacterSprite;
            option.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            option.onClick.AddListener(() => OnCharacerSelected(option.name));
            option.gameObject.SetActive(true);
            //add lock button if character is locked
        }

    }

    public void OnCharacerSelected(string name)
    {
        foreach (var character in characters)
        {
            if (character.name.Equals(name))
            {
                GameManager.Instance.SetCharacterKey(name);
                GameManager.Instance.SetPlayer(SaveSystem.Instance.GetCharacterKey(), new Vector3(0, 3, 0));
                break;
            }

        }

    }
}
