using Ejercicio3_Laberinto.Search;

namespace Ejercicio3_Laberinto.Maze;

/// <summary>
/// Resuelve el laberinto usando Breadth First Search (BFS).
/// Basado en el Capítulo 6 del libro "Design Patterns for Searching in C#".
///
/// BFS garantiza encontrar el camino más corto porque explora
/// todos los nodos de una generación antes de pasar a la siguiente.
/// </summary>
public class BfsMazeSolver : IMazeSolver
{
    public string AlgorithmName => "BFS (Breadth First Search)";

    public List<Position>? Solve(MazeGrid maze)
    {
        var visited = new HashSet<Position> { maze.Start };

        var rootNode = new MazeNode(maze.Start, maze, visited);
        var graph = new Graph<MazeNode>(rootNode);

        // Usar el iterador BFS del libro para explorar el laberinto
        foreach (MazeNode node in graph.BreadthFirst())
        {
            if (maze.IsGoal(node.Position))
                return node.BuildPath();
        }

        return null; // No existe camino
    }
}
