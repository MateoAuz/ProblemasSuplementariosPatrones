namespace TicTacToeInteligenteGUI;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        btn00 = new Button();
        btn01 = new Button();
        btn02 = new Button();
        btn10 = new Button();
        btn11 = new Button();
        btn12 = new Button();
        btn20 = new Button();
        btn21 = new Button();
        btn22 = new Button();
        lblStatus = new Label();
        btnRestart = new Button();
        SuspendLayout();
        // 
        // btn00
        // 
        btn00.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
        btn00.Location = new Point(127, 76);
        btn00.Name = "btn00";
        btn00.Size = new Size(80, 80);
        btn00.TabIndex = 0;
        btn00.UseVisualStyleBackColor = true;
        btn00.Click += btn00_Click;
        // 
        // btn01
        // 
        btn01.Font = new Font("Segoe UI", 24F);
        btn01.Location = new Point(230, 76);
        btn01.Name = "btn01";
        btn01.Size = new Size(80, 80);
        btn01.TabIndex = 1;
        btn01.UseVisualStyleBackColor = true;
        btn01.Click += btn01_Click;
        // 
        // btn02
        // 
        btn02.Font = new Font("Segoe UI", 24F);
        btn02.Location = new Point(328, 76);
        btn02.Name = "btn02";
        btn02.Size = new Size(80, 80);
        btn02.TabIndex = 2;
        btn02.UseVisualStyleBackColor = true;
        btn02.Click += btn02_Click;
        // 
        // btn10
        // 
        btn10.Font = new Font("Segoe UI", 24F);
        btn10.Location = new Point(127, 162);
        btn10.Name = "btn10";
        btn10.Size = new Size(80, 80);
        btn10.TabIndex = 3;
        btn10.UseVisualStyleBackColor = true;
        btn10.Click += btn10_Click;
        // 
        // btn11
        // 
        btn11.Font = new Font("Segoe UI", 24F);
        btn11.Location = new Point(230, 162);
        btn11.Name = "btn11";
        btn11.Size = new Size(80, 80);
        btn11.TabIndex = 4;
        btn11.UseVisualStyleBackColor = true;
        btn11.Click += btn11_Click;
        // 
        // btn12
        // 
        btn12.Font = new Font("Segoe UI", 24F);
        btn12.Location = new Point(328, 162);
        btn12.Name = "btn12";
        btn12.Size = new Size(80, 80);
        btn12.TabIndex = 5;
        btn12.UseVisualStyleBackColor = true;
        btn12.Click += btn12_Click;
        // 
        // btn20
        // 
        btn20.Font = new Font("Segoe UI", 24F);
        btn20.Location = new Point(127, 248);
        btn20.Name = "btn20";
        btn20.Size = new Size(80, 80);
        btn20.TabIndex = 6;
        btn20.UseVisualStyleBackColor = true;
        btn20.Click += btn20_Click;
        // 
        // btn21
        // 
        btn21.Font = new Font("Segoe UI", 24F);
        btn21.Location = new Point(230, 248);
        btn21.Name = "btn21";
        btn21.Size = new Size(80, 80);
        btn21.TabIndex = 7;
        btn21.UseVisualStyleBackColor = true;
        btn21.Click += btn21_Click;
        // 
        // btn22
        // 
        btn22.Font = new Font("Segoe UI", 24F);
        btn22.Location = new Point(328, 248);
        btn22.Name = "btn22";
        btn22.Size = new Size(80, 80);
        btn22.TabIndex = 8;
        btn22.UseVisualStyleBackColor = true;
        btn22.Click += btn22_Click;
        // 
        // lblStatus
        // 
        lblStatus.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblStatus.Location = new Point(12, 9);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(527, 46);
        lblStatus.TabIndex = 9;
        lblStatus.Text = "Tu turno";
        lblStatus.TextAlign = ContentAlignment.TopCenter;
        // 
        // btnRestart
        // 
        btnRestart.Location = new Point(185, 357);
        btnRestart.Name = "btnRestart";
        btnRestart.Size = new Size(138, 46);
        btnRestart.TabIndex = 10;
        btnRestart.Text = "Reiniciar";
        btnRestart.UseVisualStyleBackColor = true;
        btnRestart.Click += btnRestart_Click;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(551, 450);
        Controls.Add(btnRestart);
        Controls.Add(lblStatus);
        Controls.Add(btn22);
        Controls.Add(btn21);
        Controls.Add(btn20);
        Controls.Add(btn12);
        Controls.Add(btn11);
        Controls.Add(btn10);
        Controls.Add(btn02);
        Controls.Add(btn01);
        Controls.Add(btn00);
        Name = "MainForm";
        Text = "Form1";
        ResumeLayout(false);
    }

    #endregion

    private Button btn00;
    private Button btn01;
    private Button btn02;
    private Button btn10;
    private Button btn11;
    private Button btn12;
    private Button btn20;
    private Button btn21;
    private Button btn22;
    private Label lblStatus;
    private Button btnRestart;
}
