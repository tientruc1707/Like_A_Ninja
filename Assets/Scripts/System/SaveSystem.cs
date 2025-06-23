using UnityEngine;

public class SaveSystem : Singleton<SaveSystem>
{
    private const string LEVEL_KEY = "Level";
    private const string CHARACTER_KEY = "Player";

    override public void Awake()
    {
        base.Awake();
        if (PlayerPrefs.HasKey(LEVEL_KEY) == false)
        {
            PlayerPrefs.SetInt(LEVEL_KEY, 1);
        }
        if (PlayerPrefs.HasKey(CHARACTER_KEY) == false)
        {
            PlayerPrefs.SetInt(CHARACTER_KEY, 1);
        }
    }

    public int GetLevel()
    {
        return PlayerPrefs.GetInt(LEVEL_KEY);
    }

    public void SetLevel(int level)
    {
        PlayerPrefs.SetInt(LEVEL_KEY, level);
        PlayerPrefs.Save();
    }

    public int GetCharacterKey()
    {
        return PlayerPrefs.GetInt(CHARACTER_KEY);
    }

    public void SetCharacterKey(int keyNumber)
    {
        PlayerPrefs.SetInt(CHARACTER_KEY, keyNumber);
        PlayerPrefs.Save();
    }

}
