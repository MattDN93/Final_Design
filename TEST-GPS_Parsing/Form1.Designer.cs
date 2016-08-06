﻿namespace TEST_GPS_Parsing
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
            this.dbLoggingThread = new System.ComponentModel.BackgroundWorker();
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
            this.groupBox1.Location = new System.Drawing.Point(15, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(303, 177);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "GPS Location Data";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(222, 128);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(57, 20);
            this.label16.TabIndex = 11;
            this.label16.Text = "m ASL";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 128);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(63, 20);
            this.label15.TabIndex = 5;
            this.label15.Text = "Altitude";
            // 
            // altitudeTextBox
            // 
            this.altitudeTextBox.Location = new System.Drawing.Point(116, 123);
            this.altitudeTextBox.Name = "altitudeTextBox";
            this.altitudeTextBox.ReadOnly = true;
            this.altitudeTextBox.Size = new System.Drawing.Size(100, 26);
            this.altitudeTextBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Longitude";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Latitude";
            // 
            // longitudeTextBox
            // 
            this.longitudeTextBox.Location = new System.Drawing.Point(116, 72);
            this.longitudeTextBox.Name = "longitudeTextBox";
            this.longitudeTextBox.ReadOnly = true;
            this.longitudeTextBox.Size = new System.Drawing.Size(162, 26);
            this.longitudeTextBox.TabIndex = 1;
            // 
            // latitudeTextBox
            // 
            this.latitudeTextBox.Location = new System.Drawing.Point(116, 31);
            this.latitudeTextBox.Name = "latitudeTextBox";
            this.latitudeTextBox.ReadOnly = true;
            this.latitudeTextBox.Size = new System.Drawing.Size(162, 26);
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
            this.groupBox2.Location = new System.Drawing.Point(348, 94);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(402, 120);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Vehicle Properties";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(336, 42);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 20);
            this.label7.TabIndex = 8;
            this.label7.Text = "knots";
            // 
            // speedKnotsTextBox
            // 
            this.speedKnotsTextBox.Location = new System.Drawing.Point(255, 35);
            this.speedKnotsTextBox.Name = "speedKnotsTextBox";
            this.speedKnotsTextBox.ReadOnly = true;
            this.speedKnotsTextBox.Size = new System.Drawing.Size(73, 26);
            this.speedKnotsTextBox.TabIndex = 7;
            // 
            // headCardTextBox
            // 
            this.headCardTextBox.Location = new System.Drawing.Point(255, 77);
            this.headCardTextBox.Name = "headCardTextBox";
            this.headCardTextBox.ReadOnly = true;
            this.headCardTextBox.Size = new System.Drawing.Size(73, 26);
            this.headCardTextBox.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(200, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 20);
            this.label6.TabIndex = 5;
            this.label6.Text = "deg / ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(200, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "km/h";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "Heading";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "Speed";
            // 
            // headDegTextBox
            // 
            this.headDegTextBox.Location = new System.Drawing.Point(120, 77);
            this.headDegTextBox.Name = "headDegTextBox";
            this.headDegTextBox.ReadOnly = true;
            this.headDegTextBox.Size = new System.Drawing.Size(73, 26);
            this.headDegTextBox.TabIndex = 1;
            // 
            // speedKphTextBox
            // 
            this.speedKphTextBox.Location = new System.Drawing.Point(120, 35);
            this.speedKphTextBox.Name = "speedKphTextBox";
            this.speedKphTextBox.ReadOnly = true;
            this.speedKphTextBox.Size = new System.Drawing.Size(73, 26);
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
            this.groupBox3.Location = new System.Drawing.Point(15, 228);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(735, 125);
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
            this.panel1.Location = new System.Drawing.Point(510, 17);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(220, 100);
            this.panel1.TabIndex = 10;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 57);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(47, 20);
            this.label14.TabIndex = 14;
            this.label14.Text = "Time:";
            // 
            // timeTextBox
            // 
            this.timeTextBox.Location = new System.Drawing.Point(57, 52);
            this.timeTextBox.Name = "timeTextBox";
            this.timeTextBox.ReadOnly = true;
            this.timeTextBox.Size = new System.Drawing.Size(154, 26);
            this.timeTextBox.TabIndex = 13;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 12);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(48, 20);
            this.label13.TabIndex = 12;
            this.label13.Text = "Date:";
            // 
            // dateTextBox
            // 
            this.dateTextBox.Location = new System.Drawing.Point(57, 9);
            this.dateTextBox.Name = "dateTextBox";
            this.dateTextBox.ReadOnly = true;
            this.dateTextBox.Size = new System.Drawing.Size(154, 26);
            this.dateTextBox.TabIndex = 11;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(405, 34);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(22, 20);
            this.label12.TabIndex = 9;
            this.label12.Text = "m";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(228, 77);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 20);
            this.label10.TabIndex = 7;
            this.label10.Text = "Quality of fix";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(225, 34);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(111, 20);
            this.label11.TabIndex = 6;
            this.label11.Text = "Accuracy of fix";
            // 
            // fixqualTextBox
            // 
            this.fixqualTextBox.Location = new System.Drawing.Point(344, 72);
            this.fixqualTextBox.Name = "fixqualTextBox";
            this.fixqualTextBox.ReadOnly = true;
            this.fixqualTextBox.Size = new System.Drawing.Size(136, 26);
            this.fixqualTextBox.TabIndex = 5;
            // 
            // accuracyTextBox
            // 
            this.accuracyTextBox.Location = new System.Drawing.Point(344, 31);
            this.accuracyTextBox.Name = "accuracyTextBox";
            this.accuracyTextBox.ReadOnly = true;
            this.accuracyTextBox.Size = new System.Drawing.Size(55, 26);
            this.accuracyTextBox.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 77);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 20);
            this.label8.TabIndex = 3;
            this.label8.Text = "Validity of fix";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 34);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(128, 20);
            this.label9.TabIndex = 2;
            this.label9.Text = "Satellites in view:";
            // 
            // fixvalTextBox
            // 
            this.fixvalTextBox.Location = new System.Drawing.Point(141, 72);
            this.fixvalTextBox.Name = "fixvalTextBox";
            this.fixvalTextBox.ReadOnly = true;
            this.fixvalTextBox.Size = new System.Drawing.Size(58, 26);
            this.fixvalTextBox.TabIndex = 1;
            // 
            // satsViewTextBox
            // 
            this.satsViewTextBox.Location = new System.Drawing.Point(141, 31);
            this.satsViewTextBox.Name = "satsViewTextBox";
            this.satsViewTextBox.ReadOnly = true;
            this.satsViewTextBox.Size = new System.Drawing.Size(58, 26);
            this.satsViewTextBox.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label22);
            this.groupBox4.Controls.Add(this.label21);
            this.groupBox4.Controls.Add(this.timeElapsedTextBox);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.packetIDTextBox);
            this.groupBox4.Location = new System.Drawing.Point(348, 31);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(402, 57);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Monitoring";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(354, 25);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(34, 20);
            this.label22.TabIndex = 13;
            this.label22.Text = "sec";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(188, 26);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(67, 20);
            this.label21.TabIndex = 12;
            this.label21.Text = "Elapsed";
            // 
            // timeElapsedTextBox
            // 
            this.timeElapsedTextBox.Location = new System.Drawing.Point(256, 20);
            this.timeElapsedTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.timeElapsedTextBox.Name = "timeElapsedTextBox";
            this.timeElapsedTextBox.ReadOnly = true;
            this.timeElapsedTextBox.Size = new System.Drawing.Size(88, 26);
            this.timeElapsedTextBox.TabIndex = 11;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(10, 26);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(79, 20);
            this.label17.TabIndex = 10;
            this.label17.Text = "Packet ID";
            // 
            // packetIDTextBox
            // 
            this.packetIDTextBox.Location = new System.Drawing.Point(99, 22);
            this.packetIDTextBox.Name = "packetIDTextBox";
            this.packetIDTextBox.ReadOnly = true;
            this.packetIDTextBox.Size = new System.Drawing.Size(73, 26);
            this.packetIDTextBox.TabIndex = 9;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(584, 385);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(64, 62);
            this.startButton.TabIndex = 6;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // openFileButton
            // 
            this.openFileButton.Location = new System.Drawing.Point(424, 358);
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(128, 63);
            this.openFileButton.TabIndex = 7;
            this.openFileButton.Text = "Open NMEA Log";
            this.openFileButton.UseVisualStyleBackColor = true;
            this.openFileButton.Click += new System.EventHandler(this.openFileButton_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(388, 395);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(20, 24);
            this.label18.TabIndex = 11;
            this.label18.Text = "1";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(555, 395);
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
            this.groupBox5.Location = new System.Drawing.Point(16, 485);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(735, 217);
            this.groupBox5.TabIndex = 13;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Raw input logfile";
            // 
            // rawLogFileTextBox
            // 
            this.rawLogFileTextBox.Location = new System.Drawing.Point(10, 28);
            this.rawLogFileTextBox.Multiline = true;
            this.rawLogFileTextBox.Name = "rawLogFileTextBox";
            this.rawLogFileTextBox.ReadOnly = true;
            this.rawLogFileTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.rawLogFileTextBox.Size = new System.Drawing.Size(714, 181);
            this.rawLogFileTextBox.TabIndex = 0;
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(688, 385);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(63, 62);
            this.stopButton.TabIndex = 14;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(654, 395);
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
            this.groupBox6.Location = new System.Drawing.Point(15, 365);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox6.Size = new System.Drawing.Size(364, 108);
            this.groupBox6.TabIndex = 16;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Status";
            // 
            // status2TextBox
            // 
            this.status2TextBox.Location = new System.Drawing.Point(10, 68);
            this.status2TextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.status2TextBox.Name = "status2TextBox";
            this.status2TextBox.ReadOnly = true;
            this.status2TextBox.Size = new System.Drawing.Size(343, 26);
            this.status2TextBox.TabIndex = 1;
            // 
            // statusTextBox
            // 
            this.statusTextBox.Location = new System.Drawing.Point(10, 31);
            this.statusTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.ReadOnly = true;
            this.statusTextBox.Size = new System.Drawing.Size(343, 26);
            this.statusTextBox.TabIndex = 0;
            // 
            // openPortButton
            // 
            this.openPortButton.Location = new System.Drawing.Point(424, 428);
            this.openPortButton.Name = "openPortButton";
            this.openPortButton.Size = new System.Drawing.Size(128, 45);
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
            this.menuStrip1.Location = new System.Drawing.Point(0, 723);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 3, 0, 3);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(771, 25);
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
            this.databaseOptionsToolStripMenuItem.Size = new System.Drawing.Size(110, 19);
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
            // dbLoggingThread
            // 
            this.dbLoggingThread.WorkerSupportsCancellation = true;
            this.dbLoggingThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.dbLoggingThread_DoWork);
            this.dbLoggingThread.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.dbLoggingThread_ProgressChanged);
            this.dbLoggingThread.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.dbLoggingThread_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 748);
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
        private System.ComponentModel.BackgroundWorker dbLoggingThread;
    }
}

