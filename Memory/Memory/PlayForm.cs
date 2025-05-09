using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Memory
{
    public partial class PlayForm : Form
    {
        private int gridSize = 3; // по умолчанию 4x4
        private List<Image> images; // список всех изображений
        private List<Card> cards = new List<Card>();
        private Card firstSelected = null;
        private Card secondSelected = null;
        private Timer gameTimer;
        private int secondsElapsed = 0;
        private string statsFile = "stats.txt";
        public PlayForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Width = 1000;
            this.Height = 700;
            // Создаем таймер
            gameTimer = new Timer();
            gameTimer.Interval = 1000; // 1 секунда
            gameTimer.Tick += GameTimer_Tick;
            // Создаем панель для карточек
            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.WrapContents = true;
            panel.AutoSize = false;
            this.Controls.Add(panel);
            // Создаем метку для таймера
            Label lblTimer = new Label();
            lblTimer.Name = "lblTimer";
            lblTimer.Text = "Время: 0 сек";
            lblTimer.Font = new Font("Arial", 14);
            lblTimer.AutoSize = true;
            lblTimer.Location = new Point(10, 10);
            this.Controls.Add(lblTimer);
            lblTimer.BringToFront();
            // Создаем кнопки для выбора уровня
            Panel topPanel = new Panel();
            topPanel.Height = 50;
            topPanel.Dock = DockStyle.Top;
            this.Controls.Add(topPanel);

            Button btn4x4 = new Button() { Text = "4x4", Tag = 4, Width = 60, Left = 300, Top = 10 };
            btn4x4.Click += LevelButton_Click;
            topPanel.Controls.Add(btn4x4);

            Button btn4x5 = new Button() { Text = "4x5", Tag = 5, Width = 60, Left = 400, Top = 10 };
            btn4x5.Click += LevelButton_Click;
            topPanel.Controls.Add(btn4x5);

            Button btn4x3 = new Button() { Text = "4x3", Tag = 3, Width = 60, Left = 200, Top = 10 };
            btn4x3.Click += LevelButton_Click;
            topPanel.Controls.Add(btn4x3);

            Button btnBackToMain = new Button()
            {
                Text = "На главную",
                Width = 100,
                Height = 30,
                Left = 700,
                Top = 10
            };
            btnBackToMain.Click += BtnBackToMain_Click;
            topPanel.Controls.Add(btnBackToMain);

            // Загрузка изображений
            LoadImages();
            // Запуск игры с выбранным уровнем
            StartGame(gridSize);

        }
        //Загрузка изображений для карточек
        private void LoadImages()
        {
            images = new List<Image>();   
            for (int i = 1; i <= 10; i++)
            {
                 Image img = Image.FromFile($"files/img{i}.png");
                 images.Add(img);
                
            }
        }
        private void LevelButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int size = (int)btn.Tag;
            StartGame(size);
        }
        private void StartGame(int size)
        {
            gridSize = size;
            secondsElapsed = 0;

            var lblTimer = this.Controls.Find("lblTimer", true).FirstOrDefault() as Label;
            if (lblTimer != null)
                lblTimer.Text = "Время: 0 сек";

            gameTimer.Stop();

            // Удаляем старые карточки
            var oldTable = this.Controls.OfType<TableLayoutPanel>().FirstOrDefault();
            if (oldTable != null)
                this.Controls.Remove(oldTable);

            cards.Clear();
            firstSelected = null;
            secondSelected = null;

            int totalCards = 4 * size;
            int rows = 4;
            int cols = size;

            // Выбор и перемешивание изображений
            var rnd = new Random();
            var selectedImages = images.OrderBy(x => rnd.Next()).Take(totalCards / 2).ToList();

            var gameImages = new List<Image>();
            foreach (var img in selectedImages)
            {
                gameImages.Add(img);
                gameImages.Add(img);
            }
            gameImages = gameImages.OrderBy(x => rnd.Next()).ToList();

            // Создание новой таблицы
            TableLayoutPanel table = new TableLayoutPanel();
            table.RowCount = rows;
            table.ColumnCount = cols;
            table.Dock = DockStyle.None;
            table.AutoSize = true;
            table.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            table.Margin = new Padding(10);
            table.Padding = new Padding(20);
            table.BackColor = Color.LightGray;
            table.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            // Устанавливаем фиксированный размер строк и столбцов
            for (int r = 0; r < rows; r++)
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            for (int c = 0; c < cols; c++)
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
            this.Controls.Add(table);
            table.BringToFront();

            // Добавление карточек
            int index = 0;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    Card card = new Card(gameImages[index]);
                    card.Button.Click += Card_Click;

                    cards.Add(card);
                    table.Controls.Add(card.Button, c, r);
                    index++;
                }
            }
            gameTimer.Start();

            // Центрируем таблицу по форме
            table.Anchor = AnchorStyles.None; // отключаем привязку к сторонам

            // Обновляем расположение вручную
            table.Location = new Point(
                (this.ClientSize.Width - table.PreferredSize.Width) / 2,
                (this.ClientSize.Height - table.PreferredSize.Height) / 2
            );
        }
        private void Card_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Card clickedCard = cards.First(c => c.Button == btn);
            if (clickedCard.IsMatched || clickedCard.IsRevealed)
                return;
            if (firstSelected != null && secondSelected != null)
                return; 

            // Раскрываем карточку
            clickedCard.Reveal();

            if (firstSelected == null)
            {
                firstSelected = clickedCard;
            }
            else if (secondSelected == null)
            {
                secondSelected = clickedCard;
                // Проверка совпадения
                if (firstSelected.Image == secondSelected.Image)
                {
                    firstSelected.IsMatched = true;
                    secondSelected.IsMatched = true;

                    Timer matchDelayTimer = new Timer();
                    matchDelayTimer.Interval = 300; //0,3 секунды
                    matchDelayTimer.Tick += (s2, e2) =>
                    {
                         matchDelayTimer.Stop();

                        firstSelected.Button.Visible = false;
                        secondSelected.Button.Visible = false;

                        firstSelected = null;
                        secondSelected = null;

                        // Проверка завершения игры после задержки
                        if (cards.All(c => c.IsMatched))
                        {
                             gameTimer.Stop();
                             MessageBox.Show($"Поздравляем! Вы прошли уровень за {secondsElapsed} секунд.", "Победа");
                             SaveResult(secondsElapsed);
                        }
                    };
                    matchDelayTimer.Start();
                }
                else
                {
                    // Не совпадают, задержка перед закрытием
                    Timer delayTimer = new Timer();
                    delayTimer.Interval = 700; // 0,7 секунды
                    delayTimer.Tick += (s, args) =>
                    {
                        delayTimer.Stop();
                        firstSelected.Hide();
                        secondSelected.Hide();
                        firstSelected = null;
                        secondSelected = null;
                    };
                    delayTimer.Start();
                }
            }
        }
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            secondsElapsed++;
            var lblTimer = this.Controls.Find("lblTimer", true).FirstOrDefault() as Label;
            if (lblTimer != null)
            {
                lblTimer.Text = $"Время: {secondsElapsed} сек";
            }
        }
        private void SaveResult(int time)
        {
            string statsFileName = $"stats_{gridSize}.txt"; // Файл для текущего размера
            string entry = $"{DateTime.Now}: 4 x{gridSize} - {time} сек";
            File.AppendAllText(statsFileName, entry + Environment.NewLine);
        }
        private void BtnBackToMain_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.Show();
        }
    }

    // Класс карточки
    public class Card
    {
        public Button Button { get; private set; }
        public Image Image { get; private set; }
        public bool IsRevealed { get; private set; } = false;
        public bool IsMatched { get; set; } = false;
        private Image backImage;
        public Card(Image image)
        {
            Image = image;
            backImage = GenerateBackImage(); // создаем задний образ
            Button = new Button();
            Button.Width = 80;
            Button.Height = 80;
            Button.Margin = new Padding(2);
            Button.BackgroundImageLayout = ImageLayout.Stretch;
            Hide();
        }

        public void Reveal()
        {
            Button.BackgroundImage = Image;
            IsRevealed = true;
        }

        public void Hide()
        {
            Button.BackgroundImage = backImage;
            IsRevealed = false;
        }
        //генерация изображения для обратной стороны
        private Image GenerateBackImage()
        {
            Bitmap bmp = new Bitmap(80, 80);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
            }
            return bmp;
        }
    }
 }
