
namespace ProblemaNReinas;

public class Board
{
    private readonly int size;

    public Board(int size)
    {
        this.size = size;
    }

    public void DisplaySolution(int[] queens)
    {
        Console.WriteLine();

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                if (queens[row] == col)
                {
                    Console.Write(" Q ");
                }
                else
                {
                    Console.Write(" . ");
                }
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }
}
