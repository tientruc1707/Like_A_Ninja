using UnityEngine;

public enum TurnSide
{
    LEFTTURN,
    RIGHTTURN
}

public class GameManager : Singleton<GameManager>
{
    public static class AnimationState
    {
        public static readonly int Attack = Animator.StringToHash("Attack");
        public static readonly int MiniHurt = Animator.StringToHash("MiniHurt");
        public static readonly int BigHurt = Animator.StringToHash("BigHurt");

    }

    private GameObject _currentPlayer;
    private GameObject _currentEnemy;

    [SerializeField] private GameObject[] Characters;
    private int keyCharacterIndex = 0;
    public TurnSide CurrentSide { get; set; }

    private void Start()
    {
        CurrentSide = TurnSide.LEFTTURN;
    }
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

    public void SetPlayer(int index, Vector3 position)
    {
        if (_currentPlayer != null)
        {
            Destroy(_currentPlayer);
        }
        _currentPlayer = Instantiate(Characters[index], position, Quaternion.identity);
        _currentPlayer.tag = StringConstant.CHARACTER.PLAYER;
    }

    public GameObject[] GetCharacters()
    {
        return Characters;
    }

    public GameObject GetPlayer()
    {
        return _currentPlayer;
    }

    public void SetCurrentEnemy(Vector3 position)
    {
        if (_currentEnemy != null)
        {
            Destroy(_currentEnemy);
        }
        int keyEnemyIndex;
        do
        {
            keyEnemyIndex = Random.Range(0, Characters.Length);

        } while (keyEnemyIndex == keyCharacterIndex);
        _currentEnemy = Instantiate(Characters[keyEnemyIndex], position, Quaternion.identity);
        _currentEnemy.gameObject.tag = StringConstant.CHARACTER.ENEMY;
    }

    public GameObject GetCurrentEnemy()
    {
        return _currentEnemy;
    }
}
