namespace Ejercicio3_Laberinto.Search;

/// <summary>
/// Clase genérica Graph basada en el patrón SEL del libro.
/// Proporciona iteradores para BFS y DFS sobre cualquier nodo que implemente IGNode&lt;T&gt;.
/// Capítulo 3 (DFS) y Capítulo 6 (BFS) del libro de Fred Mellender.
/// </summary>
/// <typeparam name="T">Tipo del nodo que implementa IGNode&lt;T&gt;.</typeparam>
public class Graph<T> where T : class, IGNode<T>
{
    private readonly T _root;

    public Graph(T root)
    {
        _root = root;
    }

    /// <summary>
    /// Iterador BFS (Breadth First Search) - Capítulo 6 del libro.
    /// Visita todos los nodos nivel por nivel, garantizando el camino más corto.
    /// </summary>
    public IEnumerable<T> BreadthFirst()
    {
        var queue = new Queue<T>();
        queue.Enqueue(_root);

        while (queue.Count > 0)
        {
            T current = queue.Dequeue();
            yield return current;

            // Encolar todos los hijos del nodo actual
            T? child = current.FirstChild();
            while (child != null)
            {
                queue.Enqueue(child);
                child = child.NextSibling();
            }
        }
    }

    /// <summary>
    /// Iterador DFS (Depth First Search) - Capítulo 3 del libro.
    /// Explora en profundidad antes de explorar en anchura.
    /// </summary>
    public IEnumerable<T> DepthFirst()
    {
        T? current = _root;

        while (current != null)
        {
            yield return current;

            T? child = current.FirstChild();
            if (child != null)
            {
                current = child;
            }
            else
            {
                // Retroceder hasta encontrar un hermano
                T? sibling = current.NextSibling();
                while (sibling == null && current.Parent != null)
                {
                    current = current.Parent;
                    sibling = current.NextSibling();
                }
                current = sibling;
            }
        }
    }
}
