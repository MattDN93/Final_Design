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
            this.overlayVideoFramesBox = new System.Windows.Forms.PictureBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.overlayVideoFramesBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rawVideoFramesBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // overlayVideoFramesBox
            // 
            this.overlayVideoFramesBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.overlayVideoFramesBox.Location = new System.Drawing.Point(12, 290);
            this.overlayVideoFramesBox.Name = "overlayVideoFramesBox";
            this.overlayVideoFramesBox.Size = new System.Drawing.Size(698, 401);
            this.overlayVideoFramesBox.TabIndex = 1;
            this.overlayVideoFramesBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(506, 6);
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
            this.rawVideoFramesBox.Location = new System.Drawing.Point(405, 22);
            this.rawVideoFramesBox.Name = "rawVideoFramesBox";
            this.rawVideoFramesBox.Size = new System.Drawing.Size(305, 234);
            this.rawVideoFramesBox.TabIndex = 2;
            this.rawVideoFramesBox.TabStop = false;
            // 
            // startCaptureButton
            // 
            this.startCaptureButton.Location = new System.Drawing.Point(12, 22);
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
            this.groupBox1.Size = new System.Drawing.Size(184, 195);
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
            // VideoOutputWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 703);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.startCaptureButton);
            this.Controls.Add(this.rawVideoFramesBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.overlayVideoFramesBox);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "VideoOutputWindow";
            this.Text = "VideoOutputWindow";
            this.Load += new System.EventHandler(this.VideoOutputWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.overlayVideoFramesBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rawVideoFramesBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox overlayVideoFramesBox;
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
    }
}