using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TEST_GPS_Parsing
{
    public partial class CameraBoundsSetup : Form
    {
        CameraSettings camParameter = new CameraSettings();         //instantiate new camParameter object
        int drawModeChoice = -1;

        public CameraBoundsSetup()
        {
            InitializeComponent();
        }

        private void setExtentsButton_Click(object sender, EventArgs e)
        {
            //warn user if setting not filled
            if (drawModeChoiceComboBox.SelectedIndex == -1 || vidSourceChoiceComboBox.SelectedIndex == -1 )
            {
                MessageBox.Show("Please select a capture mode and/or a draw mode before continuing.", "Choose settings first!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                
                //The index of draw mode defines the selected item. 0=Random 1=Ordered 2=Tracking
                switch (drawModeChoiceComboBox.SelectedIndex)
                {
                    case 0: drawModeChoice = 0;break;
                    case 1: drawModeChoice = 1;break;
                    case 2: drawModeChoice = 2;break;
                    default: drawModeChoice = -1;
                        break;
                }

                //The index of video mode defines the selected item. 0=Video on PC 1=Webcam
                switch (vidSourceChoiceComboBox.SelectedIndex)
                {
                    case 0: chooseVideoFileFialog.ShowDialog();break;                   //show user file dialog first
                    case 1: camParameter.setVideoParameters(0, drawModeChoice);break;   //using webcam so no filename needed
                    default:
                        break;
                }

                //now start the process!
                camParameter.startNewVideoConsole();


            }
        }

        private void chooseVideoFileFialog_FileOk(object sender, CancelEventArgs e)
        {
            //set the parameters of the file and arguments we'll pass 
            //if here, we assume the user is opening a file
            camParameter.setVideoParameters(1, drawModeChoice,chooseVideoFileFialog.FileName.ToString());
        }

        private void clearFieldsButton_Click(object sender, EventArgs e)
        {
            latBottomLeft.Clear();
            latUpperLeft.Clear();
            latUpperRight.Clear();
            longBottomLeft.Clear();
            longUpperLeft.Clear();
            longUpperRight.Clear();

        }

        private void goBackButton_Click(object sender, EventArgs e)
        {
            checkFieldsTimer.Stop();        //stop and dispose the timer
            checkFieldsTimer.Dispose();
            this.Close();
        }

        private void CameraBoundsSetup_Load(object sender, EventArgs e)
        {
            //checkFieldsTimer = new Timer(); //instantiate the timer
            checkFieldsTimer.Start();       //start the timer to check status of the entry of points
        }

        private void checkFieldsTimer_Tick(object sender, EventArgs e)
        {
            
            if (longUpperLeft.Text == "" || latUpperLeft.Text == "" || latBottomLeft.Text == "" || longUpperRight.Text == "" )
            {
                setExtentsButton.Enabled = false;      //keep disabled until all fields are filled so they can't start prematurely
                camViewStatusTextBox.Clear();           //clears from last tick
                camViewStatusTextBox.BackColor = Color.Orange;
                camViewStatusTextBox.AppendText("Fields empty:");   //append which fields are still empty until they're fixed
                if (longUpperLeft.Text == "")
                {
                    camViewStatusTextBox.AppendText(" Long. Step 1;");
                }
                if (latUpperLeft.Text == "")
                {
                    camViewStatusTextBox.AppendText(" Lat. Step 1;");
                }
                if (longUpperRight.Text == "")
                {
                    camViewStatusTextBox.AppendText(" Long. Step 2;");
                }
                if (latBottomLeft.Text == "")
                {
                    camViewStatusTextBox.AppendText(" Lat. Step 3;");
                }
            }
            else
            {
                camViewStatusTextBox.Clear();
                camViewStatusTextBox.BackColor = Color.LightSeaGreen;
                camViewStatusTextBox.AppendText("Fields filled. Ready to start video.");

                //save all values to vars - if this process fails, inform user and don't let them start
                bool setBoundsResult = false;
                setBoundsResult = camParameter.setCameraCoordBounds(latUpperLeft.Text, longUpperLeft.Text, longUpperRight.Text, latBottomLeft.Text);
                if (setBoundsResult == true)
                {
                    setExtentsButton.Enabled = true;        //keep start button enabled if fields contain something
                }
                else
                {
                    camViewStatusTextBox.Clear();
                    camViewStatusTextBox.BackColor = Color.PaleVioletRed;
                    camViewStatusTextBox.AppendText("Failed to set fields - have you used the right format?");
                    setExtentsButton.Enabled = false;
                    return;
                }
                
         
              

                

            }

            latUpperRight.Text = latUpperLeft.Text;
            longBottomLeft.Text = longUpperLeft.Text;
        }


    }
}
