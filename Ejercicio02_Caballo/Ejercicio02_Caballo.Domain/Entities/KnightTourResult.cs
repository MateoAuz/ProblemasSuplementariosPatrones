using Ejercicio02_Caballo.Domain.ValueObjects;

namespace Ejercicio02_Caballo.Domain.Entities;

public class KnightTourResult
{
    public bool IsSuccessful { get; }
    public IReadOnlyList<Position> Path { get; }

    private KnightTourResult(bool isSuccessful, IReadOnlyList<Position> path)
    {
        IsSuccessful = isSuccessful;
        Path = path;
    }

    public static KnightTourResult Success(IReadOnlyList<Position> path)
    {
        return new KnightTourResult(true, path);
    }

    public static KnightTourResult Failure()
    {
        return new KnightTourResult(false, new List<Position>().AsReadOnly());
    }
}