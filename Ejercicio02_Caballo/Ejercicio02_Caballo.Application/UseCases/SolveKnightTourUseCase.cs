using Ejercicio02_Caballo.Domain.Entities;
using Ejercicio02_Caballo.Domain.Exceptions;
using Ejercicio02_Caballo.Domain.Interfaces;
using Ejercicio02_Caballo.Domain.ValueObjects;

namespace Ejercicio02_Caballo.Application.UseCases;

public class SolveKnightTourUseCase
{
    private readonly IKnightTourSolver _solver;

    public SolveKnightTourUseCase(IKnightTourSolver solver)
    {
        _solver = solver;
    }

    public KnightTourResult Execute(int rows, int columns, int startX, int startY)
    {
        var board = new Board(rows, columns);
        var startPosition = new Position(startX, startY);

        if (!board.IsValidPosition(startPosition))
        {
            throw new DomainValidationException("La posición inicial está fuera de los límites del tablero.");
        }

        return _solver.Solve(board, startPosition);
    }
}