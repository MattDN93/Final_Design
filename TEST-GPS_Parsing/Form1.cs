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
        bool debug = true;
        //*************************************
        public string logFilename;
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
        }

        private void openFileButton_Click(object sender, EventArgs e)
        {
                //open the dialog
                openLogDialog.ShowDialog();   
        }

        private void openLogDialog_FileOk(object sender, CancelEventArgs e)
        {

            MessageBox.Show("GPS NMEA log file ready. Click Start Tracking.");
            logFilename = openLogDialog.FileName;       //save the filename of the logfile
            startButton.Enabled = true;   
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            openFileButton.Enabled = false;
            startButton.Enabled = false;
            stopButton.Enabled = true;
            GPSPacket gpsData = new GPSPacket();
            gpsData.gpsLogfilename = logFilename;       //save filename to associated GPS class

            //set up the IO stream reader
            string sentenceBuffer;
            System.IO.StreamReader inputFile = new System.IO.StreamReader(gpsData.gpsLogfilename);

            //start reading the line
            rawLogFileTextBox.Text = "";
            //For simulating NMEA behaviour
            int count = 0;

            //Thread.BeginCriticalRegion();
            while ((sentenceBuffer = inputFile.ReadLine()) != null)
            {
                //This function updates the UI elements with the parsed NMEA data
                gpsData = parseSelection(sentenceBuffer, gpsData);
                //update the display
                updateUI(gpsData);
                //update the raw output textbox
                rawLogFileTextBox.AppendText(sentenceBuffer);
                sentenceBuffer = "";
                //FOR SIMULATION ONLY
                //Simulating 6 NMEA strings per second*******************
                if (debug)
                {
                    if (count % 6 == 0)
                    {
                        Thread.Sleep(1000);
                    }
                    
                }
                //**************************************
                count++;
            }
            //Thread.EndCriticalRegion();
            MessageBox.Show("Done!");
            inputFile.Close();
        }

        private void updateUI(GPSPacket gpsDataForUI)
        {
            //This takes the gpsData object and populates all the fields with updated values (if any)
            //Monitoring
            packetIDTextBox.Text = gpsDataForUI.ID.ToString();
            //GPS Core Data
            latitudeTextBox.Clear();
            latitudeTextBox.AppendText(gpsDataForUI.latitude.ToString());
            longitudeTextBox.Text = gpsDataForUI.longitude.ToString();
            altitudeTextBox.Text = gpsDataForUI.altitude.ToString();
            //Vehicle Properties
            speedKnotsTextBox.Text = gpsDataForUI.grspd_k.ToString();
            speedKphTextBox.Text = gpsDataForUI.grspd_kph.ToString();
            headDegTextBox.Text = gpsDataForUI.trkangle.ToString();

            //Fix Information
            satsViewTextBox.Clear();
            satsViewTextBox.AppendText(gpsDataForUI.numsats.ToString());
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

            if (sentenceBuffer.Contains("GPRMC"))
            {
                updatedGpsData = parseGPRMC(sentenceBuffer, gpsData);
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
                return updatedGpsData;
            }
            else
            {
                return updatedGpsData;     //we don't consider all the other NMEA strings
            }

        }

        private GPSPacket parseGPVTG(string sentenceBuffer, GPSPacket gpsData)
        {
            throw new NotImplementedException();
            return gpsData;
        }

        private GPSPacket parseGPGGA(string sentenceBuffer, GPSPacket gpsData)
        {
            throw new NotImplementedException();
            return gpsData;
        }

        private GPSPacket parseGPRMC(string sentenceBuffer, GPSPacket gpsData)
        {
            int sectionCount=0;                 //count for subsections of the GPRMC string
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
                                gpsData.friendlyFlagString(gpsData.fixtype);   //get the friendly string for the fix
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
            Thread.CurrentThread.Abort();
            stopButton.Enabled = false;
            startButton.Enabled = true;
            openFileButton.Enabled = true;
        }
    }
}
