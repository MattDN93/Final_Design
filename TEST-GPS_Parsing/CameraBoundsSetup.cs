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
        #region Initialization objects and vars
        //CameraSettings camParameter = new CameraSettings();         //instantiate new camParameter object
        //video GPS coordinate extents
        private double[] upperLeftCoords = new double[2];       //[0] = latitude top left; [1] = longitude top left
        private double[] outerLimitCoords = new double[2];      //[0] = longitude top right; [1] = latitude bottom left
        int drawModeChoice = -1;

        //video parameters
        //The index of video mode defines the selected item. 0=Video on PC 1=Webcam
        //The index of draw mode defines the selected item. 0=Random 1=Ordered 2=Tracking
        public int videoSource;
        public int drawMode;
        public string filenameToOpen;

        VideoOutputWindow vo;

        //-------SIMULATION
        public static int DRAW_MODE_RANDOM = 0;
        public static int DRAW_MODE_ORDERED = 1;
        public static int DRAW_MODE_TRACKING = 2;
        public static int DRAW_MODE_REVOBJTRACK = 3;

        public CameraBoundsSetup(VideoOutputWindow incoming_vo)
        {
            InitializeComponent();
            vo = incoming_vo;           //create a VO object for the parser to use
        }
        #endregion 

        #region Form load and Verify Methods

        private void CameraBoundsSetup_Load(object sender, EventArgs e)
        {
            //checkFieldsTimer = new Timer(); //instantiate the timer
            checkFieldsTimer.Start();       //start the timer to check status of the entry of points
            vo.initCamStreams();
        }

        private void checkFieldsTimer_Tick(object sender, EventArgs e)
        {

            //check default camera status and display on UI
            if (vo != null)         {    centreCamStatusLabel.Text = "Ready / Available";           }
            else                    {    centreCamStatusLabel.Text = "Not Available";               }
            if (vo.cscLeft != null && vo.cscLeft.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight) != 0) {    leftCamStatusLabel.Text = "Ready / Available";             }
            else                    {    leftCamStatusLabel.Text = "Not Available";  }
            if (vo.cscRight != null && vo.cscRight.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight) != 0) {    rightCamStatusLabel.Text = "Ready / Available";            }
            else                    {    rightCamStatusLabel.Text = "Not Available"; }



            if (longUpperLeftTextbox.Text == "" || latUpperLeftTextbox.Text == "" || latBottomLeftTextbox.Text == "" || longUpperRightTextbox.Text == "" )
            {
                setExtentsButton.Enabled = false;      //keep disabled until all fields are filled so they can't start prematurely
                camViewStatusTextBox.Clear();           //clears from last tick
                camViewStatusTextBox.BackColor = Color.Orange;
                camViewStatusTextBox.AppendText("Fields empty:");   //append which fields are still empty until they're fixed
                if (longUpperLeftTextbox.Text == "")
                {
                    camViewStatusTextBox.AppendText(" Long. Step 1;");
                }
                if (latUpperLeftTextbox.Text == "")
                {
                    camViewStatusTextBox.AppendText(" Lat. Step 1;");
                }
                if (longUpperRightTextbox.Text == "")
                {
                    camViewStatusTextBox.AppendText(" Long. Step 2;");
                }
                if (latBottomLeftTextbox.Text == "")
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
                setBoundsResult = setCameraCoordBounds(latUpperLeftTextbox.Text, longUpperLeftTextbox.Text, longUpperRightTextbox.Text, latBottomLeftTextbox.Text);
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

            latUpperRightTextbox.Text = latUpperLeftTextbox.Text;
            longBottomLeftTextbox.Text = longUpperLeftTextbox.Text;
        }
        #endregion

        #region UI and button methods
        private void setExtentsButton_Click(object sender, EventArgs e)
        {
            //warn user if setting not filled
            if (drawModeChoiceComboBox.SelectedIndex == -1 || vidSourceChoiceComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a capture mode and/or a draw mode before continuing.",
                    "Choose settings first!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
            }
            else
            {

                //The index of draw mode defines the selected item. 0=Random 1=Ordered 2=Tracking 3=object tracking onscreen
                switch (drawModeChoiceComboBox.SelectedIndex)
                {
                    case 0: drawModeChoice = DRAW_MODE_RANDOM; break;
                    case 1: drawModeChoice = DRAW_MODE_ORDERED; break;
                    case 2: drawModeChoice = DRAW_MODE_TRACKING; break;
                    case 3: drawModeChoice = DRAW_MODE_REVOBJTRACK; break;
                    default:
                        drawModeChoice = -1;
                        break;
                }

                //The index of video mode defines the selected item. 0=Video on PC 1=Webcam
                switch (vidSourceChoiceComboBox.SelectedIndex)
                {
                    case 0: chooseVideoFileFialog.ShowDialog(); break;                   //show user file dialog first
                    case 1: setVideoParameters(0, drawModeChoice); break;   //using webcam so no filename needed
                    case 2: setVideoParameters(1, drawModeChoice); break;   //using webcam and GPS values
                    default:
                        break;
                }

                //now start the process!
                try
                {
                    bool startedOK = false;
                    startedOK = startNewVideoConsole();
                    if (startedOK)
                    {
                        this.Hide();           //close the form and open the next one
                    }

                }
                catch (NotSupportedException ns)
                {
                    MessageBox.Show("The video stream start failed. Details: " + ns.Message,
                        "Something happened", MessageBoxButtons.OK, MessageBoxIcon.Error,MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                }



            }
        }

        private void chooseVideoFileFialog_FileOk(object sender, CancelEventArgs e)
        {
            //set the parameters of the file and arguments we'll pass 
            //if here, we assume the user is opening a file
            setVideoParameters(1, drawModeChoice, chooseVideoFileFialog.FileName.ToString());
        }

        private void clearFieldsButton_Click(object sender, EventArgs e)
        {
            latBottomLeftTextbox.Clear();
            latUpperLeftTextbox.Clear();
            latUpperRightTextbox.Clear();
            longBottomLeftTextbox.Clear();
            longUpperLeftTextbox.Clear();
            longUpperRightTextbox.Clear();

        }

        private void goBackButton_Click(object sender, EventArgs e)
        {
            checkFieldsTimer.Stop();        //stop and dispose the timer
            checkFieldsTimer.Dispose();
            vo.ReleaseData();               //dispose of the capture methods
            this.Close();
        }
        #endregion

        #region Camera modification methods
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

        public bool startNewVideoConsole()
        {
            vo.fileName = filenameToOpen;       //set the filename to open on the other form
            vo.drawMode_Overlay = drawMode;             //set the draw mode on the other form
            vo.captureChoice = videoSource;     //set the video source on the other form

            //pass parameters of the outer limits to the other UI
            vo.upperLeftBound[0] = upperLeftCoords[0];
            vo.upperLeftBound[1] = upperLeftCoords[1];
            vo.outerLimitBound[0] = outerLimitCoords[0];
            vo.outerLimitBound[1] = outerLimitCoords[1];

            if (vo.Visible == true)
            {
                MessageBox.Show("Only one instance of the video capture window may be open at a time. Close the other first.", 
                    "Instance already running!", MessageBoxButtons.OK, MessageBoxIcon.Warning,MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                return false;
            }
            else
            {
                try
                {
                    vo.TopMost = true;      //set the window to the top and make it visible
                    vo.Visible = true;
                    vo.Show();

                }
                catch (ObjectDisposedException d)
                {
                    vo.fileName = filenameToOpen;       //set the filename to open on the other form
                    vo.drawMode_Overlay = drawMode;             //set the draw mode on the other form
                    vo.captureChoice = videoSource;     //set the video source on the other form

                    //pass parameters of the outer limits to the other UI
                    vo.upperLeftBound[0] = upperLeftCoords[0];
                    vo.upperLeftBound[1] = upperLeftCoords[1];
                    vo.outerLimitBound[0] = outerLimitCoords[0];
                    vo.outerLimitBound[1] = outerLimitCoords[1];
                    vo.Focus();      //set the window to the top and make it visible
                    vo.Visible = true;
                    vo.Show();
                }
                return true;
            }


        }
        #endregion


        private void drawModeChoiceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

