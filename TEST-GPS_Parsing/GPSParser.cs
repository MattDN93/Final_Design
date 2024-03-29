﻿using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace TEST_GPS_Parsing
{
    public partial class GPSParser : Form
    {
        #region Initialization and Vars
        //*********THIS VAR IS FOR TESTING FEATURES, set to false for debug features off
        bool debug = true;
        //*************************************
        public string inputLogFilename;
        bool dbLoggingActive = true;
        bool parseIsRunning = false;                  //bool to denote whether the parser is in progress or not.
        bool videoOutputRunning = false;                //bool to check if the vo object has been created or not
        bool usingWebLogging;                       //flag to mention whether file or webserver parsing is used
        TimeSpan gpsConnectedTimeSpan = new TimeSpan(0, 0, 10); //10s timespan
        DateTime sinceStartPressed;                 //date/time since start of log

        //web request status flag returns
        private static int requestCompleteOK = 0;
        private static int serverReturnedFail = 1;
        private static int dataReadFail = 2;

        private Object coordsSendLock = new Object();


        /*IMPORTANT: These wait handles are for the mutex locks on the multithreading
            waitHandleParser:   FALSE = parser has lock, other threads must wait till release
                                TRUE = if the dbthread is waiting with WaitOne(), setting this with .Set() unlocks and hands to the db thread
            waitHandleDatabase: FALSE = database writer has lock,parser thread must wait till release
                                TRUE = use .set() to relinquish lock back to the parser thread
             
             */
        static EventWaitHandle _waitHandleParser = new AutoResetEvent(false);
        static EventWaitHandle _waitHandleDatabase = new AutoResetEvent(false);
        static EventWaitHandle _waitHangleWebRequest = new AutoResetEvent(true);

        GPSPacket gpsData = new GPSPacket();          //global GPS data packet for UI display                                                     
        Mapping mapData = new Mapping();              //set up a new mapping object for mapping function access
        GMapOverlay locationMarkersOverlay;           //overlay for the location markers on map
        VideoOutputWindow vo;                         //object for the video output window
        MySQLInterface sqlDb;                           //MySQL database instance

        string serverUri;                               //displayed to user to show which server they're connected to

        string sentenceBuffer;                        //global buffer to read incoming data used for parsing
        string rawBuffer;                             //not used for parsing , but for display only

        int duplicatePacketCounter = 1;                     //used to ensure duplicate packets aren't saved into the DB

        /*PacketID meanings
         *gpsData.ID = GLOBAL ID; consists of object tracking points + incoming GPS points
         *             Seen? On the "ID" pane in the UI and the "PacketID" column in the DB
         *gpsData.packetID = GPS Cumulative ID: a cumulative ID from the GPS logger's remote DB
         *             Seen? Not shown - used to determine whether GPS is available & for DB duplicate logger
         *gpsData.sessionID = Current session of power-on by GPS. Mirrored on remote DB and here
         *             Seen? In the SessionID column in the DB and used to check if the GPS is OK        
         */
        int referenceGlobalPacketID = 0;                  //see above
        int referenceSessionPacketID = -1;                   //see above
        int referencePacketID = -1;                         //see above
        private int localRequestResult;

        //check time of last method call for disconnect timer or cam switch
        DateTime? lastCallTime = null;

        bool gpsConnectionOK;                       //flag to show GPS is connected

        public bool newLogEveryStart { get; private set; }

        //private XmlWriter fileStream;
        #endregion

        #region Class Instantiation
        public GPSParser()
        {
            InitializeComponent();
            //set all the values in the UI to default
            //set up a new packet object with default constructor
            GPSPacket gpsData = new GPSPacket();

            //Monitoring
            globalIDTextBox.Text = gpsData.globalID.ToString();
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
            bool initSuccess = initMappingPane();
            if (initSuccess == false)
            {
                status2TextBox.Clear();
                status2TextBox.AppendText("Mapping control load failed.");
            }

        }
        #endregion

        #region Init Panes and UI
        private bool initMappingPane()
        {
            //Set up the Mapping provider to show the maps in the pane
            //Using OpenStreetMaps for now because of Google's licencing issues (Need to use their API otherwise)     
            try
            {
                mapPane.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
                GMaps.Instance.Mode = AccessMode.ServerAndCache;      //we want to cache data locally so network doesn't suffer
                mapPane.SetPositionByKeywords("Durban, South Africa");                  //init the map before logging starts
                openStreetMapsToolStripMenuItem.Checked = true;                         //use OSM by default

                //set up the overlay on which to display the markers of user location
                locationMarkersOverlay = new GMapOverlay("locationMarker");

            }
            catch (SystemException)
            {
                return false;

            }
            return true;
        }

        private void initOpenLogDialog()
        {
            //prep the load box
            //openLogDialog = new OpenFileDialog();     //create new instance of the openFileDialog object
            startButton.Enabled = false;
            stopButton.Enabled = false;
            dbSetupButton.Enabled = false;
            dbUsernameTextbox.Enabled = false;
            dbPwdTextbox.Enabled = false;
            dbLoggedOnLabel.Visible = false;
            statusTextBox.BackColor = System.Drawing.Color.LemonChiffon;
            statusTextBox.AppendText("Ready. Click 1. to open server.");

            //TEST - init the networking session for the GPS
            servStatusTextbox.Text = "Connect to server...";

            //setup the cam disconnect time with now
            lastCallTime = DateTime.Now;

        }
        #endregion

        #region Main User Button Interaction
        private void dbSetupButton_Click(object sender, EventArgs e)
        {
            dbSetupButton.Enabled = true;
            
            sqlDb = new MySQLInterface();
            try
            {
                //try logon to the DB with the user credentials
                sqlDb.loginProcess(dbUsernameTextbox.Text, dbPwdTextbox.Text);
                dbSetupButton.Enabled = false;
                dbStatusTextbox.BackColor = System.Drawing.Color.LawnGreen;
                dbStatusTextbox.Text = "Logged onto DB OK - Ready!";
                //remove the logon details for this session since it'll stay logged in
                dbPwdTextbox.Visible = false;
                dbUsernameTextbox.Visible = false;
                dbSetupButton.Visible = false;
                unLabel.Visible = false;
                pwLabel.Visible = false;
                dbLoggedOnLabel.Visible = true;
            }
            catch (Exception open_ex)
            {
                dbStatusTextbox.BackColor = System.Drawing.Color.PaleVioletRed;
                dbStatusTextbox.Text = open_ex.Message.ToString();
            }
            
        }


        private void openFileButton_Click(object sender, EventArgs e)
        {
            //open the dialog
            openLogDialog.ShowDialog();
            dbSetupButton.Enabled = true;
            dbUsernameTextbox.Enabled = true;
            dbPwdTextbox.Enabled = true;
        }

        private void openLogDialog_FileOk(object sender, CancelEventArgs e)
        {
            //disable the webserver logging now
            testGpsGetButton.Enabled = false;
            usingWebLogging = false;
            //MessageBox.Show("GPS NMEA log file ready. Click Start Tracking.");
            inputLogFilename = openLogDialog.FileName;       //save the filename of the logfile
            gpsData.gpsLogfilename = inputLogFilename;       //save filename to associated GPS class
            startButton.Enabled = true;
            statusTextBox.Clear();
            statusTextBox.AppendText("Server opened OK. Click start above.");
        }

 

        /// <summary>
        /// The "start"button click action.
        /// Starts the appropriate thread for logging data or setting up the database if required
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startButton_Click(object sender, EventArgs e)
        {
            //sinceStartPressed = DateTime.Now; //used for time elapsed box since start pressed
            //threading start!
            if (!recvRawDataWorker.CancellationPending)
            {
                openFileButton.Enabled = false;
                startButton.Enabled = false;
                stopButton.Enabled = true;
                parseIsRunning = true;
                trayIconParsing.Text = "GPS logging active...";
                trayIconParsing.ShowBalloonTip(5, "Logging running...", "Logger will continue running here if main window closed.",ToolTipIcon.Info);
                if (sqlDb != null)
                {
                    gpsData.globalID = sqlDb.getGlobalID() + 1;     //request the global ID from the DB
                }              
                referenceGlobalPacketID = gpsData.globalID - 1; //initially set the two to different things

                if (usingWebLogging == true)
                {
                    localRequestResult = -1;                //init to a non-used value
                    webRequestThread.RunWorkerAsync();      //start the web thread if needed
                }


                recvRawDataWorker.RunWorkerAsync();         //starts the thread to parse data and update UI

                if (dbLoggingActive == true)
                {
                    dbLoggingThread.RunWorkerAsync();       //starts thread to manage DB data if logging is on
                }

            }
            else
            {
                recvRawDataWorker.CancelAsync();
                if (usingWebLogging == true) { webRequestThread.CancelAsync(); }             
                statusTextBox.Clear();
                trayIconParsing.Text = "Parsing error! See main window.";
                trayIconParsing.ShowBalloonTip(5, "GPS Logging error" ,"The last operation isn't finished yet. Please wait and try again.", ToolTipIcon.Error);
                statusTextBox.AppendText("Error: previous task not cancelled yet. Try again.");
            }


        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            //only stop after confirmation
            DialogResult quitQuestion = MessageBox.Show("This will stop the GPS log. Stop logging now?", "Stop GPS logging?", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (quitQuestion == DialogResult.Yes)
            {
                //enable the choices again
                testGpsGetButton.Text = "Connect to server";
                testGpsGetButton.Enabled = true;
                openFileButton.Enabled = true;

                stopButton.Enabled = false;
                startButton.Enabled = true;
                if (usingWebLogging == true) { webRequestThread.CancelAsync(); } //cancel the web thread
                recvRawDataWorker.CancelAsync(); //requests cancellation of the worker
                _waitHandleDatabase.Set();      //pings the parser thread to release the lock
                dbLoggingThread.CancelAsync(); //requests cancellation of the database thread
                _waitHandleParser.Set();
                statusTextBox.Clear();
                statusTextBox.AppendText("Stop requested, ending thread...");
                
            }



        }

        //------------------Open the Video Streaming Component------------------
        private void openPortButton_Click(object sender, EventArgs e)
        {
            //updateUITimer.Start();
            //if (!vo.IsDisposed)
            //{
            //CameraBoundsSetup cmBound = new CameraBoundsSetup(vo);
            vo = new VideoOutputWindow();
                videoOutputRunning = true; //used to inform the parser it can send co-ords to the video method now
                vo.Show();
            //DialogResult cmResult = cmBound.ShowDialog();
            updateUITimer.Start();
            openVideoButton.Enabled = false;
            //}
        }

        //tests connection to the server
        private void testGpsGetButton_Click(object sender, EventArgs e)
        {

            //sends a basic HTTP command to the server to make sure it's online
            //Web service options for the GPS webservice
            WebRequest request;
            WebResponse response;
            try
            {
                request = WebRequest.Create(
"http://ec2-54-244-63-232.us-west-2.compute.amazonaws.com");
                // Get the response.
                request.Proxy = null;
                response = request.GetResponse();
                // Display the status.
                string servResponse = ((HttpWebResponse)response).StatusDescription;
                if (servResponse != "OK")
                {
                    servStatusTextbox.Text = "Server request failed.";
                    response.Close();
                    return;
                }
                else
                {
                    servStatusTextbox.BackColor = System.Drawing.Color.LightGreen;
                    //results success so server is online, go ahead with setting up
                    testGpsGetButton.Text = "Server online";
                    serverUri = ((HttpWebResponse)response).ResponseUri.ToString();
                    serverUri = serverUri.Substring(0, serverUri.LastIndexOf('/')) + "/phpmyadmin";
                    testGpsGetButton.Enabled = false;
                    usingWebLogging = true;
                    openFileButton.Enabled = false;
                    servStatusTextbox.Text = servResponse;

                    dbSetupButton.Enabled = true;
                    dbUsernameTextbox.Enabled = true;
                    dbPwdTextbox.Enabled = true;
                    startButton.Enabled = true;
                    statusTextBox.Clear();
                    statusTextBox.AppendText("Server opened OK. Click start above.");
                    response.Close();
                }
            }
            catch (Exception excpt)
            {
                servStatusTextbox.BackColor = System.Drawing.Color.PaleVioletRed;
                servStatusTextbox.Text = "Error: Check connection: " + excpt.Message.ToString();
                return;
            }
            


        }
        #endregion

        #region Updating UI methods

        private void updateUITimer_Tick(object sender, EventArgs e)
        {
            if (parseIsRunning)
            {
                gpsData.timeElapsed++; //update the running timer only if parsing is active
            }
            if (recvRawDataWorker.IsBusy)
            {
                
                if (!videoOutputRunning)
                {
                    statusTextBox.BackColor = System.Drawing.Color.Orange;
                    statusTextBox.Text = "Warning: Video capture not set up yet!";
                }

            }

            if (parseIsRunning && (gpsData.timeElapsed % 5 == 0))
            {

                //compare the packet from 10 seconds ago to the one now - if they match GPS is off
                if ((gpsData.gpsSessionID != referenceSessionPacketID))
                {
                    gpsConnectionOK = true;
                    status2TextBox.BackColor = System.Drawing.Color.LightGreen;
                    status2TextBox.Text = "GPS is available & connected.";
                    referenceSessionPacketID = gpsData.gpsSessionID;
                }
                else
                {
                    gpsConnectionOK = false;
                    status2TextBox.BackColor = System.Drawing.Color.Orange;
                    status2TextBox.Text = "GPS feed lost - SMS 'status' to check.";
                    referenceSessionPacketID = gpsData.gpsSessionID;
                }
            }


            //also check on status of database logging requirement
            if (dbLoggingActive == true)
            {
                dbStatusTextbox.Clear();
                dbStatusTextbox.AppendText("Database logging enabled.");
            }
            else if (dbLoggingActive == false)
            {
                dbLoggingThread.CancelAsync();      //end the logging thread
                
                dbStatusTextbox.Clear();
                dbStatusTextbox.AppendText("Database logging disabled.");
            }

            if (vo != null)
            {
                if (vo.Visible)
                {
                    openVideoButton.Enabled = false;
                }
                else
                {
                    openVideoButton.Enabled = true;
                }
            }


        }

        public void doChecksumCheck()
        {
            //check if any checksum calcs have failed and set string to display on UI
            if (gpsData.checksumGNGGA == "FAIL" || gpsData.checksumGNRMC == "FAIL" || gpsData.checksumGNVTG == "FAIL")
            {
                gpsData.checksumResultStatusForDisplay = "";
                if (gpsData.checksumGNGGA == "FAIL")
                {
                    gpsData.checksumResultStatusForDisplay += "GPGGA fail| ";
                }
                if (gpsData.checksumGNRMC == "FAIL")
                {
                    gpsData.checksumResultStatusForDisplay += "GPRMC fail| ";
                }
                if (gpsData.checksumGNVTG == "FAIL")
                {
                    gpsData.checksumResultStatusForDisplay += "GPGGA fail";
                }
            }
            else
            {
                gpsData.checksumResultStatusForDisplay = "";
                gpsData.checksumResultStatusForDisplay = "Checksums OK";
            }
        }

        //-------------------------UI update method------------------------------------------

        private void updateUI(GPSPacket gpsDataForUI, string sentenceBufferForUI)
        {
            //This takes the gpsData object and populates all the fields with updated values (if any)

            //updates the map pane overlay with location
            locationMarkersOverlay = mapData.plotOnMap(locationMarkersOverlay);          //passes the initialised overlay to be populated
            mapPane.Overlays.Add(locationMarkersOverlay);

            //Show the buffer of the raw text file being read 
            rawLogFileTextBox.AppendText(rawBuffer);
            sentenceBufferForUI = "";
            //Monitoring
            globalIDTextBox.Text = gpsDataForUI.globalID.ToString();
            //timeElapsedTextBox.Text = DateTime.Now.Subtract(sinceStartPressed).Seconds.ToString();
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

            //show the status of the checksum calculation
            checksumTextbox.Clear();
            checksumTextbox.Text = gpsData.checksumResultStatusForDisplay;

            //show the status of other components
            servStatusTextbox.Clear();
            servStatusTextbox.Text = gpsData.webserverRequestResult;


        }
        #endregion

        #region Web requests for GPS data

        public int makeWebRequest()
        {
            //Web service options for the GPS webservice
            WebRequest request;
            WebResponse response;
            request = WebRequest.Create(
"http://ec2-54-244-63-232.us-west-2.compute.amazonaws.com/send_gpsData.php");
            // If required by the server, set the credentials.
            //request.Credentials = CredentialCache.DefaultCredentials;
            request.Proxy = null;
            // Get the response.
            response = request.GetResponse();
            // Display the status - get URI to send to link.
            serverUri = ((HttpWebResponse)response).ResponseUri.ToString();
            serverUri = serverUri.Substring(0, serverUri.LastIndexOf('/')) + "/phpmyadmin";

            string servResponse = ((HttpWebResponse)response).StatusDescription;
            servStatusTextbox.Text = servResponse;

            if (servResponse != "OK")
            {
                string fullResponse = response.ToString();  //full response string
                gpsData.webserverRequestResult = fullResponse;
                response.Close();
                return serverReturnedFail;
            }
            else
            {
                gpsData.webserverRequestResult = ((HttpWebResponse)response).StatusDescription;
                Stream datastream = response.GetResponseStream();
                if (datastream.CanRead == false)
                {
                    gpsData.webserverRequestResult = "Datastream read failed - check server up!";
                    response.Close();
                    return dataReadFail;
                }
                else
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(datastream);
                    // Read the content.
                    string responseFromServer = reader.ReadToEnd();
                    //pulls out the packet ID and checks if it's larger than the last one - else suspect internet!
                    int packetID = Convert.ToInt32(responseFromServer.Substring(0, responseFromServer.IndexOf(';')));
                    // Display the content in the raw buffers 
                    rawBuffer = responseFromServer;
                    // Clean up the streams and the response and write the main response to the buffer for parsing
                    sentenceBuffer = responseFromServer;
                    reader.Close();
                    response.Close();
                    return requestCompleteOK;
                }

            }

        }

        private void servAccessLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (serverUri != null)      //user should have opened the server first
            {
                System.Diagnostics.Process.Start(serverUri);
            }
            
        }

        #endregion

        //-------------------------THREAD 1: FOR UPDATING UI and DATA----------------
        #region Background Worker Thread
        //-------------------------THREAD 0: FOR MAKING WEB REQUESTS-----------------
        private void webRequestThread_DoWork(object sender, DoWorkEventArgs e)
        {
            //only run if parsing is happening
            if (!webRequestThread.CancellationPending && recvRawDataWorker.IsBusy)
            {
                Console.Write("Making Web REquest");
                localRequestResult = makeWebRequest();

            }
        }

        //-------------------------THREAD 1: FOR BACKGROUND WORK--------------------
        private void recvRawDataWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Set thread parameters
            Thread.CurrentThread.Name = "GPS Logging Thread";

            //This thread now uses 2 different methods: one parses raw NMEA, the other parses from a webserver
            //------------------METHOD 1: PARSING USING THE WEBSERVER--------------
            #region Parsing with real GPS webserver source
            //setup the web source
            int countWeb = 0;
            while (usingWebLogging == true && !recvRawDataWorker.CancellationPending)
            {
                if (localRequestResult == requestCompleteOK)
                {
                    if (referencePacketID == -1)
                    {
                        referencePacketID = gpsData.packetID;   //set the reference packet ID first time
                    }
                    //compares session ID which is ONLY updated by incoming GPS coordinates, if they don't change, GPS is stuck
                    if ((referencePacketID < gpsData.packetID) || gpsData.gpsSessionID == 0) //normal operation - server and GPS ok
                    {
                        gpsConnectionOK = true;
                    }
          
                    //otherwise do a full update
                    
                    rawBuffer += sentenceBuffer;                         //aggregates the raw buffer to display it
                    
                    //Checks if the resource is available 
                    Console.WriteLine("Parser has data-lock");
                    gpsData = gpsData.parseSelection(sentenceBuffer, gpsData, true, referencePacketID);  //perform the parsing operation
                    referencePacketID = gpsData.packetID;
                    mapData.parseLatLong(gpsData.latitude, gpsData.longitude, true);  //pass the data to the mapping method                        


                    //send the co-ordinates to the video output UI - keeps calling till its set
                    //pass the new instance of the overlay if it's been disposed before
                    if (parseIsRunning)
                    {
                        try
                        {
                            if (!vo.IsDisposed)
                            {
                                lock (coordsSendLock)
                                {
                                    if ((referencePacketID < gpsData.packetID) || gpsConnectionOK)
                                    {
                                        vo.overlayTick(mapData.latitudeD, mapData.longitudeD);                //send to the vid output class - force a "tick" to update coords
                                    }
                                }

                            }

                        }
                        catch (NullReferenceException)
                        {
                            videoOutputRunning = false;
                        }


                    }

                }   //end of webserver flag check
                else
                {
                    countWeb++;
                    if (countWeb > 20)
                    {
                        countWeb = 0;
                    }
                    if (localRequestResult == serverReturnedFail)    //inform the user of the fail
                    {
                        gpsData.webserverRequestResult = "Connectivity fail...";
                    }
                    if (rawBuffer == null || sentenceBuffer == null) //account for the fact that the request might not be done yet
                    {
                        rawBuffer = "";
                        sentenceBuffer = "";
                        
                    }
                }
                        gpsData.deltaCount++;
                //locationMarkersOverlay = mapData.plotOnMap(locationMarkersOverlay);          //passes the initialised overlay to be populated
                makeWebRequest();
                //unlock the data to write to the DB 
                _waitHandleParser.Set();
                
                Console.WriteLine("Parser released lock");

                Console.WriteLine("Waiting for DB write to finish");
                        if (dbLoggingActive == true)
                        {
                            _waitHandleDatabase.WaitOne();                  //only wait for write if there's actually a write happening
                        }

                        Console.WriteLine("Parser obtained UI write lock");
                        recvRawDataWorker.ReportProgress(countWeb, gpsData); //only update the UI if a whole new packet has been read- saves calling every char                                                

                        if (dbLoggingActive == true)
                        {
                            _waitHandleParser.Set(); //inform the db thread that the lock has been released
                        }

                        Console.WriteLine("Parser released UI lock");
                        //rawBuffer = "";
            }


           
            #endregion

            #region Parsing with simulated NMEA files
            if (usingWebLogging == false)
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


                        //Checks if the resource is available 
                        Console.WriteLine("Parser has data-lock");
                        gpsData = gpsData.parseSelection(sentenceBuffer, gpsData,false, referencePacketID);  //perform the parsing operation
                           //shift the packetID forwards                                                        //doChecksumCheck();                                          //check on the checksums of the current method
                        mapData.parseLatLong(gpsData.latitude, gpsData.longitude,false);  //pass the data to the mapping method

                        count++;

                        //this allows for a thread-safe variable access
                        if (gpsData.packetID > gpsData.deltaCount)
                        {
                            gpsData.deltaCount++;
                            //locationMarkersOverlay = mapData.plotOnMap(locationMarkersOverlay);          //passes the initialised overlay to be populated

                            //unlock the data to write to the DB 
                            _waitHandleParser.Set();
                            Console.WriteLine("Parser released lock");

                            //send the co-ordinates to the video output UI - keeps calling till its set
                            //pass the new instance of the overlay if it's been disposed before
                            if (parseIsRunning)
                            {
                                try
                                {
                                    if (!vo.IsDisposed)
                                    {
                                        lock (coordsSendLock)
                                        {
                                            vo.overlayTick(mapData.latitudeD, mapData.longitudeD);                  //send to the vid output class - force a "tick" to update coords
                                        }

                                    }

                                }
                                catch (NullReferenceException)
                                {
                                    videoOutputRunning = false;
                                }


                            }




                            Console.WriteLine("Waiting for DB write to finish");
                            if (dbLoggingActive == true)
                            {
                                _waitHandleDatabase.WaitOne();                  //only wait for write if there's actually a write happening
                            }

                            Console.WriteLine("Parser obtained UI write lock");
                            recvRawDataWorker.ReportProgress(count, gpsData); //only update the UI if a whole new packet has been read- saves calling every char                                                

                            if (dbLoggingActive == true)
                            {
                                _waitHandleParser.Set(); //inform the db thread that the lock has been released
                            }

                            Console.WriteLine("Parser released UI lock");
                            //rawBuffer = "";
                        }


                        //FOR SIMULATION ONLY
                        if (debug)
                        {
                            Thread.Sleep(300);
                        }

                        //---------------
                    }
                    else
                    {
                        //everything else is handled in the completion thread
                        break;
                    }
                }
                inputFile.Close();
            }
            #endregion





        
        }

        #endregion
        //------------------------THREAD 2: FOR UPDATING THE DATABASE----------------
        #region Database Update Thread

        private void dbLoggingThread_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.CurrentThread.Name = "Database Write Thread";
            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            //creates XMLNode object - only call the create method if logging is needed
            XMLNodes myXmlDb = new XMLNodes();
            
            //set up the XML database - if the user wants it of course! this is the initial check for database active or not
            if (dbLoggingActive == true)    //initial check if user wants DB logging
            {
                myXmlDb.createXmlDbFile();  //calls the method to set the DB up

                int packetCount = 0;
                //now enter the continuous logging loop for as long as the incoming data parser thread is running
                while (parseIsRunning)      //stay in this loop as long as the parser is active
                {
                    packetCount++;          //increment number of packets processed
                    if (!dbLoggingThread.CancellationPending)
                    {
                        if (vo != null)
                        {
                            //query the object tracker - returns null if object tracker is off
                            if (!gpsConnectionOK) //only run if GPS is disconnected!
                            {
                                double[] tempGetCoords = new double[2];
                                tempGetCoords = vo.getObjTrackingCoords();
                                if (tempGetCoords != null && tempGetCoords[0] != 0 ) //i.e. object tracking is ON
                                {
                                    //write the onscreen co-ords to a gpsData obj for database storages
                                    //add timestamp and date for database
                                    if (gpsData.latitude != tempGetCoords[0].ToString()) //avoid duplicates
                                    {
                                        gpsData.latitude = tempGetCoords[0].ToString();
                                        gpsData.longitude = tempGetCoords[1].ToString();
                                        gpsData.date = DateTime.Now.ToString("yyyy-MM-dd");
                                        gpsData.time = DateTime.Now.ToString("HH:mm:ss");
                                        gpsData.globalID = gpsData.globalID + 1;
                                    }
 
                                }
                            }
                            
                        }

                        Console.WriteLine("Waiting for parser to release lock");
                        /*  tells the db logger thread to block until it receives a Set() signal from the parser thread 
                            to indicate that the parser has released the lock */
                        _waitHandleParser.WaitOne();

                        //Once this line is passed we assume the lock was released so the db thread has lock on the data
                        Console.WriteLine("DB thread obtained write lock");
                        //bool writeComplete = writeToDatabase(myXmlDb, gpsData);


                        //New SQL Database call to update the database
                        bool writeCompleteSQL = writeToDabataseSQL(gpsData);                        

                        while (!writeCompleteSQL)
                        {
                            Console.WriteLine("Writing to DB...");
                        }
                        //now the blocking parser thread gets sent a signal that the db thread is releasing the resources
                        _waitHandleDatabase.Set();
                        Console.WriteLine("Write done - DB releasing lock");

                        //write a new 
                    }
                    else
                    {
                        //a unusual cancel has been requested, try close XML file safely
                        bool quitResult;
                        quitResult = myXmlDb.closeXmlDbFile();
                        if (quitResult == false)
                        {
                            status2TextBox.Clear();
                            status2TextBox.AppendText("Error closing DB. Corruption possible.");
                            break;
                        }
                        else
                        {
                            break;
                        }
                        
                    }
                }

            }
            _waitHandleDatabase.Set();  //ping the main thread to keep logging since database logging is off
        }


        //-------------------------DB update method---------------
        private bool writeToDabataseSQL(GPSPacket gpsDataForDB)
        {
            //log a camera switch event
            if (vo != null && vo.logCamSwitchToDb == true && sqlDb != null)
            {
                vo.logCamSwitchToDb = false;
                sqlDb.populateDbFieldsVideo(gpsDataForDB, vo, "camera_Switch");
            }
            //log a camera disconnection event - check Pc time since GPS time will be stuck
            if (vo != null && vo.grabResult == false && sqlDb != null && DateTime.Now > lastCallTime)
            {
                lastCallTime = DateTime.Now;        //update the last call time
                sqlDb.populateDbFieldsVideo(gpsDataForDB, vo, "connection_Fail");
            }
            //don't write duplicate packets OR (if GPS fail) don't write packets that haven't been located on screen
            if (referenceGlobalPacketID < gpsDataForDB.globalID && referenceGlobalPacketID >0 && gpsData.latitude != "0")
            {
                try
                {
                    sqlDb.populateDbFieldsGPS(gpsDataForDB);
                    referenceGlobalPacketID++;
                }
                catch (Exception)
                {
                    return true;
                }

                return true;
            }
            return true;
        }

        private bool writeToDatabase(XMLNodes myXmlDb, GPSPacket gpsDataForDB)
        {
            //don't write semi-filled packets to the DB
            if (duplicatePacketCounter != gpsDataForDB.packetID)
            {
                duplicatePacketCounter++;
                myXmlDb.populateDbFields(gpsDataForDB);

                return true;
            }
            return true;
        }
        #endregion


        #region Thread Update Methods

        private void dbLoggingThread_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
        }

        private void recvRawDataWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!recvRawDataWorker.CancellationPending)
            {
                //check the status of the system each time a new sentence received and inform the user
                statusTextBox.Clear();
                if (videoOutputRunning)
                {
                    statusTextBox.BackColor = System.Drawing.Color.ForestGreen;
                    statusTextBox.AppendText("Parsing in progress. No errors.");
                }
                else
                {
                    statusTextBox.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                    statusTextBox.AppendText("Parsing in progress with warning...");
                }

                    //function called to get all the incoming data and refresh the UI
                    updateUI(gpsData, sentenceBuffer);
                    rawBuffer = "";             //since earlier call is asynchronous, only clear the raw buffer once 100% sure that its data has reached the UI - which is now.

            }
            
        }

        //----------------------------Thread completion methods---------------------------
        
        private void recvRawDataWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (dbLoggingActive == true)
            {
                _waitHandleDatabase.Set();      //pings the parser thread to release the lock
                dbLoggingThread.CancelAsync(); //requests cancellation of the database thread
                _waitHandleParser.Set();        //parser informs the waiting db thread lock is released so it can close the file
            }

            try
            {
                statusTextBox.Clear();
                statusTextBox.AppendText("GPS parsing complete or interrupted.");
                rawLogFileTextBox.Text = "";
                stopButton.Enabled = false;
                startButton.Enabled = true;
                openFileButton.Enabled = true;
                testGpsGetButton.Enabled = true;
                testGpsGetButton.Text = "Connect to server";
                parseIsRunning = false;
                trayIconParsing.Text = "Logging stopped.";
                trayIconParsing.ShowBalloonTip(5, "GPS Logging stopped", "Logging stopped or interrupted. Open a new file to restart logging.", ToolTipIcon.Error);
            }
            catch (ObjectDisposedException)
            {
                parseIsRunning = false;
            }
           
        }

        private void dbLoggingThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           XMLNodes temp = new XMLNodes();

           //temp.closeXmlDbFile();
        }
        #endregion

        //-------------------------End threads------------------------------------

        #region UI Menu Element Methods

        private void trayIconParsing_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void dbUsernameTextbox_MouseClick(object sender, MouseEventArgs e)
        {
            if (dbUsernameTextbox.Text == "DB Username")
            {
                dbUsernameTextbox.Clear();
            }

        }

        private void dbPwdTextbox_TextChanged(object sender, EventArgs e)
        {
            if (dbPwdTextbox.Text == "password")
            {
                dbPwdTextbox.Clear();
            }
        }

        //ToolStrip Methods - to enable or disable the database or other settings---------------------------------

        private void enabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (enabledToolStripMenuItem.Checked == true)
            {
                MessageBox.Show("Database logging is on by default.","Logging already on",MessageBoxButtons.OK,MessageBoxIcon.Information);
                dbLoggingActive = true;
                creationOptionsToolStripMenuItem.Enabled = true;
                enabledToolStripMenuItem.Checked = true;
                disabledToolStripMenuItem.Checked = false;
            }
            else if (enabledToolStripMenuItem.Checked == false && disabledToolStripMenuItem.Checked == true && parseIsRunning == false)
            {
                MessageBox.Show("Logging to the database has been enabled.", "Logging turned on", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dbLoggingActive = true;
                creationOptionsToolStripMenuItem.Enabled = true;
                enabledToolStripMenuItem.Checked = true;
                disabledToolStripMenuItem.Checked = false;
            }
            else
            {
                MessageBox.Show("This change cannot be made whilst the parser is running. Stop it first, then try again.", "Cannot change setting now", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void disabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (disabledToolStripMenuItem.Checked == false && enabledToolStripMenuItem.Checked == true && parseIsRunning == false)
            {
                DialogResult quitDB = MessageBox.Show("This turns off logging to XML database, but keeps receiving incoming GPS data. Keep receiving data but stop logging to database?", "Stop database logging?", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (quitDB == DialogResult.Yes)
                {
                    MessageBox.Show("Logging to the database has been disabled.","Database logging off",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    dbLoggingActive = false;
                    disabledToolStripMenuItem.Checked = true;
                    enabledToolStripMenuItem.Checked = false;
                    creationOptionsToolStripMenuItem.Enabled = false;
                }
            }
            else if (disabledToolStripMenuItem.Checked == true && enabledToolStripMenuItem.Checked == false)
            {
                MessageBox.Show("Database logging is already disabled.","Database logging already off",MessageBoxButtons.OK,MessageBoxIcon.Information);
                creationOptionsToolStripMenuItem.Enabled = false;   //disable options for file creation
                dbLoggingActive = false;
            }
            else
            {
                MessageBox.Show("this change cannot be made whilst the parser is running. stop it first, then try again.", "cannot change setting now", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void openXMLInSeparateViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult userChoice;
            //warn user that opening the database whilst parsing could damage the XML file
            if (parseIsRunning == false)
            {
                openXmlInSystemViewer();
            }
            else
            {
                userChoice = MessageBox.Show("Warning: Opening the current database file whilst logging could corrupt the file. Continue opening the file?", "Be careful!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (userChoice == DialogResult.No)
                {
                    return;
                }
                else if (userChoice == DialogResult.Yes)
                {
                    openXmlInSystemViewer();
                }
            }
        }

        private void openXmlInSystemViewer()
        {
            try
            {
                //try open the file with the system viewer by pulling the name generated for this current session
                System.Diagnostics.Process.Start(XMLNodes.dbFileToOpenName);
            }
            catch (System.IO.FileNotFoundException f)
            {
                //if the file has been moved throw an exception
                MessageBox.Show("Failed to find the database file. Details: " + f.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidOperationException i)
            {
                //if the filename is null - occurs if the user tries to open the log when they havent logged anything yet
                MessageBox.Show("Cannot open file - have you actually logged anything yet? Details: " + i.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mapDisplayBrowserWindow_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void offlineCacheOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            offlineCacheOnlyToolStripMenuItem.Checked = true;
            onlineCacheToolStripMenuItem.Checked = false;
            GMaps.Instance.Mode = AccessMode.CacheOnly;
        }

        private void onlineCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            offlineCacheOnlyToolStripMenuItem.Checked = false;
            onlineCacheToolStripMenuItem.Checked = true;
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
        }

        private void googleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Warning: Accessing Google's Map data in this way may violate their terms of service. Proceed at your own risk?", "Use Google Maps?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                bingToolStripMenuItem.Checked = false;
                openStreetMapsToolStripMenuItem.Checked = false;
                googleToolStripMenuItem.Checked = true;
                mapPane.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
                trayIconParsing.ShowBalloonTip(3, "Mapping Info", "(At Own Risk) Map provider switched to Google", ToolTipIcon.Warning);
            }
        }

        private void openStreetMapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bingToolStripMenuItem.Checked = false;
            openStreetMapsToolStripMenuItem.Checked = true;
            googleToolStripMenuItem.Checked = false;
            mapPane.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            trayIconParsing.ShowBalloonTip(3, "Mapping Info", "Map provider switched to OpenStreetMaps", ToolTipIcon.Info);
        }

        private void bingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bingToolStripMenuItem.Checked = true;
            openStreetMapsToolStripMenuItem.Checked = false;
            googleToolStripMenuItem.Checked = false;
            mapPane.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
            trayIconParsing.ShowBalloonTip(3, "Mapping Info", "Map provider switched to Bing", ToolTipIcon.Info);
        }
        #endregion

        #region Form Shutdown

        //-------------------------Form shutdown method-----------
        //Warns the user before shutting down the app if a process is running
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult userQuit;
            if (parseIsRunning == true)
            {
                userQuit = MessageBox.Show("Quitting while parsing is running could corrupt the database; reccommend stopping the process first. Do you still want to force quit and risk losing data?", "Be careful! Forcefully quit?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (userQuit == DialogResult.Yes)
                {
                    parseIsRunning = false;
                    statusTextBox.Clear();
                    statusTextBox.AppendText("Stop requested, ending thread...");
                    recvRawDataWorker.CancelAsync(); //requests cancellation of the worker
                    _waitHandleDatabase.Set();      //pings the parser thread to release the lock
                    dbLoggingThread.CancelAsync(); //requests cancellation of the database thread
                    _waitHandleParser.Set();
                    e.Cancel = false;               //event shouldn't be cancelled i.e. we want to close the app
                }
                else
                {
                    //DO NOT close the app, return to caller
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                userQuit = MessageBox.Show("Close the program?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (userQuit != DialogResult.Yes)
                {
                    //tells the form caller we DO NOT want to close the app
                    e.Cancel = true;
                    return;
                }
            }


        }


        #endregion

        #region Database viewing
        private void openInappToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (parseIsRunning)
            {
                MessageBox.Show("It's recommended to cancel capture/parsing first to get latest data. You may open the database now but the latest data may not be displayed", "Concurrency Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            dbViewer dbView = new dbViewer();
            dbView.ShowDialog();
             

        }
        #endregion

        private void dbUsernameTextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void timeElapsedTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void mapPane_Load(object sender, EventArgs e)
        {

        }


    }


}
    


