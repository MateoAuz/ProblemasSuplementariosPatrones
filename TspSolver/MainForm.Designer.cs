namespace TspSolver
{
    partial class MainForm
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
            this.gridDistances = new System.Windows.Forms.DataGridView();
            this.btnSolve = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblMatrix = new System.Windows.Forms.Label();
            this.lblOrigin = new System.Windows.Forms.Label();
            this.cmbOrigin = new System.Windows.Forms.ComboBox();
            this.lblDestination = new System.Windows.Forms.Label();
            this.cmbDestination = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridDistances)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(273, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Problema del Viajero TSP";
            // 
            // lblOrigin
            // 
            this.lblOrigin.AutoSize = true;
            this.lblOrigin.Location = new System.Drawing.Point(14, 50);
            this.lblOrigin.Name = "lblOrigin";
            this.lblOrigin.Size = new System.Drawing.Size(46, 15);
            this.lblOrigin.TabIndex = 5;
            this.lblOrigin.Text = "Origen:";
            // 
            // cmbOrigin
            // 
            this.cmbOrigin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrigin.FormattingEnabled = true;
            this.cmbOrigin.Location = new System.Drawing.Point(65, 47);
            this.cmbOrigin.Name = "cmbOrigin";
            this.cmbOrigin.Size = new System.Drawing.Size(121, 23);
            this.cmbOrigin.TabIndex = 6;
            // 
            // lblDestination
            // 
            this.lblDestination.AutoSize = true;
            this.lblDestination.Location = new System.Drawing.Point(200, 50);
            this.lblDestination.Name = "lblDestination";
            this.lblDestination.Size = new System.Drawing.Size(50, 15);
            this.lblDestination.TabIndex = 7;
            this.lblDestination.Text = "Destino:";
            // 
            // cmbDestination
            // 
            this.cmbDestination.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDestination.FormattingEnabled = true;
            this.cmbDestination.Location = new System.Drawing.Point(255, 47);
            this.cmbDestination.Name = "cmbDestination";
            this.cmbDestination.Size = new System.Drawing.Size(121, 23);
            this.cmbDestination.TabIndex = 8;
            // 
            // lblMatrix
            // 
            this.lblMatrix.AutoSize = true;
            this.lblMatrix.Location = new System.Drawing.Point(14, 80);
            this.lblMatrix.Name = "lblMatrix";
            this.lblMatrix.Size = new System.Drawing.Size(123, 15);
            this.lblMatrix.TabIndex = 1;
            this.lblMatrix.Text = "Matriz de Distancias:";
            // 
            // gridDistances
            // 
            this.gridDistances.AllowUserToAddRows = false;
            this.gridDistances.AllowUserToDeleteRows = false;
            this.gridDistances.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDistances.Location = new System.Drawing.Point(16, 100);
            this.gridDistances.Name = "gridDistances";
            this.gridDistances.RowTemplate.Height = 25;
            this.gridDistances.Size = new System.Drawing.Size(500, 200);
            this.gridDistances.TabIndex = 2;
            // 
            // btnSolve
            // 
            this.btnSolve.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSolve.Location = new System.Drawing.Point(16, 310);
            this.btnSolve.Name = "btnSolve";
            this.btnSolve.Size = new System.Drawing.Size(500, 40);
            this.btnSolve.TabIndex = 3;
            this.btnSolve.Text = "Viajar";
            this.btnSolve.UseVisualStyleBackColor = true;
            this.btnSolve.Click += new System.EventHandler(this.btnSolve_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(16, 360);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(500, 200);
            this.txtOutput.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 580);
            this.Controls.Add(this.cmbDestination);
            this.Controls.Add(this.lblDestination);
            this.Controls.Add(this.cmbOrigin);
            this.Controls.Add(this.lblOrigin);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnSolve);
            this.Controls.Add(this.gridDistances);
            this.Controls.Add(this.lblMatrix);
            this.Controls.Add(this.lblTitle);
            this.Name = "MainForm";
            this.Text = "Viajero TSP Solver";
            ((System.ComponentModel.ISupportInitialize)(this.gridDistances)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
