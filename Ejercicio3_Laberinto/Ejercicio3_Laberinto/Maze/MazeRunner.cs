namespace Ejercicio3_Laberinto.Maze;

/// <summary>
/// Orquesta la ejecución del laberinto con un solucionador específico.
/// Aplica el patrón Strategy: el algoritmo puede intercambiarse sin cambiar esta clase.
/// Principio OCP (Open/Closed): abierto para extensión (nuevos algoritmos), cerrado para modificación.
/// </summary>
public class MazeRunner
{
    private readonly IMazeSolver _solver;

    public MazeRunner(IMazeSolver solver)
    {
        _solver = solver;
    }

    /// <summary>
    /// Ejecuta la solución del laberinto y muestra los resultados en consola.
    /// </summary>
    public void Run(MazeGrid maze, string mazeLabel)
    {
        Console.WriteLine($"\n{'=',50}");
        Console.WriteLine($"  Laberinto: {mazeLabel}");
        Console.WriteLine($"  Algoritmo: {_solver.AlgorithmName}");
        Console.WriteLine($"{'=',50}");

        Console.WriteLine("\n[Laberinto original]");
        Console.Write("  ");
        Console.ForegroundColor = ConsoleColor.Green;  Console.Write("S"); Console.ResetColor(); Console.Write(" Inicio   ");
        Console.ForegroundColor = ConsoleColor.Red;    Console.Write("G"); Console.ResetColor(); Console.Write(" Meta   ");
        Console.Write("#"); Console.Write(" Pared   ");
        Console.ForegroundColor = ConsoleColor.White;  Console.Write("."); Console.ResetColor(); Console.Write(" Libre   ");
        Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("."); Console.ResetColor(); Console.WriteLine(" Solución");
        Console.WriteLine();
        maze.Print();

        var path = _solver.Solve(maze);

        if (path is null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n  No se encontró un camino desde el inicio hasta la meta.");
            Console.ResetColor();
            return;
        }

        Console.WriteLine($"\n[Camino encontrado - {path.Count} pasos]\n");
        maze.Print(path);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("\n  Ruta: ");
        Console.WriteLine(string.Join(" -> ", path));
        Console.ResetColor();
    }
}
