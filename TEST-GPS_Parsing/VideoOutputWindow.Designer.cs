﻿namespace TEST_GPS_Parsing
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
            this.longTopRightLabel = new System.Windows.Forms.Label();
            this.latBotLeftLabel = new System.Windows.Forms.Label();
            this.longTopLeftLabel = new System.Windows.Forms.Label();
            this.latTopLeftLabel = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.LongitudeLabel = new System.Windows.Forms.Label();
            this.latitudeLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.longOORTextBox = new System.Windows.Forms.TextBox();
            this.latOORStatusBox = new System.Windows.Forms.TextBox();
            this.status1TextBox = new System.Windows.Forms.TextBox();
            this.refreshOverlay = new System.Windows.Forms.Timer(this.components);
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.rawVideoFramesBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.overlayVideoFramesBox)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(555, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Raw video footage";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(321, 274);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Footage with Overlay";
            // 
            // rawVideoFramesBox
            // 
            this.rawVideoFramesBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rawVideoFramesBox.Location = new System.Drawing.Point(482, 22);
            this.rawVideoFramesBox.Name = "rawVideoFramesBox";
            this.rawVideoFramesBox.Size = new System.Drawing.Size(228, 234);
            this.rawVideoFramesBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.rawVideoFramesBox.TabIndex = 2;
            this.rawVideoFramesBox.TabStop = false;
            // 
            // startCaptureButton
            // 
            this.startCaptureButton.Location = new System.Drawing.Point(9, 18);
            this.startCaptureButton.Name = "startCaptureButton";
            this.startCaptureButton.Size = new System.Drawing.Size(185, 23);
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
            this.groupBox1.Location = new System.Drawing.Point(13, 61);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(203, 123);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parameters";
            // 
            // frameHeightLabel
            // 
            this.frameHeightLabel.AutoSize = true;
            this.frameHeightLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frameHeightLabel.Location = new System.Drawing.Point(121, 90);
            this.frameHeightLabel.Name = "frameHeightLabel";
            this.frameHeightLabel.Size = new System.Drawing.Size(21, 13);
            this.frameHeightLabel.TabIndex = 7;
            this.frameHeightLabel.Text = "__";
            // 
            // frameWidthLabel
            // 
            this.frameWidthLabel.AutoSize = true;
            this.frameWidthLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frameWidthLabel.Location = new System.Drawing.Point(121, 68);
            this.frameWidthLabel.Name = "frameWidthLabel";
            this.frameWidthLabel.Size = new System.Drawing.Size(21, 13);
            this.frameWidthLabel.TabIndex = 6;
            this.frameWidthLabel.Text = "__";
            // 
            // drawModeLabel
            // 
            this.drawModeLabel.AutoSize = true;
            this.drawModeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.drawModeLabel.Location = new System.Drawing.Point(121, 44);
            this.drawModeLabel.Name = "drawModeLabel";
            this.drawModeLabel.Size = new System.Drawing.Size(21, 13);
            this.drawModeLabel.TabIndex = 5;
            this.drawModeLabel.Text = "__";
            // 
            // videoModeLabel
            // 
            this.videoModeLabel.AutoSize = true;
            this.videoModeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.videoModeLabel.Location = new System.Drawing.Point(121, 19);
            this.videoModeLabel.Name = "videoModeLabel";
            this.videoModeLabel.Size = new System.Drawing.Size(21, 13);
            this.videoModeLabel.TabIndex = 4;
            this.videoModeLabel.Text = "__";
            // 
            // frame
            // 
            this.frame.AutoSize = true;
            this.frame.Location = new System.Drawing.Point(6, 90);
            this.frame.Name = "frame";
            this.frame.Size = new System.Drawing.Size(73, 13);
            this.frame.TabIndex = 3;
            this.frame.Text = "Frame Height:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Frame Width:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Point Mapping Mode:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Video Mode: ";
            // 
            // overlayVideoFramesBox
            // 
            this.overlayVideoFramesBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.overlayVideoFramesBox.Location = new System.Drawing.Point(18, 290);
            this.overlayVideoFramesBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.overlayVideoFramesBox.Name = "overlayVideoFramesBox";
            this.overlayVideoFramesBox.Size = new System.Drawing.Size(692, 427);
            this.overlayVideoFramesBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.overlayVideoFramesBox.TabIndex = 2;
            this.overlayVideoFramesBox.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.startCaptureButton);
            this.groupBox2.Location = new System.Drawing.Point(9, 10);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(207, 50);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Capture Control";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.longTopRightLabel);
            this.groupBox3.Controls.Add(this.latBotLeftLabel);
            this.groupBox3.Controls.Add(this.longTopLeftLabel);
            this.groupBox3.Controls.Add(this.latTopLeftLabel);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.LongitudeLabel);
            this.groupBox3.Controls.Add(this.latitudeLabel);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(221, 10);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Size = new System.Drawing.Size(255, 174);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Incoming Data";
            // 
            // longTopRightLabel
            // 
            this.longTopRightLabel.AutoSize = true;
            this.longTopRightLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.longTopRightLabel.Location = new System.Drawing.Point(142, 125);
            this.longTopRightLabel.Name = "longTopRightLabel";
            this.longTopRightLabel.Size = new System.Drawing.Size(21, 13);
            this.longTopRightLabel.TabIndex = 17;
            this.longTopRightLabel.Text = "__";
            // 
            // latBotLeftLabel
            // 
            this.latBotLeftLabel.AutoSize = true;
            this.latBotLeftLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.latBotLeftLabel.Location = new System.Drawing.Point(142, 143);
            this.latBotLeftLabel.Name = "latBotLeftLabel";
            this.latBotLeftLabel.Size = new System.Drawing.Size(21, 13);
            this.latBotLeftLabel.TabIndex = 16;
            this.latBotLeftLabel.Text = "__";
            // 
            // longTopLeftLabel
            // 
            this.longTopLeftLabel.AutoSize = true;
            this.longTopLeftLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.longTopLeftLabel.Location = new System.Drawing.Point(142, 75);
            this.longTopLeftLabel.Name = "longTopLeftLabel";
            this.longTopLeftLabel.Size = new System.Drawing.Size(21, 13);
            this.longTopLeftLabel.TabIndex = 15;
            this.longTopLeftLabel.Text = "__";
            // 
            // latTopLeftLabel
            // 
            this.latTopLeftLabel.AutoSize = true;
            this.latTopLeftLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.latTopLeftLabel.Location = new System.Drawing.Point(142, 94);
            this.latTopLeftLabel.Name = "latTopLeftLabel";
            this.latTopLeftLabel.Size = new System.Drawing.Size(21, 13);
            this.latTopLeftLabel.TabIndex = 8;
            this.latTopLeftLabel.Text = "__";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(4, 125);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(130, 13);
            this.label11.TabIndex = 14;
            this.label11.Text = "Longitude Upper Right(C):";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(4, 143);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(119, 13);
            this.label12.TabIndex = 13;
            this.label12.Text = "Latitude Bottom Left(D):";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 76);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(126, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "Longitude Upper Left (A):";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 95);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(114, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Latitude Upper Left(B):";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(43, 51);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(169, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "GPS bounds for this camera:";
            // 
            // LongitudeLabel
            // 
            this.LongitudeLabel.AutoSize = true;
            this.LongitudeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LongitudeLabel.Location = new System.Drawing.Point(186, 27);
            this.LongitudeLabel.Name = "LongitudeLabel";
            this.LongitudeLabel.Size = new System.Drawing.Size(21, 13);
            this.LongitudeLabel.TabIndex = 9;
            this.LongitudeLabel.Text = "__";
            // 
            // latitudeLabel
            // 
            this.latitudeLabel.AutoSize = true;
            this.latitudeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.latitudeLabel.Location = new System.Drawing.Point(65, 26);
            this.latitudeLabel.Name = "latitudeLabel";
            this.latitudeLabel.Size = new System.Drawing.Size(21, 13);
            this.latitudeLabel.TabIndex = 8;
            this.latitudeLabel.Text = "__";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(118, 27);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Longitude:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 27);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Latitude:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.longOORTextBox);
            this.groupBox4.Controls.Add(this.latOORStatusBox);
            this.groupBox4.Controls.Add(this.status1TextBox);
            this.groupBox4.Location = new System.Drawing.Point(13, 189);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox4.Size = new System.Drawing.Size(464, 66);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Current Status";
            // 
            // longOORTextBox
            // 
            this.longOORTextBox.Location = new System.Drawing.Point(237, 41);
            this.longOORTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.longOORTextBox.Name = "longOORTextBox";
            this.longOORTextBox.Size = new System.Drawing.Size(223, 20);
            this.longOORTextBox.TabIndex = 2;
            // 
            // latOORStatusBox
            // 
            this.latOORStatusBox.Location = new System.Drawing.Point(5, 41);
            this.latOORStatusBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.latOORStatusBox.Name = "latOORStatusBox";
            this.latOORStatusBox.Size = new System.Drawing.Size(228, 20);
            this.latOORStatusBox.TabIndex = 1;
            // 
            // status1TextBox
            // 
            this.status1TextBox.Location = new System.Drawing.Point(5, 18);
            this.status1TextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.status1TextBox.Name = "status1TextBox";
            this.status1TextBox.Size = new System.Drawing.Size(455, 20);
            this.status1TextBox.TabIndex = 0;
            // 
            // refreshOverlay
            // 
            this.refreshOverlay.Enabled = true;
            this.refreshOverlay.Interval = 500;
            this.refreshOverlay.Tick += new System.EventHandler(this.refreshOverlay_Tick);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(10, 258);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(30, 13);
            this.label13.TabIndex = 15;
            this.label13.Text = "(B,A)";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(683, 258);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(30, 13);
            this.label14.TabIndex = 16;
            this.label14.Text = "(B,C)";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(7, 723);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(31, 13);
            this.label15.TabIndex = 17;
            this.label15.Text = "(D,A)";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(683, 719);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(31, 13);
            this.label16.TabIndex = 18;
            this.label16.Text = "(D,C)";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(10, 275);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(28, 13);
            this.label17.TabIndex = 19;
            this.label17.Text = "(0,0)";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(682, 274);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(27, 13);
            this.label18.TabIndex = 20;
            this.label18.Text = "(x,0)";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(686, 737);
            this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(26, 13);
            this.label19.TabIndex = 21;
            this.label19.Text = "(x,y)";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(7, 737);
            this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(27, 13);
            this.label20.TabIndex = 22;
            this.label20.Text = "(0,y)";
            // 
            // VideoOutputWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 754);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.overlayVideoFramesBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.rawVideoFramesBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "VideoOutputWindow";
            this.Text = "Video Capture Window";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VideoOutputWindow_FormClosing);
            this.Load += new System.EventHandler(this.VideoOutputWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rawVideoFramesBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.overlayVideoFramesBox)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
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
        private System.Windows.Forms.TextBox latOORStatusBox;
        private System.Windows.Forms.TextBox status1TextBox;
        private System.Windows.Forms.Timer refreshOverlay;
        private System.Windows.Forms.Label LongitudeLabel;
        private System.Windows.Forms.Label latitudeLabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox longOORTextBox;
        private System.Windows.Forms.Label longTopRightLabel;
        private System.Windows.Forms.Label latBotLeftLabel;
        private System.Windows.Forms.Label longTopLeftLabel;
        private System.Windows.Forms.Label latTopLeftLabel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
    }
}