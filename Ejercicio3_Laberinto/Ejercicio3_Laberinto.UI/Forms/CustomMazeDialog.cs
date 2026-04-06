using Ejercicio3_Laberinto.Maze;

namespace Ejercicio3_Laberinto.UI.Forms;

/// <summary>
/// Diálogo para crear un laberinto personalizado haciendo clic sobre las celdas.
/// - Se escoge el tamaño → aparece la cuadrícula
/// - Se selecciona el modo (Inicio / Meta / Pared / Libre)
/// - Se hace clic sobre las celdas para asignarlas
/// - Se valida que haya exactamente 1 inicio y 1 meta antes de aceptar
/// </summary>
public class CustomMazeDialog : Form
{
    private int _rows = 4;
    private int _cols = 4;
    private int[,] _gridData = new int[4, 4];
    private Position? _startPos;
    private Position? _goalPos;
    private string _drawMode = "wall";

    private NumericUpDown _nudRows   = null!;
    private NumericUpDown _nudCols   = null!;
    private Panel         _gridPanel = null!;
    private Button _btnModeStart = null!, _btnModeGoal = null!,
                   _btnModeWall  = null!, _btnModeFree = null!;
    private Label  _lblHint     = null!;
    private Button _btnOk       = null!, _btnCancel = null!;

    private static readonly Color ColWall  = Color.FromArgb(30,  41,  59);
    private static readonly Color ColFree  = Color.White;
    private static readonly Color ColStart = Color.FromArgb(21,  128, 61);
    private static readonly Color ColGoal  = Color.FromArgb(185, 28,  28);
    private static readonly Color ColGrid  = Color.FromArgb(200, 200, 200);

    public MazeGrid? Result { get; private set; }

    public CustomMazeDialog()
    {
        Text            = "Crear laberinto personalizado";
        Size            = new Size(560, 580);
        MinimumSize     = new Size(560, 580);
        MaximumSize     = new Size(560, 580);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox     = false;
        StartPosition   = FormStartPosition.CenterParent;
        BackColor       = Color.FromArgb(240, 240, 240);
        Font            = new Font("Segoe UI", 9f);

        BuildLayout();
        // GenerateGrid se llama automáticamente cuando _gridPanel recibe su tamaño real (SizeChanged)
    }

    private void BuildLayout()
    {
        // En WinForms con Dock, el orden de inserción importa:
        // Bottom primero, luego Fill, luego los Top en orden INVERSO al visual

        // -- Botones de acción (Bottom)
        var btnPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom, Height = 44,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(8, 6, 8, 0)
        };
        _btnCancel = new Button { Text = "Cancelar", Width = 90, Height = 30, DialogResult = DialogResult.Cancel };
        _btnOk = new Button
        {
            Text = "Aceptar", Width = 90, Height = 30,
            BackColor = Color.FromArgb(21, 128, 61), ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        _btnOk.Click += BtnOk_Click;
        btnPanel.Controls.Add(_btnCancel);
        btnPanel.Controls.Add(_btnOk);
        Controls.Add(btnPanel);

        // -- Panel cuadrícula (Fill)
        _gridPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(245, 245, 245),
            Padding = new Padding(8)
        };
        _gridPanel.SizeChanged += (_, _) => GenerateGrid();
        Controls.Add(_gridPanel);

        // -- Top controls en orden INVERSO al visual (último agregado = más arriba)

        // 3ro visualmente → pista de estado
        _lblHint = new Label
        {
            Dock = DockStyle.Top, Height = 26,
            Padding = new Padding(14, 4, 0, 0),
            Font = new Font("Segoe UI", 8.5f),
            ForeColor = Color.FromArgb(100, 100, 100)
        };
        Controls.Add(_lblHint);

        // 2do visualmente → botones de modo
        var modePanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top, Height = 44,
            Padding = new Padding(12, 4, 12, 0),
            FlowDirection = FlowDirection.LeftToRight
        };
        _btnModeStart = MkModeBtn("S  Inicio", ColStart, Color.White);
        _btnModeGoal  = MkModeBtn("G  Meta",   ColGoal,  Color.White);
        _btnModeWall  = MkModeBtn("█  Pared",  ColWall,  Color.White);
        _btnModeFree  = MkModeBtn(".  Libre",  Color.FromArgb(210, 210, 210), Color.Black);
        _btnModeStart.Click += (_, _) => SetMode("start");
        _btnModeGoal.Click  += (_, _) => SetMode("goal");
        _btnModeWall.Click  += (_, _) => SetMode("wall");
        _btnModeFree.Click  += (_, _) => SetMode("free");
        modePanel.Controls.AddRange(new Control[] { _btnModeStart, _btnModeGoal, _btnModeWall, _btnModeFree });
        Controls.Add(modePanel);

        // 1ro visualmente → filas/columnas
        var topPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top, Height = 44,
            Padding = new Padding(12, 8, 12, 0),
            FlowDirection = FlowDirection.LeftToRight
        };
        topPanel.Controls.Add(MkLabel("Filas:"));
        _nudRows = new NumericUpDown { Minimum = 2, Maximum = 10, Value = 4, Width = 50, Margin = new Padding(0, 0, 14, 0) };
        _nudRows.ValueChanged += (_, _) => GenerateGrid();
        topPanel.Controls.Add(_nudRows);
        topPanel.Controls.Add(MkLabel("Columnas:"));
        _nudCols = new NumericUpDown { Minimum = 2, Maximum = 10, Value = 4, Width = 50, Margin = new Padding(0, 0, 14, 0) };
        _nudCols.ValueChanged += (_, _) => GenerateGrid();
        topPanel.Controls.Add(_nudCols);
        topPanel.Controls.Add(MkLabel("← haz clic en las celdas para dibujar", Color.FromArgb(90, 90, 90)));
        Controls.Add(topPanel);

        SetMode("wall");
    }

    private void GenerateGrid()
    {
        // Protección: puede dispararse antes de que los controles estén listos
        if (_nudRows == null || _nudCols == null || _gridPanel == null) return;

        int newRows = (int)_nudRows.Value;
        int newCols = (int)_nudCols.Value;

        // Si cambiaron las dimensiones, resetear todo el estado
        bool dimensionsChanged = newRows != _rows || newCols != _cols;
        if (dimensionsChanged)
        {
            _rows     = newRows;
            _cols     = newCols;
            _gridData = new int[_rows, _cols];
            _startPos = null;
            _goalPos  = null;
        }

        _gridPanel.Controls.Clear();

        // Esperar a que el panel tenga tamaño real
        if (_gridPanel.ClientSize.Width < 10 || _gridPanel.ClientSize.Height < 10)
            return;

        int padding  = 16;
        int available = Math.Min(
            _gridPanel.ClientSize.Width  - padding,
            _gridPanel.ClientSize.Height - padding);
        int cell = Math.Max(28, Math.Min(60, available / Math.Max(_rows, _cols)));

        int totalW = _cols * cell;
        int totalH = _rows * cell;
        int ox = (_gridPanel.ClientSize.Width  - totalW) / 2;
        int oy = (_gridPanel.ClientSize.Height - totalH) / 2;

        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _cols; c++)
            {
                int row = r, col = c;

                // Determinar estado actual de la celda
                Color back; Color fore; string text; string tag;
                var pos = new Position(row, col);

                if (!dimensionsChanged && _startPos.HasValue && _startPos.Value == pos)
                    (back, fore, text, tag) = (ColStart, Color.White, "S", "start");
                else if (!dimensionsChanged && _goalPos.HasValue && _goalPos.Value == pos)
                    (back, fore, text, tag) = (ColGoal, Color.White, "G", "goal");
                else if (!dimensionsChanged && _gridData[row, col] == 1)
                    (back, fore, text, tag) = (ColWall, ColWall, "", "wall");
                else
                    (back, fore, text, tag) = (ColFree, Color.Black, "", "free");

                var btn = new Button
                {
                    Bounds    = new Rectangle(ox + col * cell, oy + row * cell, cell - 2, cell - 2),
                    BackColor = back,
                    ForeColor = fore,
                    Text      = text,
                    Tag       = tag,
                    FlatStyle = FlatStyle.Flat,
                    Cursor    = Cursors.Hand,
                    Font      = new Font("Segoe UI", cell * 0.30f, FontStyle.Bold, GraphicsUnit.Pixel)
                };
                btn.FlatAppearance.BorderColor = ColGrid;
                btn.FlatAppearance.BorderSize  = 1;
                btn.Click += (_, _) => CellClicked(row, col, btn);
                _gridPanel.Controls.Add(btn);
            }
        }

        UpdateHint();
    }

    private void CellClicked(int row, int col, Button btn)
    {
        switch (_drawMode)
        {
            case "start":
                if (_startPos.HasValue) ResetCellAt(_startPos.Value);
                _startPos = new Position(row, col);
                _gridData[row, col] = 0;
                StyleCell(btn, ColStart, Color.White, "S", "start");
                break;

            case "goal":
                if (_goalPos.HasValue) ResetCellAt(_goalPos.Value);
                _goalPos = new Position(row, col);
                _gridData[row, col] = 0;
                StyleCell(btn, ColGoal, Color.White, "G", "goal");
                break;

            case "wall":
                if ((string?)btn.Tag is "start" or "goal") return;
                _gridData[row, col] = 1;
                StyleCell(btn, ColWall, ColWall, "", "wall");
                break;

            case "free":
                if ((string?)btn.Tag == "start") _startPos = null;
                if ((string?)btn.Tag == "goal")  _goalPos  = null;
                _gridData[row, col] = 0;
                StyleCell(btn, ColFree, Color.Black, "", "free");
                break;
        }
        UpdateHint();
    }

    private void ResetCellAt(Position pos)
    {
        int cell = GetCellSize();
        int totalW = _cols * cell;
        int totalH = _rows * cell;
        int ox = (_gridPanel.ClientSize.Width  - totalW) / 2;
        int oy = (_gridPanel.ClientSize.Height - totalH) / 2;

        int cx = ox + pos.Col * cell + cell / 2;
        int cy = oy + pos.Row * cell + cell / 2;

        var btn = _gridPanel.Controls.OfType<Button>()
            .FirstOrDefault(b => b.Bounds.Contains(cx, cy));

        if (btn != null)
        {
            _gridData[pos.Row, pos.Col] = 0;
            StyleCell(btn, ColFree, Color.Black, "", "free");
        }
    }

    private static void StyleCell(Button btn, Color back, Color fore, string text, string tag)
    {
        btn.BackColor = back;
        btn.ForeColor = fore;
        btn.Text      = text;
        btn.Tag       = tag;
    }

    private int GetCellSize()
    {
        int available = Math.Min(_gridPanel.ClientSize.Width - 24, _gridPanel.ClientSize.Height - 24);
        return Math.Max(32, Math.Min(60, available / Math.Max(_rows, _cols)));
    }

    private void SetMode(string mode)
    {
        _drawMode = mode;
        foreach (var b in new[] { _btnModeStart, _btnModeGoal, _btnModeWall, _btnModeFree })
            b.FlatAppearance.BorderSize = 0;

        var active = mode switch
        {
            "start" => _btnModeStart,
            "goal"  => _btnModeGoal,
            "wall"  => _btnModeWall,
            _       => _btnModeFree
        };
        active.FlatAppearance.BorderSize  = 2;
        active.FlatAppearance.BorderColor = Color.Yellow;
        UpdateHint();
    }

    private void UpdateHint()
    {
        string modeText = _drawMode switch
        {
            "start" => "Inicio (S)",
            "goal"  => "Meta (G)",
            "wall"  => "Pared",
            _       => "Libre"
        };
        string si = _startPos.HasValue ? $"S ({_startPos.Value.Row},{_startPos.Value.Col})" : "sin inicio";
        string gi = _goalPos.HasValue  ? $"G ({_goalPos.Value.Row},{_goalPos.Value.Col})"   : "sin meta";

        bool ok = _startPos.HasValue && _goalPos.HasValue;
        _lblHint.Text      = $"Modo activo: {modeText}   |   {si}   |   {gi}";
        _lblHint.ForeColor = ok ? Color.FromArgb(21, 128, 61) : Color.FromArgb(185, 28, 28);
    }

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        if (!_startPos.HasValue)
        {
            MessageBox.Show("Debes colocar exactamente un punto de Inicio (S).",
                "Falta inicio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (!_goalPos.HasValue)
        {
            MessageBox.Show("Debes colocar exactamente un punto de Meta (G).",
                "Falta meta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        Result = new MazeGrid(_gridData, _startPos.Value, _goalPos.Value);
        DialogResult = DialogResult.OK;
        Close();
    }

    private static Label MkLabel(string text, Color? color = null) => new()
    {
        Text = text, AutoSize = true,
        Margin = new Padding(0, 6, 6, 0),
        ForeColor = color ?? Color.FromArgb(40, 40, 40)
    };

    private static Button MkModeBtn(string text, Color back, Color fore) => new()
    {
        Text = text, Width = 90, Height = 30,
        BackColor = back, ForeColor = fore,
        FlatStyle = FlatStyle.Flat,
        Margin = new Padding(0, 0, 6, 0),
        Cursor = Cursors.Hand,
        Font = new Font("Segoe UI", 8.5f, FontStyle.Bold)
    };
}
