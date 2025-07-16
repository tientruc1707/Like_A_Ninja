using System;
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

    private GameObject m_Player;
    private GameObject m_CurrentEnemy;

    [SerializeField] private GameObject[] m_Characters;
    private int m_KeyCharacterIndex = 0;
    public TurnSide CurrentSide { get; set; }
    public event Action<int, int> OnClearCell;

    private void Start()
    {
        CurrentSide = TurnSide.LEFTTURN;
    }

    public void SetCharacterKey(string name)
    {
        for (int i = 0; i < m_Characters.Length; i++)
        {
            if (m_Characters[i].name.Equals(name))
            {
                m_KeyCharacterIndex = i;
                SaveSystem.Instance.SetCharacterKey(m_KeyCharacterIndex);
            }
        }
    }

    public void SetPlayer(int index, Vector3 position)
    {
        if (m_Player != null)
        {
            Destroy(m_Player);
        }
        m_Player = Instantiate(m_Characters[index], position, Quaternion.identity);
        m_Player.tag = StringConstant.CHARACTER.PLAYER;
    }

    public GameObject[] GetCharacters()
    {
        return m_Characters;
    }

    public GameObject GetPlayer()
    {
        return m_Player;
    }

    public void SetCurrentEnemy(Vector3 position)
    {
        if (m_CurrentEnemy != null)
        {
            Destroy(m_CurrentEnemy);
        }
        int keyEnemyIndex;
        do
        {
            keyEnemyIndex = UnityEngine.Random.Range(0, m_Characters.Length);

        } while (keyEnemyIndex == m_KeyCharacterIndex);
        m_CurrentEnemy = Instantiate(m_Characters[keyEnemyIndex], position, Quaternion.identity);
        m_CurrentEnemy.tag = StringConstant.CHARACTER.ENEMY;
    }

    public GameObject GetCurrentEnemy()
    {
        return m_CurrentEnemy;
    }

    //Triggers the OnClearCell event
    public void ClearCell(int row, int col)
    {
        OnClearCell?.Invoke(row, col);
    }

}
