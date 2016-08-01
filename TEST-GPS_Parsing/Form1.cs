using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
        int packetID = 0;               //session-based packet ID, one packet is equivalent to a set of NMEA strings starting with GPRMC

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
                recvRawDataWorker.RunWorkerAsync();         //starts the data receiving in the background
            }
            else
            {
                recvRawDataWorker.CancelAsync();
                statusTextBox.Clear();
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
                        gpsData = parseSelection(sentenceBuffer, gpsData);
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



        private void recvRawDataWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

                statusTextBox.Clear();
                statusTextBox.AppendText("Parsing in progress. No errors.");
                updateUI(gpsData, sentenceBuffer);

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
            headDegTextBox.AppendText( gpsDataForUI.trkangle);
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

        //-------------------------PARSING SELECTION METHODS----------------------------

        private GPSPacket parseSelection(string sentenceBuffer, GPSPacket gpsData)
        {
            GPSPacket updatedGpsData = gpsData; //sets up a new object which methods return with updated values
            if (!(sentenceBuffer.StartsWith("$")))
            {
                return updatedGpsData;     //the sentence is invalid 
            }

            if (sentenceBuffer.Contains("GPRMC")) //We assume that each "packet" of sentences begins with a GPRMC hence update the packet ID each time a GPRMC is found!
            {
                updatedGpsData.ID = packetID;
                packetID++;
                updatedGpsData = parseGPRMC(sentenceBuffer, gpsData);

                //do some last-minute formatting to the fields based on known issues
                updatedGpsData.date = updatedGpsData.date.TrimEnd('/'); //remove trailing "/"
                if (updatedGpsData.time.StartsWith("0") || updatedGpsData.longitude.StartsWith("0") || updatedGpsData.latitude.StartsWith("0"))
                {
                    //updatedGpsData.time = updatedGpsData.time.TrimStart('0'); //remove leading zeroes
                    updatedGpsData.latitude = updatedGpsData.latitude.TrimStart('0');
                    updatedGpsData.longitude = updatedGpsData.longitude.TrimStart('0');
                }

                //convert the track angle in degrees true to a cardinal heading
                trackToCardinal(updatedGpsData);

                //convert the time from a horrible string to something nice
                timeNiceDisplay(updatedGpsData);

                return updatedGpsData;
            }
            else if (sentenceBuffer.Contains("GPGGA"))
            {
                updatedGpsData = parseGPGGA(sentenceBuffer, gpsData);
                return updatedGpsData;
            }
            else if (sentenceBuffer.Contains("GPVTG"))
            {
                updatedGpsData = parseGPVTG(sentenceBuffer, gpsData);
                //trim off leading zeroes from the speed
                if (updatedGpsData.grspd_kph.StartsWith("0"))
                {
                    updatedGpsData.grspd_kph = updatedGpsData.grspd_kph.TrimStart('0');
                }
                return updatedGpsData;
            }
            else
            {
                return updatedGpsData;     //we don't consider all the other NMEA strings
            }

        }

        /*----------------Time and Date Parsing Method 
         Input: GPSData object
         Output: Time and date parsed correctly 
         Objective: parse time and date from raw numbers and letters to a nice format & to computable values. Assumed time HHMMSS.SSS and date DD/MM/YY
             */

        private void timeNiceDisplay(GPSPacket updatedGpsData)
        {
            string newDateTime; //immutable string so make a new one to copy into
            string fixTime;     //new temp string for date
            string format = "dd/MM/yyyy HH:mm:ss.fff"; //set date time format
           
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture; //provider for display of date and time SA style

            newDateTime = updatedGpsData.date.Insert(6, "20");  //assume 2 digit date >2000
            //---for UI
            updatedGpsData.date = newDateTime;
            fixTime = updatedGpsData.time.Insert(2, ":").Insert(5, ":");
            updatedGpsData.time = fixTime;
            //--end UI formatting

            //now format to a DateTime object for later calc
            newDateTime += " " + fixTime;

            updatedGpsData.dt =  DateTime.ParseExact(newDateTime,format,provider); //this will be used for computation
        }

        private GPSPacket trackToCardinal(GPSPacket updatedGpsData)
        {
            //each degree direction corresponds to some cardinal direction
            //first convert trkangle to a double to evaluate it
            double trkAngle_double;
            bool result = Double.TryParse(updatedGpsData.trkangle, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out trkAngle_double);

            if (result == true)
            {
                if ((348.75 >= trkAngle_double && trkAngle_double <= 360)||((0 >= trkAngle_double && trkAngle_double <= 33.75)))
                {
                    updatedGpsData.cardAngle = "N";
                }
                else if ((33.75 > trkAngle_double && trkAngle_double <= 78.75))
                {
                    updatedGpsData.cardAngle = "NE";
                }
                else if ((78.75 > trkAngle_double && trkAngle_double <= 101.25))
                {
                    updatedGpsData.cardAngle = "E";
                }
                else if ((101.25 > trkAngle_double && trkAngle_double <= 168.75))
                {
                    updatedGpsData.cardAngle = "SE";
                }
                else if ((168.75 > trkAngle_double && trkAngle_double <= 213.75))
                {
                    updatedGpsData.cardAngle = "S";
                }
                else if ((213.75 > trkAngle_double && trkAngle_double <= 258.75))
                {
                    updatedGpsData.cardAngle = "SW";
                }
                else if ((258.75 > trkAngle_double && trkAngle_double <= 303.75))
                {
                    updatedGpsData.cardAngle = "W";
                }
                else if ((303.75 > trkAngle_double && trkAngle_double < 348.75))
                {
                    updatedGpsData.cardAngle = "NW";
                }

            }

            return updatedGpsData;
        }

        private GPSPacket parseGPVTG(string sentenceBuffer, GPSPacket gpsData)
        {
            int sectionCount = 0;
            string subField = "";
            foreach (var item in sentenceBuffer)
            {
                if (item.ToString() != "*")      //the asterisk specifies the end of line
                {
                    if (item.ToString() == ",")  //increment counter for next segment
                    {
                        sectionCount++;
                        subField = "";
                    }

                    switch (sectionCount)       //store the fields based on the expected section
                    {
                        //GPVTG section 7 is speed in KPH; we need the speed in KPH
                        case 7:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.grspd_kph = subField; //parse double to a string for display
                            }
                            break;
                        default: break;
                    }
                }
            }
            return gpsData;
        }

        private GPSPacket parseGPGGA(string sentenceBuffer, GPSPacket gpsData)
        {
            int sectionCount = 0;
            string subField = "";
            foreach (var item in sentenceBuffer)
            {
                if (item.ToString() != "*")      //the asterisk specifies the end of line
                {
                    if (item.ToString() == ",")  //increment counter for next segment
                    {
                        sectionCount++;
                        subField = "";
                    }

                    switch (sectionCount)       //store the fields based on the expected section
                    {
                        //GPGGA section 5 is is the fix quality
                        case 6:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.fixqual = subField; //add the cardinal heading to the latitude
                                gpsData.friendlyFlagString(gpsData.fixtype, gpsData.fixqual); //get the friendly name of the fix quality
                            }
                            break;
                        //GGA section 7 is the number of satellites in view
                        case 7:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.numsats = subField;
                            }
                            break;
                        //section 8 is the accuracy of the GPS fix
                        case 8:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.accuracy = subField;
                            }
                            break;
                        //section 9 is the altitude ASL
                        case 9:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.altitude = subField;
                            }
                            break;
                        default: break;
                    }
                }
            }
            return gpsData;
        }

        private GPSPacket parseGPRMC(string sentenceBuffer, GPSPacket gpsData)
        {
            int sectionCount=0;                 //count for subsections of the GPRMC string
            int dateCtr = 0;                    //for formatting the date nicely
            gpsData.date = "";
            string subField = ""; 
            foreach (var item in sentenceBuffer)
            {
                if (item.ToString() != "*")      //the asterisk specifies the end of line
                {
                    if (item.ToString() == ",")  //increment counter for next segment
                    {
                        sectionCount++;
                        subField = "";
                    }

                    switch (sectionCount)       //store the fields based on the expected section
                    {
                        case 0: break;
                        //GPRMC section 1 = time in HHMMSS
                        case 1:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.time = subField;
                            }
                            break;
                        //GPRMC section 2 = fix, A or V
                        case 2:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.fixtype = subField;    //pick the 2nd char ",A" for example
                                gpsData.friendlyFlagString(gpsData.fixtype,gpsData.fixqual);   //get the friendly string for the fix
                            }
                            break;
                        //GPRMC section 3 = Latitude DDMMSS.SSS
                        case 3:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                               subField += item;
                               gpsData.latitude = subField ; //parse double to a string for display
                            }
                            break;
                        //GPRMC section 4 = Latitude Cardinal Heading
                        case 4:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.latitude += subField; //add the cardinal heading to the latitude
                            }
                            break;
                        //GPRMC section 5 = Longitude DDMMSS.SSS
                        case 5:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.longitude = subField; //parse double to a string for display
                            }
                            break;
                        //GPRMC section 6 = Longitude Cardinal Heading
                        case 6:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.longitude += subField; //add the cardinal heading to the longitude
                            }
                            break;
                        //GPRMC section 7 = Ground speed in Knots
                        case 7:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.grspd_k = subField; 
                            }
                            break;
                        //GPRMC section 8 = Track angle in degrees true
                        case 8:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.trkangle = subField; 
                            }
                            break;
                        //GPRMC section 9 = Date ,DD/MM/YY
                        case 9:
                            
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                dateCtr++;
                                if (dateCtr % 2 == 0)
                                {
                                    subField = item.ToString();
                                    gpsData.date += subField;
                                    gpsData.date += "/"; //format the date nicely 
                                }
                                else
                                {
                                    subField = item.ToString();
                                    gpsData.date += subField;
                                    
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    break;
                }
                
            }

            //return completed packet
            return gpsData;

        }

        //------------------------------------------------------------------

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
        }
    }
}
