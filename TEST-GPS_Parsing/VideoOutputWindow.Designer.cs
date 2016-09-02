namespace TEST_GPS_Parsing
{
    partial class VideoOutputWindow
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
            this.label2 = new System.Windows.Forms.Label();
            this.rawVideoFramesBox = new Emgu.CV.UI.ImageBox();
            this.startCaptureButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.frameHeightLabel = new System.Windows.Forms.Label();
            this.frameWidthLabel = new System.Windows.Forms.Label();
            this.drawModeLabel = new System.Windows.Forms.Label();
            this.videoModeLabel = new System.Windows.Forms.Label();
            this.frame = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.overlayVideoFramesBox = new Emgu.CV.UI.ImageBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.status1TextBox = new System.Windows.Forms.TextBox();
            this.status2TextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.rawVideoFramesBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.overlayVideoFramesBox)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(675, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Raw video footage";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(428, 337);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Footage with Overlay";
            // 
            // rawVideoFramesBox
            // 
            this.rawVideoFramesBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rawVideoFramesBox.Location = new System.Drawing.Point(540, 27);
            this.rawVideoFramesBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rawVideoFramesBox.Name = "rawVideoFramesBox";
            this.rawVideoFramesBox.Size = new System.Drawing.Size(405, 287);
            this.rawVideoFramesBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.rawVideoFramesBox.TabIndex = 2;
            this.rawVideoFramesBox.TabStop = false;
            // 
            // startCaptureButton
            // 
            this.startCaptureButton.Location = new System.Drawing.Point(12, 22);
            this.startCaptureButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.startCaptureButton.Name = "startCaptureButton";
            this.startCaptureButton.Size = new System.Drawing.Size(247, 28);
            this.startCaptureButton.TabIndex = 4;
            this.startCaptureButton.Text = "Start Capture";
            this.startCaptureButton.UseVisualStyleBackColor = true;
            this.startCaptureButton.Click += new System.EventHandler(this.startCaptureButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.frameHeightLabel);
            this.groupBox1.Controls.Add(this.frameWidthLabel);
            this.groupBox1.Controls.Add(this.drawModeLabel);
            this.groupBox1.Controls.Add(this.videoModeLabel);
            this.groupBox1.Controls.Add(this.frame);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(17, 75);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(271, 151);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parameters";
            // 
            // frameHeightLabel
            // 
            this.frameHeightLabel.AutoSize = true;
            this.frameHeightLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frameHeightLabel.Location = new System.Drawing.Point(161, 111);
            this.frameHeightLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.frameHeightLabel.Name = "frameHeightLabel";
            this.frameHeightLabel.Size = new System.Drawing.Size(26, 17);
            this.frameHeightLabel.TabIndex = 7;
            this.frameHeightLabel.Text = "__";
            // 
            // frameWidthLabel
            // 
            this.frameWidthLabel.AutoSize = true;
            this.frameWidthLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frameWidthLabel.Location = new System.Drawing.Point(161, 84);
            this.frameWidthLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.frameWidthLabel.Name = "frameWidthLabel";
            this.frameWidthLabel.Size = new System.Drawing.Size(26, 17);
            this.frameWidthLabel.TabIndex = 6;
            this.frameWidthLabel.Text = "__";
            // 
            // drawModeLabel
            // 
            this.drawModeLabel.AutoSize = true;
            this.drawModeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.drawModeLabel.Location = new System.Drawing.Point(161, 54);
            this.drawModeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.drawModeLabel.Name = "drawModeLabel";
            this.drawModeLabel.Size = new System.Drawing.Size(26, 17);
            this.drawModeLabel.TabIndex = 5;
            this.drawModeLabel.Text = "__";
            // 
            // videoModeLabel
            // 
            this.videoModeLabel.AutoSize = true;
            this.videoModeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.videoModeLabel.Location = new System.Drawing.Point(161, 23);
            this.videoModeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.videoModeLabel.Name = "videoModeLabel";
            this.videoModeLabel.Size = new System.Drawing.Size(26, 17);
            this.videoModeLabel.TabIndex = 4;
            this.videoModeLabel.Text = "__";
            // 
            // frame
            // 
            this.frame.AutoSize = true;
            this.frame.Location = new System.Drawing.Point(8, 111);
            this.frame.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.frame.Name = "frame";
            this.frame.Size = new System.Drawing.Size(97, 17);
            this.frame.TabIndex = 3;
            this.frame.Text = "Frame Height:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 84);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 17);
            this.label5.TabIndex = 2;
            this.label5.Text = "Frame Width:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 54);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "Point Mapping Mode:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 25);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Video Mode: ";
            // 
            // overlayVideoFramesBox
            // 
            this.overlayVideoFramesBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.overlayVideoFramesBox.Location = new System.Drawing.Point(17, 358);
            this.overlayVideoFramesBox.Name = "overlayVideoFramesBox";
            this.overlayVideoFramesBox.Size = new System.Drawing.Size(928, 483);
            this.overlayVideoFramesBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.overlayVideoFramesBox.TabIndex = 2;
            this.overlayVideoFramesBox.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.startCaptureButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(276, 62);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Capture Control";
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(295, 17);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(238, 209);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Incoming Data";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.status2TextBox);
            this.groupBox4.Controls.Add(this.status1TextBox);
            this.groupBox4.Location = new System.Drawing.Point(17, 233);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(516, 81);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Current Status";
            // 
            // status1TextBox
            // 
            this.status1TextBox.Location = new System.Drawing.Point(7, 22);
            this.status1TextBox.Name = "status1TextBox";
            this.status1TextBox.Size = new System.Drawing.Size(503, 22);
            this.status1TextBox.TabIndex = 0;
            // 
            // status2TextBox
            // 
            this.status2TextBox.Location = new System.Drawing.Point(7, 50);
            this.status2TextBox.Name = "status2TextBox";
            this.status2TextBox.Size = new System.Drawing.Size(503, 22);
            this.status2TextBox.TabIndex = 1;
            // 
            // VideoOutputWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 865);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.overlayVideoFramesBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.rawVideoFramesBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "VideoOutputWindow";
            this.Text = "VideoOutputWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VideoOutputWindow_FormClosing);
            this.Load += new System.EventHandler(this.VideoOutputWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rawVideoFramesBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.overlayVideoFramesBox)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Emgu.CV.UI.ImageBox rawVideoFramesBox;
        private System.Windows.Forms.Button startCaptureButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label frameHeightLabel;
        private System.Windows.Forms.Label frameWidthLabel;
        private System.Windows.Forms.Label drawModeLabel;
        private System.Windows.Forms.Label videoModeLabel;
        private System.Windows.Forms.Label frame;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private Emgu.CV.UI.ImageBox overlayVideoFramesBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox status2TextBox;
        private System.Windows.Forms.TextBox status1TextBox;
    }
}