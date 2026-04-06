namespace Ejercicio3_Laberinto.Maze;

/// <summary>
/// Representa el tablero del laberinto (matriz de celdas).
/// 0 = camino libre, 1 = pared.
/// Encapsula toda la lógica relacionada con la estructura del laberinto (SRP).
/// </summary>
public class MazeGrid
{
    private readonly int[,] _grid;

    public int Rows { get; }
    public int Cols { get; }
    public Position Start { get; }
    public Position Goal { get; }

    public MazeGrid(int[,] grid, Position start, Position goal)
    {
        _grid = grid;
        Rows = grid.GetLength(0);
        Cols = grid.GetLength(1);
        Start = start;
        Goal = goal;
    }

    /// <summary>
    /// Verifica si una posición está dentro del laberinto y es un camino libre.
    /// </summary>
    public bool IsWalkable(Position pos) =>
        pos.Row >= 0 && pos.Row < Rows &&
        pos.Col >= 0 && pos.Col < Cols &&
        _grid[pos.Row, pos.Col] == 0;

    /// <summary>
    /// Verifica si la posición es la meta.
    /// </summary>
    public bool IsGoal(Position pos) => pos == Goal;

    /// <summary>
    /// Imprime el laberinto en consola.
    /// #          = pared
    /// . (blanco) = camino libre
    /// . (amarillo) = camino solución
    /// S (verde)  = inicio
    /// G (rojo)   = meta
    /// </summary>
    public void Print(IEnumerable<Position>? path = null)
    {
        var pathSet = path != null ? new HashSet<Position>(path) : [];

        for (int r = 0; r < Rows; r++)
        {
            Console.Write("  ");
            for (int c = 0; c < Cols; c++)
            {
                var pos = new Position(r, c);

                if (pos == Start)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" S ");
                }
                else if (pos == Goal)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" G ");
                }
                else if (pathSet.Contains(pos))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(" . ");
                }
                else if (_grid[r, c] == 1)
                {
                    Console.ResetColor();
                    Console.Write(" # ");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" . ");
                }

                Console.ResetColor();
            }
            Console.WriteLine();
        }

        Console.ResetColor();
    }

    /// <summary>
    /// Retorna el laberinto del enunciado del ejercicio (4x4).
    /// </summary>
    public static MazeGrid CreateExample()
    {
        int[,] grid =
        {
            { 0, 0, 1, 0 },
            { 1, 0, 1, 0 },
            { 0, 0, 0, 0 },
            { 1, 1, 0, 1 }
        };
        return new MazeGrid(grid, new Position(0, 0), new Position(3, 2));
    }
}
