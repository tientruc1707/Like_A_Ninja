using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BoardPresenter : MonoBehaviour
{
    [SerializeField] private BoardUI board;
    [SerializeField] private BoardModel m_BoardModel;
    private List<TileSelector> m_Selections = new();

    void OnEnable()
    {
        m_BoardModel.SwapCellsEvent += OnSwapCells;
        m_BoardModel.ClearCellsEvent += OnClearCells;
        m_BoardModel.DropCellsEvent += OnDropCells;
        m_BoardModel.FillEmptyCellsEvent += OnFillEmptyCells;
    }

    public void Select(TileSelector tile)
    {
        if (GameManager.Instance.CurrentSide == TurnSide.RIGHTTURN)
        {
            return;
        }
        if (!m_Selections.Contains(tile)) m_Selections.Add(tile);
        if (m_Selections.Count != 2) return;
        if (m_Selections[0] == m_Selections[1]) return;
        if (!IsValidSwap(m_Selections[0], m_Selections[1]))
        {
            m_Selections.Clear();
            return;
        }
        m_BoardModel.SwapCell(m_Selections[0].x, m_Selections[0].y, m_Selections[1].x, m_Selections[1].y, true);
        if (m_BoardModel.MarkMatchedCells())
        {
            m_BoardModel.ClearMarkedCells();
            m_BoardModel.DropCells();
            m_BoardModel.FillEmptyCells();
        }
        else
        {
            m_BoardModel.SwapCell(m_Selections[0].x, m_Selections[0].y, m_Selections[1].x, m_Selections[1].y, true);
        }
        //change side after do the turn
        EventSystem.Instance.TriggerEvent(StringConstant.EVENT.CHANG_SIDE);

        m_Selections.Clear();
    }

    private bool IsValidSwap(TileSelector tile1, TileSelector tile2)
    {
        return (tile1.y == tile2.y && Mathf.Abs(tile1.x - tile2.x) == 1) ||
               (tile1.x == tile2.x && Mathf.Abs(tile1.y - tile2.y) == 1);
    }

    private void OnSwapCells(Vector2Int cell1, Vector2Int cell2)
    {
        StartCoroutine(board.Swap(board.tileDict[(cell1.x, cell1.y)], board.tileDict[(cell2.x, cell2.y)]));
    }

    private void OnClearCells(Vector2Int pos)
    {
        StartCoroutine(board.RemoveMatchedCells(pos));
    }

    private void OnDropCells(Vector2Int cell1, Vector2Int cell2)
    {
        StartCoroutine(board.DropCells(cell1, cell2));
    }

    private void OnFillEmptyCells(Vector2Int cell)
    {
        StartCoroutine(board.FillEmptyCells(cell));
    }

}
