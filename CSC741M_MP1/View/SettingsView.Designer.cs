namespace CSC741M_MP1
{
    partial class SettingsView
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.DatabasePathTextBox = new System.Windows.Forms.TextBox();
            this.ImageDatabasePathBrowseButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SimilarityThresholdTextBox = new System.Windows.Forms.TextBox();
            this.RelevanceThresholdTextBox = new System.Windows.Forms.TextBox();
            this.RelevanceThresholdLabel = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.CenterAmountTextBox = new System.Windows.Forms.TextBox();
            this.CenterAmountLabel = new System.Windows.Forms.Label();
            this.Tooltip1 = new System.Windows.Forms.ToolTip(this.components);
            this.DefaultSearchPathTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ConnectednessThresholdTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.DefaultSearchPathBrowseButton = new System.Windows.Forms.Button();
            this.EightConnectedComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Image Database Path";
            this.Tooltip1.SetToolTip(this.label1, "The folder path containing the database images.");
            // 
            // DatabasePathTextBox
            // 
            this.DatabasePathTextBox.Location = new System.Drawing.Point(149, 59);
            this.DatabasePathTextBox.Name = "DatabasePathTextBox";
            this.DatabasePathTextBox.Size = new System.Drawing.Size(178, 20);
            this.DatabasePathTextBox.TabIndex = 1;
            this.Tooltip1.SetToolTip(this.DatabasePathTextBox, "The folder path containing the database images.");
            // 
            // ImageDatabasePathBrowseButton
            // 
            this.ImageDatabasePathBrowseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ImageDatabasePathBrowseButton.Location = new System.Drawing.Point(333, 57);
            this.ImageDatabasePathBrowseButton.Name = "ImageDatabasePathBrowseButton";
            this.ImageDatabasePathBrowseButton.Size = new System.Drawing.Size(93, 23);
            this.ImageDatabasePathBrowseButton.TabIndex = 2;
            this.ImageDatabasePathBrowseButton.Text = "Browse";
            this.ImageDatabasePathBrowseButton.UseVisualStyleBackColor = true;
            this.ImageDatabasePathBrowseButton.Click += new System.EventHandler(this.ImageDatabasePathBrowseButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(11, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Settings";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Similarity Threshold";
            this.Tooltip1.SetToolTip(this.label3, "Threshold that determines whether or not an image will be included as a result.");
            // 
            // SimilarityThresholdTextBox
            // 
            this.SimilarityThresholdTextBox.Location = new System.Drawing.Point(149, 85);
            this.SimilarityThresholdTextBox.Name = "SimilarityThresholdTextBox";
            this.SimilarityThresholdTextBox.Size = new System.Drawing.Size(76, 20);
            this.SimilarityThresholdTextBox.TabIndex = 5;
            this.Tooltip1.SetToolTip(this.SimilarityThresholdTextBox, "Threshold that determines whether or not an image will be included as a result.");
            // 
            // RelevanceThresholdTextBox
            // 
            this.RelevanceThresholdTextBox.Location = new System.Drawing.Point(149, 111);
            this.RelevanceThresholdTextBox.Name = "RelevanceThresholdTextBox";
            this.RelevanceThresholdTextBox.Size = new System.Drawing.Size(76, 20);
            this.RelevanceThresholdTextBox.TabIndex = 7;
            this.Tooltip1.SetToolTip(this.RelevanceThresholdTextBox, "Threshold that determines which discretized colors to include based on frequency." +
        "");
            // 
            // RelevanceThresholdLabel
            // 
            this.RelevanceThresholdLabel.AutoSize = true;
            this.RelevanceThresholdLabel.Location = new System.Drawing.Point(12, 114);
            this.RelevanceThresholdLabel.Name = "RelevanceThresholdLabel";
            this.RelevanceThresholdLabel.Size = new System.Drawing.Size(109, 13);
            this.RelevanceThresholdLabel.TabIndex = 6;
            this.RelevanceThresholdLabel.Text = "Relevance Threshold";
            this.Tooltip1.SetToolTip(this.RelevanceThresholdLabel, "Threshold that determines which discretized colors to include based on frequency." +
        "");
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveButton.Location = new System.Drawing.Point(173, 231);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(93, 23);
            this.SaveButton.TabIndex = 8;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // CenterAmountTextBox
            // 
            this.CenterAmountTextBox.Location = new System.Drawing.Point(149, 137);
            this.CenterAmountTextBox.Name = "CenterAmountTextBox";
            this.CenterAmountTextBox.Size = new System.Drawing.Size(76, 20);
            this.CenterAmountTextBox.TabIndex = 10;
            this.Tooltip1.SetToolTip(this.CenterAmountTextBox, "The percentage of centering represented as a floating point number when using the" +
        " Color Histogram with Centering Refinement algorithm.");
            // 
            // CenterAmountLabel
            // 
            this.CenterAmountLabel.AutoSize = true;
            this.CenterAmountLabel.Location = new System.Drawing.Point(12, 140);
            this.CenterAmountLabel.Name = "CenterAmountLabel";
            this.CenterAmountLabel.Size = new System.Drawing.Size(77, 13);
            this.CenterAmountLabel.TabIndex = 9;
            this.CenterAmountLabel.Text = "Center Amount";
            this.Tooltip1.SetToolTip(this.CenterAmountLabel, "The percentage of centering represented as a floating point number when using the" +
        " Color Histogram with Centering Refinement algorithm.");
            // 
            // DefaultSearchPathTextBox
            // 
            this.DefaultSearchPathTextBox.Location = new System.Drawing.Point(149, 33);
            this.DefaultSearchPathTextBox.Name = "DefaultSearchPathTextBox";
            this.DefaultSearchPathTextBox.Size = new System.Drawing.Size(178, 20);
            this.DefaultSearchPathTextBox.TabIndex = 12;
            this.Tooltip1.SetToolTip(this.DefaultSearchPathTextBox, "The default starting folder when searching for a query image.");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Default Search Path";
            this.Tooltip1.SetToolTip(this.label4, "The default starting folder when searching for a query image.");
            // 
            // ConnectednessThresholdTextBox
            // 
            this.ConnectednessThresholdTextBox.Location = new System.Drawing.Point(149, 191);
            this.ConnectednessThresholdTextBox.Name = "ConnectednessThresholdTextBox";
            this.ConnectednessThresholdTextBox.Size = new System.Drawing.Size(76, 20);
            this.ConnectednessThresholdTextBox.TabIndex = 17;
            this.Tooltip1.SetToolTip(this.ConnectednessThresholdTextBox, "The percentage of centering represented as a floating point number when using the" +
        " Color Histogram with Centering Refinement algorithm.");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 194);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(131, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Connectedness Threshold";
            this.Tooltip1.SetToolTip(this.label6, "The percentage of centering represented as a floating point number when using the" +
        " Color Histogram with Centering Refinement algorithm.");
            // 
            // DefaultSearchPathBrowseButton
            // 
            this.DefaultSearchPathBrowseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DefaultSearchPathBrowseButton.Location = new System.Drawing.Point(333, 31);
            this.DefaultSearchPathBrowseButton.Name = "DefaultSearchPathBrowseButton";
            this.DefaultSearchPathBrowseButton.Size = new System.Drawing.Size(93, 23);
            this.DefaultSearchPathBrowseButton.TabIndex = 13;
            this.DefaultSearchPathBrowseButton.Text = "Browse";
            this.DefaultSearchPathBrowseButton.UseVisualStyleBackColor = true;
            this.DefaultSearchPathBrowseButton.Click += new System.EventHandler(this.DefaultSearchPathBrowseButton_Click);
            // 
            // EightConnectedComboBox
            // 
            this.EightConnectedComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.EightConnectedComboBox.FormattingEnabled = true;
            this.EightConnectedComboBox.Items.AddRange(new object[] {
            "true",
            "false"});
            this.EightConnectedComboBox.Location = new System.Drawing.Point(149, 164);
            this.EightConnectedComboBox.Name = "EightConnectedComboBox";
            this.EightConnectedComboBox.Size = new System.Drawing.Size(76, 21);
            this.EightConnectedComboBox.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 167);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Eight Connected";
            // 
            // SettingsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(438, 266);
            this.Controls.Add(this.ConnectednessThresholdTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.EightConnectedComboBox);
            this.Controls.Add(this.DefaultSearchPathBrowseButton);
            this.Controls.Add(this.DefaultSearchPathTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CenterAmountTextBox);
            this.Controls.Add(this.CenterAmountLabel);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.RelevanceThresholdTextBox);
            this.Controls.Add(this.RelevanceThresholdLabel);
            this.Controls.Add(this.SimilarityThresholdTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ImageDatabasePathBrowseButton);
            this.Controls.Add(this.DatabasePathTextBox);
            this.Controls.Add(this.label1);
            this.Name = "SettingsView";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox DatabasePathTextBox;
        private System.Windows.Forms.Button ImageDatabasePathBrowseButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox SimilarityThresholdTextBox;
        private System.Windows.Forms.TextBox RelevanceThresholdTextBox;
        private System.Windows.Forms.Label RelevanceThresholdLabel;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.ToolTip Tooltip1;
        private System.Windows.Forms.TextBox CenterAmountTextBox;
        private System.Windows.Forms.Label CenterAmountLabel;
        private System.Windows.Forms.Button DefaultSearchPathBrowseButton;
        private System.Windows.Forms.TextBox DefaultSearchPathTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox EightConnectedComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ConnectednessThresholdTextBox;
        private System.Windows.Forms.Label label6;
    }
}