using Ejercicio02_Caballo.Domain.Entities;
using Ejercicio02_Caballo.Domain.Interfaces;
using Ejercicio02_Caballo.Domain.ValueObjects;

namespace Ejercicio02_Caballo.Infrastructure.Services;

public class KnightTourSolver : IKnightTourSolver
{
    public KnightTourResult Solve(Board board, Position startPosition)
    {
        int[,] visited = new int[board.Columns, board.Rows];
        for (int i = 0; i < board.Columns; i++)
        {
            for (int j = 0; j < board.Rows; j++)
            {
                visited[i, j] = -1;
            }
        }

        var path = new List<Position>();
        
        // Start at 0th move
        visited[startPosition.X, startPosition.Y] = 0;
        path.Add(startPosition);

        if (SolveDFS(board, visited, startPosition, 1, path))
        {
            return KnightTourResult.Success(path.AsReadOnly());
        }

        return KnightTourResult.Failure();
    }

    private bool SolveDFS(Board board, int[,] visited, Position current, int moveCount, List<Position> path)
    {
        if (moveCount == board.TotalCells)
        {
            return true;
        }

        // Warnsdorff's Heuristic: order possible moves by the number of subsequent available moves
        var validMoves = GetValidMoves(board, visited, current);
        
        var movesWithDegrees = validMoves.Select(move => 
            new 
            { 
                Move = move, 
                Degree = GetValidMoves(board, visited, move).Count 
            })
            .OrderBy(m => m.Degree)
            .ToList();

        foreach (var nextMove in movesWithDegrees)
        {
            Position nextPosition = nextMove.Move;
            visited[nextPosition.X, nextPosition.Y] = moveCount;
            path.Add(nextPosition);

            if (SolveDFS(board, visited, nextPosition, moveCount + 1, path))
            {
                return true;
            }

            // Backtracking
            visited[nextPosition.X, nextPosition.Y] = -1;
            path.RemoveAt(path.Count - 1);
        }

        return false;
    }

    private List<Position> GetValidMoves(Board board, int[,] visited, Position current)
    {
        var validMoves = new List<Position>();

        foreach (var offset in MoveOffset.KnightMoves)
        {
            var nextPosition = new Position(current.X + offset.DeltaX, current.Y + offset.DeltaY);

            if (board.IsValidPosition(nextPosition) && visited[nextPosition.X, nextPosition.Y] == -1)
            {
                validMoves.Add(nextPosition);
            }
        }

        return validMoves;
    }
}