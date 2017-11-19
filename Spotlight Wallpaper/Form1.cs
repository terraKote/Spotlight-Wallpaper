using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spotlight_Wallpaper
{
    public partial class Form1 : Form
    {
        private Point mouseLocation;
        private Image[] images;
        private string[] imageResolutions;
        private Image selectedImage;

        private string pathToImages = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Packages\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\LocalState\Assets";

        public Form1()
        {
            InitializeComponent();

            selectionPicker.Top = aboutButton.Top;
        }

        private void AppExit(object sender, EventArgs e)
        {
            images = null;
            Application.Exit();
        }

        private void SelectWindow(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            switch (button.Name)
            {
                case "wallpaperButton":
                    PickImages();
                    wallpaperPanel.BringToFront();
                    break;

                case "aboutButton":
                    aboutPanel.BringToFront();
                    break;
            }

            selectionPicker.Top = button.Top;
            selectedTabLabel.Text = button.Text;
        }

        private void PickImages()
        {
            images = GetImages(pathToImages, out imageResolutions);
            listBox1.Items.Clear();
            listBox1.Items.AddRange(imageResolutions);
        }

        private void DragWindow(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = MousePosition;
                mousePos.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePos;
            }
        }

        private void BeginDragWindow(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private Image[] GetImages(string path, out string[] imageNames)
        {
            List<Image> images = new List<Image>();
            List<string> imageSize = new List<string>();

            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);

                foreach (string fileName in files)
                {
                    try
                    {
                        Image image = Image.FromFile(fileName);

                        if (image != null)
                        {
                            if (image.Width >= 300)
                            {
                                images.Add(image);

                                // Add image resolution
                                imageSize.Add(string.Format("{0} x {1}", image.Width, image.Height));
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            imageNames = imageSize.ToArray();
            return images.ToArray();
        }

        private void SelectImage(object sender, EventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            selectedImage = images[listBox.SelectedIndex];
            pictureBox1.Image = selectedImage;
        }

        private void SaveImage(object sender, EventArgs e)
        {
            if (selectedImage == null)
            {
                return;
            }

            DialogResult result = saveFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                selectedImage.Save(saveFileDialog1.FileName);
            }
        }
    }
}