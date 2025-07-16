
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the visual representation of the board.
/// </summary>
public class BoardView : MonoBehaviour
{
    public GameObject tilePrefab;
    public float tileSize;
    public Vector2 gridOffset;
    public Dictionary<(int, int), TileSelector> tileDict = new();

    private Match3Board m_Board;
    private bool m_Initialized;
    private CharacterPresenter m_Charactor;
    private CharacterPresenter m_Player, m_Enemy;

    void Awake()
    {

        if (!m_Initialized)
        {
            InitializeDict();
        }
    }

    void Start()
    {
        CalculateTileSize();
        gridOffset = GetComponent<RectTransform>().anchoredPosition;
        m_Player = GameManager.Instance.GetPlayer().GetComponent<CharacterPresenter>();
        m_Enemy = GameManager.Instance.GetCurrentEnemy().GetComponent<CharacterPresenter>();
    }

    void InitializeDict()
    {
        m_Board = GetComponent<Match3Board>();

        if (tileDict != null)
            foreach (var item in tileDict)
            {
                if (item.Value)
                {
                    Destroy(item.Value.gameObject);
                }
            }

        for (int i = 0; i < m_Board.GetMaxBoardSize().Rows; i++)
        {
            for (int j = 0; j < m_Board.GetMaxBoardSize().Columns; j++)
            {
                var obj = Instantiate(tilePrefab, transform.position, Quaternion.identity, transform);
                obj.name = $"r{i}c{j}";
                tileDict.Add((i, j), obj.GetComponent<TileSelector>());
            }
        }

        m_Initialized = true;
    }

    private void CalculateTileSize()
    {
        var boardAreaSize = GetComponent<RectTransform>().rect.size;
        var boardSize = m_Board.GetCurrentBoardSize();
        tileSize = Mathf.Min(boardAreaSize.x / boardSize.Columns, boardAreaSize.y / boardSize.Rows);
    }

    void Update()
    {
        if (!m_Board)
        {
            m_Board = GetComponent<Match3Board>();
        }
        var currentSize = m_Board.GetCurrentBoardSize();
        for (int i = 0; i < currentSize.Rows; i++)
        {
            for (int j = 0; j < currentSize.Columns; j++)
            {
                var value = Match3Board.k_EmptyCell;
                var specialType = 0;
                if (m_Board.Cells != null && i < currentSize.Rows && j < currentSize.Columns)
                {
                    value = m_Board.GetCellType(i, j);
                    specialType = m_Board.GetSpecialType(i, j);
                }
                var pos = new Vector3(j, i, 0) * tileSize + new Vector3(tileSize / 2, tileSize / 2, 0);
                tileDict[(i, j)].transform.position = transform.TransformPoint(pos);
                tileDict[(i, j)].GetComponent<RectTransform>().sizeDelta = new Vector2(tileSize, tileSize);
                tileDict[(i, j)].SetActiveTile(value, specialType);

            }
        }
    }

    public Vector3 GridToWorldPosition(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * tileSize + gridOffset.x, gridPos.y * tileSize + gridOffset.y, 0);
    }

}