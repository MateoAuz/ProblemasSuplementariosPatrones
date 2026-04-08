using Ejercicio02_Caballo.Application.UseCases;
using Ejercicio02_Caballo.Domain.ValueObjects;

namespace Ejercicio02_Caballo.WinForms;

public partial class Form1 : Form
{
    private readonly SolveKnightTourUseCase _useCase;
    private int _startX = -1;
    private int _startY = -1;
    private IReadOnlyList<Position>? _path;
    private int _currentStepIndex;
    private const string KnightSymbol = "♞";

    // Required for Designer
    public Form1()
    {
        InitializeComponent();
    }

    public Form1(SolveKnightTourUseCase useCase) : this()
    {
        _useCase = useCase;
        
        btnBuild.Click += BtnBuild_Click;
        btnSolve.Click += BtnSolve_Click;
        dgvBoard.CellClick += DgvBoard_CellClick;
        timerAnimation.Tick += TimerAnimation_Tick;

        dgvBoard.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvBoard.DefaultCellStyle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        dgvBoard.DefaultCellStyle.SelectionBackColor = Color.Transparent;
        dgvBoard.DefaultCellStyle.SelectionForeColor = Color.Black;
        
        dgvBoard.Resize += (s, e) => ResizeCells();
    }

    private void BtnBuild_Click(object? sender, EventArgs e)
    {
        int cols = (int)numCols.Value;
        int rows = (int)numRows.Value;

        dgvBoard.Columns.Clear();
        dgvBoard.Rows.Clear();

        for (int i = 0; i < cols; i++)
        {
            var col = new DataGridViewTextBoxColumn
            {
                Name = $"Col{i}",
                Width = 50
            };
            dgvBoard.Columns.Add(col);
        }

        dgvBoard.Rows.Add(rows);

        // Pattern the board like chess
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                dgvBoard.Rows[r].Cells[c].Style.BackColor = ((r + c) % 2 == 0) ? Color.WhiteSmoke : Color.LightGray;
                dgvBoard.Rows[r].Cells[c].Value = "";
            }
        }

        ResizeCells();
        _startX = -1;
        _startY = -1;
        btnSolve.Enabled = false;
        lblStatus.Text = "Tablero Creado. Haz clic en una celda para seleccionar la posición de inicio del caballo.";
    }

    private void ResizeCells()
    {
        if (dgvBoard.Columns.Count == 0 || dgvBoard.Rows.Count == 0) return;

        int cellWidth = dgvBoard.ClientSize.Width / dgvBoard.Columns.Count;
        int cellHeight = dgvBoard.ClientSize.Height / dgvBoard.Rows.Count;

        // Make square
        int size = Math.Min(cellWidth, cellHeight);

        foreach (DataGridViewColumn col in dgvBoard.Columns) col.Width = size;
        foreach (DataGridViewRow row in dgvBoard.Rows) row.Height = size;
    }

    private void DgvBoard_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (!btnBuild.Enabled) return; // Prevent changing during animation

        // Clean previous 
        if (_startX >= 0 && _startY >= 0 && _startX < dgvBoard.Columns.Count && _startY < dgvBoard.Rows.Count)
        {
            dgvBoard.Rows[_startY].Cells[_startX].Value = "";
        }

        _startX = e.ColumnIndex;
        _startY = e.RowIndex;

        dgvBoard.Rows[_startY].Cells[_startX].Value = KnightSymbol;
        dgvBoard.Rows[_startY].Cells[_startX].Style.ForeColor = Color.DarkSlateBlue;

        btnSolve.Enabled = true;
        lblStatus.Text = $"Posición seleccionada: ({_startX}, {_startY}).\nPresiona Resolver.";
    }

    private async void BtnSolve_Click(object? sender, EventArgs e)
    {
        if (_startX < 0 || _startY < 0) return;

        btnBuild.Enabled = false;
        btnSolve.Enabled = false;
        dgvBoard.Enabled = false;

        lblStatus.Text = "Calculando el recorrido...";

        int rows = (int)numRows.Value;
        int cols = (int)numCols.Value;

        // Limpiar el tablero dejando solo el inicio
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (r != _startY || c != _startX)
                {
                    dgvBoard.Rows[r].Cells[c].Value = "";
                }
            }
        }

        try
        {
            // Execute in background to avoid freezing UI
            var result = await Task.Run(() => _useCase.Execute(rows, cols, _startX, _startY));
            
            if (result.IsSuccessful)
            {
                _path = result.Path;
                _currentStepIndex = 1; // Starts from second jump since first is already setup
                lblStatus.Text = "Solución encontrada. Animando...";
                timerAnimation.Interval = 400; // 400ms per step
                timerAnimation.Start();
            }
            else
            {
                MessageBox.Show("No se encontró una solución para este tablero y posición inicial.", "Tour Fallido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetUI();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error al resolver", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ResetUI();
        }
    }

    private void TimerAnimation_Tick(object? sender, EventArgs e)
    {
        if (_path == null || _currentStepIndex > _path.Count)
        {
            timerAnimation.Stop();
            lblStatus.Text = "¡Recorrido terminado!";
            ResetUI();
            return;
        }

        // Dejar numero en la celda anterior
        var prevPos = _path[_currentStepIndex - 1];
        dgvBoard.Rows[prevPos.Y].Cells[prevPos.X].Value = _currentStepIndex.ToString();
        dgvBoard.Rows[prevPos.Y].Cells[prevPos.X].Style.ForeColor = Color.Black;

        // Estamos todavía dentro del rango?
        if (_currentStepIndex < _path.Count)
        {
            // Poner el caballo en la celda nueva
            var nextPos = _path[_currentStepIndex];
            dgvBoard.Rows[nextPos.Y].Cells[nextPos.X].Value = KnightSymbol;
            dgvBoard.Rows[nextPos.Y].Cells[nextPos.X].Style.ForeColor = Color.DarkRed;
        }

        _currentStepIndex++;
    }

    private void ResetUI()
    {
        btnBuild.Enabled = true;
        btnSolve.Enabled = true;
        dgvBoard.Enabled = true;
    }
}
