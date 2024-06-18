using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace dogrulama
{
    public partial class Form1 : Form
    {
        private Button[] btnArray;
        private PictureBox[,] pictureBoxArray;
        private Button validationButton; // Doğrulama butonu

        public Form1()
        {
            InitializeComponent();
            InitializeDragDropComponents();
            AssignFixedColors();
            AssignRandomColors();
        }

        private void InitializeDragDropComponents()
        {

            btnArray = new Button[3];
            pictureBoxArray = new PictureBox[3, 3];

            for (int i = 0; i < 3; i++)
            {

                btnArray[i] = new Button();
                btnArray[i].Text = "Sürükle";
                btnArray[i].Location = new Point(30 + i * 120, 30);
                btnArray[i].Size = new Size(100, 30);
                btnArray[i].MouseDown += BtnValidate_MouseDown;
                this.Controls.Add(btnArray[i]);
            }


            validationButton = new Button();
            validationButton.Text = "Doğrulama";
            validationButton.Size = new Size(100, 30);
            validationButton.Location = new Point(this.ClientSize.Width - validationButton.Width - 30, this.ClientSize.Height - validationButton.Height - 30);
            validationButton.Click += ValidationButton_Click;
            this.Controls.Add(validationButton);


            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    pictureBoxArray[i, j] = new PictureBox();
                    pictureBoxArray[i, j].Location = new Point(30 + i * 120, 80 + j * 120);
                    pictureBoxArray[i, j].Size = new Size(100, 100);
                    pictureBoxArray[i, j].BackColor = Color.LightGray;
                    pictureBoxArray[i, j].AllowDrop = true;
                    pictureBoxArray[i, j].DragEnter += PictureBoxTarget_DragEnter;
                    pictureBoxArray[i, j].DragDrop += PictureBoxTarget_DragDrop;
                    this.Controls.Add(pictureBoxArray[i, j]);
                }
            }
        }

        private void AssignFixedColors()
        {

            Color[] fixedColors = { Color.Red, Color.Green, Color.Blue };
            string[] colorNames = { "Kırmızı", "Yeşil", "Mavi" };


            for (int i = 0; i < 3; i++)
            {
                
                btnArray[i].BackColor = fixedColors[i];
                btnArray[i].Text = colorNames[i] + " ";


                for (int j = 0; j < 3; j++)
                {
                    pictureBoxArray[i, j].BackColor = fixedColors[i];
                }
            }
        }

        private void AssignRandomColors()
        {

            Color[] availableColors = { Color.Red, Color.Green, Color.Blue };


            Random random = new Random();


            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Color randomPictureBoxColor = availableColors[random.Next(availableColors.Length)];
                    pictureBoxArray[i, j].BackColor = randomPictureBoxColor;
                }
            }
        }

        private void BtnValidate_MouseDown(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                btn.DoDragDrop(btn.Text, DragDropEffects.Copy);
            }
        }

        private void PictureBoxTarget_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private List<PictureBox> matchedPictureBoxes = new List<PictureBox>(); // Eşleşen PictureBox listesi
        private int correctMatches = 0;

        private void PictureBoxTarget_DragDrop(object sender, DragEventArgs e)
        {
            string data = (string)e.Data.GetData(DataFormats.Text);
            Button draggedButton = null;

            foreach (Button btn in btnArray)
            {
                if (btn.Text == data)
                {
                    draggedButton = btn;
                    break;
                }
            }

            PictureBox pictureBox = sender as PictureBox;
            if (pictureBox != null && draggedButton != null)
            {
                draggedButton.Location = new Point(pictureBox.Location.X + (pictureBox.Width - draggedButton.Width) / 2, pictureBox.Location.Y + (pictureBox.Height - draggedButton.Height) / 2);

                
                if (pictureBox.BackColor == draggedButton.BackColor)
                {
                    correctMatches++; 
                }
            }
            else
            {
                MessageBox.Show("Butonlar PictureBox'ın üzerine sürüklenemez!");
            }
        }


        private void ValidationButton_Click(object sender, EventArgs e)
        {
            if (correctMatches >= 3)
            {
                MessageBox.Show("Doğrulama Başarılı! En az 3 renk doğru eşleşti.");
            }
            else
            {
                MessageBox.Show("Doğrulama Başarısız! En az 3 renk doğru eşleşmedi.");
            }
        }

        }
    }

