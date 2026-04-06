using System;
using System.Collections.Generic;
using System.Linq;

namespace TspSolver
{
    // 2. Modelado del estado
    public class TspState
    {
        public int CurrentCity { get; }
        public IReadOnlyList<int> VisitedCities { get; }

        public TspState(int currentCity, IEnumerable<int> visitedCities)
        {
            CurrentCity = currentCity;
            VisitedCities = visitedCities.ToList().AsReadOnly();
        }

        public bool IsGoal(int totalCities)
        {
            return VisitedCities.Count == totalCities;
        }
    }

    // 3. Representación del nodo
    public class Node : IComparable<Node>
    {
        public TspState State { get; }
        public Node Parent { get; }
        public int Cost { get; }
        public int Depth { get; }
        public string Action { get; }

        public Node(TspState state, Node parent, int cost, int depth, string action)
        {
            State = state;
            Parent = parent;
            Cost = cost;
            Depth = depth;
            Action = action;
        }

        public int CompareTo(Node other)
        {
            // Para Priority Queue en C# .NET (min-heap natural si la implementamos o usamos PriorityQueue de .NET 6+)
            int compare = Cost.CompareTo(other.Cost);
            if (compare == 0)
                return other.Depth.CompareTo(Depth); // Prefer deeper nodes on tie
            return compare;
        }
    }

    public class BranchAndBoundSolver
    {
        private int[,] distances;
        private string[] cityNames;

        public BranchAndBoundSolver(int[,] distances, string[] cityNames)
        {
            this.distances = distances;
            this.cityNames = cityNames;
        }

        public Node Solve()
        {
            var pq = new PriorityQueue<Node, int>();
            int totalCities = cityNames.Length;
            int bestCost = int.MaxValue;
            Node bestGoalNode = null;

            // Iniciar desde la ciudad 0 (Ambato) o probar todas como inicio
            // Probar todas como ciudad de inicio para encontrar la ruta global más corta
            for (int i = 0; i < totalCities; i++)
            {
                var initialState = new TspState(i, new List<int> { i });
                var root = new Node(initialState, null, 0, 0, $"Iniciar en {cityNames[i]}");
                pq.Enqueue(root, root.Cost);
            }

            while (pq.Count > 0)
            {
                var current = pq.Dequeue();

                // Poda: si este nodo ya cuesta más que la mejor solución encontrada, descartar.
                if (current.Cost >= bestCost)
                    continue;

                // Es meta?
                if (current.State.IsGoal(totalCities))
                {
                    // Versión simplificada: no regresamos a la de origen en este caso (o sí?)
                    // Asumiremos que no se regresa a la ciudad de origen porque dice "que visite todas las ciudades una vez"
                    if (current.Cost < bestCost)
                    {
                        bestCost = current.Cost;
                        bestGoalNode = current;
                    }
                    continue;
                }

                // Generar hijos
                for (int nextCity = 0; nextCity < totalCities; nextCity++)
                {
                    if (!current.State.VisitedCities.Contains(nextCity))
                    {
                        int addedCost = distances[current.State.CurrentCity, nextCity];
                        if (addedCost > 0) // Should always be > 0 for different cities
                        {
                            int newCost = current.Cost + addedCost;
                            
                            // Poda: si supera el mejor costo conocido, no lo agregamos (bound)
                            if (newCost < bestCost)
                            {
                                var newVisited = new List<int>(current.State.VisitedCities) { nextCity };
                                var newState = new TspState(nextCity, newVisited);
                                var childNode = new Node(newState, current, newCost, current.Depth + 1, $"Ir a {cityNames[nextCity]}");
                                pq.Enqueue(childNode, childNode.Cost);
                            }
                        }
                    }
                }
            }

            return bestGoalNode;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] cities = { "Ambato", "Quito", "Riobamba", "Latacunga", "Puyo" };
            int[,] distances = new int[,]
            {
                { 0,   120, 60,  45,  100 },
                { 120, 0,   180, 90,  220 },
                { 60,  180, 0,   105, 130 },
                { 45,  90,  105, 0,   145 },
                { 100, 220, 130, 145, 0   }
            };

            var solver = new BranchAndBoundSolver(distances, cities);
            var solutionNode = solver.Solve();

            if (solutionNode != null)
            {
                Console.WriteLine("Mejor Costo: " + solutionNode.Cost);
                // Obtener ruta leyendo padres hacia arriba (postreverso)
                var current = solutionNode;
                var pathNodes = new List<Node>();
                while (current != null)
                {
                    pathNodes.Insert(0, current);
                    current = current.Parent;
                }

                foreach (var n in pathNodes)
                {
                    Console.WriteLine(n.Action + " -> Costo acumulado: " + n.Cost);
                }
            }
            else
            {
                Console.WriteLine("No se encontro solucion.");
            }
            
            // Evaluando también el costo cerrando el ciclo (TSP standard)
            Console.WriteLine("--- Evaluando con retorno ---");
            var solverCycle = new BranchAndBoundSolverCycle(distances, cities);
            var solCycle = solverCycle.Solve();
            if(solCycle != null) Console.WriteLine("Con retorno, Mejor Costo: " + solCycle.Cost);
        }
    }
    
    // Y para un solver con retorno
    public class BranchAndBoundSolverCycle
    {
        private int[,] distances;
        private string[] cityNames;

        public BranchAndBoundSolverCycle(int[,] distances, string[] cityNames)
        {
            this.distances = distances;
            this.cityNames = cityNames;
        }

        public Node Solve()
        {
            var pq = new PriorityQueue<Node, int>();
            int totalCities = cityNames.Length;
            int bestCost = int.MaxValue;
            Node bestGoalNode = null;

            // En TSP simétrico, podemos iniciar de un punto fijo siempre
            var initialState = new TspState(0, new List<int> { 0 });
            var root = new Node(initialState, null, 0, 0, $"Iniciar en {cityNames[0]}");
            pq.Enqueue(root, root.Cost);

            while (pq.Count > 0)
            {
                var current = pq.Dequeue();

                if (current.Cost >= bestCost)
                    continue;

                if (current.State.IsGoal(totalCities))
                {
                    // Sumamos regreso
                    int returnCost = distances[current.State.CurrentCity, current.State.VisitedCities[0]];
                    int finalCost = current.Cost + returnCost;
                    if (finalCost < bestCost)
                    {
                        bestCost = finalCost;
                        bestGoalNode = new Node(current.State, current, finalCost, current.Depth + 1, $"Regresar a {cityNames[current.State.VisitedCities[0]]}");
                    }
                    continue;
                }

                for (int nextCity = 0; nextCity < totalCities; nextCity++)
                {
                    if (!current.State.VisitedCities.Contains(nextCity))
                    {
                        int addedCost = distances[current.State.CurrentCity, nextCity];
                        if (addedCost > 0)
                        {
                            int newCost = current.Cost + addedCost;
                            
                            if (newCost < bestCost)
                            {
                                var newVisited = new List<int>(current.State.VisitedCities) { nextCity };
                                var newState = new TspState(nextCity, newVisited);
                                var childNode = new Node(newState, current, newCost, current.Depth + 1, $"Ir a {cityNames[nextCity]}");
                                pq.Enqueue(childNode, childNode.Cost);
                            }
                        }
                    }
                }
            }

            return bestGoalNode;
        }
    }
}
