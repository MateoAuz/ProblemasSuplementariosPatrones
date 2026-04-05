
namespace TicTacToeInteligente;

public class Game
{
    private readonly Board board;
    private readonly MinimaxAI ai;
    private readonly char humanSymbol;
    private readonly char aiSymbol;

    public Game()
    {
        board = new Board();
        humanSymbol = 'X';
        aiSymbol = 'O';
        ai = new MinimaxAI(aiSymbol, humanSymbol);
    }

    public void Start()
    {
        Console.WriteLine("=== TIC-TAC-TOE INTELIGENTE ===");
        Console.WriteLine("Jugador: X");
        Console.WriteLine("Computadora: O");

        while (true)
        {
            board.DisplayBoard();
            HumanTurn();

            if (CheckGameOver())
            {
                break;
            }

            ComputerTurn();

            if (CheckGameOver())
            {
                break;
            }
        }
    }

    private void HumanTurn()
    {
        bool validMove = false;

        while (!validMove)
        {
            Console.Write("Ingresa la fila (1-3): ");
            string? rowInput = Console.ReadLine();

            Console.Write("Ingresa la columna (1-3): ");
            string? colInput = Console.ReadLine();

            bool validRow = int.TryParse(rowInput, out int row);
            bool validCol = int.TryParse(colInput, out int col);

            if (!validRow || !validCol)
            {
                Console.WriteLine("Entrada inválida. Debes ingresar números.");
                continue;
            }

            row--;
            col--;

            if (row < 0 || row >= Board.Size || col < 0 || col >= Board.Size)
            {
                Console.WriteLine("La posición está fuera del rango permitido.");
                continue;
            }

            validMove = board.MakeMove(row, col, humanSymbol);

            if (!validMove)
            {
                Console.WriteLine("Esa casilla ya está ocupada. Intenta otra vez.");
            }
        }
    }

    private void ComputerTurn()
    {
        Console.WriteLine("Turno de la computadora...");

        (int row, int col) bestMove = ai.FindBestMove(board);
        board.MakeMove(bestMove.row, bestMove.col, aiSymbol);
    }

    private bool CheckGameOver()
    {
        char winner = board.CheckWinner();

        if (winner == humanSymbol)
        {
            board.DisplayBoard();
            Console.WriteLine("¡Ganaste!");
            return true;
        }

        if (winner == aiSymbol)
        {
            board.DisplayBoard();
            Console.WriteLine("La computadora ganó.");
            return true;
        }

        if (board.IsDraw())
        {
            board.DisplayBoard();
            Console.WriteLine("Empate.");
            return true;
        }

        return false;
    }
}
