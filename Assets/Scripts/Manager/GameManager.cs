
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameManager : Singleton<GameManager>
{
    private GameObject _currentCharacter;
    [SerializeField] private GameObject[] Characters;
    private int keyCharacterIndex = 0;

    public void SetCharacterKey(string name)
    {
        for (int i = 0; i < Characters.Length; i++)
        {
            if (Characters[i].name.Equals(name))
            {
                keyCharacterIndex = i;
                SaveSystem.Instance.SetCharacterKey(keyCharacterIndex);
            }
        }
    }

    public void SetMainCharacter(int index, Vector3 position)
    {
        if (_currentCharacter != null)
        {
            Destroy(_currentCharacter);
        }
        _currentCharacter = Instantiate(Characters[index], position, Quaternion.identity);
    }

    public GameObject[] GetCharacters()
    {
        return Characters;
    }

    public GameObject GetMainCharacter()
    {
        return _currentCharacter;
    }

    public GameObject SetRandomEnemy(Vector3 position)
    {
        GameObject enemy = Characters[Random.Range(0, Characters.Length)];
        Instantiate(enemy, position, Quaternion.identity);
        return enemy;
    }
}
