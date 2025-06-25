using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;

    private Item _item;
    public Item Item
    {
        get => _item;
        set
        {
            if (_item == value) return; // Avoid unnecessary updates
            _item = value;
            if (icon != null)
            {
                icon.sprite = _item.sprite;
            }
        }
    }
    public Image icon;
    public Button button;

    public Tile Top => y > 0 ? Board.Instance.Tiles[x, y - 1] : null;
    public Tile Bot => y < Board.Instance.boardSize - 1 ? Board.Instance.Tiles[x, y + 1] : null;
    public Tile Left => x > 0 ? Board.Instance.Tiles[x - 1, y] : null;
    public Tile Right => x < Board.Instance.boardSize - 1 ? Board.Instance.Tiles[x + 1, y] : null;

    public Tile[] Neighbors => new Tile[]
    {
        Top,
        Bot,
        Left,
        Right
    };

    private void Start()
    {
        button.onClick.AddListener(() => Board.Instance.Select(this));
    }

    public List<Tile> GetConnectionsTile(List<Tile> visited = null)
    {
        var result = new List<Tile> { this, };

        if (visited == null)
        {
            visited = new List<Tile> { this, };
        }
        else
        {
            visited.Add(this);
        }

        foreach (Tile neighbor in Neighbors)
        {
            if (neighbor == null || visited.Contains(neighbor) || neighbor.Item != Item)
            {
                continue;
            }
            result.AddRange(neighbor.GetConnectionsTile(visited));
        }

        return result;
    }

}
