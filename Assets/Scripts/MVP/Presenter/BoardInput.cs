using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class BoardInput : MonoBehaviour
{
    private Match3Board m_Board;
    private BoardView m_BoardView;
    private Dictionary<(int, int), TileSelector> tileDict;
    private bool m_Swapping;
    private CharacterPresenter m_Charactor;
    private CharacterPresenter m_Player, m_Enemy;


    void OnEnable()
    {
        GameManager.Instance.OnClearCell += UpdateCharactorStat;
        InputSystem.Instance.SwipeDetected += HandleSwipe;
    }

    void OnDisable()
    {
        GameManager.Instance.OnClearCell -= UpdateCharactorStat;
        InputSystem.Instance.SwipeDetected -= HandleSwipe;
    }

    private void Start()
    {
        m_Board = GetComponent<Match3Board>();
        m_BoardView = GetComponent<BoardView>();
        tileDict = m_BoardView.tileDict;
        m_Player = GameManager.Instance.GetPlayer().GetComponent<CharacterPresenter>();
        m_Enemy = GameManager.Instance.GetCurrentEnemy().GetComponent<CharacterPresenter>();

    }
    void Update()
    {
        if (GameManager.Instance.CurrentSide == TurnSide.LEFTTURN)
        {
            m_Charactor = m_Player;
        }
        else
        {
            m_Charactor = m_Enemy;
        }
    }

    private void HandleSwipe(Vector2 startPos, Vector2 direction)
    {
        if (m_Swapping) return;

        Vector2Int gridPos = ScreenToGridPosition(startPos);
        Vector2Int targetPos = gridPos + Vector2Int.RoundToInt(direction.normalized);

        if (m_Board.IsValidPosition(gridPos) && m_Board.IsValidPosition(targetPos))
        {
            m_Board.SwapCells(gridPos, targetPos);
            StartCoroutine(AnimatedSwap(gridPos, targetPos));
            m_Swapping = false;
            if (m_Board.MarkMatchedCells())
            {
                m_Board.ClearMatchedCells();
                m_Board.DropCells();
                m_Board.FillFromAbove();
            }
            else
            {
                m_Board.SwapCells(gridPos, targetPos);
                StartCoroutine(AnimatedSwap(gridPos, targetPos));
                m_Swapping = false;
            }
        }
        else
        {
            Debug.LogWarning($"Invalid swap positions: {gridPos} to {targetPos}");
        }
    }

    private Vector2Int ScreenToGridPosition(Vector2 startPos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            m_BoardView.GetComponent<RectTransform>(),
            startPos,
            Camera.main,
            out Vector2 worldPos);
        int row = Mathf.FloorToInt(worldPos.x / m_BoardView.tileSize);
        int col = Mathf.FloorToInt(worldPos.y / m_BoardView.tileSize);

        Debug.Log($"Screen position {startPos} converted to world position {worldPos}, grid position: ({row}, {col})");
        return new Vector2Int(row, col);
    }

    private void UpdateCharactorStat(int row, int col)
    {
        if (tileDict.TryGetValue((row, col), out var tile))
        {
            int value = tile.ItemType.value * (m_Board.GetSpecialType(row, col) + 1);
            switch (tile.ItemType.itemType)
            {
                case ItemType.Health:
                    m_Charactor.GetComponent<HealthPresenter>().IncreaseHealth(value);
                    break;
                case ItemType.Mana:
                    m_Charactor.GetComponent<ManaPresenter>().IncreaseMana(value);
                    break;
                case ItemType.Shuriken:
                    m_Charactor.Attack();
                    break;
                default:
                    Debug.LogWarning($"Unknown item type: {tile.ItemType.itemType}");
                    break;
            }

        }
        else
        {
            Debug.LogWarning($"Tile at ({row}, {col}) not found in tileDict.");
        }
    }

    public System.Collections.IEnumerator AnimatedSwap(Vector2Int from, Vector2Int to)
    {
        m_Swapping = true;
        GameObject fromTile = tileDict[(from.x, from.y)].gameObject;
        GameObject toTile = tileDict[(to.x, to.y)].gameObject;

        Vector3 fromPos = m_BoardView.GridToWorldPosition(from);
        Vector3 toPos = m_BoardView.GridToWorldPosition(to);

        float elapsedTime = 0f;
        float moveDuration = 0.2f;
        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            fromTile.transform.position = Vector3.Lerp(fromPos, toPos, t);
            toTile.transform.position = Vector3.Lerp(toPos, fromPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fromTile.transform.position = toPos;
        toTile.transform.position = fromPos;
    }


}
