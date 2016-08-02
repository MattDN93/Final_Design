using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;


namespace TEST_GPS_Parsing
{
    public partial class Form1 : Form
    {
        //*********THIS VAR IS FOR TESTING FEATURES, set to false for debug features off
        bool debug = false;
        //*************************************
        public string logFilename;
        GPSPacket gpsData = new GPSPacket();          //global GPS data packet for UI display
        bool haveStarted = false;   //bool to indicate if logging is happening or not (for timer)
        string sentenceBuffer;      //global buffer to read incoming data
        

        public Form1()
        {
            InitializeComponent();
            //set all the values in the UI to default
            //set up a new packet object with default constructor
            GPSPacket gpsData = new GPSPacket();

            //Monitoring
            packetIDTextBox.Text = gpsData.ID.ToString();
            //GPS Core Data
            latitudeTextBox.Text = gpsData.latitude.ToString();
            longitudeTextBox.Text = gpsData.longitude.ToString();
            altitudeTextBox.Text = gpsData.altitude.ToString();
            //Vehicle Properties
            speedKnotsTextBox.Text = gpsData.grspd_k.ToString();
            speedKphTextBox.Text = gpsData.grspd_kph.ToString();
            headDegTextBox.Text = gpsData.trkangle.ToString();
            //Fix Information
            satsViewTextBox.Text = gpsData.numsats.ToString();
            fixqualTextBox.Text = gpsData.fixqual_f;
            fixvalTextBox.Text = gpsData.fixtype_f;
            accuracyTextBox.Text = gpsData.accuracy.ToString();
            //Date and time
            dateTextBox.Text = gpsData.date;
            timeTextBox.Text = gpsData.time;

            //prep the load box
            initOpenLogDialog();
            trayIconParsing.Visible = true;

        }

        private void initOpenLogDialog()
        {
            //prep the load box
            //openLogDialog = new OpenFileDialog();     //create new instance of the openFileDialog object
            startButton.Enabled = false;
            stopButton.Enabled = false;
            statusTextBox.AppendText("Ready. Click the button to open a file.");
        }

        private void openFileButton_Click(object sender, EventArgs e)
        {
                //open the dialog
                openLogDialog.ShowDialog();   
        }

        private void openLogDialog_FileOk(object sender, CancelEventArgs e)
        {

            //MessageBox.Show("GPS NMEA log file ready. Click Start Tracking.");
            logFilename = openLogDialog.FileName;       //save the filename of the logfile
            gpsData.gpsLogfilename = logFilename;       //save filename to associated GPS class
            startButton.Enabled = true;
            statusTextBox.Clear();
            statusTextBox.AppendText("File opened OK. Click start to start parsing.");
        }

        //We update the UI 
        //END THREAD

        private void startButton_Click(object sender, EventArgs e)
        {

            //threading start!
            if (!recvRawDataWorker.CancellationPending)
            {
                openFileButton.Enabled = false;
                startButton.Enabled = false;
                stopButton.Enabled = true;
                trayIconParsing.Text = "GPS logging active...";
                trayIconParsing.ShowBalloonTip(5, "Logging running...", "Logger will continue running here if main window closed.",ToolTipIcon.Info);
                recvRawDataWorker.RunWorkerAsync();         //starts the data receiving in the background
            }
            else
            {
                recvRawDataWorker.CancelAsync();
                statusTextBox.Clear();
                trayIconParsing.Text = "Parsing error! See main window.";
                trayIconParsing.ShowBalloonTip(5, "GPS Logging error" ,"The last operation isn't finished yet. Please wait and try again.", ToolTipIcon.Error);
                statusTextBox.AppendText("Error: previous task not cancelled yet. Try again.");
            }


        }

        private void updateUITimer_Tick(object sender, EventArgs e)
        {
            if (recvRawDataWorker.IsBusy)
            {
                gpsData.timeElapsed++; //update the running timer only if parsing is active
            }
            

        }

        //-------------------------THREAD FOR BACKGROUND WORK--------------------
        private void recvRawDataWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //GPSPacket gpsData = new GPSPacket();
            System.IO.StreamReader inputFile = new System.IO.StreamReader(gpsData.gpsLogfilename);

            //start reading the line
            //For simulating NMEA behaviour
            int count = 0;

            
                while (inputFile.EndOfStream == false)
                {
                    if (!recvRawDataWorker.CancellationPending) //check for thread cancel request from stop button (since it's only active here)
                    {
                        sentenceBuffer = inputFile.ReadLine();
                        //This function updates the UI elements with the parsed NMEA data
                        gpsData = gpsData.parseSelection(sentenceBuffer, gpsData);
                        count++;
                        //this allows for a thread-safe variable access
                        recvRawDataWorker.ReportProgress(count, gpsData);

                        //FOR SIMULATION ONLY
                        Thread.Sleep(100);
                        //---------------
                    }
                    else
                    {
                    MessageBox.Show("File parsing cancelled.");
                    inputFile.Close();
                    //everything else is handled in the completion thread
                    break;
                    }
                }
            inputFile.Close();

        
        }

        private void updateUI(GPSPacket gpsDataForUI, string sentenceBufferForUI)
        {
            //This takes the gpsData object and populates all the fields with updated values (if any)
            //Show the buffer of the raw text file being read 
            rawLogFileTextBox.AppendText(sentenceBufferForUI);
            sentenceBufferForUI = "";
            //Monitoring
            packetIDTextBox.Text = gpsDataForUI.ID.ToString();
            timeElapsedTextBox.Text = gpsDataForUI.timeElapsed.ToString();
            //GPS Core Data
            latitudeTextBox.Clear();
            latitudeTextBox.AppendText(gpsDataForUI.latitude);
            longitudeTextBox.Clear();
            longitudeTextBox.AppendText(gpsDataForUI.longitude);
            altitudeTextBox.Clear();
            altitudeTextBox.AppendText(gpsDataForUI.altitude);

            //Vehicle Properties
            speedKnotsTextBox.Clear();
            speedKnotsTextBox.AppendText(gpsDataForUI.grspd_k);
            speedKphTextBox.Clear();
            speedKphTextBox.AppendText(gpsDataForUI.grspd_kph);
            headDegTextBox.Clear();
            headDegTextBox.AppendText(gpsDataForUI.trkangle);
            headCardTextBox.Clear();
            headCardTextBox.AppendText(gpsDataForUI.cardAngle);

            //Fix Information
            satsViewTextBox.Clear();
            satsViewTextBox.AppendText(gpsDataForUI.numsats);
            fixqualTextBox.Clear();
            fixqualTextBox.AppendText(gpsDataForUI.fixqual_f);
            fixvalTextBox.Clear();
            fixvalTextBox.AppendText(gpsDataForUI.fixtype_f);
            accuracyTextBox.Clear();
            accuracyTextBox.AppendText(gpsDataForUI.accuracy.ToString());

            //Date and time
            dateTextBox.Clear();
            dateTextBox.AppendText(gpsDataForUI.date);

            //do some formatting
            timeTextBox.Clear();
            timeTextBox.AppendText(gpsDataForUI.time);


        }

        private void recvRawDataWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

                statusTextBox.Clear();
                statusTextBox.AppendText("Parsing in progress. No errors.");
                updateUI(gpsData, sentenceBuffer);

        }


        private void stopButton_Click(object sender, EventArgs e)
        {
            //only stop after confirmation
            DialogResult quitQuestion = MessageBox.Show("This will stop the GPS log. Stop logging now?", "Stop GPS logging?", MessageBoxButtons.YesNo);
            if (quitQuestion == DialogResult.Yes)
            {
                stopButton.Enabled = false;
                startButton.Enabled = true;
                openFileButton.Enabled = true;
                recvRawDataWorker.CancelAsync(); //requests cancellation of the worker
                statusTextBox.Clear();
                statusTextBox.AppendText("Stop requested, ending thread...");
            }

            
        }

        private void recvRawDataWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            statusTextBox.Clear();
            statusTextBox.AppendText("GPS parsing complete or interrupted.");
            rawLogFileTextBox.Text = "";
            stopButton.Enabled = false;
            startButton.Enabled = true;
            openFileButton.Enabled = true;
            trayIconParsing.Text = "Logging stopped.";
            trayIconParsing.ShowBalloonTip(5, "GPS Logging stopped", "Logging stopped or interrupted. Open a new file to restart logging.", ToolTipIcon.Error);
        }

        private void openPortButton_Click(object sender, EventArgs e)
        {

        }

        private void trayIconParsing_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        //ToolStrip Methods - to enable or disable the database

        

        private void enabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (enabledToolStripMenuItem.Checked == true)
            {
                MessageBox.Show("Database logging is on by default.","Logging already on");
                enabledToolStripMenuItem.Checked = true;
                disabledToolStripMenuItem.Checked = false;
            }
            else
            {
                MessageBox.Show("Logging to the database has been enabled.","Logging turned on");
                enabledToolStripMenuItem.Checked = true;
                disabledToolStripMenuItem.Checked = false;
            }
            
        }

        private void disabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (disabledToolStripMenuItem.Checked == false && enabledToolStripMenuItem.Checked == true)
            {
                DialogResult quitDB = MessageBox.Show("This turns off logging to XML database, but keeps receiving incoming GPS data. Keep receiving data but stop logging to database?", "Stop database logging?", MessageBoxButtons.YesNo);
                if (quitDB == DialogResult.Yes)
                {
                    MessageBox.Show("Logging to the database has been disabled.","Database logging off");
                    disabledToolStripMenuItem.Checked = true;
                    enabledToolStripMenuItem.Checked = false;
                }
            }
            else if (disabledToolStripMenuItem.Checked == true && enabledToolStripMenuItem.Checked == false)
            {
                MessageBox.Show("Database logging is already disabled.","Database logging already off");
            }

        }

        private void enabledToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void disabledToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {

        }


    }
}
