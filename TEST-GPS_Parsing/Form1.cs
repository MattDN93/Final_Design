using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;


namespace TEST_GPS_Parsing
{
    public partial class Form1 : Form
    {
        //*********THIS VAR IS FOR TESTING FEATURES, set to false for debug features off
        bool debug = false;
        //*************************************
        public string logFilename;
        bool dbLoggingActive = true;
        bool newLogEveryStart = true;                 //TRUE = makes new file every time start is clicked; FALSE = once per session
        GPSPacket gpsData = new GPSPacket();          //global GPS data packet for UI display
        string sentenceBuffer;                        //global buffer to read incoming data used for parsing
        string rawBuffer;                             //not used for parsing , but for display only


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

 

        /// <summary>
        /// The "start"button click action.
        /// Starts the appropriate thread for logging data or setting up the database if required
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                recvRawDataWorker.RunWorkerAsync();         //starts the thread to parse data and update UI
                if (dbLoggingActive == true)
                {
                    dbLoggingThread.RunWorkerAsync();       //starts thread to manage DB data if logging is on
                }

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
                dbLoggingThread.CancelAsync(); //requests cancellation of the database thread
                statusTextBox.Clear();
                statusTextBox.AppendText("Stop requested, ending thread...");
            }


        }

        private void updateUITimer_Tick(object sender, EventArgs e)
        {
            if (recvRawDataWorker.IsBusy)
            {
                gpsData.timeElapsed++; //update the running timer only if parsing is active
            }

            //also check on status of database logging requirement
            if (dbLoggingActive == true)
            {
                status2TextBox.Clear();
                status2TextBox.AppendText("Database logging enabled.");
            }
            else if (dbLoggingActive == false)
            {
                dbLoggingThread.CancelAsync();      //end the logging thread
                status2TextBox.Clear();
                status2TextBox.AppendText("Database logging disabled.");
            }

        }

        //-------------------------THREADING SECTION--------------------------------
        //                          THREAD 1: FOR BACKGROUND WORK--------------------
        private void recvRawDataWorker_DoWork(object sender, DoWorkEventArgs e)
        {
       

            //create a new streamreader instance for the incoming file
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
                        rawBuffer += sentenceBuffer;                         //aggregates the raw buffer to display it
                        gpsData = gpsData.parseSelection(sentenceBuffer, gpsData);
                        count++;
                        
                    //this allows for a thread-safe variable access
                    if (gpsData.packetID > gpsData.deltaCount)
                    {
                        gpsData.deltaCount++;
                        recvRawDataWorker.ReportProgress(count, gpsData); //only update the UI if a whole new packet has been read- saves calling every char
                        //rawBuffer = "";
                    }
                    

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

        //------------------------THREAD 2: FOR UPDATING THE DATABASE--------------------------------

        private void dbLoggingThread_DoWork(object sender, DoWorkEventArgs e)
        {
            //set up the XML database - if the user wants it of course! this is the initial check for database active or not
            if (dbLoggingActive == true)    //initial check if user wants DB logging
            {
                XmlDocument databaseDoc = new XmlDocument();
                //if (newLogEveryStart == true) //if set, a new log is created every time "Start" is clicked 
                //{
                //create a basic document skeleton and save it
                //set up the root node

                XmlNode rootNode = databaseDoc.CreateElement("GPSLog");
                databaseDoc.AppendChild(rootNode);

                //insert comment for info
                XmlComment docComment, docComment2;
                docComment = databaseDoc.CreateComment("GPS Data Logging Session Start");
                docComment2 = databaseDoc.CreateComment("Logging started at " + DateTime.Now.ToString());
                databaseDoc.InsertBefore(docComment, rootNode);
                databaseDoc.InsertBefore(docComment2, rootNode);

                string dbFileName = "GPSLogDB" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xml";

                databaseDoc.Save(dbFileName);
                System.IO.StreamWriter dbOutputFile = new System.IO.StreamWriter(dbFileName);

                int packetCount = 0;
                //now enter the continuous logging loop for as long as the incoming data parser thread is running
                while (recvRawDataWorker.IsBusy)
                {
                    packetCount++;          //increment number of packets processed
                    if (!dbLoggingThread.CancellationPending)
                    {
                        //dbLoggingThread.ReportProgress(packetCount, databaseDoc);
                        //write a new 
                    }
                    else
                    {
                        //a unusual cancel has been requested, try close XML file safely

                        break;
                    }
                }

            }
        }
        
        

        //----------------------Progress Changed on thread methods-------------------------------------

        private void dbLoggingThread_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
        }

        private void recvRawDataWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //check the status of the system each time a new sentence received and inform the user
            statusTextBox.Clear();
            statusTextBox.AppendText("Parsing in progress. No errors.");
          
            //function called to get all the incoming data and refresh the UI
            updateUI(gpsData, sentenceBuffer);
            rawBuffer = "";             //since earlier call is asynchronous, only clear the raw buffer once 100% sure that its data has reached the UI - which is now.
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

        //-------------------------UI update method------------------------------------------

        private void updateUI(GPSPacket gpsDataForUI, string sentenceBufferForUI)
        {
            //This takes the gpsData object and populates all the fields with updated values (if any)
            //Show the buffer of the raw text file being read 
            rawLogFileTextBox.AppendText(rawBuffer);
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

        //-------------------------End threads---------------------------------------------

        private void openPortButton_Click(object sender, EventArgs e)
        {

        }

        private void trayIconParsing_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        //ToolStrip Methods - to enable or disable the database or other settings---------------------------------

        private void enabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (enabledToolStripMenuItem.Checked == true)
            {
                MessageBox.Show("Database logging is on by default.","Logging already on");
                dbLoggingActive = true;
                creationOptionsToolStripMenuItem.Enabled = true;
                enabledToolStripMenuItem.Checked = true;
                disabledToolStripMenuItem.Checked = false;
            }
            else
            {
                MessageBox.Show("Logging to the database has been enabled.","Logging turned on");
                dbLoggingActive = true;
                creationOptionsToolStripMenuItem.Enabled = true;
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
                    dbLoggingActive = false;
                    disabledToolStripMenuItem.Checked = true;
                    enabledToolStripMenuItem.Checked = false;
                    creationOptionsToolStripMenuItem.Enabled = false;
                }
            }
            else if (disabledToolStripMenuItem.Checked == true && enabledToolStripMenuItem.Checked == false)
            {
                MessageBox.Show("Database logging is already disabled.","Database logging already off");
                creationOptionsToolStripMenuItem.Enabled = false;   //disable options for file creation
                dbLoggingActive = false;
            }

        }

        private void oncePerSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //the default option is creating a new db logfile every time the start button is clicked
            if (oncePerSessionToolStripMenuItem.Checked == false && newFileAtEachStartToolStripMenuItem.Checked == true)
            {
                oncePerSessionToolStripMenuItem.Checked = true;
                newFileAtEachStartToolStripMenuItem.Checked = false;
                newLogEveryStart = false;

            }
        }

        private void newFileAtEachStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (oncePerSessionToolStripMenuItem.Checked == true && newFileAtEachStartToolStripMenuItem.Checked == false)
            {
                oncePerSessionToolStripMenuItem.Checked = false;
                newFileAtEachStartToolStripMenuItem.Checked = true;
                newLogEveryStart = true;
            }
        }


    }
}
