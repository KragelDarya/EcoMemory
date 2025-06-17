using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;
using WMPLib;

namespace Memory
{
    public partial class MainForm : Form
    {
        private string backgroundPath;
        private WindowsMediaPlayer mediaPlayer = new WindowsMediaPlayer();
        public MainForm()
        {
            InitializeComponent();
            Global.MainForm = this;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetBackgroundImage(comboBox1.SelectedIndex + 1);
        }

        private void SetBackgroundImage(int index)
        {
            backgroundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files", $"f{index}.bmp");
            Global.BackgroundPath = backgroundPath;

            if (File.Exists(backgroundPath))
            {
                this.BackgroundImage = System.Drawing.Image.FromFile(backgroundPath);
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            mediaPlayer.settings.setMode("loop", true); // Включаем повтор
            string url = string.Empty;
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    url = "files/calm1.mp3";
                    break;
                case 1:
                    url = "files/dinamic1.mp3";
                    break;
                case 2:
                    url = "files/dinamic2.mp3";
                    break;
                case 3:
                    url = "files/zvuki-gitary.mp3";
                    break;
                case 4:
                    url = "files/calm2.mp3";
                    break;
            }

            mediaPlayer.URL = url;
            mediaPlayer.controls.play();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            backgroundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files/f2.bmp");
            Global.BackgroundPath = backgroundPath;
            this.BackgroundImage = System.Drawing.Image.FromFile(backgroundPath);
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.StartPosition = FormStartPosition.CenterScreen;
            label1.Text = "Экопамять";
            label1.Font = new Font("Arial", 20);
            label1.BackColor = Color.Transparent;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            PlayForm playForm = new PlayForm();
            playForm.Show();
            this.Hide();
        }
        private void PlayMusic(string filename)
        {
            SoundPlayer player = new SoundPlayer(filename);
 
            player.PlaySync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ResultsForm resultsForm = new ResultsForm();
            resultsForm.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Цель игры: Найти все пары одинаковых карточек за минимальное время.\n\n" +
                    "Как играть:\n" +
                    "Выберите уровень сложности (размер игрового поля: 4×3, 4×4 или 4×5). Игра представляет собой набор закрытых карточек, разложенных на экране в виде сетки. Каждая карточка содержит изображение, и каждая из них имеет одну соответствующую ей пару с таким же изображением. В начале раунда карточки перевернуты рубашкой вверх, изображения не видны. За один ход Вы можете открыть две любые карточки. Если изображения совпадают — карточки исчезают с игрового поля. Если изображения не совпадают — карточки автоматически закрываются через короткий промежуток времени.\n\n" +
                    " Результат:\n" +
                    "По завершении игры будет показано, за сколько секунд вы справились. Результат сохраняется в таблицу рекордов для выбранного уровня.",
                    "Правила игры", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, helpProvider1.HelpNamespace);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Global.LoadingForm.Close();
        }
    }  
}
