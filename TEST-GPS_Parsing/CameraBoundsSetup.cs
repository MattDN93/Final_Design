using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;

namespace TEST_GPS_Parsing
{
    public partial class CameraBoundsSetup : Form
    {
        #region Initialization objects and vars
        //video GPS coordinate extents
        private double[] upperLeftCoords_CB = new double[2];       //[0] = latitude top left; [1] = longitude top left
        private double[] outerLimitCoords_CB = new double[2];      //[0] = longitude top right; [1] = latitude bottom left

        //----------COPY OF VIDEO PARAMETERS--------------
        /*
         These parameters are local to this class instance, they are requested by the video output class
         at call, where this class sets up all the functions, and copies them back to the video output
         before being disposed.

         This can happen as many times as needed to update the camera data / settings

             */
        //The index of video mode defines the selected item. 0=Video on PC 1=Webcam
        //The index of draw mode defines the selected item. 0=Random 1=Ordered 2=Tracking
        public int videoSource_CB = -1;
        public int drawMode_CB = -1;
        public string filenameToOpen_CB;

        //-----------Capture Var Copies--------------------
        public Capture cscCentre_CB;
        public Capture cscLeft_CB;
        public Capture cscRight_CB;
        public Capture camStreamCapture_CB;

        //-------SIMULATION
        public static int DRAW_MODE_RANDOM = 0;
        public static int DRAW_MODE_ORDERED = 1;
        public static int DRAW_MODE_TRACKING = 2;
        public static int DRAW_MODE_REVOBJTRACK = 3;

        //-------flags for program readiness
        bool coordsReady;
        bool camTrackReady;
        bool ipCamReady;


        public CameraBoundsSetup(VideoOutputWindow incoming_vo)
        {
            InitializeComponent();
        }
        #endregion 

        #region Form load and Verify Methods

        private void CameraBoundsSetup_Load(object sender, EventArgs e)
        {
            //checkFieldsTimer = new Timer(); //instantiate the timer
            checkFieldsTimer.Start();       //start the timer to check status of the entry of points
            coordsReady = false;
            camTrackReady = false;
            ipCamReady = false;
        }

        private void checkFieldsTimer_Tick(object sender, EventArgs e)
        {
            //run the readiness check to enable/disable the start button
            checkReadiness();

            //if user has chosen internal cameras, try to connect them automagically
            if (vidSourceChoiceComboBox.SelectedIndex == 1)
            {
                //disable IP cam setting section
                checkIpAddrButton.Enabled = false;
                camLeftIpTextBox.Enabled = false;
                camRightIpTextBox.Enabled = false;
                camCentreIpTextBox.Enabled = false;

                //allow the start

                //disable this if using local cams since they'll refresh automagically :P
                refreshStatusButton.Enabled = false;

                //if any capture objects haven't been setup yet, run the init routine
                if (cscCentre_CB == null || cscLeft_CB == null || cscRight_CB == null)
                {
                    initCamStreams();
                }
            }
            else if (vidSourceChoiceComboBox.SelectedIndex == 2)
            {
                //TODO: Implement IP camera initialisation
                checkIpAddrButton.Enabled = true;
                camLeftIpTextBox.Enabled = true;
                camRightIpTextBox.Enabled = true;
                camCentreIpTextBox.Enabled = true;
            }

            //check default camera status and display on UI
            if (cscCentre_CB != null && cscCentre_CB.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight) != 0) { centreCamStatusLabel.Text = "Ready / Available"; }
            else { centreCamStatusLabel.Text = "Not Available"; camViewStatusTextBox.Text = "Not all cameras available."; }
            if (cscLeft_CB != null && cscLeft_CB.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight) != 0) { leftCamStatusLabel.Text = "Ready / Available"; }
            else { leftCamStatusLabel.Text = "Not Available"; camViewStatusTextBox.Text = "Not all cameras available."; }
            if (cscRight_CB != null && cscRight_CB.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight) != 0) { rightCamStatusLabel.Text = "Ready / Available"; }
            else { rightCamStatusLabel.Text = "Not Available"; camViewStatusTextBox.Text = "Not all cameras available."; }


            if (longUpperLeftTextbox.Text == "" || latUpperLeftTextbox.Text == "" || latBottomLeftTextbox.Text == "" || longUpperRightTextbox.Text == "" )
            {
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

                coordsReady = false;                    //ensures start button stays disabled
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
                    coordsReady = true;
                }
                else
                {
                    camViewStatusTextBox.Clear();
                    camViewStatusTextBox.BackColor = Color.PaleVioletRed;
                    camViewStatusTextBox.AppendText("Failed to set fields - have you used the right format?");
                    coordsReady = false;
                    return;
                }

            }

            latUpperRightTextbox.Text = latUpperLeftTextbox.Text;
            longBottomLeftTextbox.Text = longUpperLeftTextbox.Text;

            //now check the status of the dropdowns
            if (drawModeChoiceComboBox.SelectedIndex == -1)
            {
                drawModeChoiceComboBox.BackColor = Color.Red;           //if user hasn't selected anything highlight this
                camTrackReady = false;
            }
            else
            {
                drawModeChoiceComboBox.BackColor = Color.WhiteSmoke;
            }  
            if (vidSourceChoiceComboBox.SelectedIndex == -1)
            {
                vidSourceChoiceComboBox.BackColor = Color.Red;          //highlight missing selection
                camTrackReady = false;
            }
            else
            {
                vidSourceChoiceComboBox.BackColor = Color.WhiteSmoke;
            }
            if (drawModeChoiceComboBox.SelectedIndex != -1 && vidSourceChoiceComboBox.SelectedIndex != -1)
            {
                //allow capture to start & reset colouring
                camTrackReady = true;
            }
        }
        #endregion

        #region Initialise camera streams
        public void initCamStreams()
        {
            //tries to initialise the default camera and the first adjacent left and right

            try
            {
                //initialise the centre, left and right camera objects - set as needed
                cscCentre_CB = new Capture(2);                                     //CENTRE CAMERA FRAME
                cscLeft_CB = new Capture(1);                                       //LEFT CAMERA FRAME
                cscRight_CB = new Capture(0);                                      //RIGHT CAMERA FRAME               

                if (cscCentre_CB.GetCaptureProperty(CapProp.FrameHeight) != 0)
                {
                    camStreamCapture_CB = cscCentre_CB;                   //initially set the centre cam to the current
                }
                else if (cscLeft_CB.GetCaptureProperty(CapProp.FrameHeight) != 0)
                {
                    camStreamCapture_CB = cscLeft_CB;                     //set initial frame to left cam if centre is not setup
                }
                else if (cscRight_CB.GetCaptureProperty(CapProp.FrameHeight) != 0)
                {
                    camStreamCapture_CB = cscRight_CB;                    //set initial frame to the right camera if centre & left not setup
                }


            }
            catch (NullReferenceException nr)
            {

                throw nr;
            }

        }
        #endregion

        #region UI and button methods
        private void setExtentsButton_Click(object sender, EventArgs e)
        {
            //warn user if setting not filled
            if (drawModeChoiceComboBox.SelectedIndex == -1 || vidSourceChoiceComboBox.SelectedIndex == -1)
            {
             //   MessageBox.Show("Please select a capture mode and/or a draw mode before continuing.",
             //       "Choose settings first!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
            }
            else
            {

                //The index of draw mode defines the selected item. 0=Random 1=Ordered 2=Tracking 3=object tracking onscreen
                switch (drawModeChoiceComboBox.SelectedIndex)
                {
                    case 0: drawMode_CB = DRAW_MODE_RANDOM; break;
                    case 1: drawMode_CB = DRAW_MODE_ORDERED; break;
                    case 2: drawMode_CB = DRAW_MODE_TRACKING; break;
                    case 3: drawMode_CB = DRAW_MODE_REVOBJTRACK; break;
                    default:
                        drawMode_CB = -1;
                        break;
                }

                //The index of video mode defines the selected item. 0=Video on PC 1=Webcam
                switch (vidSourceChoiceComboBox.SelectedIndex)
                {
                    case 0: chooseVideoFileFialog.ShowDialog(); break;                   //show user file dialog first
                    case 1: setVideoParameters(0, drawMode_CB); break;   //using webcam so no filename needed
                    case 2: setVideoParameters(1, drawMode_CB); break;   //using webcam and GPS values
                    default:
                        break;
                }

                //now start the process!
                try
                {
                    bool startedOK = false;
                    //startedOK = startNewVideoConsole();
                    if (startedOK)
                    {
                        this.Hide();           //close the form and open the next one
                    }
                    else
                    {
                        //vo.ReleaseData();               //dispose of the capture methods
                        this.Dispose();
                    }

                }
                catch (NotSupportedException ns)
                {
                    MessageBox.Show("The video stream start failed. Details: " + ns.Message,
                        "Something happened", MessageBoxButtons.OK, MessageBoxIcon.Error,MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                }



            }
        }

        private void checkReadiness()
        {
            //if choosing internal cams & those aren't setup OR when choosing IP and THOSE aren't ready
            //disable the start button
            if (!coordsReady    ||
                (vidSourceChoiceComboBox.SelectedIndex == 1 && !camTrackReady) 
                                ||
                (vidSourceChoiceComboBox.SelectedIndex == 2 && !ipCamReady)  )
            {
                setExtentsButton.Enabled = false;
            }
            else
            {
                setExtentsButton.Enabled = true;
            }
        }

        private void chooseVideoFileFialog_FileOk(object sender, CancelEventArgs e)
        {
            //set the parameters of the file and arguments we'll pass 
            //if here, we assume the user is opening a file
            setVideoParameters(1, drawMode_CB, chooseVideoFileFialog.FileName.ToString());
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
            //vo.ReleaseData();               //dispose of the capture methods
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
            parseSuccess = double.TryParse(_upperLeft0, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out upperLeftCoords_CB[0]);
            if (parseSuccess) { parseSuccessCount++; } else { parseSuccess = false; }
            parseSuccess = double.TryParse(_upperLeft1, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out upperLeftCoords_CB[1]);
            if (parseSuccess) { parseSuccessCount++; } else { parseSuccess = false; }
            parseSuccess = double.TryParse(_upperRight0, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out outerLimitCoords_CB[0]);
            if (parseSuccess) { parseSuccessCount++; } else { parseSuccess = false; }
            parseSuccess = double.TryParse(_bottomLeft1, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out outerLimitCoords_CB[1]);
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
                videoSource_CB = _videoSource;
                drawMode_CB = _drawMode;
            }
            else
            {
                Console.Write("Failed to set draw mode choice.");
                return;
            }


            //only set the filename if the source is PC video
            if (_videoSource == 1)
            {
                filenameToOpen_CB = _filenameToOpen;
            }
            else
            {
                filenameToOpen_CB = "false";
            }

            Console.Write("Set video parameters successfully.");
        }

        //public bool startNewVideoConsole()
        //{
        //    vo.fileName = filenameToOpen_CB;       //set the filename to open on the other form
        //    vo.drawMode_Overlay = drawMode_CB;             //set the draw mode on the other form
        //    vo.captureChoice = videoSource_CB;     //set the video source on the other form

        //    //pass parameters of the outer limits to the other UI
        //    vo.upperLeftBound[0] = upperLeftCoords[0];
        //    vo.upperLeftBound[1] = upperLeftCoords[1];
        //    vo.outerLimitBound[0] = outerLimitCoords[0];
        //    vo.outerLimitBound[1] = outerLimitCoords[1];

        //    if (vo.Visible == true)
        //    {
        //        MessageBox.Show("Only one instance of the video capture window may be open at a time. Close the other first.", 
        //            "Instance already running!", MessageBoxButtons.OK, MessageBoxIcon.Warning,MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
        //        return false;
        //    }
        //    else
        //    {
        //        try
        //        {
        //            vo.TopMost = true;      //set the window to the top and make it visible
        //            vo.Visible = true;
        //            vo.Show();

        //        }
        //        catch (ObjectDisposedException d)
        //        {
        //            return false;
        //            //vo.fileName = filenameToOpen;       //set the filename to open on the other form
        //            //vo.drawMode_Overlay = drawMode;             //set the draw mode on the other form
        //            //vo.captureChoice = videoSource;     //set the video source on the other form

        //            ////pass parameters of the outer limits to the other UI
        //            //vo.upperLeftBound[0] = upperLeftCoords[0];
        //            //vo.upperLeftBound[1] = upperLeftCoords[1];
        //            //vo.outerLimitBound[0] = outerLimitCoords[0];
        //            //vo.outerLimitBound[1] = outerLimitCoords[1];
        //            //vo.Focus();      //set the window to the top and make it visible
        //            //vo.Visible = true;
        //            //vo.Show();
        //        }
        //        return true;
        //    }


        //}
        #endregion

        #region Help system methods 
        private void drawModeChoiceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void coordsHelpLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // 0 = help with coords setup
            setupHelpPane shp = new setupHelpPane(0);
            shp.ShowDialog();
        }

        private void cameraHelpLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // 1 = help with camera setup
            setupHelpPane shp = new setupHelpPane(1);
            shp.ShowDialog();
        }

        private void rightCamStatusLabel_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}

