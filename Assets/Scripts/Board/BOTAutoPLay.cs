using UnityEngine;

public class BOTAutoPLay : MonoBehaviour
{
    private BoardController m_Board;
    void Start()
    {
        m_Board = FindFirstObjectByType<BoardController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CurrentSide == TurnSide.RIGHTTURN)
        {
        }
    }
}
