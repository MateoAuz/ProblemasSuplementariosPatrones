using Ejercicio3_Laberinto.Search;

namespace Ejercicio3_Laberinto.Maze;

/// <summary>
/// Nodo del laberinto. Implementa IGNode&lt;T&gt; siguiendo el patrón del libro
/// (similar a la clase Cave del Capítulo 3 de Fred Mellender).
///
/// Modelado del estado:
///   El estado es la posición actual (fila, columna) en el laberinto.
///   El nodo también almacena qué posiciones ya fueron visitadas para evitar ciclos.
///
/// Representación del nodo:
///   - Position: estado actual del problema.
///   - Parent: nodo padre en el árbol de búsqueda.
///   - _visited: conjunto de posiciones ya exploradas (evita ciclos).
///   - _directionIndex: índice del siguiente movimiento a intentar.
/// </summary>
public class MazeNode : IGNode<MazeNode>
{
    private readonly MazeGrid _maze;
    private readonly HashSet<Position> _visited;
    private int _directionIndex;

    /// <summary>Estado del problema: posición actual en el laberinto.</summary>
    public Position Position { get; }

    /// <summary>Referencia al nodo padre (para reconstruir el camino).</summary>
    public MazeNode? Parent { get; set; }

    public MazeNode(Position position, MazeGrid maze, HashSet<Position> visited, MazeNode? parent = null)
    {
        Position = position;
        _maze = maze;
        _visited = visited;
        _directionIndex = 0;
        Parent = parent;
    }

    /// <summary>
    /// Retorna el primer nodo vecino no visitado (primer hijo en el árbol de búsqueda).
    /// Basado en el método firstChild() de la clase Cave del libro.
    /// </summary>
    public MazeNode? FirstChild()
    {
        return FindNextUnvisitedNeighbor();
    }

    /// <summary>
    /// Retorna el siguiente vecino no explorado del mismo padre (siguiente hermano).
    /// Basado en el método nextSibling() de la clase Cave del libro.
    /// </summary>
    public MazeNode? NextSibling()
    {
        if (Parent == null) return null;

        return Parent.FindNextUnvisitedNeighbor();
    }

    /// <summary>
    /// Busca el siguiente movimiento válido desde esta posición.
    /// Itera sobre las 4 direcciones usando el índice interno.
    /// </summary>
    private MazeNode? FindNextUnvisitedNeighbor()
    {
        while (_directionIndex < Position.Directions.Length)
        {
            Position next = Position.Move(Position.Directions[_directionIndex]);
            _directionIndex++;

            if (_maze.IsWalkable(next) && !_visited.Contains(next))
            {
                _visited.Add(next);
                return new MazeNode(next, _maze, _visited, this);
            }
        }
        return null;
    }

    /// <summary>
    /// Reconstruye el camino desde este nodo hasta la raíz siguiendo los padres.
    /// </summary>
    public List<Position> BuildPath()
    {
        var path = new List<Position>();
        MazeNode? current = this;

        while (current != null)
        {
            path.Add(current.Position);
            current = current.Parent;
        }

        path.Reverse();
        return path;
    }
}
