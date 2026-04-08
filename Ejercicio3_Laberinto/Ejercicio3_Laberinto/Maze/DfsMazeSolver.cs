using Ejercicio3_Laberinto.Search;

namespace Ejercicio3_Laberinto.Maze;

/// <summary>
/// Resuelve el laberinto usando Depth First Search (DFS).
/// Basado en el Capítulo 3 del libro "Design Patterns for Searching in C#".
/// DFS no garantiza el camino más corto pero usa menos memoria que BFS.
/// </summary>
public class DfsMazeSolver : IMazeSolver
{
    public string AlgorithmName => "DFS (Depth First Search)";

    public List<Position>? Solve(MazeGrid maze)
    {
        var (_, path) = SolveWithSteps(maze);
        return path;
    }

    public (List<Position> Visited, List<Position>? Path) SolveWithSteps(MazeGrid maze)
    {
        var visited = new HashSet<Position> { maze.Start };
        var visitedOrder = new List<Position>();

        var rootNode = new MazeNode(maze.Start, maze, visited);
        var graph = new Graph<MazeNode>(rootNode);

        foreach (MazeNode node in graph.DepthFirst())
        {
            visitedOrder.Add(node.Position);

            if (maze.IsGoal(node.Position))
                return (visitedOrder, node.BuildPath());
        }

        return (visitedOrder, null);
    }
}
