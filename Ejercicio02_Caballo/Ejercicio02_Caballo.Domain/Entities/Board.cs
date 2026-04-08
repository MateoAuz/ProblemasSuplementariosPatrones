using Ejercicio02_Caballo.Domain.Exceptions;
using Ejercicio02_Caballo.Domain.ValueObjects;

namespace Ejercicio02_Caballo.Domain.Entities;

public class Board
{
    public int Rows { get; }
    public int Columns { get; }
    public int TotalCells => Rows * Columns;

    public Board(int rows, int columns)
    {
        if (rows <= 0 || columns <= 0)
        {
            throw new DomainValidationException("El tamaño del tablero debe ser mayor a cero.");
        }

        // We could limit the max board size to avoid extreme calculations even with heuristics.
        // But 8x8, 10x10, etc are perfectly fine.

        Rows = rows;
        Columns = columns;
    }

    public bool IsValidPosition(Position position)
    {
        return position.X >= 0 && position.X < Columns &&
               position.Y >= 0 && position.Y < Rows;
    }
}