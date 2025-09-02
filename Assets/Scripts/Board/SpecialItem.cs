using System;
using System.Collections.Generic;
using UnityEngine;

public class SpecialItem : Item
{
    public enum eSpecialType
    {
        NONE,
        ROW_CLEAR,
        COLUMN_CLEAR,
        ALL_CLEAR
    }

    public eSpecialType SpecialType;

    public void SetType(eSpecialType type)
    {
        SpecialType = type;
    }

    protected override string GetPrefabName()
    {
        string prefabName = string.Empty;
        switch (SpecialType)
        {
            case eSpecialType.ROW_CLEAR:
                prefabName = StringConstant.ITEM_PREFAB_PATH.ROW_CLEAR;
                break;
            case eSpecialType.COLUMN_CLEAR:
                prefabName = StringConstant.ITEM_PREFAB_PATH.COLUMN_CLEAR;
                break;
            case eSpecialType.ALL_CLEAR:
                prefabName = StringConstant.ITEM_PREFAB_PATH.ALL_CLEAR;
                break;
            default:
                prefabName = base.GetPrefabName();
                break;
        }

        return prefabName;

    }

    internal override bool IsSameType(Item other)
    {
        SpecialItem sI = other as SpecialItem;

        return sI != null && sI.SpecialType == SpecialType;
    }

    internal override void AnimationDestroy()
    {
        ExecuteSpecialEffect();
        base.AnimationDestroy();
    }

    private void ExecuteSpecialEffect()
    {
        switch (SpecialType)
        {
            case eSpecialType.ROW_CLEAR:
                ClearRow();
                break;
            case eSpecialType.COLUMN_CLEAR:
                ClearColumn();
                break;
            case eSpecialType.ALL_CLEAR:
                ClearAll();
                break;
            default:
                break;
        }
    }

    private void ClearRow()
    {
        List<Cell> cells = new();
        Cell newCell = Cell;

        while (true)
        {
            Cell next = newCell.NeighbourLeft;
            if (next == null) break;
            cells.Add(next);
            newCell = next;
        }

        newCell = Cell;
        while (true)
        {
            Cell next = newCell.NeighbourRight;
            if (next == null) break;
            cells.Add(next);
            newCell = next;
        }

        foreach (Cell cell in cells)
        {
            cell.DestroyItem();
        }

    }

    private void ClearColumn()
    {
        List<Cell> cells = new();
        Cell newCell = Cell;

        while (true)
        {
            Cell next = newCell.NeighbourUp;
            if (next == null) break;
            cells.Add(next);
            newCell = next;
        }

        newCell = Cell;
        while (true)
        {
            Cell next = newCell.NeighbourDown;
            if (next == null) break;
            cells.Add(next);
            newCell = next;
        }

        foreach (Cell cell in cells)
        {
            cell.DestroyItem();
        }
    }

    private void ClearAll()
    {
        List<Cell> list = new();
        if (Cell.NeighbourDown) list.Add(Cell.NeighbourDown);
        if (Cell.NeighbourUp) list.Add(Cell.NeighbourUp);
        if (Cell.NeighbourLeft)
        {
            list.Add(Cell.NeighbourLeft);
            if (Cell.NeighbourLeft.NeighbourUp)
            {
                list.Add(Cell.NeighbourLeft.NeighbourUp);
            }
            if (Cell.NeighbourLeft.NeighbourDown)
            {
                list.Add(Cell.NeighbourLeft.NeighbourDown);
            }
        }
        if (Cell.NeighbourRight)
        {
            list.Add(Cell.NeighbourRight);
            if (Cell.NeighbourRight.NeighbourUp)
            {
                list.Add(Cell.NeighbourRight.NeighbourUp);
            }
            if (Cell.NeighbourRight.NeighbourDown)
            {
                list.Add(Cell.NeighbourRight.NeighbourDown);
            }
        }

        foreach (Cell cell in list)
        {
            cell.DestroyItem();
        }
    }

}
