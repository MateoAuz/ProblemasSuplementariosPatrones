using Ejercicio3_Laberinto.Maze;

namespace Ejercicio3_Laberinto.UI.Controls;

/// <summary>
/// Control personalizado que dibuja el laberinto usando GDI+.
/// Muestra: paredes, celdas libres, visitadas, camino solución, inicio y meta.
/// </summary>
public class MazePanel : Panel
{
    private MazeGrid? _maze;
    private HashSet<Position> _visited = [];
    private HashSet<Position> _path = [];

    private static readonly Color ColorWall    = Color.FromArgb(30, 41, 59);
    private static readonly Color ColorFree    = Color.FromArgb(248, 250, 252);
    private static readonly Color ColorVisited = Color.FromArgb(219, 234, 254);
    private static readonly Color ColorPath    = Color.FromArgb(254, 240, 138);
    private static readonly Color ColorStart   = Color.FromArgb(21, 128, 61);
    private static readonly Color ColorGoal    = Color.FromArgb(185, 28, 28);
    private static readonly Color ColorBorder  = Color.FromArgb(226, 232, 240);

    public MazePanel()
    {
        DoubleBuffered = true;
        BackColor = Color.FromArgb(241, 245, 249);
    }

    public void SetMaze(MazeGrid maze)
    {
        _maze = maze;
        _visited.Clear();
        _path.Clear();
        Invalidate();
    }

    public void UpdateState(IEnumerable<Position> visited, IEnumerable<Position>? path = null)
    {
        _visited = new HashSet<Position>(visited);
        _path = path != null ? new HashSet<Position>(path) : [];
        Invalidate();
    }

    public void Reset()
    {
        _visited.Clear();
        _path.Clear();
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (_maze == null) return;

        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        int cellSize = CalculateCellSize();
        int offsetX  = (Width  - _maze.Cols * cellSize) / 2;
        int offsetY  = (Height - _maze.Rows * cellSize) / 2;

        for (int r = 0; r < _maze.Rows; r++)
        {
            for (int c = 0; c < _maze.Cols; c++)
            {
                var pos = new Position(r, c);
                var rect = new Rectangle(
                    offsetX + c * cellSize + 2,
                    offsetY + r * cellSize + 2,
                    cellSize - 4,
                    cellSize - 4);

                Color fillColor = GetCellColor(pos);
                using var brush = new SolidBrush(fillColor);
                g.FillRectangle(brush, rect);

                if (fillColor != ColorWall)
                {
                    using var pen = new Pen(ColorBorder, 0.5f);
                    g.DrawRectangle(pen, rect);
                }

                DrawCellLabel(g, pos, rect);
            }
        }
    }

    private Color GetCellColor(Position pos)
    {
        if (_maze == null) return ColorFree;
        if (pos == _maze.Start)   return ColorStart;
        if (pos == _maze.Goal)    return ColorGoal;
        if (_path.Contains(pos))  return ColorPath;
        if (_visited.Contains(pos)) return ColorVisited;
        if (!_maze.IsWalkable(pos)) return ColorWall;
        return ColorFree;
    }

    private void DrawCellLabel(Graphics g, Position pos, Rectangle rect)
    {
        if (_maze == null) return;

        string? label = null;
        Color textColor = Color.Black;

        if (pos == _maze.Start)
        {
            label = "S";
            textColor = Color.FromArgb(220, 252, 231);
        }
        else if (pos == _maze.Goal)
        {
            label = "G";
            textColor = Color.FromArgb(254, 226, 226);
        }
        else if (_path.Contains(pos))
        {
            label = "·";
            textColor = Color.FromArgb(133, 77, 14);
        }

        if (label == null) return;

        using var font = new Font("Segoe UI", rect.Height * 0.38f, FontStyle.Bold, GraphicsUnit.Pixel);
        using var brush = new SolidBrush(textColor);
        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString(label, font, brush, rect, sf);
    }

    private int CalculateCellSize()
    {
        if (_maze == null) return 40;
        int byWidth  = (Width  - 20) / _maze.Cols;
        int byHeight = (Height - 20) / _maze.Rows;
        return Math.Max(20, Math.Min(byWidth, byHeight));
    }
}
