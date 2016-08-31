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
        //CameraSettings camParameter = new CameraSettings();         //instantiate new camParameter object
        //video GPS coordinate extents
        double[] upperLeftCoords = new double[2];       //[0] = latitude top left; [1] = longitude top left
        double[] outerLimitCoords = new double[2];      //[0] = longitude top right; [1] = latitude bottom left
        int drawModeChoice = -1;

        //video parameters
        //The index of video mode defines the selected item. 0=Video on PC 1=Webcam
        //The index of draw mode defines the selected item. 0=Random 1=Ordered 2=Tracking
        public int videoSource;
        public int drawMode;
        public string filenameToOpen;

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
                    case 1: setVideoParameters(0, drawModeChoice);break;   //using webcam so no filename needed
                    default:
                        break;
                }

                //now start the process!
                try
                {
                    startNewVideoConsole();
                }
                catch (NotSupportedException ns)
                {
                    MessageBox.Show("The video stream start failed. Details: " + ns.Message, "Something happened", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                this.Close();           //close the form and open the next one
            }
        }

        private void chooseVideoFileFialog_FileOk(object sender, CancelEventArgs e)
        {
            //set the parameters of the file and arguments we'll pass 
            //if here, we assume the user is opening a file
            setVideoParameters(1, drawModeChoice,chooseVideoFileFialog.FileName.ToString());
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
                setBoundsResult = setCameraCoordBounds(latUpperLeft.Text, longUpperLeft.Text, longUpperRight.Text, latBottomLeft.Text);
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


        //Camera modification methods---------------------------------------


        public bool setCameraCoordBounds(string _upperLeft0, string _upperLeft1, string _upperRight0, string _bottomLeft1)
        {
            //save the co-ordinate limits to the relative variables
            bool parseSuccess = false;
            int parseSuccessCount = 0;

            //convert the text to double and check
            parseSuccess = double.TryParse(_upperLeft0, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out upperLeftCoords[0]);
            if (parseSuccess) { parseSuccessCount++; } else { parseSuccess = false; }
            parseSuccess = double.TryParse(_upperLeft1, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out upperLeftCoords[1]);
            if (parseSuccess) { parseSuccessCount++; } else { parseSuccess = false; }
            parseSuccess = double.TryParse(_upperRight0, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out outerLimitCoords[0]);
            if (parseSuccess) { parseSuccessCount++; } else { parseSuccess = false; }
            parseSuccess = double.TryParse(_bottomLeft1, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out outerLimitCoords[1]);
            if (parseSuccess) { parseSuccessCount++; } else { parseSuccess = false; }

            //make sure all co-ordinates were parsed OK
            if (parseSuccessCount == 4)
            {
                parseSuccessCount = 0;
                Console.Write("Parsed camera bound co-ordinates OK.");
                return true;
            }
            else
            {
                Console.Write("One or more camera bound co-ordinates couldn't be parsed.");
                parseSuccessCount = 0;
                return false;
            }
        }

        public void setVideoParameters(int _videoSource, int _drawMode, string _filenameToOpen = "false")
        {
            if (_videoSource > -1)
            {
                videoSource = _videoSource;
                drawMode = _drawMode;
            }
            else
            {
                Console.Write("Failed to set draw mode choice.");
                return;
            }


            //only set the filename if the source is PC video
            if (_videoSource == 1)
            {
                filenameToOpen = _filenameToOpen;
            }
            else
            {
                filenameToOpen = "false";
            }

            Console.Write("Set video parameters successfully.");
        }

        public void startNewVideoConsole()
        {
            VideoOutputWindow vo = new VideoOutputWindow();
            vo.fileName = filenameToOpen;       //set the filename to open on the other form
            vo.drawMode = drawMode;             //set the draw mode on the other form
            vo.captureChoice = videoSource;     //set the video source on the other form
            vo.TopMost = true;
            vo.Show();
            

            //now actually launch the process!
            //Process videoCaptureProcess = new Process();
            //videoCaptureProcess.StartInfo.FileName = "C:\\Users\\Matt\\Documents\\Source\\Final_Design\\x64\\Debug\\VideoStreamCPlusPlus.exe";

            /*parse through the arguments from the other app
            Arg[0] = process filename
            Arg[1] = lat-topLeft
            Arg[2] = long-topLeft
            Arg[3] = long-topRight
            Arg[4] = lat-botLeft
            Arg[5] = Picture mode; (0 = Internal Webcam; 1 = specify a file)
            Arg[6] = Draw mode; (0 = Random points; 1 = Ordered line)
            Arg[7] = file to open
            */
            /*videoCaptureProcess.StartInfo.Arguments = upperLeftCoords[0].ToString() + " " +
                                                      upperLeftCoords[1].ToString() + " " +
                                                      outerLimitCoords[0].ToString() + " " +
                                                      outerLimitCoords[1].ToString() + " " +
                                                      videoSource.ToString() + " " +
                                                      drawMode.ToString() + " " +
                                                      filenameToOpen;
            //videoCaptureProcess.Start();
            */
        }
    }
}

