namespace Ejercicio02_Caballo.WinForms;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        panelControl = new Panel();
        lblStatus = new Label();
        btnSolve = new Button();
        btnBuild = new Button();
        numCols = new NumericUpDown();
        lblCols = new Label();
        numRows = new NumericUpDown();
        lblRows = new Label();
        dgvBoard = new DataGridView();
        timerAnimation = new System.Windows.Forms.Timer(components);
        panelControl.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numCols).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numRows).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvBoard).BeginInit();
        SuspendLayout();
        // 
        // panelControl
        // 
        panelControl.Controls.Add(lblStatus);
        panelControl.Controls.Add(btnSolve);
        panelControl.Controls.Add(btnBuild);
        panelControl.Controls.Add(numCols);
        panelControl.Controls.Add(lblCols);
        panelControl.Controls.Add(numRows);
        panelControl.Controls.Add(lblRows);
        panelControl.Dock = DockStyle.Left;
        panelControl.Location = new Point(0, 0);
        panelControl.Name = "panelControl";
        panelControl.Size = new Size(244, 561);
        panelControl.TabIndex = 0;
        // 
        // lblStatus
        // 
        lblStatus.Location = new Point(12, 196);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(215, 100);
        lblStatus.TabIndex = 6;
        lblStatus.Text = "Escoge las dimensiones del tablero y pulsa 'Crear Tablero'. Luego haz clic en cualquier celda para definir la posición inicial del caballo.";
        // 
        // btnSolve
        // 
        btnSolve.Enabled = false;
        btnSolve.Location = new Point(12, 149);
        btnSolve.Name = "btnSolve";
        btnSolve.Size = new Size(215, 34);
        btnSolve.TabIndex = 5;
        btnSolve.Text = "Resolver (Iniciar Animación)";
        btnSolve.UseVisualStyleBackColor = true;
        // 
        // btnBuild
        // 
        btnBuild.Location = new Point(12, 105);
        btnBuild.Name = "btnBuild";
        btnBuild.Size = new Size(215, 34);
        btnBuild.TabIndex = 4;
        btnBuild.Text = "Crear Tablero";
        btnBuild.UseVisualStyleBackColor = true;
        // 
        // numCols
        // 
        numCols.Location = new Point(91, 56);
        numCols.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        numCols.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
        numCols.Name = "numCols";
        numCols.Size = new Size(136, 23);
        numCols.TabIndex = 3;
        numCols.Value = new decimal(new int[] { 8, 0, 0, 0 });
        // 
        // lblCols
        // 
        lblCols.AutoSize = true;
        lblCols.Location = new Point(12, 58);
        lblCols.Name = "lblCols";
        lblCols.Size = new Size(64, 15);
        lblCols.TabIndex = 2;
        lblCols.Text = "Columnas:";
        // 
        // numRows
        // 
        numRows.Location = new Point(91, 16);
        numRows.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        numRows.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
        numRows.Name = "numRows";
        numRows.Size = new Size(136, 23);
        numRows.TabIndex = 1;
        numRows.Value = new decimal(new int[] { 8, 0, 0, 0 });
        // 
        // lblRows
        // 
        lblRows.AutoSize = true;
        lblRows.Location = new Point(12, 18);
        lblRows.Name = "lblRows";
        lblRows.Size = new Size(33, 15);
        lblRows.TabIndex = 0;
        lblRows.Text = "Filas:";
        // 
        // dgvBoard
        // 
        dgvBoard.AllowUserToAddRows = false;
        dgvBoard.AllowUserToDeleteRows = false;
        dgvBoard.AllowUserToResizeColumns = false;
        dgvBoard.AllowUserToResizeRows = false;
        dgvBoard.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvBoard.ColumnHeadersVisible = false;
        dgvBoard.Dock = DockStyle.Fill;
        dgvBoard.Location = new Point(244, 0);
        dgvBoard.MultiSelect = false;
        dgvBoard.Name = "dgvBoard";
        dgvBoard.ReadOnly = true;
        dgvBoard.RowHeadersVisible = false;
        dgvBoard.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
        dgvBoard.ScrollBars = ScrollBars.None;
        dgvBoard.SelectionMode = DataGridViewSelectionMode.CellSelect;
        dgvBoard.Size = new Size(640, 561);
        dgvBoard.TabIndex = 1;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(884, 561);
        Controls.Add(dgvBoard);
        Controls.Add(panelControl);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Recorrido del Caballo (Knight's Tour)";
        panelControl.ResumeLayout(false);
        panelControl.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numCols).EndInit();
        ((System.ComponentModel.ISupportInitialize)numRows).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvBoard).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private Panel panelControl;
    private Label lblRows;
    private NumericUpDown numRows;
    private NumericUpDown numCols;
    private Label lblCols;
    private Button btnSolve;
    private Button btnBuild;
    private Label lblStatus;
    private DataGridView dgvBoard;
    private System.Windows.Forms.Timer timerAnimation;
}
