namespace Ejercicio02_Caballo.Domain.ValueObjects;

public readonly record struct MoveOffset(int DeltaX, int DeltaY)
{
    public static IReadOnlyList<MoveOffset> KnightMoves { get; } = new List<MoveOffset>
    {
        new(-2, -1), new(-2, 1),
        new(-1, -2), new(-1, 2),
        new(1, -2), new(1, 2),
        new(2, -1), new(2, 1)
    }.AsReadOnly();
}