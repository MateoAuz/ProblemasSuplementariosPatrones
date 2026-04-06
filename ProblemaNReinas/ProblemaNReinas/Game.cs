
namespace ProblemaNReinas;

public class Game
{
    public void Start()
    {
        Console.WriteLine("==================================");
        Console.WriteLine("     PROBLEMA DE LAS N REINAS     ");
        Console.WriteLine("==================================");

        int n = ReadBoardSize();

        NQueensSolver solver = new NQueensSolver(n);
        solver.Solve();

        Console.WriteLine($"\nTotal de soluciones encontradas: {solver.SolutionCount}");
    }

    private int ReadBoardSize()
    {
        while (true)
        {
            Console.Write("\nIngresa el valor de N: ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int n) && n > 0)
            {
                return n;
            }

            Console.WriteLine("Entrada inválida. Debes ingresar un número entero mayor que 0.");
        }
    }
}

