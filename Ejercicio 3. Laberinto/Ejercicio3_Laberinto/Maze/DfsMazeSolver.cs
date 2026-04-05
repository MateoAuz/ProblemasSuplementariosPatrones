using Ejercicio3_Laberinto.Search;

namespace Ejercicio3_Laberinto.Maze;

/// <summary>
/// Resuelve el laberinto usando Depth First Search (DFS).
/// Basado en el Capítulo 3 del libro "Design Patterns for Searching in C#"
/// (idéntico al ejemplo Cave de Fred Mellender).
///
/// DFS no garantiza el camino más corto pero usa menos memoria que BFS.
/// </summary>
public class DfsMazeSolver : IMazeSolver
{
    public string AlgorithmName => "DFS (Depth First Search)";

    public List<Position>? Solve(MazeGrid maze)
    {
        var visited = new HashSet<Position> { maze.Start };

        var rootNode = new MazeNode(maze.Start, maze, visited);
        var graph = new Graph<MazeNode>(rootNode);

        // Usar el iterador DFS del libro para explorar el laberinto
        foreach (MazeNode node in graph.DepthFirst())
        {
            if (maze.IsGoal(node.Position))
                return node.BuildPath();
        }

        return null; // No existe camino
    }
}
