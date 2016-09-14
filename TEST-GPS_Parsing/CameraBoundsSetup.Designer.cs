namespace TEST_GPS_Parsing
{
    partial class CameraBoundsSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraBoundsSetup));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.longUpperLeftTextbox = new System.Windows.Forms.TextBox();
            this.longUpperRightTextbox = new System.Windows.Forms.TextBox();
            this.latUpperLeftTextbox = new System.Windows.Forms.TextBox();
            this.latBottomLeftTextbox = new System.Windows.Forms.TextBox();
            this.latUpperRightTextbox = new System.Windows.Forms.TextBox();
            this.longBottomLeftTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.goBackButton = new System.Windows.Forms.Button();
            this.clearFieldsButton = new System.Windows.Forms.Button();
            this.setExtentsButton = new System.Windows.Forms.Button();
            this.camViewStatusTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkFieldsTimer = new System.Windows.Forms.Timer(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.drawModeChoiceComboBox = new System.Windows.Forms.ComboBox();
            this.vidSourceChoiceComboBox = new System.Windows.Forms.ComboBox();
            this.chooseVideoFileFialog = new System.Windows.Forms.OpenFileDialog();
            this.label8 = new System.Windows.Forms.Label();
            this.leftCamStatusLabel = new System.Windows.Forms.Label();
            this.centreCamStatusLabel = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.rightCamStatusLabel = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox1.Image = global::TEST_GPS_Parsing.Properties.Resources.Cmaera_Selection;
            this.pictureBox1.Location = new System.Drawing.Point(24, 46);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1053, 562);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // longUpperLeftTextbox
            // 
            this.longUpperLeftTextbox.BackColor = System.Drawing.Color.Green;
            this.longUpperLeftTextbox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.longUpperLeftTextbox.Location = new System.Drawing.Point(160, 124);
            this.longUpperLeftTextbox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.longUpperLeftTextbox.Name = "longUpperLeftTextbox";
            this.longUpperLeftTextbox.Size = new System.Drawing.Size(127, 22);
            this.longUpperLeftTextbox.TabIndex = 1;
            // 
            // longUpperRightTextbox
            // 
            this.longUpperRightTextbox.BackColor = System.Drawing.Color.Green;
            this.longUpperRightTextbox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.longUpperRightTextbox.Location = new System.Drawing.Point(899, 117);
            this.longUpperRightTextbox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.longUpperRightTextbox.Name = "longUpperRightTextbox";
            this.longUpperRightTextbox.Size = new System.Drawing.Size(127, 22);
            this.longUpperRightTextbox.TabIndex = 2;
            // 
            // latUpperLeftTextbox
            // 
            this.latUpperLeftTextbox.BackColor = System.Drawing.Color.Blue;
            this.latUpperLeftTextbox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.latUpperLeftTextbox.Location = new System.Drawing.Point(160, 158);
            this.latUpperLeftTextbox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.latUpperLeftTextbox.Name = "latUpperLeftTextbox";
            this.latUpperLeftTextbox.Size = new System.Drawing.Size(127, 22);
            this.latUpperLeftTextbox.TabIndex = 3;
            // 
            // latBottomLeftTextbox
            // 
            this.latBottomLeftTextbox.BackColor = System.Drawing.Color.Blue;
            this.latBottomLeftTextbox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.latBottomLeftTextbox.Location = new System.Drawing.Point(160, 540);
            this.latBottomLeftTextbox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.latBottomLeftTextbox.Name = "latBottomLeftTextbox";
            this.latBottomLeftTextbox.Size = new System.Drawing.Size(127, 22);
            this.latBottomLeftTextbox.TabIndex = 4;
            // 
            // latUpperRightTextbox
            // 
            this.latUpperRightTextbox.Location = new System.Drawing.Point(900, 150);
            this.latUpperRightTextbox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.latUpperRightTextbox.Name = "latUpperRightTextbox";
            this.latUpperRightTextbox.ReadOnly = true;
            this.latUpperRightTextbox.Size = new System.Drawing.Size(127, 22);
            this.latUpperRightTextbox.TabIndex = 5;
            // 
            // longBottomLeftTextbox
            // 
            this.longBottomLeftTextbox.Location = new System.Drawing.Point(160, 505);
            this.longBottomLeftTextbox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.longBottomLeftTextbox.Name = "longBottomLeftTextbox";
            this.longBottomLeftTextbox.ReadOnly = true;
            this.longBottomLeftTextbox.Size = new System.Drawing.Size(127, 22);
            this.longBottomLeftTextbox.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 640);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(593, 136);
            this.label1.TabIndex = 7;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(283, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(446, 29);
            this.label2.TabIndex = 8;
            this.label2.Text = "Let\'s set up the camera\'s field of view";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.goBackButton);
            this.groupBox1.Controls.Add(this.clearFieldsButton);
            this.groupBox1.Controls.Add(this.setExtentsButton);
            this.groupBox1.Location = new System.Drawing.Point(709, 698);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(367, 78);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ready?";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(144, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "OR";
            // 
            // goBackButton
            // 
            this.goBackButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.goBackButton.Location = new System.Drawing.Point(272, 21);
            this.goBackButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.goBackButton.Name = "goBackButton";
            this.goBackButton.Size = new System.Drawing.Size(79, 48);
            this.goBackButton.TabIndex = 2;
            this.goBackButton.Text = "Go back";
            this.goBackButton.UseVisualStyleBackColor = true;
            this.goBackButton.Click += new System.EventHandler(this.goBackButton_Click);
            // 
            // clearFieldsButton
            // 
            this.clearFieldsButton.Location = new System.Drawing.Point(187, 21);
            this.clearFieldsButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clearFieldsButton.Name = "clearFieldsButton";
            this.clearFieldsButton.Size = new System.Drawing.Size(79, 48);
            this.clearFieldsButton.TabIndex = 1;
            this.clearFieldsButton.Text = "Clear fields";
            this.clearFieldsButton.UseVisualStyleBackColor = true;
            this.clearFieldsButton.Click += new System.EventHandler(this.clearFieldsButton_Click);
            // 
            // setExtentsButton
            // 
            this.setExtentsButton.Location = new System.Drawing.Point(19, 21);
            this.setExtentsButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.setExtentsButton.Name = "setExtentsButton";
            this.setExtentsButton.Size = new System.Drawing.Size(119, 48);
            this.setExtentsButton.TabIndex = 0;
            this.setExtentsButton.Text = "START VIDEO CAPTURE";
            this.setExtentsButton.UseVisualStyleBackColor = true;
            this.setExtentsButton.Click += new System.EventHandler(this.setExtentsButton_Click);
            // 
            // camViewStatusTextBox
            // 
            this.camViewStatusTextBox.Location = new System.Drawing.Point(580, 577);
            this.camViewStatusTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.camViewStatusTextBox.Name = "camViewStatusTextBox";
            this.camViewStatusTextBox.ReadOnly = true;
            this.camViewStatusTextBox.Size = new System.Drawing.Size(480, 22);
            this.camViewStatusTextBox.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(523, 578);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "Status:";
            // 
            // checkFieldsTimer
            // 
            this.checkFieldsTimer.Interval = 1000;
            this.checkFieldsTimer.Tick += new System.EventHandler(this.checkFieldsTimer_Tick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(172, 610);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(215, 25);
            this.label5.TabIndex = 12;
            this.label5.Text = "Help and Instructions";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.drawModeChoiceComboBox);
            this.groupBox2.Controls.Add(this.vidSourceChoiceComboBox);
            this.groupBox2.Location = new System.Drawing.Point(709, 615);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(367, 78);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Capture Settings";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(189, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 17);
            this.label7.TabIndex = 3;
            this.label7.Text = "Choose mode:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(145, 17);
            this.label6.TabIndex = 2;
            this.label6.Text = "Choose video source:";
            // 
            // drawModeChoiceComboBox
            // 
            this.drawModeChoiceComboBox.FormattingEnabled = true;
            this.drawModeChoiceComboBox.Items.AddRange(new object[] {
            "Random",
            "Ordered",
            "Tracking",
            "Object-Based Tracking"});
            this.drawModeChoiceComboBox.Location = new System.Drawing.Point(189, 44);
            this.drawModeChoiceComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.drawModeChoiceComboBox.Name = "drawModeChoiceComboBox";
            this.drawModeChoiceComboBox.Size = new System.Drawing.Size(160, 24);
            this.drawModeChoiceComboBox.TabIndex = 1;
            this.drawModeChoiceComboBox.Text = "Choose draw mode...";
            this.drawModeChoiceComboBox.SelectedIndexChanged += new System.EventHandler(this.drawModeChoiceComboBox_SelectedIndexChanged);
            // 
            // vidSourceChoiceComboBox
            // 
            this.vidSourceChoiceComboBox.FormattingEnabled = true;
            this.vidSourceChoiceComboBox.Items.AddRange(new object[] {
            "Video file on PC",
            "Device Webcam",
            "External Cameras"});
            this.vidSourceChoiceComboBox.Location = new System.Drawing.Point(27, 44);
            this.vidSourceChoiceComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.vidSourceChoiceComboBox.Name = "vidSourceChoiceComboBox";
            this.vidSourceChoiceComboBox.Size = new System.Drawing.Size(143, 24);
            this.vidSourceChoiceComboBox.TabIndex = 0;
            this.vidSourceChoiceComboBox.Text = "Choose a source...";
            // 
            // chooseVideoFileFialog
            // 
            this.chooseVideoFileFialog.Filter = "Video Files | *.avi, *.mp4, *.mpeg, *.mpg, *.wmv, *.mkv, *.mov | All files | *.*";
            this.chooseVideoFileFialog.FileOk += new System.ComponentModel.CancelEventHandler(this.chooseVideoFileFialog_FileOk);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(157, 338);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 17);
            this.label8.TabIndex = 14;
            this.label8.Text = "Status:";
            // 
            // leftCamStatusLabel
            // 
            this.leftCamStatusLabel.AutoSize = true;
            this.leftCamStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.leftCamStatusLabel.Location = new System.Drawing.Point(167, 365);
            this.leftCamStatusLabel.Name = "leftCamStatusLabel";
            this.leftCamStatusLabel.Size = new System.Drawing.Size(26, 17);
            this.leftCamStatusLabel.TabIndex = 15;
            this.leftCamStatusLabel.Text = "__";
            // 
            // centreCamStatusLabel
            // 
            this.centreCamStatusLabel.AutoSize = true;
            this.centreCamStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.centreCamStatusLabel.Location = new System.Drawing.Point(578, 245);
            this.centreCamStatusLabel.Name = "centreCamStatusLabel";
            this.centreCamStatusLabel.Size = new System.Drawing.Size(26, 17);
            this.centreCamStatusLabel.TabIndex = 16;
            this.centreCamStatusLabel.Text = "__";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Location = new System.Drawing.Point(570, 221);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(52, 17);
            this.label11.TabIndex = 17;
            this.label11.Text = "Status:";
            // 
            // rightCamStatusLabel
            // 
            this.rightCamStatusLabel.AutoSize = true;
            this.rightCamStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rightCamStatusLabel.Location = new System.Drawing.Point(933, 365);
            this.rightCamStatusLabel.Name = "rightCamStatusLabel";
            this.rightCamStatusLabel.Size = new System.Drawing.Size(26, 17);
            this.rightCamStatusLabel.TabIndex = 19;
            this.rightCamStatusLabel.Text = "__";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(923, 338);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(52, 17);
            this.label13.TabIndex = 18;
            this.label13.Text = "Status:";
            // 
            // CameraBoundsSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1107, 788);
            this.Controls.Add(this.rightCamStatusLabel);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.centreCamStatusLabel);
            this.Controls.Add(this.leftCamStatusLabel);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.camViewStatusTextBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.longBottomLeftTextbox);
            this.Controls.Add(this.latUpperRightTextbox);
            this.Controls.Add(this.latBottomLeftTextbox);
            this.Controls.Add(this.latUpperLeftTextbox);
            this.Controls.Add(this.longUpperRightTextbox);
            this.Controls.Add(this.longUpperLeftTextbox);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "CameraBoundsSetup";
            this.Text = "Select Extent of Camera View";
            this.Load += new System.EventHandler(this.CameraBoundsSetup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox longUpperLeftTextbox;
        private System.Windows.Forms.TextBox longUpperRightTextbox;
        private System.Windows.Forms.TextBox latUpperLeftTextbox;
        private System.Windows.Forms.TextBox latBottomLeftTextbox;
        private System.Windows.Forms.TextBox latUpperRightTextbox;
        private System.Windows.Forms.TextBox longBottomLeftTextbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button goBackButton;
        private System.Windows.Forms.Button clearFieldsButton;
        private System.Windows.Forms.Button setExtentsButton;
        private System.Windows.Forms.TextBox camViewStatusTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer checkFieldsTimer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox drawModeChoiceComboBox;
        private System.Windows.Forms.ComboBox vidSourceChoiceComboBox;
        private System.Windows.Forms.OpenFileDialog chooseVideoFileFialog;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label leftCamStatusLabel;
        private System.Windows.Forms.Label centreCamStatusLabel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label rightCamStatusLabel;
        private System.Windows.Forms.Label label13;
    }
}