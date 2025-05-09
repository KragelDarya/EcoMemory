using System;
using System.IO;
using System.Windows.Forms;

namespace Memory
{
    public partial class ResultsForm : Form
    {
        private Label lblResults;
        public ResultsForm()
        {
            InitializeComponent();
            InitializeUI();
        }
        private void InitializeUI()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Text = "Результаты";
            this.Size = new System.Drawing.Size(1000, 700);

            Button btn4x3 = new Button { Text = "Показать 4x3", Top = 10, Left = 10, Width = 170 };
            Button btn4x4 = new Button { Text = "Показать 4x4", Top = 40, Left = 10, Width = 170 };
            Button btn4x5 = new Button { Text = "Показать 4x5", Top = 70, Left = 10, Width = 170 };
            
            Button btnBack = new Button { Text = "На главную", Top = 100, Left = 10, Width = 170 };

            btn4x3.Click += (s, e) => ShowResults(3);
            btn4x4.Click += (s, e) => ShowResults(4);
            btn4x5.Click += (s, e) => ShowResults(5);
            
            btnBack.Click += (s, e) =>
            {
                this.Hide();
                var mainForm = new MainForm();
                mainForm.Show();
            };
            lblResults = new Label
            {
                Top = 10,
                Left = 220,
                Width = 320,
                Height = 300,
                AutoSize = false,
                BorderStyle = BorderStyle.FixedSingle
            };

            this.Controls.Add(btn4x3);
            this.Controls.Add(btn4x4);
            this.Controls.Add(btn4x5);
            this.Controls.Add(btnBack);
            this.Controls.Add(lblResults);
        }
        private void ShowResults(int gridSize)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"stats_{gridSize}.txt");
            if (File.Exists(path))
            {
                lblResults.Text = File.ReadAllText(path);
            }
            else
            {
                lblResults.Text = $"Файл результатов для 4x{gridSize} не найден.";
            }
        }
    }
}
