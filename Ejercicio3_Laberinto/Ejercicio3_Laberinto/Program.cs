using Ejercicio3_Laberinto.Maze;

namespace Ejercicio3_Laberinto;

/// <summary>
/// Punto de entrada de la aplicación.
/// Problema del Laberinto - Patrones de Software
/// Universidad Técnica de Ambato - FISEI - Enero 2026
///
/// Implementa BFS y DFS basados en el libro:
/// "Design Patterns for Searching in C#" - Fred Mellender
/// </summary>
internal class Program
{
    static void Main(string[] args)
    {
        PrintHeader();

        bool continuar = true;
        while (continuar)
        {
            int opcion = ShowMainMenu();

            switch (opcion)
            {
                case 1:
                    RunWithMaze(MazeGrid.CreateExample(), "Laberinto del enunciado (4x4)");
                    break;
                case 2:
                    RunWithCustomMaze();
                    break;
                case 0:
                    continuar = false;
                    break;
            }

            if (continuar)
            {
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
                PrintHeader();
            }
        }

        Console.WriteLine("\n  ¡Hasta luego!\n");
    }

    private static void RunWithMaze(MazeGrid maze, string label)
    {
        int algoritmo = ShowAlgorithmMenu();
        if (algoritmo == 0) return;

        IMazeSolver solver = algoritmo == 1
            ? new BfsMazeSolver()
            : new DfsMazeSolver();

        var runner = new MazeRunner(solver);
        runner.Run(maze, label);
    }

    private static void RunWithCustomMaze()
    {
        Console.Clear();
        Console.WriteLine("\n  === LABERINTO PERSONALIZADO ===");
        Console.WriteLine("  Ingrese el número de filas:");
        if (!int.TryParse(Console.ReadLine(), out int rows) || rows < 2)
        {
            Console.WriteLine("  Valor inválido. Mínimo 2 filas.");
            return;
        }

        Console.WriteLine("  Ingrese el número de columnas:");
        if (!int.TryParse(Console.ReadLine(), out int cols) || cols < 2)
        {
            Console.WriteLine("  Valor inválido. Mínimo 2 columnas.");
            return;
        }

        int[,] grid = new int[rows, cols];

        Console.WriteLine($"\n  Ingrese el laberinto fila por fila ({rows} filas).");
        Console.WriteLine("  Use 0 = camino libre, 1 = pared. Separados por espacios.");
        Console.WriteLine($"  Ejemplo para {cols} columnas: 0 0 1 0\n");

        for (int r = 0; r < rows; r++)
        {
            Console.Write($"  Fila {r + 1}: ");
            string? input = Console.ReadLine();
            var parts = input?.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts == null || parts.Length != cols)
            {
                Console.WriteLine("  Entrada inválida. Cancelando.");
                return;
            }

            for (int c = 0; c < cols; c++)
            {
                if (!int.TryParse(parts[c], out int val) || (val != 0 && val != 1))
                {
                    Console.WriteLine("  Solo se permiten valores 0 o 1.");
                    return;
                }
                grid[r, c] = val;
            }
        }

        Console.WriteLine("\n  Posición de inicio (fila columna, ej: 0 0):");
        var startParts = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (startParts?.Length != 2 ||
            !int.TryParse(startParts[0], out int sr) ||
            !int.TryParse(startParts[1], out int sc))
        {
            Console.WriteLine("  Entrada inválida.");
            return;
        }

        Console.WriteLine("  Posición de meta (fila columna, ej: 2 3):");
        var goalParts = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (goalParts?.Length != 2 ||
            !int.TryParse(goalParts[0], out int gr) ||
            !int.TryParse(goalParts[1], out int gc))
        {
            Console.WriteLine("  Entrada inválida.");
            return;
        }

        var maze = new MazeGrid(grid, new Position(sr, sc), new Position(gr, gc));
        RunWithMaze(maze, $"Laberinto personalizado ({rows}x{cols})");
    }

    private static int ShowMainMenu()
    {
        Console.WriteLine("\n  Seleccione una opción:");
        Console.WriteLine("  [1] Laberinto del enunciado (4x4)");
        Console.WriteLine("  [2] Ingresar laberinto personalizado");
        Console.WriteLine("  [0] Salir");
        Console.Write("\n  Opción: ");

        return int.TryParse(Console.ReadLine(), out int op) && op >= 0 && op <= 2 ? op : 1;
    }

    private static int ShowAlgorithmMenu()
    {
        Console.WriteLine("\n  Seleccione el algoritmo de búsqueda:");
        Console.WriteLine("  [1] BFS - Breadth First Search (camino más corto)");
        Console.WriteLine("  [2] DFS - Depth First Search");
        Console.WriteLine("  [0] Volver");
        Console.Write("\n  Opción: ");

        return int.TryParse(Console.ReadLine(), out int op) && op >= 0 && op <= 2 ? op : 1;
    }

    private static void PrintHeader()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n  ╔══════════════════════════════════════════════════╗");
        Console.WriteLine("  ║         PROBLEMA DEL LABERINTO                   ║");
        Console.WriteLine("  ║   Patrones de Software - UTA FISEI 2026          ║");
        Console.WriteLine("  ║   Algoritmos: BFS y DFS (Fred Mellender)         ║");
        Console.WriteLine("  ╚══════════════════════════════════════════════════╝");
        Console.ResetColor();
    }
}
