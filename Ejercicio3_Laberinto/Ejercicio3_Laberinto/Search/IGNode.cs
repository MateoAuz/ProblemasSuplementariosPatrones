namespace Ejercicio3_Laberinto.Search;

/// <summary>
/// Interfaz basada en el patrón SEL del libro "Design Patterns for Searching in C#"
/// de Fred Mellender. Define la estructura de un nodo en el árbol de búsqueda.
/// Un nodo debe saber obtener su primer hijo, su siguiente hermano y su padre.
/// </summary>
/// <typeparam name="T">Tipo concreto del nodo que implementa la interfaz.</typeparam>
public interface IGNode<T> where T : class
{
    /// <summary>Retorna el primer hijo del nodo actual, o null si no hay.</summary>
    T? FirstChild();

    /// <summary>Retorna el siguiente hermano del nodo actual, o null si no hay.</summary>
    T? NextSibling();

    /// <summary>Referencia al nodo padre en el árbol de búsqueda.</summary>
    T? Parent { get; set; }
}
