
using System;
using Unity.MLAgents;
using Unity.MLAgents.Integrations.Match3;
using UnityEngine;

public class Match3Board : AbstractBoard
{
    [SerializeField] private Board _board;
    [SerializeField] private BoardView _boardView;

    private Agent _agent;

    private void Awake()
    {
        _agent = GetComponent<Agent>();
    }
    private void OnEnable()
    {
        _boardView.Bind(_board);
        EventSystem.Instance.RegisterListener(StringConstant.EVENT.CHANG_SIDE, OnChangeSide);
    }
    public void Start()
    {
        _board.InitializeBoard();
        _board.GenerateBoard();
        _boardView.InitializeView(_board);
        _boardView.RenderBoard(_board);
    }
    private void OnChangeSide()
    {
        if (GameManager.Instance.CurrentSide == TurnSide.RIGHTTURN)
            _agent.RequestDecision();
    }

    private void OnDisable()
    {
        _boardView.Unbind(_board);
        EventSystem.Instance.UnregisterListener(StringConstant.EVENT.CHANG_SIDE, OnChangeSide);
    }
    public override int GetCellType(int row, int col)
    {
        return _board.GetItemSOIndex(row, col);
    }

    public override BoardSize GetMaxBoardSize()
    {
        LevelSO levelSO = _board.level;

        return new BoardSize
        {
            Rows = levelSO.width,
            Columns = levelSO.height,
            NumCellTypes = levelSO.itemList.Count,
            NumSpecialTypes = 3
        };
    }

    public override int GetSpecialType(int row, int col)
    {
        return 0;
    }

    public override bool IsMoveValid(Move m)
    {
        int startX = m.Column;
        int startY = m.Row;
        var moveEnd = m.OtherCell();
        int endX = moveEnd.Column;
        int endY = moveEnd.Row;
        return _board.IsValidMove(startX, startY, endX, endY);
    }

    public override bool MakeMove(Move m)
    {
        int startX = m.Column;
        int startY = m.Row;
        var moveEnd = m.OtherCell();
        int endX = moveEnd.Column;
        int endY = moveEnd.Row;
        if (_board.IsValidMove(startX, startY, endX, endY))
        {
            EventSystem.Instance.TriggerEvent(StringConstant.EVENT.PAUSE_TIMER);
            _board.Swap(startX, startY, endX, endY);
            EventSystem.Instance.TriggerEvent(StringConstant.EVENT.PAUSE_TIMER);
            EventSystem.Instance.TriggerEvent(StringConstant.EVENT.CHANG_SIDE);
            return true;
        }
        else
        {
            return false;
        }
    }
}
