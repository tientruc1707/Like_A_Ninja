using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;
    public Board board;
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

    public Tile Top => y > 0 ? board.Tiles[x, y - 1] : null;
    public Tile Bot => y < board.boardSize - 1 ? board.Tiles[x, y + 1] : null;
    public Tile Left => x > 0 ? board.Tiles[x - 1, y] : null;
    public Tile Right => x < board.boardSize - 1 ? board.Tiles[x + 1, y] : null;

    public Tile[] Neighbors => new Tile[]
    {
        Top,
        Bot,
        Left,
        Right
    };

    private void Start()
    {
        board = GetComponentInParent<Board>();
        button.onClick.AddListener(() => board.GetComponent<BoardPresenter>().Select(this));
    }

    public List<Tile> GetConnectionsTile(HashSet<Tile> visited = null)
    {
        if (visited == null)
        {
            visited = new HashSet<Tile>();
        }

        var result = new List<Tile>();

        if (!visited.Contains(this))
        {
            visited.Add(this);
            result.Add(this);

            foreach (Tile neighbor in Neighbors)
            {
                if (neighbor == null || visited.Contains(neighbor) || neighbor.Item != Item)
                {
                    continue;
                }
                result.AddRange(neighbor.GetConnectionsTile(visited));
            }
        }

        return result;
    }

}
