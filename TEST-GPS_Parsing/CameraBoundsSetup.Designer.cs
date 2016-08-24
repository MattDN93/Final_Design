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
            this.longUpperLeft = new System.Windows.Forms.TextBox();
            this.longUpperRight = new System.Windows.Forms.TextBox();
            this.latUpperLeft = new System.Windows.Forms.TextBox();
            this.latBottomLeft = new System.Windows.Forms.TextBox();
            this.latUpperRight = new System.Windows.Forms.TextBox();
            this.longBottomLeft = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.setExtentsButton = new System.Windows.Forms.Button();
            this.clearFieldsButton = new System.Windows.Forms.Button();
            this.goBackButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.camViewStatusTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkFieldsTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox1.Image = global::TEST_GPS_Parsing.Properties.Resources.Cmaera_Selection;
            this.pictureBox1.Location = new System.Drawing.Point(24, 46);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1053, 562);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // longUpperLeft
            // 
            this.longUpperLeft.BackColor = System.Drawing.Color.Green;
            this.longUpperLeft.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.longUpperLeft.Location = new System.Drawing.Point(160, 124);
            this.longUpperLeft.Name = "longUpperLeft";
            this.longUpperLeft.Size = new System.Drawing.Size(127, 22);
            this.longUpperLeft.TabIndex = 1;
            // 
            // longUpperRight
            // 
            this.longUpperRight.BackColor = System.Drawing.Color.Green;
            this.longUpperRight.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.longUpperRight.Location = new System.Drawing.Point(898, 117);
            this.longUpperRight.Name = "longUpperRight";
            this.longUpperRight.Size = new System.Drawing.Size(127, 22);
            this.longUpperRight.TabIndex = 2;
            // 
            // latUpperLeft
            // 
            this.latUpperLeft.BackColor = System.Drawing.Color.Blue;
            this.latUpperLeft.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.latUpperLeft.Location = new System.Drawing.Point(160, 158);
            this.latUpperLeft.Name = "latUpperLeft";
            this.latUpperLeft.Size = new System.Drawing.Size(127, 22);
            this.latUpperLeft.TabIndex = 3;
            // 
            // latBottomLeft
            // 
            this.latBottomLeft.BackColor = System.Drawing.Color.Blue;
            this.latBottomLeft.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.latBottomLeft.Location = new System.Drawing.Point(160, 540);
            this.latBottomLeft.Name = "latBottomLeft";
            this.latBottomLeft.Size = new System.Drawing.Size(127, 22);
            this.latBottomLeft.TabIndex = 4;
            // 
            // latUpperRight
            // 
            this.latUpperRight.Location = new System.Drawing.Point(900, 150);
            this.latUpperRight.Name = "latUpperRight";
            this.latUpperRight.ReadOnly = true;
            this.latUpperRight.Size = new System.Drawing.Size(127, 22);
            this.latUpperRight.TabIndex = 5;
            // 
            // longBottomLeft
            // 
            this.longBottomLeft.Location = new System.Drawing.Point(160, 504);
            this.longBottomLeft.Name = "longBottomLeft";
            this.longBottomLeft.ReadOnly = true;
            this.longBottomLeft.Size = new System.Drawing.Size(127, 22);
            this.longBottomLeft.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 613);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(593, 119);
            this.label1.TabIndex = 7;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(282, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(453, 29);
            this.label2.TabIndex = 8;
            this.label2.Text = "Let\'s set up the camera\'s field of view!";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.goBackButton);
            this.groupBox1.Controls.Add(this.clearFieldsButton);
            this.groupBox1.Controls.Add(this.setExtentsButton);
            this.groupBox1.Location = new System.Drawing.Point(710, 630);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(367, 99);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ready?";
            // 
            // setExtentsButton
            // 
            this.setExtentsButton.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.setExtentsButton.Location = new System.Drawing.Point(13, 24);
            this.setExtentsButton.Name = "setExtentsButton";
            this.setExtentsButton.Size = new System.Drawing.Size(119, 60);
            this.setExtentsButton.TabIndex = 0;
            this.setExtentsButton.Text = "Set these extents and START VIDEO";
            this.setExtentsButton.UseVisualStyleBackColor = true;
            this.setExtentsButton.Click += new System.EventHandler(this.setExtentsButton_Click);
            // 
            // clearFieldsButton
            // 
            this.clearFieldsButton.Location = new System.Drawing.Point(186, 24);
            this.clearFieldsButton.Name = "clearFieldsButton";
            this.clearFieldsButton.Size = new System.Drawing.Size(79, 60);
            this.clearFieldsButton.TabIndex = 1;
            this.clearFieldsButton.Text = "Clear fields";
            this.clearFieldsButton.UseVisualStyleBackColor = true;
            this.clearFieldsButton.Click += new System.EventHandler(this.clearFieldsButton_Click);
            // 
            // goBackButton
            // 
            this.goBackButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.goBackButton.Location = new System.Drawing.Point(272, 24);
            this.goBackButton.Name = "goBackButton";
            this.goBackButton.Size = new System.Drawing.Size(79, 60);
            this.goBackButton.TabIndex = 2;
            this.goBackButton.Text = "Go back";
            this.goBackButton.UseVisualStyleBackColor = true;
            this.goBackButton.Click += new System.EventHandler(this.goBackButton_Click);
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
            // camViewStatusTextBox
            // 
            this.camViewStatusTextBox.Location = new System.Drawing.Point(580, 577);
            this.camViewStatusTextBox.Name = "camViewStatusTextBox";
            this.camViewStatusTextBox.ReadOnly = true;
            this.camViewStatusTextBox.Size = new System.Drawing.Size(480, 22);
            this.camViewStatusTextBox.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(522, 578);
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
            // CameraBoundsSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1106, 741);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.camViewStatusTextBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.longBottomLeft);
            this.Controls.Add(this.latUpperRight);
            this.Controls.Add(this.latBottomLeft);
            this.Controls.Add(this.latUpperLeft);
            this.Controls.Add(this.longUpperRight);
            this.Controls.Add(this.longUpperLeft);
            this.Controls.Add(this.pictureBox1);
            this.Name = "CameraBoundsSetup";
            this.Text = "Select Extent of Camera View";
            this.Load += new System.EventHandler(this.CameraBoundsSetup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox longUpperLeft;
        private System.Windows.Forms.TextBox longUpperRight;
        private System.Windows.Forms.TextBox latUpperLeft;
        private System.Windows.Forms.TextBox latBottomLeft;
        private System.Windows.Forms.TextBox latUpperRight;
        private System.Windows.Forms.TextBox longBottomLeft;
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
    }
}