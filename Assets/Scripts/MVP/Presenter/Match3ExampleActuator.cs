
using Unity.MLAgents.Integrations.Match3;

public class Match3ExampleActuator : Match3Actuator
{
    private Match3Board m_Board;
    public Match3Board Board => m_Board;
    public Match3ExampleActuator(Match3Board board, bool forceHeuristic, int seed, string name)
     : base(board, forceHeuristic, seed, name)
    {
        m_Board = board;
    }

    protected override int EvalMovePoints(Move move)
    {
        var pointsByType = new[] { m_Board.BasicCellPoint, m_Board.SpecialCell1Point, m_Board.SpecialCell2Point };
        var moveVal = m_Board.GetCellType(move.Row, move.Column);
        var moveSpecial = m_Board.GetSpecialType(move.Row, move.Column);
        var (otherRow, otherCol) = move.OtherCell();
        var newVal = m_Board.GetCellType(otherRow, otherCol);
        var newSpecial = m_Board.GetSpecialType(otherRow, otherCol);

        //Count basic point
        int movePoint = EvaluateHalfMove(otherRow, otherCol, moveVal, moveSpecial, move.Direction, pointsByType);
        int otherPoint = EvaluateHalfMove(move.Row, move.Column, newVal, newSpecial, move.OtherDirection(), pointsByType);

        //make potential special
        int bonusPoint = 0;
        if (m_Board.CurrentEnemyHealth() <= 40)
        {
            if (moveVal == (int)ItemType.Health || newVal == (int)ItemType.Health)
            {
                bonusPoint += 100; // Bonus for health
            }
        }
        else if (m_Board.CurrentEnemyMana() == 0)
        {
            if (moveVal == (int)ItemType.Mana || newVal == (int)ItemType.Mana)
            {
                bonusPoint += 70; // Bonus for mana
            }
        }
        else if (m_Board.AbleToUseSkill())
        {
            bonusPoint += 50; // Bonus for being able to use skill
        }
        else if (moveSpecial > 0 || newSpecial > 0)
        {
            bonusPoint += 10; // Bonus for special cells
        }
        return movePoint + otherPoint + bonusPoint;
    }

    private int EvaluateHalfMove(int newRow, int newCol, int newValue, int newSpecial, Direction direction, int[] pointsByType)
    {
        var currentBoardSize = m_Board.GetCurrentBoardSize();
        int matchedLeft = 0, matchedRight = 0, matchedUp = 0, matchedDown = 0;
        int scoreLeft = 0, scoreRight = 0, scoreUp = 0, scoreDown = 0;

        if (direction != Direction.Right)
        {
            for (var c = newCol - 1; c >= 0; c--)
            {
                if (m_Board.GetCellType(newRow, c) == newValue)
                {
                    matchedLeft++;
                    scoreLeft += pointsByType[m_Board.GetSpecialType(newRow, c)];
                }
                else break;
            }
        }

        if (direction != Direction.Left)
        {
            for (var c = newCol + 1; c < currentBoardSize.Columns; c++)
            {
                if (m_Board.GetCellType(newRow, c) == newValue)
                {
                    matchedRight++;
                    scoreRight += pointsByType[m_Board.GetSpecialType(newRow, c)];
                }
                else break;
            }
        }

        if (direction != Direction.Down)
        {
            for (var r = newRow + 1; r < currentBoardSize.Rows; r++)
            {
                if (m_Board.GetCellType(r, newCol) == newValue)
                {
                    matchedUp++;
                    scoreUp += pointsByType[m_Board.GetSpecialType(r, newCol)];
                }
                else break;
            }
        }

        if (direction != Direction.Up)
        {
            for (var r = newRow - 1; r >= 0; r--)
            {
                if (m_Board.GetCellType(r, newCol) == newValue)
                {
                    matchedDown++;
                    scoreDown += pointsByType[m_Board.GetSpecialType(r, newCol)];
                }
                else break;
            }
        }

        if (matchedUp + matchedDown >= 2 || matchedLeft + matchedRight >= 2)
        {
            var totalScore = pointsByType[newSpecial];
            if (matchedUp + matchedDown >= 2)
            {
                totalScore += scoreUp + scoreDown;
            }
            if (matchedLeft + matchedRight >= 2)
            {
                totalScore += scoreLeft + scoreRight;
            }
            return totalScore;
        }
        return 0;
    }


}
