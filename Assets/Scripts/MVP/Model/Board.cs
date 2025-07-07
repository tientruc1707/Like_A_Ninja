
using System;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Health = 1,
    Mana = 2,
    Shuriken = 3,
    Coin = 4
}

public enum SpecialItemType
{
    LineHorizontal = 1,
    LineVertical = 2,
    AllOfItemType = 3
}

/// <summary>
/// Handles the logical state and rules of the board.
/// </summary>
public class Board : MonoBehaviour
{
    public LevelSO level;
    public event Action<int, int> OnTileClear;
    public event Action<int, int, ItemSO> OnTileUpdate;

    private List<ItemSO> _items;
    private int _row;
    private int _col;
    private ItemSO[,] _itemBoard;

    private CharacterController _character;
    private HealthPresenter _health;
    private ManaPresenter _mana;
    public float warningHealth = 40f;

    public int Row => _row;
    public int Col => _col;
    public ItemSO[,] ItemBoard => _itemBoard;

    public void InitializeBoard()
    {
        _row = level.width;
        _col = level.height;
        _items = level.itemList;
        _itemBoard = new ItemSO[_row, _col];

        _character = GetComponent<CharacterController>();
        _health = GetComponent<HealthPresenter>();
        _mana = GetComponent<ManaPresenter>();
    }

    public void GenerateBoard()
    {
        for (int x = 0; x < _row; x++)
        {
            for (int y = 0; y < _col; y++)
            {
                ItemSO item = GenerateRandomItem(x, y);
                _itemBoard[x, y] = item;
            }
        }
    }

    private ItemSO GenerateRandomItem(int x, int y)
    {
        List<ItemSO> availableItems = new(_items);

        if (x >= 2)
        {
            ItemSO leftType = _itemBoard[x - 1, y];
            if (leftType != null && leftType.itemType == _itemBoard[x - 2, y]?.itemType)
                availableItems.Remove(leftType);
        }
        if (y >= 2)
        {
            ItemSO upType = _itemBoard[x, y - 1];
            if (upType != null && upType.itemType == _itemBoard[x, y - 2]?.itemType)
                availableItems.Remove(upType);
        }

        if (availableItems.Count == 0)
        {
            Debug.LogWarning($"No available items to place at ({x},{y})");
            return _items[0];
        }
        return availableItems[UnityEngine.Random.Range(0, availableItems.Count)];
    }

    public bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < _row && y >= 0 && y < _col;
    }

    public int GetItemSOIndex(int x, int y)
    {
        if (IsValidPosition(x, y))
        {
            ItemSO target = _itemBoard[x, y];
            return _items.IndexOf(target);
        }
        return 0;
    }

    public bool IsValidMove(int startX, int startY, int endX, int endY)
    {
        if (!IsValidPosition(startX, startY) || !IsValidPosition(endX, endY)) return false;

        int dx = Mathf.Abs(startX - endX);
        int dy = Mathf.Abs(startY - endY);
        if (!((dx == 1 && dy == 0) || (dx == 0 && dy == 1))) return false;

        ItemSO[,] boardClone = (ItemSO[,])_itemBoard.Clone();
        (boardClone[endX, endY], boardClone[startX, startY]) = (boardClone[startX, startY], boardClone[endX, endY]);

        if (!CheckMatchesAfterSwap(boardClone)) return false;

        ItemSO item = GetItem(boardClone, startX, startY, endX, endY);
        if (_health != null && _health.GetCurrentHealth() <= warningHealth && item.itemType != ItemType.Health)
            return false;

        return true;
    }

    private bool CheckMatchesAfterSwap(ItemSO[,] tempBoard)
    {
        for (int x = 0; x < _row; x++)
        {
            for (int y = 0; y < _col - 2; y++)
            {
                if (tempBoard[x, y] != null && tempBoard[x, y] == tempBoard[x, y + 1] && tempBoard[x, y] == tempBoard[x, y + 2])
                    return true;
            }
        }
        for (int y = 0; y < _col; y++)
        {
            for (int x = 0; x < _row - 2; x++)
            {
                if (tempBoard[x, y] != null && tempBoard[x, y] == tempBoard[x + 1, y] && tempBoard[x, y] == tempBoard[x + 2, y])
                    return true;
            }
        }
        return false;
    }

    private ItemSO GetItem(ItemSO[,] tempBoard, int startX, int startY, int endX, int endY)
    {
        ItemSO itemType = tempBoard[startX, startY];

        for (int x = 0; x < _row - 2; x++)
        {
            if (tempBoard[startX, startY] == itemType && tempBoard[startX, startY + 1] == itemType && tempBoard[startX, startY + 2] == itemType)
                return itemType;
            if (tempBoard[endX, endY] == itemType && tempBoard[endX, endY + 1] == itemType && tempBoard[endX, endY + 2] == itemType)
                return itemType;
        }
        for (int y = 0; y < _col - 2; y++)
        {
            if (tempBoard[startX, startY] == itemType && tempBoard[startX + 1, startY] == itemType && tempBoard[startX + 2, startY] == itemType)
                return itemType;
            if (tempBoard[endX, endY] == itemType && tempBoard[endX + 1, endY] == itemType && tempBoard[endX + 2, endY] == itemType)
                return itemType;
        }
        return null;
    }

    public void Swap(int startX, int startY, int endX, int endY)
    {
        (_itemBoard[endX, endY], _itemBoard[startX, startY]) = (_itemBoard[startX, startY], _itemBoard[endX, endY]);
        ClearMatches();
    }

    public void ClearMatches()
    {
        bool hasMatches;
        do
        {
            hasMatches = false;
            // Horizontal matches
            for (int x = 0; x < _row; x++)
            {
                for (int y = 0; y < _col - 2; y++)
                {
                    if (_itemBoard[x, y] != null && _itemBoard[x, y] == _itemBoard[x, y + 1] && _itemBoard[x, y] == _itemBoard[x, y + 2])
                    {
                        PerformItemTypeValue(_itemBoard[x, y]);
                        _itemBoard[x, y] = null;
                        OnTileClear?.Invoke(x, y);
                        _itemBoard[x, y + 1] = null;
                        OnTileClear?.Invoke(x, y + 1);
                        _itemBoard[x, y + 2] = null;
                        OnTileClear?.Invoke(x, y + 1);
                        hasMatches = true;
                    }
                }
            }
            // Vertical matches
            for (int y = 0; y < _col; y++)
            {
                for (int x = 0; x < _row - 2; x++)
                {
                    if (_itemBoard[x, y] != null && _itemBoard[x, y] == _itemBoard[x + 1, y] && _itemBoard[x, y] == _itemBoard[x + 2, y])
                    {
                        PerformItemTypeValue(_itemBoard[x, y]);
                        _itemBoard[x, y] = null;
                        OnTileClear?.Invoke(x, y);
                        _itemBoard[x + 1, y] = null;
                        OnTileClear?.Invoke(x + 1, y);
                        _itemBoard[x + 2, y] = null;
                        OnTileClear?.Invoke(x + 2, y);
                        hasMatches = true;
                    }
                }
            }
            // Drop items and fill empty spaces
            if (hasMatches)
            {
                for (int y = 0; y < _col; y++)
                {
                    int emptyRow = _row - 1;
                    for (int x = _row - 1; x >= 0; x--)
                    {
                        if (_itemBoard[x, y] != null)
                        {
                            _itemBoard[emptyRow, y] = _itemBoard[x, y];
                            if (emptyRow != x)
                            {
                                // Visual update handled by BoardView
                                OnTileUpdate?.Invoke(emptyRow, y, _itemBoard[x, y]);
                                _itemBoard[x, y] = null;
                            }
                            emptyRow--;
                        }
                    }
                    // Generate new items in empty tiles
                    for (int x = emptyRow; x >= 0; x--)
                    {
                        ItemSO item = GenerateRandomItem(x, y);
                        _itemBoard[x, y] = item;
                        OnTileUpdate?.Invoke(x, y, item);
                    }
                }
            }
        } while (hasMatches);
    }

    private void PerformItemTypeValue(ItemSO item)
    {
        switch (item.itemType)
        {
            case ItemType.Health:
                _health?.IncreaseHealth(item.value);
                break;
            case ItemType.Mana:
                _mana?.IncreaseMana(item.value);
                break;
            case ItemType.Shuriken:
                _character?.GetComponent<CharacterPresenter>()?.Attack();
                break;
            default:
                Debug.Log("Invalid item");
                break;
        }
    }

    public float GetHP() => _health.GetCurrentHealth();
    public float GetMana() => _mana.GetCurrentMana();
    public float GetDamage() => 10;
}