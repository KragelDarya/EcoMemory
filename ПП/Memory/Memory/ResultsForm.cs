using System;
using System.Drawing;
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
            this.Size = new System.Drawing.Size(1080, 830);

            PictureBox pictureTitle = new PictureBox
            {
                Image = Image.FromFile("files/TWStat.png"), // укажите нужное изображение
                Location = new Point(125, 120),
                Size = new Size(300, 60), // можно отрегулировать размер под изображение
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };

            Button btn4x3 = new Button { Top = 220, Left = 155, Width = 220, Height = 60 };
            Button btn4x4 = new Button { Top = 320, Left = 155, Width = 220, Height = 60 };
            Button btn4x5 = new Button { Top = 420, Left = 155, Width = 220, Height = 60 };
            Button btnBack = new Button { Top = 560, Left = 155, Width = 220, Height = 60 };

            // Картинки для кнопок
            btn4x3.BackgroundImage = Image.FromFile("files/TW4x3.png");
            btn4x3.BackgroundImageLayout = ImageLayout.Stretch;
            btn4x3.Text = "";

            btn4x4.BackgroundImage = Image.FromFile("files/TW4x4.png");
            btn4x4.BackgroundImageLayout = ImageLayout.Stretch;
            btn4x4.Text = "";

            btn4x5.BackgroundImage = Image.FromFile("files/TW4x5.png");
            btn4x5.BackgroundImageLayout = ImageLayout.Stretch;
            btn4x5.Text = "";

            btnBack.BackgroundImage = Image.FromFile("files/TWToMain.png");
            btnBack.BackgroundImageLayout = ImageLayout.Stretch;
            btnBack.Text = "";
            // Скрываем рамки и фон, оставляем только картинку
            btn4x3.FlatStyle = FlatStyle.Flat;
            btn4x3.FlatAppearance.BorderSize = 0;
            btn4x3.BackColor = Color.Transparent;

            btn4x4.FlatStyle = FlatStyle.Flat;
            btn4x4.FlatAppearance.BorderSize = 0;
            btn4x4.BackColor = Color.Transparent;

            btn4x5.FlatStyle = FlatStyle.Flat;
            btn4x5.FlatAppearance.BorderSize = 0;
            btn4x5.BackColor = Color.Transparent;

            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.BackColor = Color.Transparent;

            btn4x3.Click += (s, e) => ShowResults(3);
            btn4x4.Click += (s, e) => ShowResults(4);
            btn4x5.Click += (s, e) => ShowResults(5);

            btnBack.Click += (s, e) => this.Close();

            lblResults = new Label
            {
                Top = 130,
                Left = 550,
                Width = 420,
                Height = 500,
                AutoSize = false,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Font = new Font("Lucida Console", 14)
            };

            this.Controls.Add(pictureTitle);
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
