namespace CSC741M_MP1
{
    partial class Main
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
            ((System.ComponentModel.ISupportInitialize)(this.queryPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // algorithmComboBox
            // 
            this.algorithmComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.algorithmComboBox.FormattingEnabled = true;
            this.algorithmComboBox.Location = new System.Drawing.Point(130, 40);
            this.algorithmComboBox.Name = "algorithmComboBox";
            this.algorithmComboBox.Size = new System.Drawing.Size(362, 21);
            this.algorithmComboBox.TabIndex = 0;
            // 
            // filePathTextBox
            // 
            this.filePathTextBox.Location = new System.Drawing.Point(130, 14);
            this.filePathTextBox.Name = "filePathTextBox";
            this.filePathTextBox.Size = new System.Drawing.Size(288, 20);
            this.filePathTextBox.TabIndex = 1;
            this.filePathTextBox.TextChanged += new System.EventHandler(this.filePathTextBox_TextChanged);
            // 
            // algorithmLabel
            // 
            this.algorithmLabel.AutoSize = true;
            this.algorithmLabel.Location = new System.Drawing.Point(13, 43);
            this.algorithmLabel.Name = "algorithmLabel";
            this.algorithmLabel.Size = new System.Drawing.Size(50, 13);
            this.algorithmLabel.TabIndex = 2;
            this.algorithmLabel.Text = "Algorithm";
            // 
            // filePathLabel
            // 
            this.filePathLabel.AutoSize = true;
            this.filePathLabel.Location = new System.Drawing.Point(13, 17);
            this.filePathLabel.Name = "filePathLabel";
            this.filePathLabel.Size = new System.Drawing.Size(111, 13);
            this.filePathLabel.TabIndex = 3;
            this.filePathLabel.Text = "Query Image File Path";
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(424, 12);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(68, 23);
            this.BrowseButton.TabIndex = 4;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // runButton
            // 
            this.runButton.Enabled = false;
            this.runButton.Location = new System.Drawing.Point(424, 67);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(68, 23);
            this.runButton.TabIndex = 5;
            this.runButton.Text = "Run";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // resultImagesPanel
            // 
            this.resultImagesPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.resultImagesPanel.Location = new System.Drawing.Point(16, 181);
            this.resultImagesPanel.Name = "resultImagesPanel";
            this.resultImagesPanel.Size = new System.Drawing.Size(476, 247);
            this.resultImagesPanel.TabIndex = 6;
            // 
            // queryPictureBox
            // 
            this.queryPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.queryPictureBox.Location = new System.Drawing.Point(196, 87);
            this.queryPictureBox.Name = "queryPictureBox";
            this.queryPictureBox.Size = new System.Drawing.Size(128, 85);
            this.queryPictureBox.TabIndex = 7;
            this.queryPictureBox.TabStop = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 440);
            this.Controls.Add(this.queryPictureBox);
            this.Controls.Add(this.resultImagesPanel);
            this.Controls.Add(this.runButton);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.filePathLabel);
            this.Controls.Add(this.algorithmLabel);
            this.Controls.Add(this.filePathTextBox);
            this.Controls.Add(this.algorithmComboBox);
            this.Name = "Main";
            this.Text = "Color-Based Image Retrieval";
            ((System.ComponentModel.ISupportInitialize)(this.queryPictureBox)).EndInit();
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
    }
}

