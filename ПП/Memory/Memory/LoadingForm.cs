using System;
using System.IO;
using System.Windows.Forms;

namespace Memory
{
    public partial class LoadingForm : Form
    {
      
        private int elapsedTime = 0;
        private string backgroundPath;
        public LoadingForm()
        {
            InitializeComponent();
            InitializeSplash();

            Global.LoadingForm = this;
        }
        private void InitializeSplash()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            ProgressBar progressBar = new ProgressBar();
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.Width = (int)(300);
            progressBar.Height = (int)(40); 
            progressBar.Top = (this.ClientSize.Height - progressBar.Height) / 2;
            progressBar.Left = (this.ClientSize.Width - progressBar.Width) / 2;
            progressBar.Name = "progressBar";
            this.Controls.Add(progressBar);

            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += (s, e) =>
            {
                elapsedTime += 100;
                progressBar.Value = Math.Min(progressBar.Maximum, (int)(progressBar.Maximum * elapsedTime / 3000));
                if (elapsedTime >= 3600)
                {
                    timer.Stop();
                    MainForm mainForm = new MainForm();
                    mainForm.Show();
                    this.Hide(); 
                }
            };
            timer.Start();
        }

        private void LoadingForm_Load(object sender, EventArgs e)
        {
            backgroundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files/5f.bmp");
            this.BackgroundImage = System.Drawing.Image.FromFile(backgroundPath);
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

    }
}