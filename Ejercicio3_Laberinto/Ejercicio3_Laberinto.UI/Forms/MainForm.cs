using Ejercicio3_Laberinto.Maze;
using Ejercicio3_Laberinto.UI.Controls;

namespace Ejercicio3_Laberinto.UI.Forms;

/// <summary>
/// Formulario principal de la aplicación.
/// Contiene el panel del laberinto, controles de algoritmo, velocidad y animación.
/// </summary>
public class MainForm : Form
{
    // Controles UI
    private MazePanel   _mazePanel    = null!;
    private RadioButton _rbBfs        = null!, _rbDfs = null!;
    private TrackBar    _trackSpeed   = null!;
    private Label       _lblSpeed     = null!;
    private Button      _btnPlay      = null!, _btnStep = null!, _btnReset = null!;
    private Button      _btnExample   = null!, _btnCustom = null!;
    private Label       _lblVisited   = null!, _lblSteps = null!, _lblStatus = null!;

    // Estado de animación
    private MazeGrid?       _currentMaze;
    private List<Position>  _visitedSteps = [];
    private List<Position>? _solutionPath;
    private int             _animIndex;
    private bool            _animFinished;
    private System.Windows.Forms.Timer _timer = null!;

    public MainForm()
    {
        InitializeComponents();
        LoadExampleMaze();
    }

    private void InitializeComponents()
    {
        Text = "Problema del Laberinto — Patrones de Software  |  UTA FISEI 2026";
        Size = new Size(860, 620);
        MinimumSize = new Size(860, 620);
        MaximumSize = new Size(860, 620);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.FromArgb(240, 240, 240);
        Font = new Font("Segoe UI", 9f);

        // ── Layout principal ──────────────────────────────────────────
        var mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Padding = new Padding(10)
        };
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        Controls.Add(mainLayout);

        // ── Panel izquierdo ───────────────────────────────────────────
        var sidebar = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(232, 232, 232),
            Padding = new Padding(10)
        };
        mainLayout.Controls.Add(sidebar, 0, 0);

        var sideFlow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
            Width = 180
        };
        sidebar.Controls.Add(sideFlow);

        // -- Sección algoritmo
        sideFlow.Controls.Add(MakeSectionLabel("Algoritmo"));
        _rbBfs = new RadioButton { Text = "BFS — Breadth First", Checked = true, AutoSize = true, Margin = new Padding(2, 2, 2, 2) };
        _rbDfs = new RadioButton { Text = "DFS — Depth First",   AutoSize = true, Margin = new Padding(2, 2, 2, 8) };
        sideFlow.Controls.Add(_rbBfs);
        sideFlow.Controls.Add(_rbDfs);

        // -- Sección velocidad
        sideFlow.Controls.Add(MakeSectionLabel("Velocidad de animación"));
        _trackSpeed = new TrackBar { Minimum = 1, Maximum = 5, Value = 3, Width = 170, TickFrequency = 1, SmallChange = 1 };
        _trackSpeed.ValueChanged += (_, _) => UpdateSpeedLabel();
        sideFlow.Controls.Add(_trackSpeed);
        _lblSpeed = new Label { Text = "Normal", AutoSize = true, ForeColor = Color.FromArgb(80, 80, 80), Margin = new Padding(2, 0, 2, 8) };
        sideFlow.Controls.Add(_lblSpeed);

        // -- Sección laberinto
        sideFlow.Controls.Add(MakeSectionLabel("Laberinto"));
        _btnExample = MakeButton("Del enunciado (4×4)", Color.FromArgb(30, 64, 175), Color.White);
        _btnCustom  = MakeButton("Personalizado...",    Color.FromArgb(67, 56, 202), Color.White);
        _btnExample.Click += (_, _) => LoadExampleMaze();
        _btnCustom.Click  += (_, _) => LoadCustomMaze();
        sideFlow.Controls.Add(_btnExample);
        sideFlow.Controls.Add(_btnCustom);

        // -- Sección acciones
        sideFlow.Controls.Add(MakeSectionLabel("Acciones"));
        _btnPlay  = MakeButton("▶  Iniciar",     Color.FromArgb(21, 128, 61),  Color.White);
        _btnStep  = MakeButton("→  Paso a paso", Color.FromArgb(29, 78, 216),  Color.White);
        _btnReset = MakeButton("↺  Reiniciar",   Color.FromArgb(107, 114, 128), Color.White);
        _btnPlay.Click  += BtnPlay_Click;
        _btnStep.Click  += BtnStep_Click;
        _btnReset.Click += BtnReset_Click;
        sideFlow.Controls.Add(_btnPlay);
        sideFlow.Controls.Add(_btnStep);
        sideFlow.Controls.Add(_btnReset);

        // -- Leyenda
        sideFlow.Controls.Add(MakeSectionLabel("Leyenda"));
        sideFlow.Controls.Add(MakeLegendItem(Color.FromArgb(21, 128, 61),   "Inicio (S)"));
        sideFlow.Controls.Add(MakeLegendItem(Color.FromArgb(185, 28, 28),   "Meta (G)"));
        sideFlow.Controls.Add(MakeLegendItem(Color.FromArgb(30, 41, 59),    "Pared"));
        sideFlow.Controls.Add(MakeLegendItem(Color.FromArgb(248, 250, 252), "Libre", bordered: true));
        sideFlow.Controls.Add(MakeLegendItem(Color.FromArgb(219, 234, 254), "Visitado"));
        sideFlow.Controls.Add(MakeLegendItem(Color.FromArgb(254, 240, 138), "Solución"));

        // ── Panel derecho: laberinto + statusbar ──────────────────────
        var rightPanel = new Panel { Dock = DockStyle.Fill };
        mainLayout.Controls.Add(rightPanel, 1, 0);

        _mazePanel = new MazePanel { Dock = DockStyle.Fill };
        rightPanel.Controls.Add(_mazePanel);

        var statusBar = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 32,
            BackColor = Color.FromArgb(224, 224, 224),
            Padding = new Padding(8, 0, 8, 0)
        };
        rightPanel.Controls.Add(statusBar);

        var statusFlow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight
        };
        statusBar.Controls.Add(statusFlow);

        _lblVisited = MakeStatLabel("Visitados: 0");
        _lblSteps   = MakeStatLabel("Pasos solución: —");
        _lblStatus  = MakeStatLabel("Listo");
        statusFlow.Controls.Add(_lblVisited);
        statusFlow.Controls.Add(new Label { Text = "|", AutoSize = true, Margin = new Padding(4, 7, 4, 0), ForeColor = Color.Gray });
        statusFlow.Controls.Add(_lblSteps);
        statusFlow.Controls.Add(new Label { Text = "|", AutoSize = true, Margin = new Padding(4, 7, 4, 0), ForeColor = Color.Gray });
        statusFlow.Controls.Add(_lblStatus);

        // ── Timer de animación ────────────────────────────────────────
        _timer = new System.Windows.Forms.Timer { Interval = 150 };
        _timer.Tick += Timer_Tick;
    }

    // ── Carga de laberintos ───────────────────────────────────────────

    private void LoadExampleMaze()
    {
        StopAnimation();
        _currentMaze = MazeGrid.CreateExample();
        _mazePanel.SetMaze(_currentMaze);
        ResetStats("Laberinto del enunciado cargado.");
    }

    private void LoadCustomMaze()
    {
        StopAnimation();
        using var dialog = new CustomMazeDialog();
        if (dialog.ShowDialog(this) == DialogResult.OK && dialog.Result != null)
        {
            _currentMaze = dialog.Result;
            _mazePanel.SetMaze(_currentMaze);
            ResetStats("Laberinto personalizado cargado.");
        }
    }

    // ── Controles de animación ────────────────────────────────────────

    private void BtnPlay_Click(object? sender, EventArgs e)
    {
        if (_currentMaze == null) return;
        if (_animFinished) BtnReset_Click(sender, e);

        var solver = GetSolver();
        (_visitedSteps, _solutionPath) = solver.SolveWithSteps(_currentMaze);
        _animIndex = 0;

        _timer.Interval = SpeedToInterval();
        _timer.Start();
        SetButtonsEnabled(false);
        SetStatus("Buscando...", Color.FromArgb(29, 78, 216));
    }

    private void BtnStep_Click(object? sender, EventArgs e)
    {
        if (_currentMaze == null) return;

        if (_visitedSteps.Count == 0)
        {
            var solver = GetSolver();
            (_visitedSteps, _solutionPath) = solver.SolveWithSteps(_currentMaze);
            _animIndex = 0;
        }

        if (_animIndex < _visitedSteps.Count)
        {
            _animIndex++;
            var shown = _visitedSteps.Take(_animIndex).ToList();
            _mazePanel.UpdateState(shown);
            UpdateVisitedLabel(_animIndex);
            SetStatus($"Paso {_animIndex} / {_visitedSteps.Count}", Color.FromArgb(29, 78, 216));
        }

        if (_animIndex >= _visitedSteps.Count)
            ShowSolution();
    }

    private void BtnReset_Click(object? sender, EventArgs e)
    {
        StopAnimation();
        _visitedSteps.Clear();
        _solutionPath = null;
        _animIndex    = 0;
        _animFinished = false;
        _mazePanel.Reset();
        SetButtonsEnabled(true);
        ResetStats("Reiniciado.");
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (_animIndex < _visitedSteps.Count)
        {
            _animIndex++;
            var shown = _visitedSteps.Take(_animIndex).ToList();
            _mazePanel.UpdateState(shown);
            UpdateVisitedLabel(_animIndex);
        }
        else
        {
            _timer.Stop();
            ShowSolution();
        }
    }

    private void ShowSolution()
    {
        _animFinished = true;
        SetButtonsEnabled(true);

        if (_solutionPath != null)
        {
            _mazePanel.UpdateState(_visitedSteps, _solutionPath);
            _lblSteps.Text = $"Pasos solución: {_solutionPath.Count}";
            SetStatus("Solución encontrada", Color.FromArgb(21, 128, 61));
        }
        else
        {
            SetStatus("No existe camino", Color.FromArgb(185, 28, 28));
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────

    private IMazeSolver GetSolver() =>
        _rbBfs.Checked ? new BfsMazeSolver() : new DfsMazeSolver();

    private void StopAnimation()
    {
        _timer.Stop();
        SetButtonsEnabled(true);
    }

    private int SpeedToInterval() => _trackSpeed.Value switch
    {
        1 => 500,
        2 => 300,
        3 => 150,
        4 => 60,
        5 => 20,
        _ => 150
    };

    private void UpdateSpeedLabel()
    {
        _lblSpeed.Text = _trackSpeed.Value switch
        {
            1 => "Muy lento",
            2 => "Lento",
            3 => "Normal",
            4 => "Rápido",
            5 => "Muy rápido",
            _ => "Normal"
        };
        if (_timer.Enabled) _timer.Interval = SpeedToInterval();
    }

    private void UpdateVisitedLabel(int count) =>
        _lblVisited.Text = $"Visitados: {count}";

    private void ResetStats(string status)
    {
        _lblVisited.Text = "Visitados: 0";
        _lblSteps.Text   = "Pasos solución: —";
        SetStatus(status, Color.FromArgb(80, 80, 80));
    }

    private void SetStatus(string text, Color color)
    {
        _lblStatus.Text      = text;
        _lblStatus.ForeColor = color;
    }

    private void SetButtonsEnabled(bool enabled)
    {
        _btnPlay.Enabled    = enabled;
        _btnStep.Enabled    = enabled;
        _btnExample.Enabled = enabled;
        _btnCustom.Enabled  = enabled;
        _rbBfs.Enabled      = enabled;
        _rbDfs.Enabled      = enabled;
    }

    // ── Factories de controles ────────────────────────────────────────

    private static Label MakeSectionLabel(string text) => new()
    {
        Text = text,
        AutoSize = true,
        Font = new Font("Segoe UI", 8f, FontStyle.Bold),
        ForeColor = Color.FromArgb(100, 100, 100),
        Margin = new Padding(2, 10, 2, 3)
    };

    private static Button MakeButton(string text, Color back, Color fore) => new()
    {
        Text = text,
        Width = 175,
        Height = 30,
        BackColor = back,
        ForeColor = fore,
        FlatStyle = FlatStyle.Flat,
        Margin = new Padding(2, 2, 2, 2),
        Cursor = Cursors.Hand
    };

    private static Label MakeStatLabel(string text) => new()
    {
        Text = text,
        AutoSize = true,
        Margin = new Padding(0, 7, 0, 0),
        Font = new Font("Segoe UI", 8.5f)
    };

    private static Panel MakeLegendItem(Color color, string label, bool bordered = false)
    {
        var p = new Panel { Width = 175, Height = 20, Margin = new Padding(2, 1, 2, 1) };
        var box = new Panel
        {
            Width = 14, Height = 14, BackColor = color, Left = 0, Top = 3,
            BorderStyle = bordered ? BorderStyle.FixedSingle : BorderStyle.None
        };
        var lbl = new Label { Text = label, Left = 20, Top = 1, AutoSize = true, ForeColor = Color.FromArgb(50, 50, 50), Font = new Font("Segoe UI", 8.5f) };
        p.Controls.Add(box);
        p.Controls.Add(lbl);
        return p;
    }
}
