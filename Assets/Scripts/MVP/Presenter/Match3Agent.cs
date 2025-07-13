using System;
using Unity.MLAgents;
using Unity.MLAgents.Integrations.Match3;
using UnityEngine;

public class Match3Agent : Agent
{
    enum State
    {
        Invalid = -1,
        FindMatches = 0,
        ClearMatched = 1,
        Drop = 2,
        FillEmpty = 3,
        WaitForMove = 4
    }

    [HideInInspector]
    public Match3Board board;

    public float MoveTime = 1.0f;
    public int MaxMove = 500;

    State currentState = State.WaitForMove;
    float timeUntilMove;
    private int moveMade;
    private const float rewardMultiplier = 0.01f;
    BoardSize boardSize;

    override protected void Awake()
    {
        base.Awake();
        board = GetComponent<Match3Board>();
        if (board == null)
        {
            Debug.LogError("Match3Board component not found on this GameObject!");
            return;
        }
        board.InitializeBoardSize();
        boardSize = board.GetCurrentBoardSize();
        if (boardSize.Rows == 0 || boardSize.Columns == 0)
        {
            Debug.LogError("Board size is invalid!");
            return;
        }
        board.InitSettled();
    }

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        currentState = State.FindMatches;
        timeUntilMove = MoveTime;
        moveMade = 0;
    }

    void FixedUpdate()
    {
        var useFastUpdate = Academy.Instance.IsCommunicatorOn;
        if (useFastUpdate)
        {
            FastUpdate();
        }
        else
        {
            UpdateState();
        }
        if (moveMade > MaxMove)
        {
            EpisodeInterrupted();
        }
    }

    void FastUpdate()
    {
        int maxAttemps = 10;
        int attemp = 0;
        while (true)
        {
            var hasMatched = board.MarkMatchedCells();
            if (!hasMatched)
            {
                break;
            }
            var pointEarned = board.ClearMatchedCells();
            AddReward(rewardMultiplier * pointEarned);
            board.DropCells();
            board.FillFromAbove();
        }

        while (!HasValidMove() && attemp < maxAttemps)
        {
            Debug.Log($"Attempt {attemp}: No valid moves, reinitializing board");
            board.InitSettled();
            attemp++;
        }

        if (attemp >= maxAttemps)
        {
            Debug.LogWarning("Cannot generate valid moves, ending episode");
            EpisodeInterrupted();
            return;
        }
        RequestDecision();
        moveMade++;
    }

    void UpdateState()
    {
        timeUntilMove -= Time.deltaTime;
        if (timeUntilMove > 0.0f)
        {
            return;
        }

        timeUntilMove = MoveTime;
        State nextState;
        switch (currentState)
        {
            case State.FindMatches:
                var hasMatched = board.MarkMatchedCells();
                nextState = hasMatched ? State.ClearMatched : State.WaitForMove;
                if (nextState == State.WaitForMove)
                {
                    moveMade++;
                }
                break;
            case State.ClearMatched:
                var pointEarned = board.ClearMatchedCells();
                AddReward(rewardMultiplier * pointEarned);
                nextState = State.Drop;
                break;
            case State.Drop:
                board.DropCells();
                nextState = State.FillEmpty;
                break;
            case State.FillEmpty:
                board.FillFromAbove();
                nextState = State.FindMatches; // Sửa: quay lại FindMatches
                break;
            case State.WaitForMove:
                int maxAttemps = 10;
                int attemp = 0;
                while (attemp < maxAttemps)
                {
                    bool hasValidMove = HasValidMove();
                    if (hasValidMove)
                    {
                        break;
                    }
                    Debug.Log($"Attempt {attemp}: No valid moves, reinitializing board");
                    board.InitSettled();
                    attemp++;
                }
                if (attemp >= maxAttemps)
                {
                    Debug.LogWarning("Cannot generate valid moves, ending episode");
                    EpisodeInterrupted();
                    return;
                }
                RequestDecision();
                nextState = State.FindMatches;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        currentState = nextState;
    }

    bool HasValidMove()
    {
        foreach (var move in board.ValidMoves())
        {
            return true;
        }
        return false;
    }
}