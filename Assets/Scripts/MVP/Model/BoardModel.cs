using System;
using UnityEngine;

public class BoardModel : MonoBehaviour
{
    public int columns; //height
    public int rows;//width
    public int numCellType;
    public int numSpecialType;
    private System.Random m_Random;
    (int CellType, int ScpecialType)[,] m_Cells;
    bool[,] m_Matched;

    public event Action<Vector2Int, Vector2Int> SwapCellsEvent;
    public event Action<Vector2Int> ClearCellsEvent;
    public event Action<Vector2Int, Vector2Int> DropCellsEvent;
    public event Action<Vector2Int> FillEmptyCellsEvent;


    void Awake()
    {
        m_Cells = new (int, int)[columns, rows];
        m_Matched = new bool[columns, rows];
        m_Random = new();
        InitializeRandomInBoard();
    }

    public void InitializeRandomInBoard()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                m_Cells[j, i] = (GetRandomCellType(), GetRandomSpecialType());
            }
        }
        ResetMarked();
    }

    private int GetRandomCellType()
    {
        return m_Random.Next(0, numCellType);
    }

    private int GetRandomSpecialType()
    {
        int N = UnityEngine.Random.Range(0, 10);
        if (N == 0) return 2;
        else if (N < 2) return 1;
        return 0;
    }

    public int GetCellType(int row, int col)
    {
        if (row > rows || col > columns || m_Cells == null)
        {
            throw new IndexOutOfRangeException();
        }

        return m_Cells[col, row].CellType;
    }

    public int GetSpecialType(int row, int col)
    {
        if (row > rows || col > columns || m_Cells == null)
        {
            throw new IndexOutOfRangeException();
        }

        return m_Cells[col, row].ScpecialType;
    }

    public bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < columns && y >= 0 && y < rows;
    }

    #region Match 3 threads
    public void SwapCell(int fromX, int fromY, int toX, int toY, bool triggerUI)
    {
        if (!IsValidPosition(fromX, fromY) || !IsValidPosition(toX, toY))
        {
#if UNITY_EDITOR
            Debug.Log("InValid Position, Error in BoardMocel");
#endif
        }
        (m_Cells[fromY, fromX], m_Cells[toY, toX]) = (m_Cells[toY, toX], m_Cells[fromY, fromX]);
        if (triggerUI)
        {
            TriggerSwapCells(new Vector2Int(fromX, fromY), new Vector2Int(toX, toY));
        }
    }

    public bool MarkMatchedCells()
    {
        ResetMarked();
        bool hasMatched = false;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                //check verticaly
                int matchedRows = 0;
                for (int iOffset = i; iOffset < rows; iOffset++)
                {
                    if (m_Cells[j, i].CellType != m_Cells[j, iOffset].CellType)
                    {
                        break;
                    }
                    matchedRows++;
                }
                if (matchedRows >= 3)
                {
                    hasMatched = true;
                    for (var k = 0; k < matchedRows; k++)
                    {
                        m_Matched[j, i + k] = true;
                    }
                }

                //check horizontaly
                var matchedCols = 0;
                for (var jOffset = j; jOffset < columns; jOffset++)
                {
                    if (m_Cells[j, i].CellType != m_Cells[jOffset, i].CellType)
                    {
                        break;
                    }

                    matchedCols++;
                }

                if (matchedCols >= 3)
                {
                    hasMatched = true;
                    for (var k = 0; k < matchedCols; k++)
                    {
                        m_Matched[j + k, i] = true;
                    }
                }
            }
        }
        return hasMatched;
    }

    public void ClearMarkedCells()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (m_Matched[j, i])
                {
                    m_Cells[j, i] = (-1, 0);
                    TriggerClearMatches(new Vector2Int(i, j));
                }
            }
        }
    }

    public void DropCells()
    {
        for (int j = 0; j < columns; j++)
        {
            int writeIndex = 0;
            for (int readIndex = 0; readIndex < rows; readIndex++)
            {
                m_Cells[j, writeIndex] = m_Cells[j, readIndex];
                if (m_Cells[j, readIndex].CellType != -1)
                {
                    writeIndex++;
                }
                //trigger event here
                else
                {
                    TriggerDropCells(new Vector2Int(readIndex, j), new Vector2Int(writeIndex, j));
                }
            }

            for (; writeIndex < rows; writeIndex++)
            {
                m_Cells[j, writeIndex] = (-1, 0);
            }
        }
    }

    public void FillEmptyCells()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (m_Cells[j, i].CellType == -1)
                {
                    m_Cells[j, i] = (GetRandomCellType(), GetRandomSpecialType());
                    //triggr event here
                    TriggerFillEmptyCells(new Vector2Int(i, j));
                }
            }
        }
    }

    #endregion
    void ResetMarked()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                m_Matched[j, i] = false;
            }
        }
    }

    public (int, int)[,] Cells
    {
        get { return m_Cells; }
    }

    public bool[,] Matched
    {
        get { return m_Matched; }
    }

    public void TriggerSwapCells(Vector2Int cell1, Vector2Int cell2) => SwapCellsEvent?.Invoke(cell1, cell2);
    public void TriggerClearMatches(Vector2Int pos) => ClearCellsEvent?.Invoke(pos);
    public void TriggerDropCells(Vector2Int from, Vector2Int to) => DropCellsEvent?.Invoke(from, to);
    public void TriggerFillEmptyCells(Vector2Int pos) => FillEmptyCellsEvent?.Invoke(pos);

}
