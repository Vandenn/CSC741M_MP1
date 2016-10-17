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
    public partial class SettingsView : Form
    {
        private Settings settings;

        public SettingsView()
        {
            InitializeComponent();
            settings = Settings.getSettings();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            DefaultSearchPathTextBox.Text = settings.DefaultSearchPath;   
            DatabasePathTextBox.Text = settings.DatabaseImagesPath;
            SimilarityThresholdTextBox.Text = settings.SimilarityThreshold.ToString();
            RelevanceThresholdTextBox.Text = settings.RelevanceThreshold.ToString();
            CenterAmountTextBox.Text = settings.CenterAmount.ToString();
            EightConnectedComboBox.SelectedIndex = settings.EightConnected ? 0 : 1;
            ConnectednessThresholdTextBox.Text = settings.ConnectednessThreshold.ToString();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            double similarityThreshold = -1;
            double relevanceThreshold = -1;
            double centerAmount = -1;
            double connectednessThreshold = -1;
            if (Directory.Exists(DatabasePathTextBox.Text) &&
                double.TryParse(SimilarityThresholdTextBox.Text, out similarityThreshold) && similarityThreshold >= 0 && similarityThreshold <= 1 &&
                double.TryParse(RelevanceThresholdTextBox.Text, out relevanceThreshold) && relevanceThreshold >= 0 && relevanceThreshold <= 1 &&
                double.TryParse(CenterAmountTextBox.Text, out centerAmount) && centerAmount >= 0 && centerAmount <= 1 &&
                double.TryParse(ConnectednessThresholdTextBox.Text, out connectednessThreshold) && connectednessThreshold >= 0 && connectednessThreshold <= 1)
            {
                settings.DefaultSearchPath = DefaultSearchPathTextBox.Text;
                settings.DatabaseImagesPath = DatabasePathTextBox.Text;
                settings.SimilarityThreshold = similarityThreshold;
                settings.RelevanceThreshold = relevanceThreshold;
                settings.CenterAmount = centerAmount;
                settings.EightConnected = EightConnectedComboBox.SelectedIndex == 0 ? true : false;
                settings.ConnectednessThreshold = connectednessThreshold;
                settings.saveSettings();
                Close();
            }
            else
            {
                MessageBox.Show("One or more invalid inputs!", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImageDatabasePathBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            browser.Description = "Set Image Database Path";
            browser.SelectedPath = Directory.Exists(DatabasePathTextBox.Text) ? DatabasePathTextBox.Text : @"C:\";
            browser.ShowNewFolderButton = false;

            if (browser.ShowDialog() == DialogResult.OK)
            {
                DatabasePathTextBox.Text = browser.SelectedPath.ToString();
            }
        }

        private void DefaultSearchPathBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            browser.Description = "Set Default Search Path";
            browser.SelectedPath = Directory.Exists(DatabasePathTextBox.Text) ? DatabasePathTextBox.Text : @"C:\";
            browser.ShowNewFolderButton = false;

            if (browser.ShowDialog() == DialogResult.OK)
            {
                DefaultSearchPathTextBox.Text = browser.SelectedPath.ToString();
            }
        }
    }
}
