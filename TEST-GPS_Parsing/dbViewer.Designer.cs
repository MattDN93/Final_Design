namespace TEST_GPS_Parsing
{
    partial class dbViewer
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.gpsDataRadioButton = new System.Windows.Forms.RadioButton();
            this.videoLogRadioButton = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimeForSearch = new System.Windows.Forms.DateTimePicker();
            this.eventSearchCombobox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.startQueryButton = new System.Windows.Forms.Button();
            this.clearFieldsButton = new System.Windows.Forms.Button();
            this.showSpecificRecordsRadio = new System.Windows.Forms.RadioButton();
            this.showAllRecordsRadio = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.usernameTextbox = new System.Windows.Forms.TextBox();
            this.passwordTextbox = new System.Windows.Forms.TextBox();
            this.loginToDbButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.PacketID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SessionPacketID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Latitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Longitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GrSpdKnots = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GrSpdKph = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Altitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurrentCamNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vidLogFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eventDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.smsNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.specDateSearchRadioButton = new System.Windows.Forms.RadioButton();
            this.specEventSearchRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.videoLogRadioButton);
            this.groupBox1.Controls.Add(this.gpsDataRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(131, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose Database";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panel2);
            this.groupBox3.Controls.Add(this.panel1);
            this.groupBox3.Location = new System.Drawing.Point(156, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(355, 100);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Display specific information";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dataGridView1);
            this.groupBox4.Location = new System.Drawing.Point(13, 148);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(693, 352);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Database Output Display";
            // 
            // gpsDataRadioButton
            // 
            this.gpsDataRadioButton.AutoSize = true;
            this.gpsDataRadioButton.Location = new System.Drawing.Point(7, 20);
            this.gpsDataRadioButton.Name = "gpsDataRadioButton";
            this.gpsDataRadioButton.Size = new System.Drawing.Size(73, 17);
            this.gpsDataRadioButton.TabIndex = 0;
            this.gpsDataRadioButton.TabStop = true;
            this.gpsDataRadioButton.Text = "GPS Data";
            this.gpsDataRadioButton.UseVisualStyleBackColor = true;
            this.gpsDataRadioButton.CheckedChanged += new System.EventHandler(this.gpsDataRadioButton_CheckedChanged);
            // 
            // videoLogRadioButton
            // 
            this.videoLogRadioButton.AccessibleRole = System.Windows.Forms.AccessibleRole.TitleBar;
            this.videoLogRadioButton.AutoSize = true;
            this.videoLogRadioButton.Location = new System.Drawing.Point(7, 57);
            this.videoLogRadioButton.Name = "videoLogRadioButton";
            this.videoLogRadioButton.Size = new System.Drawing.Size(73, 17);
            this.videoLogRadioButton.TabIndex = 1;
            this.videoLogRadioButton.TabStop = true;
            this.videoLogRadioButton.Text = "Video Log";
            this.videoLogRadioButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Shows GPS co-ordinates";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Shows events logged";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Search for specific date:";
            // 
            // dateTimeForSearch
            // 
            this.dateTimeForSearch.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimeForSearch.Location = new System.Drawing.Point(162, 23);
            this.dateTimeForSearch.Name = "dateTimeForSearch";
            this.dateTimeForSearch.Size = new System.Drawing.Size(99, 20);
            this.dateTimeForSearch.TabIndex = 1;
            // 
            // eventSearchCombobox
            // 
            this.eventSearchCombobox.FormattingEnabled = true;
            this.eventSearchCombobox.Items.AddRange(new object[] {
            "Camera Switch",
            "Connection Fail"});
            this.eventSearchCombobox.Location = new System.Drawing.Point(162, 51);
            this.eventSearchCombobox.Name = "eventSearchCombobox";
            this.eventSearchCombobox.Size = new System.Drawing.Size(100, 21);
            this.eventSearchCombobox.TabIndex = 2;
            this.eventSearchCombobox.SelectedIndexChanged += new System.EventHandler(this.eventSearchCombobox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(128, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Search for specific event:";
            // 
            // startQueryButton
            // 
            this.startQueryButton.Location = new System.Drawing.Point(463, 119);
            this.startQueryButton.Name = "startQueryButton";
            this.startQueryButton.Size = new System.Drawing.Size(118, 23);
            this.startQueryButton.TabIndex = 4;
            this.startQueryButton.Text = "Start Lookup";
            this.startQueryButton.UseVisualStyleBackColor = true;
            this.startQueryButton.Click += new System.EventHandler(this.startQueryButton_Click);
            // 
            // clearFieldsButton
            // 
            this.clearFieldsButton.Location = new System.Drawing.Point(587, 119);
            this.clearFieldsButton.Name = "clearFieldsButton";
            this.clearFieldsButton.Size = new System.Drawing.Size(118, 23);
            this.clearFieldsButton.TabIndex = 5;
            this.clearFieldsButton.Text = "Clear Fields";
            this.clearFieldsButton.UseVisualStyleBackColor = true;
            this.clearFieldsButton.Click += new System.EventHandler(this.clearFieldsButton_Click);
            // 
            // showSpecificRecordsRadio
            // 
            this.showSpecificRecordsRadio.AutoSize = true;
            this.showSpecificRecordsRadio.Location = new System.Drawing.Point(3, 5);
            this.showSpecificRecordsRadio.Name = "showSpecificRecordsRadio";
            this.showSpecificRecordsRadio.Size = new System.Drawing.Size(132, 17);
            this.showSpecificRecordsRadio.TabIndex = 6;
            this.showSpecificRecordsRadio.Text = "Find Specific Records:";
            this.showSpecificRecordsRadio.UseVisualStyleBackColor = true;
            this.showSpecificRecordsRadio.CheckedChanged += new System.EventHandler(this.showSpecificRecordsRadio_CheckedChanged);
            // 
            // showAllRecordsRadio
            // 
            this.showAllRecordsRadio.AutoSize = true;
            this.showAllRecordsRadio.Checked = true;
            this.showAllRecordsRadio.Location = new System.Drawing.Point(3, 5);
            this.showAllRecordsRadio.Name = "showAllRecordsRadio";
            this.showAllRecordsRadio.Size = new System.Drawing.Size(66, 17);
            this.showAllRecordsRadio.TabIndex = 7;
            this.showAllRecordsRadio.TabStop = true;
            this.showAllRecordsRadio.Text = "Show All";
            this.showAllRecordsRadio.UseVisualStyleBackColor = true;
            this.showAllRecordsRadio.CheckedChanged += new System.EventHandler(this.showAllRecordsRadio_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.loginToDbButton);
            this.groupBox5.Controls.Add(this.passwordTextbox);
            this.groupBox5.Controls.Add(this.usernameTextbox);
            this.groupBox5.Location = new System.Drawing.Point(517, 13);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(189, 100);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Database logon";
            // 
            // usernameTextbox
            // 
            this.usernameTextbox.Location = new System.Drawing.Point(76, 16);
            this.usernameTextbox.Name = "usernameTextbox";
            this.usernameTextbox.Size = new System.Drawing.Size(100, 20);
            this.usernameTextbox.TabIndex = 0;
            // 
            // passwordTextbox
            // 
            this.passwordTextbox.Location = new System.Drawing.Point(76, 44);
            this.passwordTextbox.Name = "passwordTextbox";
            this.passwordTextbox.Size = new System.Drawing.Size(100, 20);
            this.passwordTextbox.TabIndex = 1;
            this.passwordTextbox.UseSystemPasswordChar = true;
            // 
            // loginToDbButton
            // 
            this.loginToDbButton.Location = new System.Drawing.Point(64, 73);
            this.loginToDbButton.Name = "loginToDbButton";
            this.loginToDbButton.Size = new System.Drawing.Size(64, 20);
            this.loginToDbButton.TabIndex = 2;
            this.loginToDbButton.Text = "Login";
            this.loginToDbButton.UseVisualStyleBackColor = true;
            this.loginToDbButton.Click += new System.EventHandler(this.loginToDbButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Username:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Password:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PacketID,
            this.SessionPacketID,
            this.Date,
            this.Time,
            this.Latitude,
            this.Longitude,
            this.GrSpdKnots,
            this.GrSpdKph,
            this.Altitude,
            this.CurrentCamNum,
            this.vidLogFile,
            this.eventDesc,
            this.smsNum});
            this.dataGridView1.Location = new System.Drawing.Point(7, 20);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(679, 326);
            this.dataGridView1.TabIndex = 0;
            // 
            // PacketID
            // 
            this.PacketID.HeaderText = "PacketID";
            this.PacketID.Name = "PacketID";
            // 
            // SessionPacketID
            // 
            this.SessionPacketID.HeaderText = "SessionPacketID";
            this.SessionPacketID.Name = "SessionPacketID";
            // 
            // Date
            // 
            this.Date.HeaderText = "Date";
            this.Date.Name = "Date";
            // 
            // Time
            // 
            this.Time.HeaderText = "Time";
            this.Time.Name = "Time";
            // 
            // Latitude
            // 
            this.Latitude.HeaderText = "Latitude";
            this.Latitude.Name = "Latitude";
            // 
            // Longitude
            // 
            this.Longitude.HeaderText = "Longitude";
            this.Longitude.Name = "Longitude";
            // 
            // GrSpdKnots
            // 
            this.GrSpdKnots.HeaderText = "Ground Speed (kn)";
            this.GrSpdKnots.Name = "GrSpdKnots";
            // 
            // GrSpdKph
            // 
            this.GrSpdKph.HeaderText = "Speed (kph)";
            this.GrSpdKph.Name = "GrSpdKph";
            // 
            // Altitude
            // 
            this.Altitude.HeaderText = "Altitude";
            this.Altitude.Name = "Altitude";
            // 
            // CurrentCamNum
            // 
            this.CurrentCamNum.HeaderText = "Cam. Number";
            this.CurrentCamNum.Name = "CurrentCamNum";
            // 
            // vidLogFile
            // 
            this.vidLogFile.HeaderText = "Video File";
            this.vidLogFile.Name = "vidLogFile";
            // 
            // eventDesc
            // 
            this.eventDesc.HeaderText = "Description";
            this.eventDesc.Name = "eventDesc";
            // 
            // smsNum
            // 
            this.smsNum.HeaderText = "SMS Number";
            this.smsNum.Name = "smsNum";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.showAllRecordsRadio);
            this.panel1.Location = new System.Drawing.Point(9, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(72, 78);
            this.panel1.TabIndex = 8;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.specEventSearchRadioButton);
            this.panel2.Controls.Add(this.specDateSearchRadioButton);
            this.panel2.Controls.Add(this.showSpecificRecordsRadio);
            this.panel2.Controls.Add(this.eventSearchCombobox);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.dateTimeForSearch);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(85, 16);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(264, 78);
            this.panel2.TabIndex = 9;
            // 
            // specDateSearchRadioButton
            // 
            this.specDateSearchRadioButton.AutoSize = true;
            this.specDateSearchRadioButton.Checked = true;
            this.specDateSearchRadioButton.Location = new System.Drawing.Point(15, 27);
            this.specDateSearchRadioButton.Name = "specDateSearchRadioButton";
            this.specDateSearchRadioButton.Size = new System.Drawing.Size(14, 13);
            this.specDateSearchRadioButton.TabIndex = 7;
            this.specDateSearchRadioButton.TabStop = true;
            this.specDateSearchRadioButton.UseVisualStyleBackColor = true;
            // 
            // specEventSearchRadioButton
            // 
            this.specEventSearchRadioButton.AutoSize = true;
            this.specEventSearchRadioButton.Location = new System.Drawing.Point(15, 54);
            this.specEventSearchRadioButton.Name = "specEventSearchRadioButton";
            this.specEventSearchRadioButton.Size = new System.Drawing.Size(14, 13);
            this.specEventSearchRadioButton.TabIndex = 8;
            this.specEventSearchRadioButton.TabStop = true;
            this.specEventSearchRadioButton.UseVisualStyleBackColor = true;
            // 
            // dbViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 512);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.clearFieldsButton);
            this.Controls.Add(this.startQueryButton);
            this.Controls.Add(this.groupBox1);
            this.Name = "dbViewer";
            this.Text = "Database Reviewer";
            this.Load += new System.EventHandler(this.dbViewer_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton videoLogRadioButton;
        private System.Windows.Forms.RadioButton gpsDataRadioButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox eventSearchCombobox;
        private System.Windows.Forms.DateTimePicker dateTimeForSearch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button clearFieldsButton;
        private System.Windows.Forms.Button startQueryButton;
        private System.Windows.Forms.RadioButton showAllRecordsRadio;
        private System.Windows.Forms.RadioButton showSpecificRecordsRadio;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button loginToDbButton;
        private System.Windows.Forms.TextBox passwordTextbox;
        private System.Windows.Forms.TextBox usernameTextbox;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn PacketID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SessionPacketID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn Latitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn Longitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn GrSpdKnots;
        private System.Windows.Forms.DataGridViewTextBoxColumn GrSpdKph;
        private System.Windows.Forms.DataGridViewTextBoxColumn Altitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrentCamNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn vidLogFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn eventDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn smsNum;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton specEventSearchRadioButton;
        private System.Windows.Forms.RadioButton specDateSearchRadioButton;
        private System.Windows.Forms.Panel panel1;
    }
}