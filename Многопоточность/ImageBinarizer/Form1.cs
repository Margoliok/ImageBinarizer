using System;
using System.Drawing;
using System.Windows.Forms;

namespace ImageBinarizer
{
    public partial class Form1 : Form
    {
        private Bitmap originalImage;

        public Form1()
        {
            InitializeComponent();
        }

        // Обработка нажатия кнопки "Загрузить изображение"
        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Файлы изображений|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                originalImage = new Bitmap(openFile.FileName);
                pictureBoxOriginal.Image = originalImage;
            }
        }

        // Обработка нажатия кнопки "Преобразовать"
        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (originalImage == null)
            {
                MessageBox.Show("Сначала загрузите изображение.");
                return;
            }

            if (!int.TryParse(txtThreshold.Text, out int threshold) || threshold < 0 || threshold > 255)
            {
                MessageBox.Show("Введите корректное пороговое значение от 0 до 255.");
                return;
            }

            string selectedChannel = comboBoxChannel.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedChannel))
            {
                MessageBox.Show("Выберите цветовой канал (R, G или B).");
                return;
            }

            Bitmap bwImage = ConvertToBlackAndWhite(originalImage, threshold, selectedChannel);
            pictureBoxResult.Image = bwImage;
        }

        // Функция преобразования изображения
        private Bitmap ConvertToBlackAndWhite(Bitmap image, int threshold, string channel)
        {
            Bitmap bwImage = new Bitmap(image.Width, image.Height);

            for (int y = 0; y < bwImage.Height; y++)
            {
                for (int x = 0; x < bwImage.Width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    int channelValue = 0;

                    switch (channel)
                    {
                        case "R":
                            channelValue = pixelColor.R;
                            break;
                        case "G":
                            channelValue = pixelColor.G;
                            break;
                        case "B":
                            channelValue = pixelColor.B;
                            break;
                    }

                    Color newColor = channelValue >= threshold ? Color.White : Color.Black;
                    bwImage.SetPixel(x, y, newColor);
                }
            }

            return bwImage;
        }
    }
}
