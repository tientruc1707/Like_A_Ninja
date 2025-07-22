
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System.Linq;
using System.Collections;


public class BoardUI : MonoBehaviour
{
    public Dictionary<(int, int), TileSelector> tileDict = new();
    public GameObject tilePrefab;
    public int Rows;
    public int Cols;

    [SerializeField] private BoardModel m_BoardModel;
    private readonly float m_tweenDuration = 0.5f;

    float m_CellSize;
    private bool m_init = false;
    private void Start()
    {
        Rows = m_BoardModel.rows;
        Cols = m_BoardModel.columns;

        InitializeTileDict();
    }

    private void InitializeTileDict()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                var obj = Instantiate(tilePrefab, transform.position, Quaternion.identity, transform);
                obj.name = $"R{i} C{j}";
                tileDict.Add((i, j), obj.GetComponent<TileSelector>());
            }
        }
        CalculateCellSize();
        m_init = true;
    }

    private void CalculateCellSize()
    {
        var boardAreaSize = GetComponent<RectTransform>().rect.size;
        m_CellSize = Mathf.Min(boardAreaSize.x / m_BoardModel.columns, boardAreaSize.y / m_BoardModel.rows);
    }

    void Update()
    {
        if (!m_init)
        {
            InitializeTileDict();
        }
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                var cellType = -1;
                var specialType = 0;

                if (m_BoardModel.Cells != null && i < Rows && j < Cols)
                {
                    cellType = m_BoardModel.GetCellType(i, j);
                    specialType = m_BoardModel.GetSpecialType(i, j);
                }

                var pos = new Vector3(j, i, 0) * m_CellSize + new Vector3(m_CellSize / 2, m_CellSize / 2);
                tileDict[(i, j)].transform.position = transform.TransformPoint(pos);
                tileDict[(i, j)].SetPositionInBoard(j, i);
                tileDict[(i, j)].GetComponent<RectTransform>().sizeDelta = new Vector2(m_CellSize, m_CellSize);
                tileDict[(i, j)].SetActiveTile(cellType, specialType);
            }
        }
    }

    public IEnumerator Swap(TileSelector tile1, TileSelector tile2)
    {

        Transform icon1transform = tile1.transform;
        Transform icon2transform = tile2.transform;

        var sequence = DOTween.Sequence();
        sequence.Join(icon1transform.DOMove(icon2transform.position, m_tweenDuration).SetEase(Ease.InOutQuad))
                .Join(icon2transform.DOMove(icon1transform.position, m_tweenDuration).SetEase(Ease.InOutQuad));

        yield return new WaitForSeconds(m_tweenDuration);
    }

    public IEnumerator RemoveMatchedCells(Vector2Int pos)
    {
        EventSystem.Instance.TriggerEvent(StringConstant.EVENT.PAUSE_TIMER);

        Transform cellTransform = tileDict[(pos.y, pos.x)].transform;
        cellTransform.DOScale(Vector3.zero, m_tweenDuration).SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(m_tweenDuration);

        EventSystem.Instance.TriggerEvent(StringConstant.EVENT.UNPAUSE_TIMER);
    }

    public IEnumerator DropCells(Vector2Int up, Vector2Int down)
    {
        TileSelector tileUp = tileDict[(up.x, up.y)];
        TileSelector tileDown = tileDict[(down.x, down.y)];

        tileUp.transform.DOMove(tileDown.transform.position, m_tweenDuration, true).SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(m_tweenDuration);
    }

    public IEnumerator FillEmptyCells(Vector2Int cell)
    {
        TileSelector tile = tileDict[(cell.x, cell.y)];

        tile.transform.DOScale(Vector3.one, m_tweenDuration).SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(m_tweenDuration);
    }

}
