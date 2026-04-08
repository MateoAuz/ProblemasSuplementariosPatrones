using Ejercicio02_Caballo.Domain.Entities;
using Ejercicio02_Caballo.Domain.ValueObjects;

namespace Ejercicio02_Caballo.Domain.Interfaces;

public interface IKnightTourSolver
{
    KnightTourResult Solve(Board board, Position startPosition);
}