namespace CSC741M_MP1
{
    partial class MainView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.algorithmComboBox = new System.Windows.Forms.ComboBox();
            this.filePathTextBox = new System.Windows.Forms.TextBox();
            this.algorithmLabel = new System.Windows.Forms.Label();
            this.filePathLabel = new System.Windows.Forms.Label();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.runButton = new System.Windows.Forms.Button();
            this.resultImagesPanel = new System.Windows.Forms.Panel();
            this.queryPictureBox = new System.Windows.Forms.PictureBox();
            this.algorithmProgressBar = new System.Windows.Forms.ProgressBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutThisMPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.evaluationResultLog = new System.Windows.Forms.TextBox();
            this.evaluationResultLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.queryPictureBox)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // algorithmComboBox
            // 
            this.algorithmComboBox.BackColor = System.Drawing.Color.White;
            this.algorithmComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.algorithmComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.algorithmComboBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.algorithmComboBox.FormattingEnabled = true;
            this.algorithmComboBox.Location = new System.Drawing.Point(130, 58);
            this.algorithmComboBox.Name = "algorithmComboBox";
            this.algorithmComboBox.Size = new System.Drawing.Size(362, 21);
            this.algorithmComboBox.TabIndex = 0;
            // 
            // filePathTextBox
            // 
            this.filePathTextBox.Location = new System.Drawing.Point(130, 32);
            this.filePathTextBox.Name = "filePathTextBox";
            this.filePathTextBox.Size = new System.Drawing.Size(288, 20);
            this.filePathTextBox.TabIndex = 1;
            this.filePathTextBox.TextChanged += new System.EventHandler(this.filePathTextBox_TextChanged);
            // 
            // algorithmLabel
            // 
            this.algorithmLabel.AutoSize = true;
            this.algorithmLabel.Location = new System.Drawing.Point(13, 61);
            this.algorithmLabel.Name = "algorithmLabel";
            this.algorithmLabel.Size = new System.Drawing.Size(50, 13);
            this.algorithmLabel.TabIndex = 2;
            this.algorithmLabel.Text = "Algorithm";
            // 
            // filePathLabel
            // 
            this.filePathLabel.AutoSize = true;
            this.filePathLabel.Location = new System.Drawing.Point(13, 35);
            this.filePathLabel.Name = "filePathLabel";
            this.filePathLabel.Size = new System.Drawing.Size(111, 13);
            this.filePathLabel.TabIndex = 3;
            this.filePathLabel.Text = "Query Image File Path";
            // 
            // BrowseButton
            // 
            this.BrowseButton.BackColor = System.Drawing.Color.White;
            this.BrowseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BrowseButton.Location = new System.Drawing.Point(424, 30);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(68, 23);
            this.BrowseButton.TabIndex = 4;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = false;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // runButton
            // 
            this.runButton.BackColor = System.Drawing.Color.White;
            this.runButton.Enabled = false;
            this.runButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.runButton.Location = new System.Drawing.Point(424, 85);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(68, 23);
            this.runButton.TabIndex = 5;
            this.runButton.Text = "Run";
            this.runButton.UseVisualStyleBackColor = false;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // resultImagesPanel
            // 
            this.resultImagesPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.resultImagesPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.resultImagesPanel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.resultImagesPanel.Location = new System.Drawing.Point(16, 199);
            this.resultImagesPanel.Name = "resultImagesPanel";
            this.resultImagesPanel.Size = new System.Drawing.Size(476, 247);
            this.resultImagesPanel.TabIndex = 6;
            // 
            // queryPictureBox
            // 
            this.queryPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.queryPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.queryPictureBox.Location = new System.Drawing.Point(196, 105);
            this.queryPictureBox.Name = "queryPictureBox";
            this.queryPictureBox.Size = new System.Drawing.Size(128, 85);
            this.queryPictureBox.TabIndex = 7;
            this.queryPictureBox.TabStop = false;
            // 
            // algorithmProgressBar
            // 
            this.algorithmProgressBar.Location = new System.Drawing.Point(16, 460);
            this.algorithmProgressBar.Name = "algorithmProgressBar";
            this.algorithmProgressBar.Size = new System.Drawing.Size(476, 23);
            this.algorithmProgressBar.TabIndex = 8;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(504, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // configureToolStripMenuItem
            // 
            this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
            this.configureToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.configureToolStripMenuItem.Text = "Configure..";
            this.configureToolStripMenuItem.Click += new System.EventHandler(this.configureToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutThisMPToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // aboutThisMPToolStripMenuItem
            // 
            this.aboutThisMPToolStripMenuItem.Name = "aboutThisMPToolStripMenuItem";
            this.aboutThisMPToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.aboutThisMPToolStripMenuItem.Text = "About this MP..";
            this.aboutThisMPToolStripMenuItem.Click += new System.EventHandler(this.aboutThisMPToolStripMenuItem_Click);
            // 
            // evaluationResultLog
            // 
            this.evaluationResultLog.Location = new System.Drawing.Point(16, 511);
            this.evaluationResultLog.Multiline = true;
            this.evaluationResultLog.Name = "evaluationResultLog";
            this.evaluationResultLog.ReadOnly = true;
            this.evaluationResultLog.Size = new System.Drawing.Size(476, 86);
            this.evaluationResultLog.TabIndex = 10;
            // 
            // evaluationResultLabel
            // 
            this.evaluationResultLabel.AutoSize = true;
            this.evaluationResultLabel.Location = new System.Drawing.Point(13, 495);
            this.evaluationResultLabel.Name = "evaluationResultLabel";
            this.evaluationResultLabel.Size = new System.Drawing.Size(111, 13);
            this.evaluationResultLabel.TabIndex = 11;
            this.evaluationResultLabel.Text = "Evaluation Result Log";
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(504, 609);
            this.Controls.Add(this.evaluationResultLabel);
            this.Controls.Add(this.evaluationResultLog);
            this.Controls.Add(this.algorithmProgressBar);
            this.Controls.Add(this.queryPictureBox);
            this.Controls.Add(this.resultImagesPanel);
            this.Controls.Add(this.runButton);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.filePathLabel);
            this.Controls.Add(this.algorithmLabel);
            this.Controls.Add(this.filePathTextBox);
            this.Controls.Add(this.algorithmComboBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainView";
            this.Text = "Color-Based Image Retrieval";
            this.Load += new System.EventHandler(this.MainView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.queryPictureBox)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox algorithmComboBox;
        private System.Windows.Forms.TextBox filePathTextBox;
        private System.Windows.Forms.Label algorithmLabel;
        private System.Windows.Forms.Label filePathLabel;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.Panel resultImagesPanel;
        private System.Windows.Forms.PictureBox queryPictureBox;
        private System.Windows.Forms.ProgressBar algorithmProgressBar;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutThisMPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem;
        private System.Windows.Forms.TextBox evaluationResultLog;
        private System.Windows.Forms.Label evaluationResultLabel;
    }
}

