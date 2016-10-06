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
        private BackgroundWorker processWorker;
        private List<String> results;

        public Main()
        {
            InitializeComponent();

            ///Property Initializations
            algoHandler = AlgorithmHandler.getInstance();
            results = new List<String>();

            /// UI Initializations
            BindingSource algorithmComboBoxSource = new BindingSource();
            algorithmComboBoxSource.DataSource = algoHandler.getAllAlgorithmNames();
            algorithmComboBox.DataSource = algorithmComboBoxSource;
            algorithmComboBox.SelectedIndex = 0;
            resultImagesPanel.AutoScroll = true;

            /// Background Worker
            processWorker = new BackgroundWorker();
            processWorker.DoWork += new DoWorkEventHandler(process_DoWork);
            processWorker.ProgressChanged += new ProgressChangedEventHandler
                    (process_ProgressChanged);
            processWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler
                    (process_RunWorkerCompleted);
            processWorker.WorkerReportsProgress = true;
            
        }

        private void process_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] parameters = e.Argument as object[];
            string queryPath = parameters[0] as string;
            int algorithm = (int)parameters[1];
            if (File.Exists(queryPath))
            {
                Algorithm algo = algoHandler.getAlgorithm((AlgorithmEnum)algorithm);
                algo.ProgressUpdate += algorithmProgressListener;
                results = algoHandler.runAlgorithm(queryPath, (AlgorithmEnum)algorithm);
            }
        }

        private void algorithmProgressListener(double progress)
        {
            processWorker.ReportProgress((int)Math.Ceiling(progress * 100));
        }

        private void process_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            algorithmProgressBar.Value = e.ProgressPercentage;
        }

        private void process_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toggleFieldsAndButtons(true);
            showImagesOnPanel(results);
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
            if (File.Exists(queryPath) && !processWorker.IsBusy)
            {
                showQueryImageOnPictureBox(queryPath);
                object[] parameters = new object[] { queryPath, algorithmComboBox.SelectedIndex };
                toggleFieldsAndButtons(false);
                processWorker.RunWorkerAsync(parameters);
            }
            else
            {
                MessageBox.Show("Algorithm or query image path is invalid!", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toggleFieldsAndButtons(bool active)
        {
            filePathTextBox.Enabled = active;
            BrowseButton.Enabled = active;
            runButton.Enabled = active;
            algorithmComboBox.Enabled = active;
        }

        private void showQueryImageOnPictureBox(string path)
        {
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
