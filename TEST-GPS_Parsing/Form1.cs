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

            Thread.BeginCriticalRegion();
            while ((sentenceBuffer = inputFile.ReadLine()) != null)
            {
                //This function updates the UI elements with the parsed NMEA data
                parseSelection(sentenceBuffer, gpsData);

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
            Thread.EndCriticalRegion();
            MessageBox.Show("Done!");
            inputFile.Close();
        }

        //-------------------------PARSING SELECTION METHODS----------------------------

        private void parseSelection(string sentenceBuffer, GPSPacket gpsData)
        {
            if (!(sentenceBuffer.StartsWith("$")))
            {
                return;     //the sentence is invalid 
            }

            if (sentenceBuffer.Contains("GPRMC"))
            {
                parseGPRMC(sentenceBuffer, gpsData);
            }
            else if (sentenceBuffer.Contains("GPGGA"))
            {
                parseGPGGA(sentenceBuffer, gpsData);
            }
            else if (sentenceBuffer.Contains("GPVTG"))
            {
                parseGPVTG(sentenceBuffer, gpsData);
            }
            else
            {
                return;     //we don't consider all the other NMEA strings
            }

        }

        private void parseGPVTG(string sentenceBuffer, GPSPacket gpsData)
        {
            throw new NotImplementedException();
        }

        private void parseGPGGA(string sentenceBuffer, GPSPacket gpsData)
        {
            throw new NotImplementedException();
        }

        private void parseGPRMC(string sentenceBuffer, GPSPacket gpsData)
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
                            subField += item;
                            gpsData.time = subField;
                            break;
                        case 2:
                            subField += item;
                            gpsData.fixtype = subField;    //pick the 2nd char ",A" for example
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
