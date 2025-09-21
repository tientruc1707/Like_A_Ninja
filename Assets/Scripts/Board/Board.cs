using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Board
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
        AdjustCameraToBoard(m_Width, m_Height);
        CreateBoard();
    }

    private void CreateBoard()
    {
        Vector3 rootPos = new(-m_Width / 2f + 0.5f, -m_Height / 2f - 3.5f, 0);
        GameObject emptyCellPrefab = Resources.Load<GameObject>(StringConstant.PREFAB_EMPTY_CELL_PATH);

        for (int x = 0; x < m_Width; x++)
        {
            for (int y = 0; y < m_Height; y++)
            {
                GameObject cellObj = GameObject.Instantiate(emptyCellPrefab, m_Root);
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

    void AdjustCameraToBoard(int boardWidth, int boardHeight)
    {
        float screenRatio = Screen.width / (float)Screen.height;
        float boardRatio = boardWidth / (float)boardHeight;

        if (boardRatio > screenRatio)
        {
            Camera.main.orthographicSize = boardWidth / screenRatio / 2f;
        }
        else
        {
            Camera.main.orthographicSize = boardHeight / 2f;
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

        item1.View.DOMove(cell2.transform.position, 0.3f);
        item2.View.DOMove(cell1.transform.position, 0.3f).OnComplete(() =>
        {
            callback?.Invoke();
        });
    }

    internal void ShiftDownItems()
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

                targetCell.SetItem(item);
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
                List<NormalItem.eNormalType> types = new();

                if (cell.NeighbourDown != null)
                {
                    if (cell.NeighbourDown.Item is NormalItem nitem)
                    {
                        types.Add(nitem.NormalType);
                    }
                }

                if (cell.NeighbourLeft != null)
                {
                    if (cell.NeighbourLeft.Item is NormalItem nitem)
                    {
                        types.Add(nitem.NormalType);
                    }
                }

                item.SetType(Utils.GetRandomNormalTypeExcept(types.ToArray()));
                item.SetView();
                item.SetViewRoot(m_Root);

                cell.SetItem(item);
                cell.SetItemPosition(false);
            }
        }
    }

    internal void Shuffle()
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

    public List<Cell> GetHorizontalMatches(Cell cell)
    {
        List<Cell> list = new List<Cell>();
        list.Add(cell);

        //check horizontal match
        Cell newcell = cell;
        while (true)
        {
            Cell neib = newcell.NeighbourRight;
            if (neib == null) break;

            if (neib.IsSameType(cell))
            {
                list.Add(neib);
                newcell = neib;
            }
            else break;
        }

        newcell = cell;
        while (true)
        {
            Cell neib = newcell.NeighbourLeft;
            if (neib == null) break;

            if (neib.IsSameType(cell))
            {
                list.Add(neib);
                newcell = neib;
            }
            else break;
        }

        return list;
    }

    public List<Cell> GetVerticalMatches(Cell cell)
    {
        List<Cell> list = new List<Cell>();
        list.Add(cell);

        Cell newcell = cell;
        while (true)
        {
            Cell neib = newcell.NeighbourUp;
            if (neib == null) break;

            if (neib.IsSameType(cell))
            {
                list.Add(neib);
                newcell = neib;
            }
            else break;
        }

        newcell = cell;
        while (true)
        {
            Cell neib = newcell.NeighbourDown;
            if (neib == null) break;

            if (neib.IsSameType(cell))
            {
                list.Add(neib);
                newcell = neib;
            }
            else break;
        }

        return list;
    }

    internal eMatchDir GetMatchDirection(List<Cell> matches)
    {
        if (matches == null || matches.Count < m_MatchMin) return eMatchDir.NONE;

        var listH = matches.Where(x => x.X == matches[0].X).ToList();
        if (listH.Count == matches.Count)
        {
            return eMatchDir.VERTICAL;
        }

        var listV = matches.Where(x => x.Y == matches[0].Y).ToList();
        if (listV.Count == matches.Count)
        {
            return eMatchDir.HORIZONTAL;
        }

        if (matches.Count > 5)
        {
            return eMatchDir.ALL;
        }

        return eMatchDir.NONE;
    }

    internal void ConvertNormalToSpecial(List<Cell> matchCells, Cell cellToConvert)
    {
        SpecialItem specialItem = new();

        eMatchDir matchDir = GetMatchDirection(matchCells);
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

        if (specialItem != null)
        {
            if (cellToConvert == null)
            {
                int rand = UnityEngine.Random.Range(0, matchCells.Count);
                cellToConvert = matchCells[rand];
            }

            specialItem.SetView();
            specialItem.SetViewRoot(m_Root);

            cellToConvert.Free();
            cellToConvert.SetItem(specialItem);
            cellToConvert.SetItemPosition(true);
        }
    }

    internal List<Cell> FindFirstMatch()
    {
        List<Cell> result = new();

        for (int x = 0; x < m_Width; x++)
        {
            for (int y = 0; y < m_Height; y++)
            {
                Cell cell = m_Cells[x, y];
                var listHor = GetHorizontalMatches(cell);
                if (listHor.Count >= m_MatchMin)
                {
                    result.AddRange(listHor);
                    break;
                }

                var listVer = GetVerticalMatches(cell);
                if (listVer.Count >= m_MatchMin)
                {
                    result.AddRange(listVer);
                    break;
                }

            }
        }

        return result;
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
        List<Cell> result = new();

        for (int x = 0; x < m_Width; x++)
        {
            for (int y = 0; y < m_Height; y++)
            {
                Cell cell = m_Cells[x, y];
                if (cell.NeighbourRight != null)
                {
                    result = GetpotentialMatch(cell, cell.NeighbourRight, cell.NeighbourUp);
                    if (result.Count > 0) break;
                }

                if (cell.NeighbourDown != null)
                {
                    result = GetpotentialMatch(cell, cell.NeighbourDown, cell.NeighbourLeft);
                    if (result.Count > 0) break;
                }

                if (cell.NeighbourLeft != null)
                {
                    result = GetpotentialMatch(cell, cell.NeighbourLeft, cell.NeighbourUp);
                    if (result.Count > 0) break;
                }

                if (cell.NeighbourUp != null)
                {
                    result = GetpotentialMatch(cell, cell.NeighbourUp, cell.NeighbourRight);
                    if (result.Count > 0) break;
                }

                Cell neib = cell.NeighbourRight;
                if (neib != null && neib.NeighbourRight != null && neib.NeighbourRight.IsSameType(cell))
                {
                    Cell secondCell = FindSecondCellVertical(neib, cell);
                    if (secondCell != null)
                    {
                        result.Add(cell);
                        result.Add(neib);
                        result.Add(neib.NeighbourRight);
                        result.Add(secondCell);
                        break;
                    }
                }
                neib = null;
                neib = cell.NeighbourUp;
                if (neib != null && neib.NeighbourUp != null && neib.NeighbourUp.IsSameType(cell))
                {
                    Cell secondCell = FindSecondCellHorizontal(neib, cell);
                    if (secondCell != null)
                    {
                        result.Add(cell);
                        result.Add(neib);
                        result.Add(neib.NeighbourUp);
                        result.Add(secondCell);
                        break;
                    }
                }
            }

            if (result.Count > 3) break;
        }

        return result;
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
                cell.Clear();

                GameObject.Destroy(cell.gameObject);
                m_Cells[x, y] = null;
            }
        }
    }

}
