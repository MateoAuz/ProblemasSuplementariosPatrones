
namespace ProblemaNReinas
{
    public class NQueensSolver
    {
        private readonly int size;
        private readonly int[] queens;
        private readonly Board board;

        public int SolutionCount { get; private set; }

        public NQueensSolver(int size)
        {
            this.size = size;
            queens = new int[size];
            board = new Board(size);
            SolutionCount = 0;

            for (int i = 0; i < size; i++)
            {
                queens[i] = -1;
            }
        }

        public void Solve()
        {
            PlaceQueen(0);
        }

        private void PlaceQueen(int row)
        {
            if (row == size)
            {
                SolutionCount++;
                Console.WriteLine($"Solución #{SolutionCount}:");
                board.DisplaySolution(queens);
                return;
            }

            for (int col = 0; col < size; col++)
            {
                if (IsSafe(row, col))
                {
                    queens[row] = col;
                    PlaceQueen(row + 1);
                    queens[row] = -1;
                }
            }
        }

        private bool IsSafe(int row, int col)
        {
            for (int previousRow = 0; previousRow < row; previousRow++)
            {
                int previousCol = queens[previousRow];

                if (previousCol == col)
                {
                    return false;
                }

                if (Math.Abs(previousCol - col) == Math.Abs(previousRow - row))
                {
                    return false;
                }
            }

            return true;
        }
    }
}