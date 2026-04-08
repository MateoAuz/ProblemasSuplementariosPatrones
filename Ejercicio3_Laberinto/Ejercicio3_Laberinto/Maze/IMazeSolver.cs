namespace Ejercicio3_Laberinto.Maze;

/// <summary>
/// Contrato para los solucionadores del laberinto.
/// Principio ISP (Interface Segregation) y DIP (Dependency Inversion).
/// Permite intercambiar algoritmos sin modificar el resto del código.
/// </summary>
public interface IMazeSolver
{
    string AlgorithmName { get; }

    /// <summary>
    /// Busca un camino desde el inicio hasta la meta en el laberinto.
    /// </summary>
    /// <returns>Lista de posiciones que forman el camino, o null si no existe.</returns>
    List<Position>? Solve(MazeGrid maze);

    /// <summary>
    /// Resuelve el laberinto y devuelve los pasos de exploración para animación.
    /// </summary>
    /// <returns>Tupla con la lista de pasos visitados y el camino final (o null).</returns>
    (List<Position> Visited, List<Position>? Path) SolveWithSteps(MazeGrid maze);
}
