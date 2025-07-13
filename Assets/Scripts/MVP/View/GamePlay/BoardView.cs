
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the visual representation of the board.
/// </summary>
public class BoardView : MonoBehaviour
{
    public GameObject tilePrefab;
    private Match3Board m_Board;
    public Dictionary<(int, int), TileSelector> tileDict = new();
    private bool m_Initialized;
    public float spacing = 50;
    void Awake()
    {
        if (!m_Initialized)
        {
            InitializeDict();
        }
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

    void Update()
    {
        if (!m_Board)
        {
            m_Board = GetComponent<Match3Board>();
        }

        if (!m_Initialized)
        {
            InitializeDict();
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
                var pos = new Vector3(j, i, 0);
                pos *= spacing;
                tileDict[(i, j)].transform.position = transform.TransformPoint(pos);
                tileDict[(i, j)].SetActiveTile(value, specialType);

            }
        }
    }
}