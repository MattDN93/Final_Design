using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using DirectShowLib;
using System.Net;

namespace TEST_GPS_Parsing
{
    public partial class CameraBoundsSetup : Form
    {
        #region Initialization objects and vars


        //-------------TEST---------------
        public DsDevice[] _SystemCameras_CB;
        public VideoOutputWindow.Video_Device[] webCams_CB;
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

        //----------------Co-ordinate matrices and vars-------------------
        //video GPS coordinate extents - for the transformed Image Screen model (the original onscreen bounds)
        public double[] upperLeftCoordsTransformed_CB = new double[2];       //[0] = latitude top left; [1] = longitude top left
        public double[] outerLimitCoordsTransformed_CB = new double[2];      //[0] = longitude top right; [1] = latitude bottom left

        //----------------------METHOD 1: TAKING TOP DOWN GPS COORDINATES AND MAPPING TO A NEAT SQUARE-------------------------
        //for the real-world mappng input coords (going from top-down varying GPS to a nice workable square on the image )
        public double[] upperLeftCoordsRealworld_CB = new double[2];        //[0] = latitude top left, [1] = longitude top left
        public double[] upperRightCoordsRealworld_CB = new double[2];       //[0] = latitude top right, [1] = longitude top right
        public double[] lowerLeftCoordsRealworld_CB = new double[2];        //[0] = latitude bott left, [1] = longitude bott left
        public double[] lowerRightCoordsRealworld_CB = new double[2];        //[0] = latitude bott right, [1] = longitude bott right

        //input into the perspectiveTransform
        /*  realworld[0].x = lat top left                       realworld[1].x = lat top right
            realworld[0].y = long top left                      realworld[1].y = long top right
            
            realworld[2].x = lat bott left                      realworld[0].y = long bott right
            realworld[2].y = long bott left                     realworld[0].y = long bott right
             
        */
        private PointF[] realWorldInputsForCalc_CB = new PointF[4];
        private PointF[] transformedPtsForCalc_CB = new PointF[4];

        //deltas for calculating the transformed points
        double longDelta_CB;
        double latDelta_CB;             
                
        public Mat transformMatrix_CB;                                 //matrix M to transform varying world co-ords to normalised co-ords

        //----------------------END METHOD 1-------------------------
        //----------------------METHOD 2: TAKING SQUARE CO-ORDINATES AND MAPPING THEM ONTO CAMERA VIEWSPACE-------------------------
        /*This method is executed after the first one. It takes the pixel grid and maps it down onto the camera plane to avoid
            Floating points in the sky. Simply a reverse plane projective transform.
        */
        // 
        
        //------------------------------------------------------------

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
                videoSource_CB = 0;
                checkIpAddrButton.Enabled = false;
                currentIpEntryBox.Enabled = false;
                listOfIps.Enabled = false;
                localOrIpCamInfoLabel.Text = "Local cameras selected. These will be initialised automatically. \n Check the status below.";
                //allow the start

                //disable this if using local cams since they'll refresh automagically :P
                refreshStatusButton.Enabled = false;

                //if any capture objects haven't been setup yet, run the init routine (once, since it'll init all)

                //--------------TEST CODE-----------------
                for (int i = 0; i < webCams_CB.Length; i++)
                {
                    if (webCams_CB[i].Device_Name == null)
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
                videoSource_CB = 1;
                localOrIpCamInfoLabel.Text =
                    "IP cameras selected. \n Enter an IP address and press Enter to add it to the list. When done, click 'Check Addresses'.\n Enter IP address only, the http:// is not needed.\n .";
                //IP camera initialisation takes place when the user presses the "check IP addresses" button.
                //the methods below enable the use of those buttons
           
                currentIpEntryBox.Enabled = true;
                listOfIps.Enabled = true;
            }
            else
            {
                checkIpAddrButton.Enabled = false;
                currentIpEntryBox.Enabled = false;
                listOfIps.Enabled = false;
            }

            //check default camera status and display on UI - code supports >3 cameras but GUI doesn't so work with GUI limit
            //Assume cam 0 = left, 1 = centre and 2=right
            camView2StatusTextBox.Clear();
            camStatusTextbox.Clear();
            for (int i = 0; i < webCams_CB.Length; i++)
            {
                //if the video object isn't null and actually is a video (frame height >0) then update GUI appropriately
                if (webCams_CB[i].Device_Name != null /*&&
                    camStreamCaptureArray_CB[i].GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight) != 0*/)
                {
                    camStatusTextbox.AppendText("Camera " + (i + 1) + " [" + webCams_CB[i].Device_Name + "]: Ready \n");
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
            if (longUpperLeftTextbox.Text == "" || latUpperLeftTextbox.Text == "" 
                || latBottomLeftTextbox.Text == "" || longBottomLeftTextbox.Text == ""
                || longUpperRightTextbox.Text == "" || latUpperRightTextbox.Text == ""
                || longBottomRightTextbox.Text == "" || latBottomRightTextbox.Text == "")
            {
                camViewStatusTextBox.Clear();           //clears from last tick
                camViewStatusTextBox.BackColor = Color.Orange;
                camViewStatusTextBox.AppendText("Fields empty:");   //append which fields are still empty until they're fixed
                if (longUpperLeftTextbox.Text == "")
                {
                    camViewStatusTextBox.AppendText(" Long. upper L;");
                }
                if (latUpperLeftTextbox.Text == "")
                {
                    camViewStatusTextBox.AppendText(" Lat. upper L;");
                }
                if (longUpperRightTextbox.Text == "")
                {
                    camViewStatusTextBox.AppendText(" Long. upper R;");
                }
                if (latUpperRightTextbox.Text == "")
                {
                    camViewStatusTextBox.AppendText(" Lat. upper R;");
                }
                if (longBottomLeftTextbox.Text == "")
                {
                    camViewStatusTextBox.AppendText(" Long. lower L;");
                }
                if (latBottomLeftTextbox.Text == "")
                {
                    camViewStatusTextBox.AppendText(" Lat. lower L;");
                }
                if (longBottomRightTextbox.Text == "")
                {
                    camViewStatusTextBox.AppendText(" Long. lower R;");
                }
                if (latBottomRightTextbox.Text == "")
                {
                    camViewStatusTextBox.AppendText(" Lat. lower R;");
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
                setBoundsResult = setCameraCoordBounds(latUpperLeftTextbox.Text, longUpperLeftTextbox.Text, latUpperRightTextbox.Text, longUpperRightTextbox.Text, 
                                                    latBottomLeftTextbox.Text,longBottomLeftTextbox.Text,latBottomRightTextbox.Text, longBottomRightTextbox.Text);
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
            webCams_CB = new VideoOutputWindow.Video_Device[_SystemCameras_CB.Length];
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

        private void SetupCapture_CB(int Camera_Identifier, string url="")
        {
            if (videoSource_CB == 0)        //webcams
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
            else if (videoSource_CB == 1)   //ip cams
            {
                //update the selected device
                CameraDevice_CB = Camera_Identifier;

                //Dispose of Capture if it was created before
                if (_capture_CB != null) _capture_CB.Dispose();
                try
                {
                    //Set up capture device
                    _capture_CB = new Capture(url);
                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
            }

        }

        public void initCamStreams()
        {
            //tries to initialise the default camera and the first adjacent left and right

            try
            {
                for (int i = 0; i < _SystemCameras_CB.Length; i++)
                {
                    webCams_CB[i] = new VideoOutputWindow.Video_Device(i, _SystemCameras_CB[i].Name, _SystemCameras_CB[i].ClassID); //fill web cam array
                }

                //setup the middlemost cam first
                //anything LOWER index is left, HIGHER is right
                SetupCapture_CB(0/*(int)_SystemCameras_CB.Length / 2*/);

            }
            catch (NullReferenceException nr)
            {

                throw nr;
            }

      }

        public bool initIpCamStreams(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD- we want to check it exists
                request.Method = "HEAD";
                request.Proxy = null;
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }

        }
        #endregion

        #region UI and button methods
        private void checkIpAddrButton_Click(object sender, EventArgs e)
        {
            //go through each cam and if any haven't loaded, set the other to centre
            int haveLoaded = 0;
            checkIpAddrButton.Text = "Checking...";
            //parse the IP addresses entered and attempt to connect to those cameras
            //For testing Vivotek cams 146.230.195.13-15/video.mjpg for 14 use video4.mjpg
            webCams_CB = new VideoOutputWindow.Video_Device[listOfIps.Items.Count];
            //test each camera's availability
            for (int i = 0; i < listOfIps.Items.Count; i++)
            {
                string currentUrl = "http://" + listOfIps.Items[i].Text + ":8080/video";
                bool urlIsValid = initIpCamStreams(currentUrl);
                if (urlIsValid)
                {
                    haveLoaded++;
                    listOfIps.Items[i].BackColor = Color.LightGreen;
                    webCams_CB[i] = new VideoOutputWindow.Video_Device(i, currentUrl); //fill web cam array with IP cams
                }
                else
                {
                    listOfIps.Items[i].BackColor = Color.OrangeRed;
                }
            }

            if (haveLoaded >= 2)
            {
                ipCamReady = true;
                for (int i = 0; i < listOfIps.Items.Count; i++)
                {
                   // SetupCapture_CB(i, webCams_CB[i].Device_Name);       //setup one of the ip cams (middle)
                }
                
                checkIpAddrButton.Text = "Check IP addresses";
                
            }
            else
            {
                checkIpAddrButton.Text = "Check IP addresses";
                ipCamReady = false;
            }
            
            //---------------END ORIGINAL CODE COMMENTED OUT
        }

        //adds to a list of IPs that the user wants to connect to
        private void currentIpEntryBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 ) //enter is pressed
            {
                if (currentIpEntryBox.Text != "")
                {
                    listOfIps.Items.Add(currentIpEntryBox.Text);    //add the current IP to the textbox   
                    currentIpEntryBox.Clear();
                    if (listOfIps.Items.Count > 0)
                    {
                        checkIpAddrButton.Enabled = true;
                    }
                }
            }

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
            for (int i = 0; i < webCams_CB.Length; i++)
            {
                //---------------ORIGINAL CODE COMMENTED OUT---------
                //check how many cameras are available and throw a warning if <3
                if (webCams_CB[i].Device_Name != null/*GetCaptureProperty(CapProp.FrameHeight) != 0*/ && (i + 1 <= totalCameraNumber_CB))
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
            latBottomRightTextbox.Clear();
            latUpperLeftTextbox.Clear();
            latUpperRightTextbox.Clear();
            longBottomLeftTextbox.Clear();
            longBottomRightTextbox.Clear();
            longUpperLeftTextbox.Clear();
            longUpperRightTextbox.Clear();           

        }

        private void clearIpsButton_Click(object sender, EventArgs e)
        {
            listOfIps.Items.Clear();
            checkIpAddrButton.Enabled = false;
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
                        videoWriterOutput_CB = new VideoWriter(videoLogFilename_CB + ".mkv", //File name
                                        VideoWriter.Fourcc('M', 'P', '4', '2'), //Video format
                                        15, //FPS
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
            longBottomLeftTextbox.Text = "30.981350";

            longUpperRightTextbox.Text = "30.983583";
            longBottomRightTextbox.Text = "30.983501";

            latUpperLeftTextbox.Text = "-29.866178";
            latUpperRightTextbox.Text = "-29.866150";

            latBottomLeftTextbox.Text = "-29.866673";
            latBottomRightTextbox.Text = "-29.866600";


            drawModeChoiceComboBox.SelectedIndex = 2;
            vidSourceChoiceComboBox.SelectedIndex = 2;

            videoFormatCombobox.SelectedIndex = 0;
            listOfIps.Items.Add("192.168.0.7");
            listOfIps.Items.Add("192.168.0.9");
            listOfIps.Items.Add("192.168.0.10");
            checkIpAddrButton.Enabled = true;
        }

        private void devLanButton_Click(object sender, EventArgs e)
        {           
            //Test Suite for co-ord checks - Howard College Traversal With Google Earth
            //bounds UpperLeft: Lat -29.866178; Long 30.981370
            //bounds UpperRight Long 30.983583
            //bounds BottomLeft Lat -29.866673
            //goes up, right off, across main, left off, across right and right off, and back up out top

            longUpperLeftTextbox.Text = "30.980211";            
            longBottomLeftTextbox.Text = "30.980166";

            longUpperRightTextbox.Text = "30.980293";
            longBottomRightTextbox.Text = "30.980258";

            latUpperLeftTextbox.Text = "-29.868590";
            latUpperRightTextbox.Text = "-29.868621";

            latBottomLeftTextbox.Text = "-29.868635";
            latBottomRightTextbox.Text = "-29.868674";

            videoFormatCombobox.SelectedIndex = 0;

            drawModeChoiceComboBox.SelectedIndex = 2;
            vidSourceChoiceComboBox.SelectedIndex = 2;
            listOfIps.Items.Add("192.168.0.9");
            listOfIps.Items.Add("192.168.0.10");
            ///listOfIps.Items.Add("192.168.135.104");
            checkIpAddrButton.Enabled = true;

        }
        #endregion

        #region Camera modification methods
        public bool setCameraCoordBounds(string _upperLeft0, string _upperLeft1, string _upperRight0, string _upperRight1, 
                                         string _bottomLeft0, string _bottomLeft1, string _bottomRight0, string _bottomRight1)
        {
            //save the co-ordinate limits to the relative variables
            bool parseSuccess = false;
            int parseSuccessCount = 0;

            //convert the text to double and check - this is the realworld co-ords not the transformed ones yet
            //latitude upper left
            parseSuccess = double.TryParse(_upperLeft0, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out upperLeftCoordsRealworld_CB[0]);
            if (parseSuccess) { parseSuccessCount++; } else { parseSuccess = false; }
            //longitude upper left
            parseSuccess = double.TryParse(_upperLeft1, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out upperLeftCoordsRealworld_CB[1]);
            if (parseSuccess) { parseSuccessCount++; } else { parseSuccess = false; }
            //latitude top right
            parseSuccess = double.TryParse(_upperRight0, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out upperRightCoordsRealworld_CB[0]);
            if (parseSuccess) { parseSuccessCount++; } else { parseSuccess = false; }
            //longitude top right
            parseSuccess = double.TryParse(_upperRight1, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out upperRightCoordsRealworld_CB[1]);
            if (parseSuccess) { parseSuccessCount++; } else { parseSuccess = false; }
            //latitude bottom left
            parseSuccess = double.TryParse(_bottomLeft0, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out lowerLeftCoordsRealworld_CB[0]);
            if (parseSuccess) { parseSuccessCount++; } else { parseSuccess = false; }
            //longitude bottom left
            parseSuccess = double.TryParse(_bottomLeft1, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out lowerLeftCoordsRealworld_CB[1]);
            if (parseSuccess) { parseSuccessCount++; } else { parseSuccess = false; }
            //latitude lower right
            parseSuccess = double.TryParse(_bottomRight0, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out lowerRightCoordsRealworld_CB[0]);
            if (parseSuccess) { parseSuccessCount++; } else { parseSuccess = false; }
            //longitude lower right
            parseSuccess = double.TryParse(_bottomRight1, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out lowerRightCoordsRealworld_CB[1]);
            if (parseSuccess) { parseSuccessCount++; } else { parseSuccess = false; }

            //make sure all co-ordinates were parsed OK
            if (parseSuccessCount == 8)
            {
                parseSuccessCount = 0;
                Console.Write("Parsed camera bound co-ordinates OK.");

                doPerspectiveTransform();           //change the co-ord mapping from the world to image plane

                return true;
            }
            else
            {
                Console.Write("One or more camera bound co-ordinates couldn't be parsed.");
                parseSuccessCount = 0;
                return false;
            }
        }

        private void doPerspectiveTransform()
        {
            /*This method consists of 5 parts:
             * 1. Taking the real world arrays and putting them into the PointF structures
             * 2. Calculating the desired image-plane co-ordinate points
             * 3. Putting the image-plane points into PointF structures
             * 4 Write the image points into the existing algorithms
             * 5. Do getperspectivetransform() and get the transform matrix             * 
             * 
             * NOTE:    X = LATITUDE ("Y DISPLACEMENT")
             *          Y = LONGITUDE ("X DISPLACEMENT")
             * */

            //Part 1: put the co-ordinates into the Pointf() form
            realWorldInputsForCalc_CB[0].X = (float)upperLeftCoordsRealworld_CB[0];
            realWorldInputsForCalc_CB[0].Y = (float)upperLeftCoordsRealworld_CB[1];

            realWorldInputsForCalc_CB[1].X = (float)upperRightCoordsRealworld_CB[0];
            realWorldInputsForCalc_CB[1].Y = (float)upperRightCoordsRealworld_CB[1];

            realWorldInputsForCalc_CB[2].X = (float)lowerLeftCoordsRealworld_CB[0];
            realWorldInputsForCalc_CB[2].Y = (float)lowerLeftCoordsRealworld_CB[1];

            realWorldInputsForCalc_CB[3].X = (float)lowerRightCoordsRealworld_CB[0];
            realWorldInputsForCalc_CB[3].Y = (float)lowerRightCoordsRealworld_CB[1];

            //Part 2: get the desired image-plane transformed points
            longDelta_CB = Math.Sqrt(Math.Pow(((double)realWorldInputsForCalc_CB[1].Y - (double)realWorldInputsForCalc_CB[0].Y), 2) +
                                  Math.Pow(((double)realWorldInputsForCalc_CB[1].X - (double)realWorldInputsForCalc_CB[0].X), 2));

            latDelta_CB = Math.Sqrt(Math.Pow(((double)realWorldInputsForCalc_CB[2].Y - (double)realWorldInputsForCalc_CB[0].Y), 2) +
                                  Math.Pow(((double)realWorldInputsForCalc_CB[2].X - (double)realWorldInputsForCalc_CB[0].X), 2));

            //Part 3: Put the image plane transformed points into PointF
            /*  transformed[0].x = lat top left                       transformed[1].x = realworld[0].x
                transformed[0].y = long top left                      transformed[1].y = realworld[0].y + longDelta
            
                realworld[2].x = realworld[0].x + latDelta          realworld[0].y = long bott right
                realworld[2].y = realworld[0].y                     realworld[0].y = long bott right
             
        */
            transformedPtsForCalc_CB[0].X = realWorldInputsForCalc_CB[0].X;
            transformedPtsForCalc_CB[0].Y = realWorldInputsForCalc_CB[0].Y;

            transformedPtsForCalc_CB[1].X = realWorldInputsForCalc_CB[0].X;
            transformedPtsForCalc_CB[1].Y = realWorldInputsForCalc_CB[0].Y + (float)longDelta_CB;

            transformedPtsForCalc_CB[2].X = realWorldInputsForCalc_CB[0].X + (float)latDelta_CB;
            transformedPtsForCalc_CB[2].Y = realWorldInputsForCalc_CB[0].Y;

            transformedPtsForCalc_CB[3].X = transformedPtsForCalc_CB[2].X;
            transformedPtsForCalc_CB[3].Y = transformedPtsForCalc_CB[1].Y;

            //Part 4: Write the boundary points into existing algo methods
            upperLeftCoordsTransformed_CB[0] = transformedPtsForCalc_CB[0].X;
            upperLeftCoordsTransformed_CB[1] = transformedPtsForCalc_CB[0].Y;
            outerLimitCoordsTransformed_CB[0] = transformedPtsForCalc_CB[1].Y;
            outerLimitCoordsTransformed_CB[1] = transformedPtsForCalc_CB[2].X;

            //Part 5: get the tansform matrix 
            transformMatrix_CB =  CvInvoke.GetPerspectiveTransform(realWorldInputsForCalc_CB, transformedPtsForCalc_CB);

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

        private void eeceDemo_Click(object sender, EventArgs e)
        {
            //Test Suite for co-ord checks - Howard College Traversal With Google Earth
            //bounds UpperLeft: Lat -29.866178; Long 30.981370
            //bounds UpperRight Long 30.983583
            //bounds BottomLeft Lat -29.866673
            //goes up, right off, across main, left off, across right and right off, and back up out top

            longUpperLeftTextbox.Text = "30.980520";
            longBottomLeftTextbox.Text = "30.980655";

            longUpperRightTextbox.Text = "30.980600";
            longBottomRightTextbox.Text = "30.980740";

            latUpperLeftTextbox.Text = "-29.868200";
            latUpperRightTextbox.Text = "-29.868075";

            latBottomLeftTextbox.Text = "-29.868248";
            latBottomRightTextbox.Text = "-29.868130";

            videoFormatCombobox.SelectedIndex = 0;

            drawModeChoiceComboBox.SelectedIndex = 2;
            vidSourceChoiceComboBox.SelectedIndex = 1;
            //listOfIps.Items.Add("192.168.0.9");
            //listOfIps.Items.Add("192.168.0.10");
            ///listOfIps.Items.Add("192.168.135.104");
            //checkIpAddrButton.Enabled = true;
        }


        private void gardenDemo_Click_1(object sender, EventArgs e)
        {
            //Test Suite for co-ord checks - Howard College Traversal With Google Earth
            //bounds UpperLeft: Lat -29.866178; Long 30.981370
            //bounds UpperRight Long 30.983583
            //bounds BottomLeft Lat -29.866673
            //goes up, right off, across main, left off, across right and right off, and back up out top

            longUpperLeftTextbox.Text = "30.984135";
            longBottomLeftTextbox.Text = "30.984010";

            longUpperRightTextbox.Text = "30.984162";
            longBottomRightTextbox.Text = "30.984040";

            latUpperLeftTextbox.Text = "-29.866585";
            latUpperRightTextbox.Text = "-29.866613";

            latBottomLeftTextbox.Text = "-29.866591";
            latBottomRightTextbox.Text = "-29.866623";

            videoFormatCombobox.SelectedIndex = 0;

            drawModeChoiceComboBox.SelectedIndex = 2;
            vidSourceChoiceComboBox.SelectedIndex = 1;
            //listOfIps.Items.Add("192.168.0.9");
            //listOfIps.Items.Add("192.168.0.10");
            ///listOfIps.Items.Add("192.168.135.104");
            //checkIpAddrButton.Enabled = true;
        }
    }
}

