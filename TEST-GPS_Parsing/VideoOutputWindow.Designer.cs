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
            ((System.ComponentModel.ISupportInitialize)(this.overlayVideoFramesBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rawVideoFramesBox)).BeginInit();
            this.SuspendLayout();
            // 
            // overlayVideoFramesBox
            // 
            this.overlayVideoFramesBox.Location = new System.Drawing.Point(12, 290);
            this.overlayVideoFramesBox.Name = "overlayVideoFramesBox";
            this.overlayVideoFramesBox.Size = new System.Drawing.Size(698, 401);
            this.overlayVideoFramesBox.TabIndex = 1;
            this.overlayVideoFramesBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(321, 3);
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
            this.rawVideoFramesBox.Location = new System.Drawing.Point(225, 22);
            this.rawVideoFramesBox.Name = "rawVideoFramesBox";
            this.rawVideoFramesBox.Size = new System.Drawing.Size(305, 234);
            this.rawVideoFramesBox.TabIndex = 2;
            this.rawVideoFramesBox.TabStop = false;
            // 
            // startCaptureButton
            // 
            this.startCaptureButton.Location = new System.Drawing.Point(12, 13);
            this.startCaptureButton.Name = "startCaptureButton";
            this.startCaptureButton.Size = new System.Drawing.Size(185, 23);
            this.startCaptureButton.TabIndex = 4;
            this.startCaptureButton.Text = "Start Capture";
            this.startCaptureButton.UseVisualStyleBackColor = true;
            this.startCaptureButton.Click += new System.EventHandler(this.startCaptureButton_Click);
            // 
            // VideoOutputWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 703);
            this.Controls.Add(this.startCaptureButton);
            this.Controls.Add(this.rawVideoFramesBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.overlayVideoFramesBox);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "VideoOutputWindow";
            this.Text = "VideoOutputWindow";
            this.Load += new System.EventHandler(this.VideoOutputWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.overlayVideoFramesBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rawVideoFramesBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox overlayVideoFramesBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Emgu.CV.UI.ImageBox rawVideoFramesBox;
        private System.Windows.Forms.Button startCaptureButton;
    }
}