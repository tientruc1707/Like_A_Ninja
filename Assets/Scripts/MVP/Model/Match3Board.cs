using System;
using Unity.MLAgents.Integrations.Match3;
using UnityEngine;

public enum ItemType
{
    Health = 1,
    Mana = 2,
    Shuriken = 3,
    Coin = 4
}

public class Match3Board : AbstractBoard
{
    public LevelSO levelSO;

    BoardSize boardSize;
    (int CellType, int SpecialType)[,] m_Cells;
    bool[,] m_Matched;

    public int randomSeed;
    System.Random random;
    public const int k_EmptyCell = -1;
    public int BasicCellPoint = 1;
    public int SpecialCell1Point = 2;
    public int SpecialCell2Point = 3;

    void Awake()
    {
        InitializeBoardSize();
    }

    void Start()
    {
        InitRandom();
    }

    public void InitializeBoardSize()
    {
        if (levelSO == null)
        {
            return;
        }
        if (levelSO.height <= 0 || levelSO.width <= 0)
        {
            return;
        }
        random = new System.Random(randomSeed == -1 ? gameObject.GetInstanceID() : randomSeed);
        if (boardSize.Rows == 0 && boardSize.Columns == 0)
        {
            m_Cells = new (int, int)[levelSO.width, levelSO.height];
            m_Matched = new bool[levelSO.width, levelSO.height];
            boardSize = new BoardSize
            {
                Rows = levelSO.height,
                Columns = levelSO.width,
                NumCellTypes = levelSO.itemList.Count,
                NumSpecialTypes = 3
            };
        }
    }

    public override int GetCellType(int row, int col)
    {
        if (row >= boardSize.Rows || col >= boardSize.Columns || m_Cells == null)
        {
            throw new IndexOutOfRangeException();
        }
        return m_Cells[col, row].CellType;
    }

    public override BoardSize GetMaxBoardSize()
    {
        return boardSize;
    }

    public override BoardSize GetCurrentBoardSize()
    {
        return boardSize;
    }

    public override int GetSpecialType(int row, int col)
    {
        if (row >= boardSize.Rows || col >= boardSize.Columns || m_Cells == null)
        {
            throw new IndexOutOfRangeException();
        }
        return m_Cells[col, row].SpecialType;
    }

    public override bool IsMoveValid(Move m)
    {
        if (m_Cells == null)
            return false;
        return SimpleIsMoveValid(m);
    }

    public override bool MakeMove(Move m)
    {
        if (!IsMoveValid(m))
            return false;
        var (endRow, endCol) = m.OtherCell();
        (m_Cells[m.Column, m.Row], m_Cells[endCol, endRow]) = (m_Cells[endCol, endRow], m_Cells[m.Column, m.Row]);

        return true;
    }

    public void SwapCells(Vector2Int from, Vector2Int to)
    {
        if (!IsValidPosition(from) || !IsValidPosition(to))
        {
            return;
        }
        (m_Cells[from.x, from.y], m_Cells[to.x, to.y]) = (m_Cells[to.x, to.y], m_Cells[from.x, from.y]);
    }

    public bool IsValidPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < boardSize.Columns && pos.y < boardSize.Rows;
    }

    int GetRandomCellType()
    {
        return random.Next(0, boardSize.NumCellTypes);
    }

    int GetRandomSpecialType()
    {
        int value = random.Next(0, 10);
        if (value == 0) return 2;
        else if (value < 3) return 1;
        return 0;
    }

    public bool MarkMatchedCells(int[,] cells = null)
    {
        ClearMarked();
        bool madeMatch = false;
        for (int row = 0; row < boardSize.Rows; row++)
        {
            for (int col = 0; col < boardSize.Columns; col++)
            {
                // Vertical Check
                int matchedRows = 0;
                for (int pos = row; pos < boardSize.Rows; pos++)
                {
                    if (m_Cells[col, row].CellType != m_Cells[col, pos].CellType)
                    {
                        break;
                    }
                    matchedRows++;
                }
                if (matchedRows >= 3)
                {
                    madeMatch = true;
                    for (int k = 0; k < matchedRows; k++)
                    {
                        m_Matched[col, row + k] = true;
                    }
                }
                // Horizontal Check
                int matchedCols = 0;
                for (int pos = col; pos < boardSize.Columns; pos++)
                {
                    if (m_Cells[col, row].CellType != m_Cells[pos, row].CellType)
                    {
                        break;
                    }
                    matchedCols++;
                }
                if (matchedCols >= 3)
                {
                    madeMatch = true;
                    for (int k = 0; k < matchedCols; k++)
                    {
                        m_Matched[col + k, row] = true;
                    }
                }
            }
        }
        return madeMatch;
    }

    public int ClearMatchedCells()
    {
        var pointByType = new[] { BasicCellPoint, SpecialCell1Point, SpecialCell2Point };
        var pointEarned = 0;
        for (int row = 0; row < boardSize.Rows; row++)
        {
            for (int col = 0; col < boardSize.Columns; col++)
            {
                if (m_Matched[col, row])
                {
                    var specialType = GetSpecialType(row, col);
                    GameManager.Instance.ClearCell(row, col);
                    pointEarned += pointByType[specialType];
                    m_Cells[col, row] = (k_EmptyCell, 0);
                }
            }
        }
        ClearMarked();
        return pointEarned;
    }

    public bool DropCells()
    {
        bool hasChange = false;
        for (int col = 0; col < boardSize.Columns; col++)
        {
            int writeRow = 0;
            for (int readRow = 0; readRow < boardSize.Rows; readRow++)
            {
                if (m_Cells[col, readRow].CellType != k_EmptyCell)
                {
                    m_Cells[col, writeRow] = m_Cells[col, readRow];
                    writeRow++;
                }
            }
            for (; writeRow < boardSize.Rows; writeRow++)
            {
                hasChange = true;
                m_Cells[col, writeRow] = (k_EmptyCell, 0);
            }
        }
        return hasChange;
    }

    public bool FillFromAbove()
    {
        bool madeChange = false;
        for (int row = 0; row < boardSize.Rows; row++)
        {
            for (int col = 0; col < boardSize.Columns; col++)
            {
                if (m_Cells[col, row].CellType == k_EmptyCell)
                {
                    madeChange = true;
                    m_Cells[col, row] = (GetRandomCellType(), GetRandomSpecialType());
                }
            }
        }
        //if no match after filling, change side
        if (!MarkMatchedCells())
            EventSystem.Instance.TriggerEvent(StringConstant.EVENT.CHANG_SIDE);
        return madeChange;
    }

    public void InitSettled()
    {
        InitRandom();
        int maxIterations = 100;
        int iteration = 0;
        while (true)
        {
            if (iteration++ >= maxIterations)
            {
                break;
            }
            bool hasMatched = MarkMatchedCells();
            if (!hasMatched)
                return;
            ClearMatchedCells();
            DropCells();
            FillFromAbove();
        }
    }

    public void InitRandom()
    {
        for (int row = 0; row < boardSize.Rows; row++)
        {
            for (int col = 0; col < boardSize.Columns; col++)
            {
                m_Cells[col, row] = (GetRandomCellType(), GetRandomSpecialType());
            }
        }
    }

    void ClearMarked()
    {
        for (int row = 0; row < boardSize.Rows; row++)
        {
            for (int col = 0; col < boardSize.Columns; col++)
            {
                m_Matched[col, row] = false;
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

}