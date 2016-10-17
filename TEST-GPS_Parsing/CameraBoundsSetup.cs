using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using DirectShowLib;


namespace TEST_GPS_Parsing
{
    public partial class CameraBoundsSetup : Form
    {
        #region Initialization objects and vars


        //-------------TEST---------------
        public DsDevice[] _SystemCameras_CB;
        public VideoOutputWindow.Video_Device[] camStreamCaptureArray_CB;
        public int CameraDevice_CB;
        public Capture _capture_CB;

               
        //-------------END TEST-----------

        //==========COPY OF VIDEO PARAMETERS===============
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

        //The videoWriter object for storing the video written
        public VideoWriter videoWriterOutput_CB;
        public string videoLogFilename_CB;

        //-----------Capture Var Copies--------------------
        //public Capture cscCentre_CB;
        //public Capture cscLeft_CB;
        //public Capture cscRight_CB;
        //public Capture[] camStreamCaptureArray_CB;                          //array to hold all camera capture objects
        public Capture currentCamStreamCapture_CB;                  //current "marker" capture object

        //video GPS coordinate extents
        public double[] upperLeftCoords_CB = new double[2];       //[0] = latitude top left; [1] = longitude top left
        public double[] outerLimitCoords_CB = new double[2];      //[0] = longitude top right; [1] = latitude bottom left

        //==========END COPY OF VIDEO PARAMETERS=============
        public static int totalCameraNumber_CB = 5;                 //edit this to change # cams

        //-------SIMULATION
        public static int DRAW_MODE_RANDOM = 0;
        public static int DRAW_MODE_ORDERED = 1;
        public static int DRAW_MODE_TRACKING = 2;
        public static int DRAW_MODE_REVOBJTRACK = 3;

        //-------flags for program readiness
        bool coordsReady;           //if the co-ordinates are entered & tracking mode selected
        bool camTrackReady;         //if the local cams are ready
        bool ipCamReady;            //if the IP cameras are ready
        bool vidLogModeReady;       //if video logging format is selected

        public CameraBoundsSetup(VideoOutputWindow incoming_vo)
        {
            InitializeComponent();
           //--------------ORIGINAL CODE----------
            // camStreamCaptureArray_CB = new Capture[totalCameraNumber_CB];   //set up an array of capture objects
        }
        #endregion

        #region Camera, Coords and Video Log Readiness
        
        private void cameraReadiness()
        {
            //if user has chosen internal cameras, try to connect them automagically
            if (vidSourceChoiceComboBox.SelectedIndex == 1)
            {
                //disable IP cam setting section
                checkIpAddrButton.Enabled = false;
                camLeftIpTextBox.Enabled = false;
                camRightIpTextBox.Enabled = false;
                camCentreIpTextBox.Enabled = false;
                localOrIpCamInfoLabel.Text = "Local cameras selected. These will be initialised automatically. \n Check the status below.";
                //allow the start

                //disable this if using local cams since they'll refresh automagically :P
                refreshStatusButton.Enabled = false;

                //if any capture objects haven't been setup yet, run the init routine (once, since it'll init all)

                //--------------TEST CODE-----------------
                for (int i = 0; i < camStreamCaptureArray_CB.Length; i++)
                {
                    if (camStreamCaptureArray_CB[i].Device_Name == null)
                    {
                        initCamStreams(); break;
                    }
                }
                //--------------END TEST CODE-------------

                //--------------ORIGINAL CODE-------------
                //for (int i = 0; i < totalCameraNumber_CB; i++)
                //{
                //    if (camStreamCaptureArray_CB[i] == null)
                //    {
                //        initCamStreams(); break;
                //    }
                //}
                //-------------END ORIGINAL CODE-------------

            }
            else if (vidSourceChoiceComboBox.SelectedIndex == 2)
            {

                localOrIpCamInfoLabel.Text =
                    "IP cameras selected. Enter 3 IP addresses below and click 'Check Addresses'.\n Enter IP address only, the http:// is not needed.\n The status of the cameras will appear in the status pane.";
                //IP camera initialisation takes place when the user presses the "check IP addresses" button.
                //the methods below enable the use of those buttons
                checkIpAddrButton.Enabled = true;
                camLeftIpTextBox.Enabled = true;
                camRightIpTextBox.Enabled = true;
                camCentreIpTextBox.Enabled = true;
            }

            //check default camera status and display on UI - code supports >3 cameras but GUI doesn't so work with GUI limit
            //Assume cam 0 = left, 1 = centre and 2=right
            camView2StatusTextBox.Clear();
            camStatusTextbox.Clear();
            for (int i = 0; i < camStreamCaptureArray_CB.Length; i++)
            {
                //if the video object isn't null and actually is a video (frame height >0) then update GUI appropriately
                if (camStreamCaptureArray_CB[i].Device_Name != null /*&&
                    camStreamCaptureArray_CB[i].GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight) != 0*/)
                {
                    camStatusTextbox.AppendText("Camera " + (i + 1) + " [" + camStreamCaptureArray_CB[i].Device_Name + "]: Ready \n");
                }
                else
                {
                    camStatusTextbox.AppendText("Camera " + (i + 1) + ": Unavailable \n");
                    camView2StatusTextBox.AppendText("Cam " + (i + 1) + " not available | ");
                }
            }
        }

        private void coordinateReadiness()
        {
            if (longUpperLeftTextbox.Text == "" || latUpperLeftTextbox.Text == "" || latBottomLeftTextbox.Text == "" || longUpperRightTextbox.Text == "")
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

        }

        private void dropdownReadiness()
        {
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

        #region Form load and Verify Methods

        private void CameraBoundsSetup_Load(object sender, EventArgs e)
        {
            //checkFieldsTimer = new Timer(); //instantiate the timer
            checkFieldsTimer.Start();       //start the timer to check status of the entry of points
            coordsReady = false;
            camTrackReady = false;
            ipCamReady = false;
            vidLogModeReady = false;

            //------------------TEST------------------
            _SystemCameras_CB = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            camStreamCaptureArray_CB = new VideoOutputWindow.Video_Device[_SystemCameras_CB.Length];
            //------------------END TEST--------------
        }

        private void checkFieldsTimer_Tick(object sender, EventArgs e)
        {
            //run the readiness check to enable/disable the start button
            checkOverallReadiness();

            cameraReadiness();          //check if cameras setup      
                
            dropdownReadiness();        //check all dropdown menus

            coordinateReadiness();      //check coordinate fields

        }
        #endregion

        #region Initialise camera streams (local and IP)

        private void SetupCapture(int Camera_Identifier)
        {
            //update the selected device
            CameraDevice_CB = Camera_Identifier;

            //Dispose of Capture if it was created before
            if (_capture_CB != null) _capture_CB.Dispose();
            try
            {
                //Set up capture device
                _capture_CB = new Capture(CameraDevice_CB);
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
        }

        public void initCamStreams()
        {
            //tries to initialise the default camera and the first adjacent left and right
            //--------------------TEST----------------

            try
            {
                for (int i = 0; i < _SystemCameras_CB.Length; i++)
                {
                    camStreamCaptureArray_CB[i] = new VideoOutputWindow.Video_Device(i, _SystemCameras_CB[i].Name, _SystemCameras_CB[i].ClassID); //fill web cam array
                }

                //setup the middlemost cam first
                //anything LOWER index is left, HIGHER is right
                SetupCapture((int)_SystemCameras_CB.Length / 2);
            }
            catch (NullReferenceException nr)
            {

                throw nr;
            }

             

            //--------------------ENDTEST---------------



            //--------------------ORIGINAL CODE-----------
            //try
            //{
            //    //initialise the centre, left and right camera objects - set as needed
            //    //cscCentre_CB = new Capture(0);                                     //CENTRE CAMERA FRAME
            //    //cscLeft_CB = new Capture(2);                                       //LEFT CAMERA FRAME
            //    //cscRight_CB = new Capture(1);                                      //RIGHT CAMERA FRAME               

            //    //The ith index starts counting from the leftmost camera and proceeds to the end L->R
            //    for (int i = 0; i < totalCameraNumber_CB ; i++)
            //    {
            //        camStreamCaptureArray_CB[i] = new Capture(i);   //initialise as many capture objects as needed
            //    }

            //    //go through each cam and if any haven't loaded, set the other to centre
            //    int haveLoaded = 0;
            //    for (int i = 0; i < totalCameraNumber_CB; i++)
            //    {
            //        //set the current camera to whichever one is within bounds that is working
            //        if (camStreamCaptureArray_CB[i].GetCaptureProperty(CapProp.FrameHeight) != 0 && (i+1<totalCameraNumber_CB))
            //        {
            //            haveLoaded++;
            //        }
            //    }

            //    //assume all local cams with frame height !=0 are working, so find the middle one of them and set the current one to that
            //    currentCamStreamCapture_CB = camStreamCaptureArray_CB[(int)haveLoaded/totalCameraNumber_CB];

            //}
            //catch (NullReferenceException nr)
            //{

            //    throw nr;
            //}
            //--------------------END ORIGINAL CODE------------------
        }

        public void initIpCamStreams()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region UI and button methods
        private void checkIpAddrButton_Click(object sender, EventArgs e)
        {
            checkIpAddrButton.Text = "Checking...";
            //parse the IP addresses entered and attempt to connect to those cameras
            //For testing Vivotek cams 146.230.195.13-15/video.mjpg for 14 use video4.mjpg

            //TODO: fix hardcoded capture array properties
            //-------------ORIGINAL CODE COMMENTED OUT----------
            string ipAddrLeft, ipAddrCentre, ipAddrRight;
            ipAddrLeft = "http://" + camLeftIpTextBox.Text.ToString() + "/video.mjpg";
            //camStreamCaptureArray_CB[0] = new Emgu.CV.Capture(ipAddrLeft);

            ipAddrCentre = "http://" + camCentreIpTextBox.Text.ToString() + "/video4.mjpg";
            //camStreamCaptureArray_CB[1] = new Emgu.CV.Capture(ipAddrCentre);

            ipAddrRight = "http://" + camRightIpTextBox.Text.ToString() + "/video.mjpg";
            //camStreamCaptureArray_CB[2] = new Emgu.CV.Capture(ipAddrRight);

            //go through each cam and if any haven't loaded, set the other to centre
            int haveLoaded = 0;
            for (int i = 0; i < camStreamCaptureArray_CB.Length; i++)
            {
                //set the current camera to whichever one is within bounds that is working
                //if (camStreamCaptureArray_CB[i].GetCaptureProperty(CapProp.FrameHeight) != 0 && (i + 1 < totalCameraNumber_CB))
                //{
                    haveLoaded++;
                //}
            }

            //assume all local cams with frame height !=0 are working, so find the middle one of them and set the current one to that
            //currentCamStreamCapture_CB = camStreamCaptureArray_CB[(int)haveLoaded / totalCameraNumber_CB];

            ipCamReady = true;
            checkIpAddrButton.Text = "Check IP addresses";
            //---------------END ORIGINAL CODE COMMENTED OUT
        }

        private void setExtentsButton_Click(object sender, EventArgs e)
        {
            checkOverallReadiness();

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

            int haveLoaded = 0;
            for (int i = 0; i < camStreamCaptureArray_CB.Length; i++)
            {
                //---------------ORIGINAL CODE COMMENTED OUT---------
                //check how many cameras are available and throw a warning if <3
                if (camStreamCaptureArray_CB[i].Device_Name != null/*GetCaptureProperty(CapProp.FrameHeight) != 0*/ && (i + 1 <= totalCameraNumber_CB))
                {
                    haveLoaded++;
                }
                //----------------END ORIG CODE OUT---------------
            }
            //Checks if the minimum number of cameras are available
            if (haveLoaded <3)
            {
                MessageBox.Show("Error - at least 3 cameras must be operational for this program to work optimally. If you choose to continue, you might encounter unexpected problems. Continue without 3 cameras?", "Camera Load error!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            }
           
        }

        private void checkOverallReadiness()
        {
            //if choosing internal cams & those aren't setup OR when choosing IP and THOSE aren't ready
            //disable the start button
            if (coordsReady && vidLogModeReady &&
                (vidSourceChoiceComboBox.SelectedIndex == 1 && camTrackReady)
                ||
                (vidSourceChoiceComboBox.SelectedIndex == 2 && ipCamReady)
                )
            {
                setExtentsButton.Enabled = true;
            }
            else
            {
                setExtentsButton.Enabled = false;
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
            //ReleaseData();               //dispose of the capture methods
            this.Close();
        }

        private void videoFormatCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            videoLogFilename_CB = "videoLogFile" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            if (videoFormatCombobox.SelectedIndex != -1)
            {
                switch (videoFormatCombobox.SelectedIndex)
                {
                    /*
                     Invoke the writer to set up the codec:
                     public VideoWriter(string fileName,int compressionCode,int fps,Size size,bool isColor)

                    compressionCodes available for use 
                    case 0 =  VideoWriter.Fourcc('M', 'P', 'G', '4')
                    case 1 =  VideoWriter.Fourcc('W', 'M', 'V', '3')
                    case 2 =  VideoWriter.Fourcc('D', 'I', 'V', 'X')
                     */
                    /*case 0: videoWriterOutput_CB = new VideoWriter(videoLogFilename_CB + ".mpg", VideoWriter.Fourcc('M', 'P', 'G', '4'), 20,new Size(640,480), true);break;                    
                    case 1: videoWriterOutput_CB = new VideoWriter(videoLogFilename_CB + ".wmv", VideoWriter.Fourcc('W', 'M', 'V', '3'), 20, new Size(640, 480), true); break;
                    case 2: videoWriterOutput_CB = new VideoWriter(videoLogFilename_CB + ".mpg", VideoWriter.Fourcc('D', 'I', 'V', 'X'), 20, new Size(640, 480), true); break;*/
                    default:
                        videoWriterOutput_CB = new VideoWriter(videoLogFilename_CB + ".avi", //File name
                                        VideoWriter.Fourcc('M', 'P', '4', '2'), //Video format
                                        20, //FPS
                                        new Size(640,480), //frame size
                                        true); //Color
                        break;
                }
                vidLogModeReady = true;
            }

        }

        private void insertTestValues_Click(object sender, EventArgs e)
        {
            //Test Suite for co-ord checks - Howard College Traversal With Google Earth
            //bounds UpperLeft: Lat -29.866178; Long 30.981370
            //bounds UpperRight Long 30.983583
            //bounds BottomLeft Lat -29.866673
            //goes up, right off, across main, left off, across right and right off, and back up out top

            longUpperLeftTextbox.Text = "30.981370";
            longUpperRightTextbox.Text = "30.983583";
            latUpperLeftTextbox.Text = "-29.866178";
            latBottomLeftTextbox.Text = "-29.866673";

            drawModeChoiceComboBox.SelectedIndex = 2;
            vidSourceChoiceComboBox.SelectedIndex = 1;
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

