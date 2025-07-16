
using Unity.MLAgents.Integrations.Match3;
using UnityEngine;

/// <summary>
/// Handles the logical state and rules of the board.
/// </summary>
public class Board : MonoBehaviour
{
    public LevelSO levelSO;
    public int Rows;
    public int Cols;
    public int NumCellTypes;
    public int NumSpecialTypes;

    public const int k_EmptyCell = -1;
    public int BasicCellPoint = 1;
    public int SpecialCell1Point = 2;
    public int SpecialCell2Point = 3;

    public int RandomSeed;
    (int CellType, int SpecialType)[,] m_Cells;
    bool[,] m_Matched;

    private BoardSize m_CurrentBoardSize;
    System.Random m_Random;

    void Awake()
    {
        Rows = levelSO.height;
        Cols = levelSO.width;
        NumCellTypes = 4;
        NumSpecialTypes = 3;

        m_Cells = new (int, int)[Rows, Cols];
        m_Matched = new bool[Rows, Cols];

        m_CurrentBoardSize = new BoardSize
        {
            Rows = Rows,
            Columns = Cols,
            NumCellTypes = NumCellTypes,
            NumSpecialTypes = NumSpecialTypes
        };
    }

    public void ClearMaked()
    {
        for (int x = 0; x < Rows; x++)
        {
            for (int y = 0; y < Cols; y++)
            {
                m_Matched[x, y] = false;
            }
        }
    }

    public void InitRandomCell()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                m_Cells[i, j] = (GetRandomCellType(), GetRandomSpecialType());
            }
        }
    }

    int GetRandomCellType()
    {
        return m_Random.Next(0, NumCellTypes);
    }
    int GetRandomSpecialType()
    {
        var N = 10;
        var value = m_Random.Next(0, N);
        if (value == 0) return 2;
        if (value <= 2) return 1;
        return 0;
    }

}