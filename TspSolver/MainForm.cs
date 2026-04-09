using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TspSolver
{
    public partial class MainForm : Form
    {
        private DataGridView gridDistances;
        private Button btnSolve;
        private TextBox txtOutput;
        private Label lblTitle;
        private Label lblMatrix;
        private Label lblOrigin;
        private ComboBox cmbOrigin;
        private Label lblDestination;
        private ComboBox cmbDestination;
        
        private string[] cities = { "Ambato", "Quito", "Riobamba", "Latacunga", "Puyo" };
        
        public MainForm()
        {
            InitializeComponent();
            SetupCustomUI();
            PopulateDefaultDistances();

            cmbOrigin.Items.AddRange(cities);
            cmbDestination.Items.AddRange(cities);
            cmbOrigin.SelectedIndex = 0;
            cmbDestination.SelectedIndex = 4;
        }

        private void SetupCustomUI()
        {
            gridDistances.ColumnCount = cities.Length;
            gridDistances.RowCount = cities.Length;

            for (int i = 0; i < cities.Length; i++)
            {
                gridDistances.Columns[i].Name = cities[i];
                gridDistances.Columns[i].Width = 70;
                gridDistances.Rows[i].HeaderCell.Value = cities[i];
            }
            gridDistances.RowHeadersWidth = 100;
        }

        private void PopulateDefaultDistances()
        {
            int[,] distances = new int[,]
            {
                { 0,   120, 60,  45,  100 },
                { 120, 0,   180, 90,  220 },
                { 60,  180, 0,   105, 130 },
                { 45,  90,  105, 0,   145 },
                { 100, 220, 130, 145, 0   }
            };

            for (int r = 0; r < cities.Length; r++)
            {
                for (int c = 0; c < cities.Length; c++)
                {
                    gridDistances.Rows[r].Cells[c].Value = distances[r, c];
                }
            }
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            try
            {
                int[,] currentDistances = new int[cities.Length, cities.Length];
                for (int r = 0; r < cities.Length; r++)
                {
                    for (int c = 0; c < cities.Length; c++)
                    {
                        if (int.TryParse(gridDistances.Rows[r].Cells[c].Value?.ToString(), out int val))
                        {
                            currentDistances[r, c] = val;
                        }
                        else
                        {
                            currentDistances[r, c] = 0;
                        }
                    }
                }

                txtOutput.Clear();
                int startCity = cmbOrigin.SelectedIndex;
                int targetCity = cmbDestination.SelectedIndex;

                if (startCity == targetCity)
                {
                    MessageBox.Show("El origen y el destino no pueden ser la misma ciudad.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                txtOutput.AppendText($"Solucionando ruta desde {cities[startCity]} hasta {cities[targetCity]} visitando todas las ciudades...\r\n\r\n");

                var solver = new BranchAndBoundSolver(currentDistances, cities, startCity, targetCity);
                var solutionNode = solver.Solve();

                if (solutionNode != null)
                {
                    txtOutput.AppendText($"Mejor Costo: {solutionNode.Cost}\r\nRuta:\r\n");
                    var current = solutionNode;
                    var pathNodes = new List<Node>();
                    while (current != null)
                    {
                        pathNodes.Insert(0, current);
                        current = current.Parent;
                    }

                    foreach (var n in pathNodes)
                    {
                        txtOutput.AppendText($"{n.Action} -> Costo acumulado: {n.Cost}\r\n");
                    }
                }
                else
                {
                    txtOutput.AppendText("No se encontro ruta valida que pase por todas las ciudades y termine en el destino indicado.\r\n");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ejecutar el solver: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
