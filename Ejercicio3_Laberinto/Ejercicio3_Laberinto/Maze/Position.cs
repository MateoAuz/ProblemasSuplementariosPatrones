namespace Ejercicio3_Laberinto.Maze;

/// <summary>
/// Representa una posición (fila, columna) en el laberinto.
/// Estructura inmutable para garantizar integridad de datos.
/// </summary>
public readonly struct Position : IEquatable<Position>
{
    public int Row { get; }
    public int Col { get; }

    public Position(int row, int col)
    {
        Row = row;
        Col = col;
    }

    /// <summary>Movimientos posibles: arriba, abajo, izquierda, derecha.</summary>
    public static readonly Position[] Directions =
    [
        new(-1, 0), // arriba
        new( 1, 0), // abajo
        new( 0,-1), // izquierda
        new( 0, 1)  // derecha
    ];

    public Position Move(Position direction) =>
        new(Row + direction.Row, Col + direction.Col);

    public bool Equals(Position other) => Row == other.Row && Col == other.Col;

    public override bool Equals(object? obj) => obj is Position p && Equals(p);

    public override int GetHashCode() => HashCode.Combine(Row, Col);

    public static bool operator ==(Position a, Position b) => a.Equals(b);

    public static bool operator !=(Position a, Position b) => !a.Equals(b);

    public override string ToString() => $"({Row},{Col})";
}
