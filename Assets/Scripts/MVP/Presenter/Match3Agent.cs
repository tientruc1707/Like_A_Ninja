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
        if (board == null) return;

        board.InitializeBoardSize();
        boardSize = board.GetCurrentBoardSize();
        if (boardSize.Rows == 0 || boardSize.Columns == 0) return;

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
        if (board.AbleToUseSkill())
        {
            StartCoroutine(WaitToRequestDecision(5f));
            AddReward(1f);
        }
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

    private System.Collections.IEnumerator WaitToRequestDecision(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
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
            board.InitSettled();
            attemp++;
        }

        if (attemp >= maxAttemps)
        {
            EpisodeInterrupted();
            return;
        }
        // If it's the left turn, we don't need to request a decision
        if (GameManager.Instance.CurrentSide == TurnSide.LEFTTURN) return;
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
                nextState = State.FindMatches;
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
                    board.InitSettled();
                    attemp++;
                }
                if (attemp >= maxAttemps)
                {
                    EpisodeInterrupted();
                    return;
                }
                // If it's the left turn, we don't need to request a decision
                if (GameManager.Instance.CurrentSide == TurnSide.LEFTTURN) return;
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