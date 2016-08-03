namespace TEST_GPS_Parsing
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.altitudeTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.longitudeTextBox = new System.Windows.Forms.TextBox();
            this.latitudeTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.speedKnotsTextBox = new System.Windows.Forms.TextBox();
            this.headCardTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.headDegTextBox = new System.Windows.Forms.TextBox();
            this.speedKphTextBox = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.timeTextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.dateTextBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.fixqualTextBox = new System.Windows.Forms.TextBox();
            this.accuracyTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.fixvalTextBox = new System.Windows.Forms.TextBox();
            this.satsViewTextBox = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.timeElapsedTextBox = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.packetIDTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.openFileButton = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.openLogDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rawLogFileTextBox = new System.Windows.Forms.TextBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.recvRawDataWorker = new System.ComponentModel.BackgroundWorker();
            this.updateUITimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.status2TextBox = new System.Windows.Forms.TextBox();
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.openPortButton = new System.Windows.Forms.Button();
            this.trayIconParsing = new System.Windows.Forms.NotifyIcon(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.databaseOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseReadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openInappToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openXMLInSeparateViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseWriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enabledToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disabledToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.creationOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oncePerSessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newFileAtEachStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.altitudeTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.longitudeTextBox);
            this.groupBox1.Controls.Add(this.latitudeTextBox);
            this.groupBox1.Location = new System.Drawing.Point(10, 24);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(202, 115);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "GPS Location Data";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(148, 83);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(38, 13);
            this.label16.TabIndex = 11;
            this.label16.Text = "m ASL";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(4, 83);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(42, 13);
            this.label15.TabIndex = 5;
            this.label15.Text = "Altitude";
            // 
            // altitudeTextBox
            // 
            this.altitudeTextBox.Location = new System.Drawing.Point(77, 80);
            this.altitudeTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.altitudeTextBox.Name = "altitudeTextBox";
            this.altitudeTextBox.ReadOnly = true;
            this.altitudeTextBox.Size = new System.Drawing.Size(68, 20);
            this.altitudeTextBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 50);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Longitude";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Latitude";
            // 
            // longitudeTextBox
            // 
            this.longitudeTextBox.Location = new System.Drawing.Point(77, 47);
            this.longitudeTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.longitudeTextBox.Name = "longitudeTextBox";
            this.longitudeTextBox.ReadOnly = true;
            this.longitudeTextBox.Size = new System.Drawing.Size(109, 20);
            this.longitudeTextBox.TabIndex = 1;
            // 
            // latitudeTextBox
            // 
            this.latitudeTextBox.Location = new System.Drawing.Point(77, 20);
            this.latitudeTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.latitudeTextBox.Name = "latitudeTextBox";
            this.latitudeTextBox.ReadOnly = true;
            this.latitudeTextBox.Size = new System.Drawing.Size(109, 20);
            this.latitudeTextBox.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.speedKnotsTextBox);
            this.groupBox2.Controls.Add(this.headCardTextBox);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.headDegTextBox);
            this.groupBox2.Controls.Add(this.speedKphTextBox);
            this.groupBox2.Location = new System.Drawing.Point(232, 61);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(268, 78);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Vehicle Properties";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(224, 27);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "knots";
            // 
            // speedKnotsTextBox
            // 
            this.speedKnotsTextBox.Location = new System.Drawing.Point(170, 23);
            this.speedKnotsTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.speedKnotsTextBox.Name = "speedKnotsTextBox";
            this.speedKnotsTextBox.ReadOnly = true;
            this.speedKnotsTextBox.Size = new System.Drawing.Size(50, 20);
            this.speedKnotsTextBox.TabIndex = 7;
            // 
            // headCardTextBox
            // 
            this.headCardTextBox.Location = new System.Drawing.Point(170, 50);
            this.headCardTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.headCardTextBox.Name = "headCardTextBox";
            this.headCardTextBox.ReadOnly = true;
            this.headCardTextBox.Size = new System.Drawing.Size(50, 20);
            this.headCardTextBox.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(133, 53);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "deg / ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(133, 27);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "km/h";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 53);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Heading";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 25);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Speed";
            // 
            // headDegTextBox
            // 
            this.headDegTextBox.Location = new System.Drawing.Point(80, 50);
            this.headDegTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.headDegTextBox.Name = "headDegTextBox";
            this.headDegTextBox.ReadOnly = true;
            this.headDegTextBox.Size = new System.Drawing.Size(50, 20);
            this.headDegTextBox.TabIndex = 1;
            // 
            // speedKphTextBox
            // 
            this.speedKphTextBox.Location = new System.Drawing.Point(80, 23);
            this.speedKphTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.speedKphTextBox.Name = "speedKphTextBox";
            this.speedKphTextBox.ReadOnly = true;
            this.speedKphTextBox.Size = new System.Drawing.Size(50, 20);
            this.speedKphTextBox.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panel1);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.fixqualTextBox);
            this.groupBox3.Controls.Add(this.accuracyTextBox);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.fixvalTextBox);
            this.groupBox3.Controls.Add(this.satsViewTextBox);
            this.groupBox3.Location = new System.Drawing.Point(10, 148);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(490, 81);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "GPS Fix Information";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.timeTextBox);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.dateTextBox);
            this.panel1.Location = new System.Drawing.Point(340, 11);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(147, 66);
            this.panel1.TabIndex = 10;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(2, 37);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(33, 13);
            this.label14.TabIndex = 14;
            this.label14.Text = "Time:";
            // 
            // timeTextBox
            // 
            this.timeTextBox.Location = new System.Drawing.Point(38, 34);
            this.timeTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.timeTextBox.Name = "timeTextBox";
            this.timeTextBox.ReadOnly = true;
            this.timeTextBox.Size = new System.Drawing.Size(104, 20);
            this.timeTextBox.TabIndex = 13;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(2, 8);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(33, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "Date:";
            // 
            // dateTextBox
            // 
            this.dateTextBox.Location = new System.Drawing.Point(38, 6);
            this.dateTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.dateTextBox.Name = "dateTextBox";
            this.dateTextBox.ReadOnly = true;
            this.dateTextBox.Size = new System.Drawing.Size(104, 20);
            this.dateTextBox.TabIndex = 11;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(270, 22);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(15, 13);
            this.label12.TabIndex = 9;
            this.label12.Text = "m";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(152, 50);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "Quality of fix";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(150, 22);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 13);
            this.label11.TabIndex = 6;
            this.label11.Text = "Accuracy of fix";
            // 
            // fixqualTextBox
            // 
            this.fixqualTextBox.Location = new System.Drawing.Point(229, 47);
            this.fixqualTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.fixqualTextBox.Name = "fixqualTextBox";
            this.fixqualTextBox.ReadOnly = true;
            this.fixqualTextBox.Size = new System.Drawing.Size(92, 20);
            this.fixqualTextBox.TabIndex = 5;
            // 
            // accuracyTextBox
            // 
            this.accuracyTextBox.Location = new System.Drawing.Point(229, 20);
            this.accuracyTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.accuracyTextBox.Name = "accuracyTextBox";
            this.accuracyTextBox.ReadOnly = true;
            this.accuracyTextBox.Size = new System.Drawing.Size(38, 20);
            this.accuracyTextBox.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 50);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Validity of fix";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 22);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Satellites in view:";
            // 
            // fixvalTextBox
            // 
            this.fixvalTextBox.Location = new System.Drawing.Point(94, 47);
            this.fixvalTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.fixvalTextBox.Name = "fixvalTextBox";
            this.fixvalTextBox.ReadOnly = true;
            this.fixvalTextBox.Size = new System.Drawing.Size(40, 20);
            this.fixvalTextBox.TabIndex = 1;
            // 
            // satsViewTextBox
            // 
            this.satsViewTextBox.Location = new System.Drawing.Point(94, 20);
            this.satsViewTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.satsViewTextBox.Name = "satsViewTextBox";
            this.satsViewTextBox.ReadOnly = true;
            this.satsViewTextBox.Size = new System.Drawing.Size(40, 20);
            this.satsViewTextBox.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label22);
            this.groupBox4.Controls.Add(this.label21);
            this.groupBox4.Controls.Add(this.timeElapsedTextBox);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.packetIDTextBox);
            this.groupBox4.Location = new System.Drawing.Point(232, 20);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(268, 37);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Monitoring";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(236, 16);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(24, 13);
            this.label22.TabIndex = 13;
            this.label22.Text = "sec";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(125, 17);
            this.label21.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(45, 13);
            this.label21.TabIndex = 12;
            this.label21.Text = "Elapsed";
            // 
            // timeElapsedTextBox
            // 
            this.timeElapsedTextBox.Location = new System.Drawing.Point(171, 13);
            this.timeElapsedTextBox.Name = "timeElapsedTextBox";
            this.timeElapsedTextBox.ReadOnly = true;
            this.timeElapsedTextBox.Size = new System.Drawing.Size(60, 20);
            this.timeElapsedTextBox.TabIndex = 11;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(7, 17);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(55, 13);
            this.label17.TabIndex = 10;
            this.label17.Text = "Packet ID";
            // 
            // packetIDTextBox
            // 
            this.packetIDTextBox.Location = new System.Drawing.Point(66, 14);
            this.packetIDTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.packetIDTextBox.Name = "packetIDTextBox";
            this.packetIDTextBox.ReadOnly = true;
            this.packetIDTextBox.Size = new System.Drawing.Size(50, 20);
            this.packetIDTextBox.TabIndex = 9;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(389, 250);
            this.startButton.Margin = new System.Windows.Forms.Padding(2);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(43, 40);
            this.startButton.TabIndex = 6;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // openFileButton
            // 
            this.openFileButton.Location = new System.Drawing.Point(283, 233);
            this.openFileButton.Margin = new System.Windows.Forms.Padding(2);
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(85, 41);
            this.openFileButton.TabIndex = 7;
            this.openFileButton.Text = "Open NMEA Log";
            this.openFileButton.UseVisualStyleBackColor = true;
            this.openFileButton.Click += new System.EventHandler(this.openFileButton_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(259, 257);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(20, 24);
            this.label18.TabIndex = 11;
            this.label18.Text = "1";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(370, 257);
            this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(20, 24);
            this.label19.TabIndex = 12;
            this.label19.Text = "2";
            // 
            // openLogDialog
            // 
            this.openLogDialog.Filter = "Text files|*.txt|All files|*.*";
            this.openLogDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openLogDialog_FileOk);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.rawLogFileTextBox);
            this.groupBox5.Location = new System.Drawing.Point(11, 315);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox5.Size = new System.Drawing.Size(490, 141);
            this.groupBox5.TabIndex = 13;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Raw input logfile";
            // 
            // rawLogFileTextBox
            // 
            this.rawLogFileTextBox.Location = new System.Drawing.Point(7, 18);
            this.rawLogFileTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.rawLogFileTextBox.Multiline = true;
            this.rawLogFileTextBox.Name = "rawLogFileTextBox";
            this.rawLogFileTextBox.ReadOnly = true;
            this.rawLogFileTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.rawLogFileTextBox.Size = new System.Drawing.Size(477, 119);
            this.rawLogFileTextBox.TabIndex = 0;
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(459, 250);
            this.stopButton.Margin = new System.Windows.Forms.Padding(2);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(42, 40);
            this.stopButton.TabIndex = 14;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(436, 257);
            this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(20, 24);
            this.label20.TabIndex = 15;
            this.label20.Text = "3";
            // 
            // recvRawDataWorker
            // 
            this.recvRawDataWorker.WorkerReportsProgress = true;
            this.recvRawDataWorker.WorkerSupportsCancellation = true;
            this.recvRawDataWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.recvRawDataWorker_DoWork);
            this.recvRawDataWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.recvRawDataWorker_ProgressChanged);
            this.recvRawDataWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.recvRawDataWorker_RunWorkerCompleted);
            // 
            // updateUITimer
            // 
            this.updateUITimer.Enabled = true;
            this.updateUITimer.Interval = 1000;
            this.updateUITimer.Tick += new System.EventHandler(this.updateUITimer_Tick);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.status2TextBox);
            this.groupBox6.Controls.Add(this.statusTextBox);
            this.groupBox6.Location = new System.Drawing.Point(10, 237);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(243, 70);
            this.groupBox6.TabIndex = 16;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Status";
            // 
            // status2TextBox
            // 
            this.status2TextBox.Location = new System.Drawing.Point(7, 44);
            this.status2TextBox.Name = "status2TextBox";
            this.status2TextBox.ReadOnly = true;
            this.status2TextBox.Size = new System.Drawing.Size(230, 20);
            this.status2TextBox.TabIndex = 1;
            // 
            // statusTextBox
            // 
            this.statusTextBox.Location = new System.Drawing.Point(7, 20);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.ReadOnly = true;
            this.statusTextBox.Size = new System.Drawing.Size(230, 20);
            this.statusTextBox.TabIndex = 0;
            // 
            // openPortButton
            // 
            this.openPortButton.Location = new System.Drawing.Point(283, 278);
            this.openPortButton.Margin = new System.Windows.Forms.Padding(2);
            this.openPortButton.Name = "openPortButton";
            this.openPortButton.Size = new System.Drawing.Size(85, 29);
            this.openPortButton.TabIndex = 17;
            this.openPortButton.Text = "Open Port";
            this.openPortButton.UseVisualStyleBackColor = true;
            this.openPortButton.Click += new System.EventHandler(this.openPortButton_Click);
            // 
            // trayIconParsing
            // 
            this.trayIconParsing.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.trayIconParsing.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIconParsing.Icon")));
            this.trayIconParsing.Text = "notifyIcon1";
            this.trayIconParsing.Visible = true;
            this.trayIconParsing.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trayIconParsing_MouseDoubleClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.menuStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.databaseOptionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 462);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(514, 24);
            this.menuStrip1.TabIndex = 18;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // databaseOptionsToolStripMenuItem
            // 
            this.databaseOptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.databaseReadToolStripMenuItem,
            this.databaseWriteToolStripMenuItem,
            this.toolStripSeparator1,
            this.creationOptionsToolStripMenuItem});
            this.databaseOptionsToolStripMenuItem.Name = "databaseOptionsToolStripMenuItem";
            this.databaseOptionsToolStripMenuItem.Size = new System.Drawing.Size(110, 20);
            this.databaseOptionsToolStripMenuItem.Text = "Database options";
            // 
            // databaseReadToolStripMenuItem
            // 
            this.databaseReadToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openInappToolStripMenuItem,
            this.openXMLInSeparateViewerToolStripMenuItem});
            this.databaseReadToolStripMenuItem.Name = "databaseReadToolStripMenuItem";
            this.databaseReadToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.databaseReadToolStripMenuItem.Text = "Database Read";
            // 
            // openInappToolStripMenuItem
            // 
            this.openInappToolStripMenuItem.Name = "openInappToolStripMenuItem";
            this.openInappToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.openInappToolStripMenuItem.Text = "Open in-app";
            // 
            // openXMLInSeparateViewerToolStripMenuItem
            // 
            this.openXMLInSeparateViewerToolStripMenuItem.Name = "openXMLInSeparateViewerToolStripMenuItem";
            this.openXMLInSeparateViewerToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.openXMLInSeparateViewerToolStripMenuItem.Text = "Open XML in separate viewer";
            // 
            // databaseWriteToolStripMenuItem
            // 
            this.databaseWriteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enabledToolStripMenuItem,
            this.disabledToolStripMenuItem});
            this.databaseWriteToolStripMenuItem.Name = "databaseWriteToolStripMenuItem";
            this.databaseWriteToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.databaseWriteToolStripMenuItem.Text = "Database Write";
            // 
            // enabledToolStripMenuItem
            // 
            this.enabledToolStripMenuItem.Checked = true;
            this.enabledToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enabledToolStripMenuItem.Name = "enabledToolStripMenuItem";
            this.enabledToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.enabledToolStripMenuItem.Text = "Enabled";
            this.enabledToolStripMenuItem.Click += new System.EventHandler(this.enabledToolStripMenuItem_Click);
            // 
            // disabledToolStripMenuItem
            // 
            this.disabledToolStripMenuItem.Name = "disabledToolStripMenuItem";
            this.disabledToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.disabledToolStripMenuItem.Text = "Disabled";
            this.disabledToolStripMenuItem.Click += new System.EventHandler(this.disabledToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(159, 6);
            // 
            // creationOptionsToolStripMenuItem
            // 
            this.creationOptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oncePerSessionToolStripMenuItem,
            this.newFileAtEachStartToolStripMenuItem});
            this.creationOptionsToolStripMenuItem.Name = "creationOptionsToolStripMenuItem";
            this.creationOptionsToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.creationOptionsToolStripMenuItem.Text = "Creation options";
            this.creationOptionsToolStripMenuItem.ToolTipText = "Options for database creation. Disabled if logging is off.";
            // 
            // oncePerSessionToolStripMenuItem
            // 
            this.oncePerSessionToolStripMenuItem.Name = "oncePerSessionToolStripMenuItem";
            this.oncePerSessionToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.oncePerSessionToolStripMenuItem.Text = "Once per session";
            this.oncePerSessionToolStripMenuItem.ToolTipText = "Creates a new database log file only when the program is opened.";
            this.oncePerSessionToolStripMenuItem.Click += new System.EventHandler(this.oncePerSessionToolStripMenuItem_Click);
            // 
            // newFileAtEachStartToolStripMenuItem
            // 
            this.newFileAtEachStartToolStripMenuItem.Checked = true;
            this.newFileAtEachStartToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.newFileAtEachStartToolStripMenuItem.Name = "newFileAtEachStartToolStripMenuItem";
            this.newFileAtEachStartToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.newFileAtEachStartToolStripMenuItem.Text = "New file at each Start";
            this.newFileAtEachStartToolStripMenuItem.ToolTipText = "Creates a new database log file each time \"Start\" is selected per file";
            this.newFileAtEachStartToolStripMenuItem.Click += new System.EventHandler(this.newFileAtEachStartToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 486);
            this.Controls.Add(this.openPortButton);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.openFileButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "GPS Logging Application";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox longitudeTextBox;
        private System.Windows.Forms.TextBox latitudeTextBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox headDegTextBox;
        private System.Windows.Forms.TextBox speedKphTextBox;
        private System.Windows.Forms.TextBox headCardTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox speedKnotsTextBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox fixqualTextBox;
        private System.Windows.Forms.TextBox accuracyTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox fixvalTextBox;
        private System.Windows.Forms.TextBox satsViewTextBox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox altitudeTextBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox timeTextBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox dateTextBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox packetIDTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button openFileButton;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.OpenFileDialog openLogDialog;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox rawLogFileTextBox;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label label20;
        public System.ComponentModel.BackgroundWorker recvRawDataWorker;
        private System.Windows.Forms.Timer updateUITimer;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox statusTextBox;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox timeElapsedTextBox;
        private System.Windows.Forms.TextBox status2TextBox;
        private System.Windows.Forms.Button openPortButton;
        private System.Windows.Forms.NotifyIcon trayIconParsing;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem databaseOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databaseReadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openInappToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openXMLInSeparateViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databaseWriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enabledToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disabledToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem creationOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oncePerSessionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFileAtEachStartToolStripMenuItem;
    }
}

