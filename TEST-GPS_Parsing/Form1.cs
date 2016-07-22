using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TEST_GPS_Parsing
{
    public partial class Form1 : Form
    {
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
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            GPSPacket gpsData = new GPSPacket();
            gpsData.gpsLogfilename = logFilename;       //save filename to associated GPS class

            //set up the IO stream reader
            string sentenceBuffer;
            System.IO.StreamReader inputFile = new System.IO.StreamReader(gpsData.gpsLogfilename);

            //start reading the line
            rawLogFileTextBox.Text = "";
            while ((sentenceBuffer = inputFile.ReadLine()) != null)
            {
                rawLogFileTextBox.AppendText(sentenceBuffer);
                sentenceBuffer = "";
                //FOR SIMULATION ONLY
                if (true)
                {

                }
            }
            MessageBox.Show("Done!");
        }
    }
}
