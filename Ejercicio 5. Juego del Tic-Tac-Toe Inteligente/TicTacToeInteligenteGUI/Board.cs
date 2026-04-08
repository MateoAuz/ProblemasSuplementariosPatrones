
namespace TicTacToeInteligenteGUI;

public class Board
{
    private char[,] grid;
    public const int Size = 3;

    public Board()
    {
        grid = new char[Size, Size];
        InitializeBoard();
    }

    public void InitializeBoard()
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                grid[i, j] = ' ';
            }
        }
    }

    public void DisplayBoard()
    {
        Console.WriteLine();

        for (int i = 0; i < Size; i++)
        {
            Console.Write(" ");

            for (int j = 0; j < Size; j++)
            {
                Console.Write(grid[i, j]);

                if (j < Size - 1)
                {
                    Console.Write(" | ");
                }
            }

            Console.WriteLine();

            if (i < Size - 1)
            {
                Console.WriteLine("---+---+---");
            }
        }

        Console.WriteLine();
    }

    public bool MakeMove(int row, int col, char symbol)
    {
        if (row >= 0 && row < Size && col >= 0 && col < Size && grid[row, col] == ' ')
        {
            grid[row, col] = symbol;
            return true;
        }

        return false;
    }

    public bool IsCellEmpty(int row, int col)
    {
        return grid[row, col] == ' ';
    }

    public bool HasEmptyCells()
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                if (grid[i, j] == ' ')
                {
                    return true;
                }
            }
        }

        return false;
    }

    public char GetCell(int row, int col)
    {
        return grid[row, col];
    }

    public void SetCell(int row, int col, char value)
    {
        grid[row, col] = value;
    }

    public char[,] GetGridCopy()
    {
        char[,] copy = new char[Size, Size];

        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                copy[i, j] = grid[i, j];
            }
        }

        return copy;
    }

    public char CheckWinner()
    {
        for (int i = 0; i < Size; i++)
        {
            if (grid[i, 0] != ' ' && grid[i, 0] == grid[i, 1] && grid[i, 1] == grid[i, 2])
            {
                return grid[i, 0];
            }
        }

        for (int j = 0; j < Size; j++)
        {
            if (grid[0, j] != ' ' && grid[0, j] == grid[1, j] && grid[1, j] == grid[2, j])
            {
                return grid[0, j];
            }
        }

        if (grid[0, 0] != ' ' && grid[0, 0] == grid[1, 1] && grid[1, 1] == grid[2, 2])
        {
            return grid[0, 0];
        }

        if (grid[0, 2] != ' ' && grid[0, 2] == grid[1, 1] && grid[1, 1] == grid[2, 0])
        {
            return grid[0, 2];
        }

        return ' ';
    }

    public bool IsDraw()
    {
        return CheckWinner() == ' ' && !HasEmptyCells();
    }
}

