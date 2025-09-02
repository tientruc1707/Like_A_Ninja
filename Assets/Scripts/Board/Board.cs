using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Board : MonoBehaviour
{
    public enum eMatchDir
    {
        NONE,
        HORIZONTAL,
        VERTICAL,
        ALL
    }

    private int m_Width;
    private int m_Height;

    private Cell[,] m_Cells;

    private Transform m_Root;

    private int m_MatchMin;

    public Board(Transform transform, GameSetting gameSetting)
    {
        m_Root = transform;

        m_MatchMin = gameSetting.MatchMin;

        m_Width = gameSetting.BoardWidth;
        m_Height = gameSetting.BoardHeight;

        m_Cells = new Cell[m_Width, m_Height];

        CreateBoard();
    }

    private void CreateBoard()
    {
        Vector3 rootPos = new Vector3(-m_Width / 2f + 0.5f, -m_Height / 2f + 0.5f, 0);
        GameObject emptyCellPrefab = Resources.Load<GameObject>(StringConstant.PREFAB_EMPTY_CELL_PATH);

        for (int x = 0; x < m_Width; x++)
        {
            for (int y = 0; y < m_Height; y++)
            {
                GameObject cellObj = Instantiate(emptyCellPrefab, m_Root);
                cellObj.transform.position = rootPos + new Vector3(x, y, 0);
                cellObj.name = $"Cell_{x}_{y}";

                Cell cell = cellObj.GetComponent<Cell>();
                cell.SetUp(x, y);

                m_Cells[x, y] = cell;

            }
        }

        for (int x = 0; x < m_Width; x++)
        {
            for (int y = 0; y < m_Height; y++)
            {
                Cell cell = m_Cells[x, y];
                if (y < m_Height - 1)
                    cell.NeighbourUp = m_Cells[x, y + 1];
                if (y > 0)
                    cell.NeighbourDown = m_Cells[x, y - 1];
                if (x > 0)
                    cell.NeighbourLeft = m_Cells[x - 1, y];
                if (x < m_Width - 1)
                    cell.NeighbourRight = m_Cells[x + 1, y];
            }
        }
    }

    #region Main Logic
    public void Swap(Cell cell1, Cell cell2, Action callback)
    {
        Item item1 = cell1.Item;
        cell1.Free();
        Item item2 = cell2.Item;
        cell1.SetItem(item2);
        cell2.Free();
        cell2.SetItem(item1);

        item1.View.DOMove(cell2.transform.position, 0.2f);
        item2.View.DOMove(cell1.transform.position, 0.2f).OnComplete(() =>
        {
            callback?.Invoke();
        });
    }

    internal void ClearAndDropItems()
    {
        for (int x = 0; x < m_Width; x++)
        {
            int shifts = 0;
            for (int y = 0; y < m_Height; y++)
            {
                Cell cell = m_Cells[x, y];
                if (cell.IsEmpty)
                {
                    shifts++;
                    continue;
                }
                if (shifts == 0) continue;

                //drop item
                Cell targetCell = m_Cells[x, y - shifts];

                Item item = cell.Item;
                cell.Free();

                targetCell.SetItem(cell.Item);
                cell.Free();
                item.View.DOMove(targetCell.transform.position, 0.2f);
            }

        }
    }

    internal void FillGapsWithNewItems()
    {
        for (int x = 0; x < m_Width; x++)
        {
            for (int y = 0; y < m_Height; y++)
            {
                Cell cell = m_Cells[x, y];
                if (!cell.IsEmpty) continue;

                NormalItem item = new();

                item.SetType(Utils.GetRandomNormalType());
                item.SetView();
                item.SetViewRoot(m_Root);

                cell.SetItem(item);
                cell.SetItemPosition(true);
            }
        }
    }
    #endregion

    public void FillNewBoard()
    {
        for (int x = 0; x < m_Width; x++)
        {
            for (int y = 0; y < m_Height; y++)
            {
                Cell cell = m_Cells[x, y];
                NormalItem item = new();
                List<NormalItem.eNormalType> tyoes = new();

                if (cell.NeighbourDown != null)
                {
                    if (cell.NeighbourDown.Item is NormalItem nitem)
                    {
                        tyoes.Add(nitem.NormalType);
                    }
                }

                if (cell.NeighbourLeft != null)
                {
                    if (cell.NeighbourLeft.Item is NormalItem nitem)
                    {
                        tyoes.Add(nitem.NormalType);
                    }
                }

                item.SetType(Utils.GetRandomNormalTypeExcept(tyoes.ToArray()));
                item.SetView();
                item.SetViewRoot(m_Root);

                cell.SetItem(item);
                cell.SetItemPosition(false);
            }
        }
    }

    internal void Suffle()
    {
        List<Item> items = new();

        //clear all cells and collect items
        for (int x = 0; x < m_Width; x++)
        {
            for (int y = 0; y < m_Height; y++)
            {
                Cell cell = m_Cells[x, y];
                if (cell.Item != null)
                {
                    items.Add(cell.Item);
                    cell.Free();
                }
            }
        }

        //shuffle items
        for (int x = 0; x < m_Width; x++)
        {
            for (int y = 0; y < m_Height; y++)
            {
                int random = UnityEngine.Random.Range(0, items.Count);
                m_Cells[x, y].SetItem(items[random]);
                m_Cells[x, y].ApplyItemMoveToPosition();

                items.RemoveAt(random);
            }
        }

    }

    internal void ClearBoard()
    {
        for (int x = 0; x < m_Width; x++)
        {
            for (int y = 0; y < m_Height; y++)
            {
                Cell cell = m_Cells[x, y];
                if (!cell.IsEmpty)
                {
                    cell.DestroyItem();
                }
            }
        }
    }

    public (List<Cell>, eMatchDir) GetMatchCells(Cell cell)
    {
        List<Cell> matchCells = new();
        eMatchDir matchDir = eMatchDir.NONE;

        if (cell.IsEmpty || cell.Item is not NormalItem)
            return (matchCells, matchDir);

        NormalItem targetItem = cell.Item as NormalItem;

        //check horizontal
        List<Cell> horizontalCells = new();
        horizontalCells.Add(cell);

        //check left
        Cell checkCell = cell.NeighbourLeft;
        while (checkCell != null && !checkCell.IsEmpty && checkCell.Item is NormalItem nitem && nitem.NormalType == targetItem.NormalType)
        {
            horizontalCells.Add(checkCell);
            checkCell = checkCell.NeighbourLeft;
        }

        //check right
        checkCell = cell.NeighbourRight;
        while (checkCell != null && !checkCell.IsEmpty && checkCell.Item is NormalItem nitem && nitem.NormalType == targetItem.NormalType)
        {
            horizontalCells.Add(checkCell);
            checkCell = checkCell.NeighbourRight;
        }

        if (horizontalCells.Count >= m_MatchMin)
        {
            matchDir = eMatchDir.HORIZONTAL;
            matchCells.AddRange(horizontalCells);
        }

        //check vertical
        List<Cell> verticalCells = new();
        verticalCells.Add(cell);

        //check up
        checkCell = cell.NeighbourUp;
        while (checkCell != null && !checkCell.IsEmpty && checkCell.Item is NormalItem nitem && nitem.NormalType == targetItem.NormalType)
        {
            verticalCells.Add(checkCell);
            checkCell = checkCell.NeighbourUp;
        }

        //check down
        checkCell = cell.NeighbourDown;
        while (checkCell != null && !checkCell.IsEmpty && checkCell.Item is NormalItem nitem && nitem.NormalType == targetItem.NormalType)
        {
            verticalCells.Add(checkCell);
            checkCell = checkCell.NeighbourDown;
        }

        if (verticalCells.Count >= m_MatchMin)
        {
            if (matchDir == eMatchDir.HORIZONTAL)
                matchDir = eMatchDir.ALL;
            else
                matchDir = eMatchDir.VERTICAL;

            matchCells.AddRange(verticalCells);
        }

        return (matchCells, matchDir);
    }

    internal void ConvertNormalToSpecial(Cell cell)
    {
        if (cell.IsEmpty || cell.Item is not NormalItem)
            return;

        SpecialItem specialItem = new();
        eMatchDir matchDir = GetMatchCells(cell).Item2;
        switch (matchDir)
        {
            case eMatchDir.HORIZONTAL:
                specialItem.SetType(SpecialItem.eSpecialType.ROW_CLEAR);
                break;
            case eMatchDir.VERTICAL:
                specialItem.SetType(SpecialItem.eSpecialType.COLUMN_CLEAR);
                break;
            case eMatchDir.ALL:
                specialItem.SetType(SpecialItem.eSpecialType.ALL_CLEAR);
                break;
            default:
                return;
        }

        specialItem.SetView();
        specialItem.SetViewRoot(m_Root);

        cell.DestroyItem();
        cell.SetItem(specialItem);
        cell.ApplyItemMoveToPosition();
    }

    internal List<Cell> FindFirstMatch()
    {
        List<Cell> matchCells = new();

        for (int x = 0; x < m_Width; x++)
        {
            for (int y = 0; y < m_Height; y++)
            {
                Cell cell = m_Cells[x, y];
                var (cells, _) = GetMatchCells(cell);
                if (cells.Count > 0)
                {
                    matchCells.AddRange(cells);
                    return matchCells;
                }
            }
        }

        return matchCells;
    }

    public List<Cell> CheckBonusItemMatch(Cell cell)
    {
        List<Cell> matchCells = new();

        if (cell.IsEmpty || cell.Item is not SpecialItem)
            return matchCells;

        SpecialItem specialItem = cell.Item as SpecialItem;

        switch (specialItem.SpecialType)
        {
            case SpecialItem.eSpecialType.ROW_CLEAR:
                //clear entire row
                for (int x = 0; x < m_Width; x++)
                {
                    Cell c = m_Cells[x, cell.Y];
                    if (!c.IsEmpty)
                        matchCells.Add(c);
                }
                break;
            case SpecialItem.eSpecialType.COLUMN_CLEAR:
                //clear entire column
                for (int y = 0; y < m_Height; y++)
                {
                    Cell c = m_Cells[cell.X, y];
                    if (!c.IsEmpty)
                        matchCells.Add(c);
                }
                break;
            case SpecialItem.eSpecialType.ALL_CLEAR:
                //clear all cells
                for (int x = 0; x < m_Width; x++)
                {
                    for (int y = 0; y < m_Height; y++)
                    {
                        Cell c = m_Cells[x, y];
                        if (!c.IsEmpty)
                            matchCells.Add(c);
                    }
                }
                break;
            default:
                break;
        }

        return matchCells;
    }

    internal List<Cell> GetPotentialMatches()
    {
        List<Cell> potentialMatchCells = new();

        for (int x = 0; x < m_Width; x++)
        {
            for (int y = 0; y < m_Height; y++)
            {
                Cell cell = m_Cells[x, y];
                if (cell.IsEmpty || cell.Item is not NormalItem)
                    continue;

                //check swap with neighbour cells
                Cell[] neighbours = new Cell[] { cell.NeighbourUp, cell.NeighbourDown, cell.NeighbourLeft, cell.NeighbourRight };
                foreach (var neighbour in neighbours)
                {
                    if (neighbour == null || neighbour.IsEmpty || neighbour.Item is not NormalItem)
                        continue;

                    //swap items
                    Item tempItem = cell.Item;
                    cell.Free();
                    cell.SetItem(neighbour.Item);
                    neighbour.Free();
                    neighbour.SetItem(tempItem);

                    var (matchCells, _) = GetMatchCells(cell);
                    if (matchCells.Count > 0)
                    {
                        potentialMatchCells.Add(cell);
                        potentialMatchCells.Add(neighbour);
                    }

                    //swap back
                    tempItem = cell.Item;
                    cell.Free();
                    cell.SetItem(neighbour.Item);
                    neighbour.Free();
                    neighbour.SetItem(tempItem);
                }
            }
        }

        return potentialMatchCells;
    }

    private List<Cell> GetpotentialMatch(Cell cell, Cell neighbour, Cell target)
    {
        List<Cell> result = new();

        if (neighbour != null && neighbour.IsSameType(cell))
        {
            Cell thirdCell = FindTheThirdCell(target, neighbour);
            if (thirdCell != null)
            {
                result.Add(cell);
                result.Add(neighbour);
                result.Add(thirdCell);
            }
        }

        return result;
    }

    private Cell FindSecondCellHorizontal(Cell target, Cell main)
    {
        if (target == null || target.IsSameType(main))
            return null;
        Cell secondCell = null;

        //find left
        secondCell = target.NeighbourLeft;
        if (secondCell != null && secondCell.IsSameType(main))
            return secondCell;

        //find right
        secondCell = target.NeighbourRight;
        if (secondCell != null && secondCell.IsSameType(main))
            return secondCell;

        return null;
    }

    private Cell FindSecondCellVertical(Cell target, Cell main)
    {
        if (target == null || target.IsSameType(main))
            return null;
        Cell secondCell = null;

        //find up
        secondCell = target.NeighbourUp;
        if (secondCell != null && secondCell.IsSameType(main))
            return secondCell;

        //find down
        secondCell = target.NeighbourDown;
        if (secondCell != null && secondCell.IsSameType(main))
            return secondCell;

        return null;
    }

    private Cell FindTheThirdCell(Cell target, Cell main)
    {
        if (target == null || target.IsSameType(main))
            return null;
        Cell thirdCell = null;
        //find up
        thirdCell = CheckThirdCell(target.NeighbourUp, main);
        if (thirdCell != null)
            return thirdCell;
        //find down
        thirdCell = CheckThirdCell(target.NeighbourDown, main);
        if (thirdCell != null)
            return thirdCell;
        //find left
        thirdCell = CheckThirdCell(target.NeighbourLeft, main);
        if (thirdCell != null)
            return thirdCell;
        //find right
        thirdCell = CheckThirdCell(target.NeighbourRight, main);
        if (thirdCell != null)
            return thirdCell;
        return null;
    }

    private Cell CheckThirdCell(Cell target, Cell main)
    {
        if (target != null && target != main && target.IsSameType(main))
        {
            return target;
        }

        return null;
    }

    public void Clear()
    {
        for (int x = 0; x < m_Width; x++)
        {
            for (int y = 0; y < m_Height; y++)
            {
                Cell cell = m_Cells[x, y];
                cell.DestroyItem();

                GameObject.Destroy(cell.gameObject);
                m_Cells[x, y] = null;
            }
        }
    }

}
