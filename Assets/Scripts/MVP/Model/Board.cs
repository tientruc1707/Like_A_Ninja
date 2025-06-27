
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;


public class Board : MonoBehaviour
{

    public Tile[,] Tiles { get; private set; }
    private Row[] rows;
    public int boardSize;
    private readonly float _tweenDuration = 0.25f;


    private void Start()
    {
        rows = GetComponentsInChildren<Row>();
        boardSize = rows.Length;
        Tiles = new Tile[boardSize, boardSize];

        InitializeBoard();
        if (Connectable())
            RemoveConnectionIfMatches();
    }

    private void InitializeBoard()
    {
        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                Tile tile = rows[y].tiles[x];

                tile.x = x;
                tile.y = y;

                tile.Item = AssetLibrary.Items[Random.Range(0, AssetLibrary.Items.Length)];

                Tiles[x, y] = tile;
            }

        }

    }

    public async Task Swap(Tile tile1, Tile tile2)
    {
        Image icon1 = tile1.icon;
        Image icon2 = tile2.icon;

        Transform icon1transform = icon1.transform;
        Transform icon2transform = icon2.transform;

        var sequence = DOTween.Sequence();
        sequence.Join(icon1transform.DOMove(icon2transform.position, _tweenDuration).SetEase(Ease.InOutQuad))
                .Join(icon2transform.DOMove(icon1transform.position, _tweenDuration).SetEase(Ease.InOutQuad));
        await sequence.Play().AsyncWaitForCompletion();

        //Set parent for icons
        icon1transform.SetParent(tile2.transform);
        icon2transform.SetParent(tile1.transform);

        // Set the icons 
        tile1.icon = icon2;
        tile2.icon = icon1;

        // Swap the items
        (tile2.Item, tile1.Item) = (tile1.Item, tile2.Item);

    }

    public bool Connectable()
    {
        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                if (Tiles[x, y].GetConnectionsTile().Count >= 3)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public async void RemoveConnectionIfMatches()
    {
        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                Tile tile = Tiles[x, y];
                List<Tile> connections = tile.GetConnectionsTile();
                if (connections.Count < 3) continue;

                var removeSequence = DOTween.Sequence();

                foreach (Tile connectedTile in connections)
                {
                    removeSequence.Join(connectedTile.icon.transform.DOScale(Vector3.zero, _tweenDuration));
                }

                await removeSequence.Play().AsyncWaitForCompletion();

                var generateSequence = DOTween.Sequence();

                foreach (Tile connectedTile in connections)
                {
                    connectedTile.Item = AssetLibrary.Items[Random.Range(0, AssetLibrary.Items.Length)];
                    generateSequence.Join(connectedTile.icon.transform.DOScale(Vector3.one, _tweenDuration));
                }

                await generateSequence.Play().AsyncWaitForCompletion();
            }
        }
    }
    public void ResetBoard()
    {
        foreach (Row row in rows)
        {
            foreach (Tile tile in row.tiles)
            {
                tile.Item = AssetLibrary.Items[Random.Range(0, AssetLibrary.Items.Length)];
            }

        }

    }

}
