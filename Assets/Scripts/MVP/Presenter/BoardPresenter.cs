using System.Collections.Generic;
using UnityEngine;

public class BoardPresenter : MonoBehaviour
{
    [SerializeField] private Board board;
    private List<Tile> _selections = new();

    public async void Select(Tile tile)
    {
        if (GameManager.Instance.CurrentSide == TurnSide.RIGHTTURN)
        {
            return;
        }
        if (!_selections.Contains(tile)) _selections.Add(tile);
        if (_selections.Count != 2) return;
        if (_selections[0] == _selections[1]) return;
        if (!IsValidSwap(_selections[0], _selections[1]))
        {
            _selections.Clear();
            return;
        }
        await board.Swap(_selections[0], _selections[1]);
        if (board.Connectable())
        {
            board.RemoveConnectionIfMatches();
        }
        else
        {
            await board.Swap(_selections[0], _selections[1]);
        }
        _selections.Clear();
    }

    private bool IsValidSwap(Tile tile1, Tile tile2)
    {
        return (tile1.y == tile2.y && Mathf.Abs(tile1.x - tile2.x) == 1) ||
               (tile1.x == tile2.x && Mathf.Abs(tile1.y - tile2.y) == 1);
    }
}
