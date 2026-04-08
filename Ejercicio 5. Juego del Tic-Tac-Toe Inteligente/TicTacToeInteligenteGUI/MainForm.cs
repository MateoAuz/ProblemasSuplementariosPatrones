using System;
using System.Windows.Forms;

namespace TicTacToeInteligenteGUI
{
    public partial class MainForm : Form
    {
        private readonly Board board;
        private readonly MinimaxAI ai;
        private readonly char humanSymbol;
        private readonly char aiSymbol;
        private bool gameOver;
        private Button[,] buttons;

        public MainForm()
        {
            InitializeComponent();

            board = new Board();
            humanSymbol = 'X';
            aiSymbol = 'O';
            ai = new MinimaxAI(aiSymbol, humanSymbol);
            gameOver = false;

            InitializeButtons();
            UpdateBoardUI();
            lblStatus.Text = "Tu turno";
        }

        private void InitializeButtons()
        {
            buttons = new Button[3, 3]
            {
                { btn00, btn01, btn02 },
                { btn10, btn11, btn12 },
                { btn20, btn21, btn22 }
            };
        }

        private void HandlePlayerMove(int row, int col)
        {
            if (gameOver)
            {
                return;
            }

            bool success = board.MakeMove(row, col, humanSymbol);

            if (!success)
            {
                return;
            }

            UpdateBoardUI();

            if (CheckGameStatus())
            {
                return;
            }

            HandleComputerMove();
        }

        private void HandleComputerMove()
        {
            lblStatus.Text = "Turno de la computadora...";

            var bestMove = ai.FindBestMove(board);

            if (bestMove.row != -1 && bestMove.col != -1)
            {
                board.MakeMove(bestMove.row, bestMove.col, aiSymbol);
            }

            UpdateBoardUI();
            CheckGameStatus();
        }

        private bool CheckGameStatus()
        {
            char winner = board.CheckWinner();

            if (winner == humanSymbol)
            {
                lblStatus.Text = "Ganaste!";
                gameOver = true;
                UpdateBoardUI();
                return true;
            }

            if (winner == aiSymbol)
            {
                lblStatus.Text = "La computadora gano";
                gameOver = true;
                UpdateBoardUI();
                return true;
            }

            if (board.IsDraw())
            {
                lblStatus.Text = "Empate";
                gameOver = true;
                UpdateBoardUI();
                return true;
            }

            lblStatus.Text = "Tu turno";
            return false;
        }

        private void UpdateBoardUI()
        {
            for (int i = 0; i < Board.Size; i++)
            {
                for (int j = 0; j < Board.Size; j++)
                {
                    char value = board.GetCell(i, j);
                    buttons[i, j].Text = value == ' ' ? "" : value.ToString();
                    buttons[i, j].Enabled = value == ' ' && !gameOver;
                }
            }
        }

        private void ResetGame()
        {
            board.InitializeBoard();
            gameOver = false;
            lblStatus.Text = "Tu turno";
            UpdateBoardUI();
        }

        private void btn00_Click(object sender, EventArgs e) => HandlePlayerMove(0, 0);
        private void btn01_Click(object sender, EventArgs e) => HandlePlayerMove(0, 1);
        private void btn02_Click(object sender, EventArgs e) => HandlePlayerMove(0, 2);

        private void btn10_Click(object sender, EventArgs e) => HandlePlayerMove(1, 0);
        private void btn11_Click(object sender, EventArgs e) => HandlePlayerMove(1, 1);
        private void btn12_Click(object sender, EventArgs e) => HandlePlayerMove(1, 2);

        private void btn20_Click(object sender, EventArgs e) => HandlePlayerMove(2, 0);
        private void btn21_Click(object sender, EventArgs e) => HandlePlayerMove(2, 1);
        private void btn22_Click(object sender, EventArgs e) => HandlePlayerMove(2, 2);

        private void btnRestart_Click(object sender, EventArgs e)
        {
            ResetGame();
        }
    }
}