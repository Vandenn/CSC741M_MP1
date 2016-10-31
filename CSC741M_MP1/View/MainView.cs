using CSC741M_MP1.Algorithms.Helpers;
using CSC741M_MP1.Model;
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
    public partial class MainView : Form
    {
        private AlgorithmHandler algoHandler;
        private BackgroundWorker processWorker;
        private BackgroundWorker imagePanelWorker;
        private BackgroundWorker testProcessWorker;
        private List<String> results;
        private Settings settings;
        private GoldStandard goldStandard;
        private double testProgressValueLO;
        private double testProgressValueHI;

        public MainView()
        {
            InitializeComponent();

            ///Property Initializations
            algoHandler = AlgorithmHandler.getInstance();
            results = new List<String>();
            settings = Settings.getSettings();
            testProgressValueLO = 0.0;
            testProgressValueHI = 0.0;

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
            imagePanelWorker = new BackgroundWorker();
            imagePanelWorker.DoWork += new DoWorkEventHandler(imagePanelWorker_DoWork);
            imagePanelWorker.ProgressChanged += new ProgressChangedEventHandler
                    (imagePanelWorker_ProgressChanged);
            imagePanelWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler
                    (imagePanelWorker_RunWorkerCompleted);
            imagePanelWorker.WorkerReportsProgress = true;
            testProcessWorker = new BackgroundWorker();
            testProcessWorker.DoWork += new DoWorkEventHandler(testProcess_DoWork);
            testProcessWorker.ProgressChanged += new ProgressChangedEventHandler
                    (testProcess_ProgressChanged);
            testProcessWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler
                    (testProcess_RunWorkerCompleted);
            testProcessWorker.WorkerReportsProgress = true;

        }

        private void MainView_Load(object sender, EventArgs e)
        {
            CIEConvert.initialize();
            goldStandard = GoldStandard.getInstance();
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
            resultImagesPanel.Controls.Clear();
            imagePanelWorker.RunWorkerAsync(results);
            showEvaluationResults();
        }

        private void imagePanelWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<String> queryPath = (List<String>)e.Argument;
            int x = 10;
            int y = 10;
            int maxHeight = -1;
            foreach (string path in results)
            {
                PictureBox picture = new PictureBox();
                picture.Image = Image.FromFile(path);
                picture.Location = new Point(x, y);
                picture.SizeMode = PictureBoxSizeMode.StretchImage;
                x += picture.Width + 10;
                maxHeight = Math.Max(maxHeight, picture.Height);
                if (x > this.ClientSize.Width - picture.Width)
                {
                    x = 10;
                    y += maxHeight + 10;
                    maxHeight = -1;
                }
                imagePanelWorker.ReportProgress(0, picture);
            }
        }

        private void imagePanelWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            resultImagesPanel.Controls.Add((PictureBox)e.UserState);
        }

        private void imagePanelWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog browser = new OpenFileDialog();
            browser.Title = "Open Query Image";
            browser.Filter = "Query Image Files|*.jpg; *.jpeg";
            browser.InitialDirectory = settings.DefaultSearchPath;
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
            settingsToolStripMenuItem.Enabled = active;
            aboutToolStripMenuItem.Enabled = active;
            runTestSetButton.Enabled = active;
        }

        private void showQueryImageOnPictureBox(string path)
        {
            queryPictureBox.Image = Image.FromFile(path);
            queryPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void showEvaluationResults()
        {
            if (goldStandard.calculateEvaluation(filePathTextBox.Text, results))
            {
                evaluationResultLog.Text = String.Format("Results for {0}:{1}Precision: {2}{3}Recall: {4}{5}F-Measure: {6}", 
                    filePathTextBox.Text, Environment.NewLine,
                    goldStandard.getPrecision(), Environment.NewLine,
                    goldStandard.getRecall(), Environment.NewLine,
                    goldStandard.getFmeasure());
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

        private void aboutThisMPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutView aboutForm = new AboutView();
            aboutForm.Show();
        }

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsView settingsForm = new SettingsView();
            settingsForm.Show();
        }

        private void runTestSetButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Please make sure that the settings' database path is set to the WANG database. Are you sure you want to run the test set? (This may take a while.)", 
                "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                object[] parameters = new object[] { algorithmComboBox.SelectedIndex };
                toggleFieldsAndButtons(false);
                testProcessWorker.RunWorkerAsync(parameters);
            }
        }

        private void testProcess_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] parameters = e.Argument as object[];
            int algorithm = (int)parameters[0];

            GoldStandard goldStandard = GoldStandard.getInstance();
            AlgorithmEnum algoToBeUsed = (AlgorithmEnum)algorithm;
            Algorithm algoObject = algoHandler.getAlgorithm(algoToBeUsed);
            algoObject.ProgressUpdate += testProgressListener;
            List<string> testResults;
            double averagePrecision = 0;
            double averageRecall = 0;
            double averageFmeasure = 0;
            int count = 0;

            for (int i = 50; i <= 950; i += 100)
            {
                testResults = algoHandler.runAlgorithm(settings.DatabaseImagesPath + i + ".jpg", algoToBeUsed);
                if (goldStandard.calculateEvaluation(settings.DatabaseImagesPath + i + ".jpg", testResults))
                {
                    averagePrecision += goldStandard.getPrecision();
                    averageRecall += goldStandard.getRecall();
                    averageFmeasure += goldStandard.getFmeasure();
                    count++;
                }
                testProgressValueHI = ((double)i / 950) * 100;
            }

            averagePrecision /= count;
            averageRecall /= count;
            averageFmeasure /= count;

            e.Result = new object[] { algoToBeUsed, averagePrecision, averageRecall, averageFmeasure };
        }

        private void testProgressListener(double progress)
        {
            testProgressValueLO = progress * 10;
            testProcessWorker.ReportProgress((int)Math.Min(testProgressValueHI + testProgressValueLO, 100));
        }

        private void testProcess_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            algorithmProgressBar.Value = e.ProgressPercentage;
        }

        private void testProcess_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            object[] results = e.Result as object[];
            AlgorithmEnum algoToBeUsed = (AlgorithmEnum)results[0];
            double precision = (double)results[1];
            double recall = (double)results[2];
            double fmeasure = (double)results[3];

            toggleFieldsAndButtons(true);
            evaluationResultLog.Text = String.Format("Results for {0}:{1}Average Precision: {2}{3}Average Recall: {4}{5}Average F-Measure: {6}",
                        Algorithm.AlgorithmEnumToString(algoToBeUsed), Environment.NewLine,
                        precision, Environment.NewLine,
                        recall, Environment.NewLine,
                        fmeasure);
        }
    }
}
