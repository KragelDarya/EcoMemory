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
            Global.ResultsForm = this;

            InitializeUI();
        }
        private void InitializeUI()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Text = "Результаты";
            this.Size = new System.Drawing.Size(1080, 830);

            Button btn4x3 = new Button { Text = "Показать 4x3", Top = 70, Left = 145, Width = 170 };
            Button btn4x4 = new Button { Text = "Показать 4x4", Top = 170, Left = 145, Width = 170 };
            Button btn4x5 = new Button { Text = "Показать 4x5", Top = 270, Left = 145, Width = 170 };
            Button btnBack = new Button { Text = "На главную", Top = 370, Left = 145, Width = 170 };

            btn4x3.Click += (s, e) => ShowResults(3);
            btn4x4.Click += (s, e) => ShowResults(4);
            btn4x5.Click += (s, e) => ShowResults(5);
            
            btnBack.Click += (s, e) =>
            {
                this.Close();
            };
            lblResults = new Label
            {
                Top = 30,
                Left = 420,
                Width = 420,
                Height = 500,
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

        private void ResultsForm_Load(object sender, EventArgs e)
        {
            if (File.Exists(Global.BackgroundPath))
            {
                this.BackgroundImage = System.Drawing.Image.FromFile(Global.BackgroundPath);
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private void ResultsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Global.MainForm.Show();
        }
    }
}
