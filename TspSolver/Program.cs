using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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
        private int startCity;
        private int targetCity;

        public BranchAndBoundSolver(int[,] distances, string[] cityNames, int startCity, int targetCity)
        {
            this.distances = distances;
            this.cityNames = cityNames;
            this.startCity = startCity;
            this.targetCity = targetCity;
        }

        public Node Solve()
        {
            var pq = new PriorityQueue<Node, int>();
            int totalCities = cityNames.Length;
            int bestCost = int.MaxValue;
            Node bestGoalNode = null;

            // Initialize exactly at the Start city
            var initialState = new TspState(startCity, new List<int> { startCity });
            var root = new Node(initialState, null, 0, 0, $"Iniciar en {cityNames[startCity]}");
            pq.Enqueue(root, root.Cost); // Enqueue based on Cost directly (A* heuristic is h=0 for simple B&B, or cost function)

            while (pq.Count > 0)
            {
                var current = pq.Dequeue();

                // Branch and Bound Pruning: if this branch exceeds the best found, discard it.
                if (current.Cost >= bestCost)
                    continue;

                // Is Goal Condition: Visited all cities AND finished exactly at the target city
                if (current.State.VisitedCities.Count == totalCities && current.State.CurrentCity == targetCity)
                {
                    if (current.Cost < bestCost)
                    {
                        bestCost = current.Cost;
                        bestGoalNode = current;
                    }
                    continue;
                }

                // If visited all cities but NOT at target city, this branch is invalid.
                if (current.State.VisitedCities.Count == totalCities)
                    continue;

                // Expand Children
                for (int nextCity = 0; nextCity < totalCities; nextCity++)
                {
                    if (!current.State.VisitedCities.Contains(nextCity))
                    {
                        // CONSTRAINTS: We can only go to targetCity if it is the VERY LAST city to visit.
                        if (nextCity == targetCity && current.State.VisitedCities.Count < totalCities - 1)
                        {
                            continue; // Skip target city if there are still other cities to visit
                        }

                        int addedCost = distances[current.State.CurrentCity, nextCity];
                        if (addedCost > 0) 
                        {
                            int newCost = current.Cost + addedCost;
                            
                            // Prune node if it exceeds known best
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

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
    

}
