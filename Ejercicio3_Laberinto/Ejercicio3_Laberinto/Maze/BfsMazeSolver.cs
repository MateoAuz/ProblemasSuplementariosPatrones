using Ejercicio3_Laberinto.Search;

namespace Ejercicio3_Laberinto.Maze;

/// <summary>
/// Resuelve el laberinto usando Breadth First Search (BFS).
/// Basado en el Capítulo 6 del libro "Design Patterns for Searching in C#".
/// BFS garantiza encontrar el camino más corto.
/// </summary>
public class BfsMazeSolver : IMazeSolver
{
    public string AlgorithmName => "BFS (Breadth First Search)";

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

        foreach (MazeNode node in graph.BreadthFirst())
        {
            visitedOrder.Add(node.Position);

            if (maze.IsGoal(node.Position))
                return (visitedOrder, node.BuildPath());
        }

        return (visitedOrder, null);
    }
}
