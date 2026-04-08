
namespace TicTacToeInteligenteGUI;

public class MinimaxAI
{
    private readonly char aiSymbol;
    private readonly char humanSymbol;

    public MinimaxAI(char aiSymbol, char humanSymbol)
    {
        this.aiSymbol = aiSymbol;
        this.humanSymbol = humanSymbol;
    }

    public (int row, int col) FindBestMove(Board board)
    {
        int bestScore = int.MinValue;
        int bestRow = -1;
        int bestCol = -1;

        for (int i = 0; i < Board.Size; i++)
        {
            for (int j = 0; j < Board.Size; j++)
            {
                if (board.IsCellEmpty(i, j))
                {
                    board.SetCell(i, j, aiSymbol);

                    int score = Minimax(board, 0, false);

                    board.SetCell(i, j, ' ');

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestRow = i;
                        bestCol = j;
                    }
                }
            }
        }

        return (bestRow, bestCol);
    }

    private int Minimax(Board board, int depth, bool isMaximizing)
    {
        char winner = board.CheckWinner();

        if (winner == aiSymbol)
        {
            return 10 - depth;
        }

        if (winner == humanSymbol)
        {
            return depth - 10;
        }

        if (board.IsDraw())
        {
            return 0;
        }

        if (isMaximizing)
        {
            int bestScore = int.MinValue;

            for (int i = 0; i < Board.Size; i++)
            {
                for (int j = 0; j < Board.Size; j++)
                {
                    if (board.IsCellEmpty(i, j))
                    {
                        board.SetCell(i, j, aiSymbol);

                        int score = Minimax(board, depth + 1, false);

                        board.SetCell(i, j, ' ');

                        if (score > bestScore)
                        {
                            bestScore = score;
                        }
                    }
                }
            }

            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;

            for (int i = 0; i < Board.Size; i++)
            {
                for (int j = 0; j < Board.Size; j++)
                {
                    if (board.IsCellEmpty(i, j))
                    {
                        board.SetCell(i, j, humanSymbol);

                        int score = Minimax(board, depth + 1, true);

                        board.SetCell(i, j, ' ');

                        if (score < bestScore)
                        {
                            bestScore = score;
                        }
                    }
                }
            }

            return bestScore;
        }
    }

}
