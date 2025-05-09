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
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
                SetBackgroundImage(comboBox1.SelectedIndex + 1);
        }

        private void SetBackgroundImage(int index)
        {
            backgroundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files", $"f{index}.bmp");
            if (File.Exists(backgroundPath))
            {
                this.BackgroundImage = System.Drawing.Image.FromFile(backgroundPath);
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            mediaPlayer.settings.setMode("loop", true); // Включаем повтор

            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    mediaPlayer.URL = "files/calm1.mp3";
                    break;
                case 1:
                    mediaPlayer.URL = "files/dinamic1.mp3";
                    break;
                case 2:
                    mediaPlayer.URL = "files/dinamic2.mp3";
                    break;
                case 3:
                    mediaPlayer.URL = "files/zvuki-gitary.mp3";
                    break;
                case 4:
                    mediaPlayer.URL = "files/calm2.mp3";
                    break;
            }
            mediaPlayer.controls.play();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            backgroundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files/f2.bmp");
            this.BackgroundImage = System.Drawing.Image.FromFile(backgroundPath);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Width = 1000;
            this.Height = 700;
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
            MessageBox.Show(" Цель игры: Найти все пары одинаковых карточек за минимальное время.\n\n" +
                    " Как играть:\n" +
                    "Выберите уровень сложности (размер игрового поля: 4×3, 4×4 или 4×5). На экране появятся карточки, перевёрнутые рубашкой вверх. Нажимайте на карточки, чтобы открыть их. Ваша задача — запомнить изображения и найти одинаковые пары. Если изображения совпадают — пара исчезает с поля. Если нет — карточки автоматически закрываются через короткое время.\n\n" +
                    " Результат:\n" +
                    "По завершении игры будет показано, за сколько секунд вы справились. Результат сохраняется в таблицу рекордов для выбранного уровня.",
                    " Правила игры", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, helpProvider1.HelpNamespace);
        }
    }
    
}
