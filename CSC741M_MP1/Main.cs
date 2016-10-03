using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSC741M_MP1
{
    public partial class Main : Form
    {
        private AlgorithmHandler algoHandler;

        public Main()
        {
            InitializeComponent();

            ///Property Initializations
            algoHandler = AlgorithmHandler.getInstance();

            /// UI Initializations
            BindingSource algorithmComboBoxSource = new BindingSource();
            algorithmComboBoxSource.DataSource = algoHandler.getAllAlgorithmNames();
            algorithmComboBox.DataSource = algorithmComboBoxSource;
            algorithmComboBox.SelectedIndex = 0;
            resultImagesPanel.AutoScroll = true;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog browser = new OpenFileDialog();
            browser.Title = "Open Query Image";
            browser.Filter = "Query Image Files|*.jpg; *.jpeg";
            browser.InitialDirectory = @"C:\";
            browser.CheckFileExists = true;

            if (browser.ShowDialog() == DialogResult.OK)
            {
                filePathTextBox.Text = browser.FileName.ToString();
            }
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            string queryPath = Path.GetFullPath(filePathTextBox.Text);
            if (File.Exists(queryPath))
            {
                List<string> results = algoHandler.runAlgorithm(queryPath, (AlgorithmEnum)algorithmComboBox.SelectedIndex);
                showQueryImageOnPictureBox(queryPath);
                showImagesOnPanel(results);
            }
            else
            {
                MessageBox.Show("Algorithm or query image path is invalid!", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void showQueryImageOnPictureBox(string path)
        {
            int maxWidth = 128;
            int maxHeight = 85;
            queryPictureBox.Image = Image.FromFile(path);
            queryPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void showImagesOnPanel(List<string> results)
        {
            int x = 10;
            int y = 10;
            int maxHeight = -1;
            foreach (string path in results)
            {
                PictureBox picture = new PictureBox();
                picture.Image = Image.FromFile(path);
                picture.Location = new Point(x, y);
                picture.SizeMode = PictureBoxSizeMode.CenterImage;
                x += picture.Width + 10;
                maxHeight = Math.Max(maxHeight, picture.Height);
                if (x > this.ClientSize.Width - picture.Width)
                {
                    x = 10;
                    y += maxHeight + 10;
                    maxHeight = -1;
                }
                resultImagesPanel.Controls.Add(picture);
            }
        }

        private void filePathTextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(filePathTextBox.Text))
            {
                runButton.Enabled = false;
            }
            else
            {
                runButton.Enabled = true;
            }
        }
    }
}
