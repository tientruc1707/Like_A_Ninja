using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the visual representation of the board.
/// </summary>
public class BoardView : MonoBehaviour
{
    public Tile[] tileList;
    public Transform boardParent;
    private Dictionary<ItemSO, GameObject> _tileDictionary;
    private GameObject[,] _visualBoard;
    public void Bind(Board board)
    {
        board.OnTileClear += ClearTile;
        board.OnTileUpdate += UpdateTile;
    }
    public void Unbind(Board board)
    {
        board.OnTileClear -= ClearTile;
        board.OnTileUpdate -= UpdateTile;
    }
    
    public void InitializeView(Board board)
    {
        _tileDictionary = new();
        foreach (var tile in tileList)
        {
            if (!_tileDictionary.ContainsKey(tile.item))
                _tileDictionary.Add(tile.item, tile.gameObject);
        }
        _visualBoard = new GameObject[board.Row, board.Col];
    }

    public void RenderBoard(Board board)
    {
        for (int x = 0; x < board.Row; x++)
        {
            for (int y = 0; y < board.Col; y++)
            {
                var item = board.ItemBoard[x, y];
                if (item != null && _tileDictionary.ContainsKey(item))
                {
                    Vector3 position = new(x, y, 0);
                    GameObject newItem = Instantiate(_tileDictionary[item], position, Quaternion.identity, boardParent);
                    _visualBoard[x, y] = newItem;
                }
            }
        }
    }

    public void ClearTile(int x, int y)
    {
        if (_visualBoard[x, y] != null)
        {
            Destroy(_visualBoard[x, y]);
            _visualBoard[x, y] = null;
        }
    }

    public void UpdateTile(int x, int y, ItemSO item)
    {
        ClearTile(x, y);
        if (item != null && _tileDictionary.ContainsKey(item))
        {
            Vector3 position = new(x, y, 0);
            GameObject newItem = Instantiate(_tileDictionary[item], position, Quaternion.identity, boardParent);
            _visualBoard[x, y] = newItem;
        }
    }

}