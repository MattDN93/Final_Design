﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using DirectShowLib;

namespace TEST_GPS_Parsing
{
    public partial class VideoOutputWindow : Form
    {
        #region Declaration Objects and Vars
        //-----------------Matrix and Capture Objects----------
        public Mat webcamVid;                  //create a Mat object to manipulate
        public Mat overlayVid;

        public Capture[] camStreamCaptureArray;                          //array to hold all camera capture objects
        
        //------------------DirectShow Capture Objects--------------
        public DsDevice[] _SystemCameras;
        public Video_Device[] WebCams;
        int CameraDevice;
        Capture _capture;

        public struct Video_Device
        {
            public string Device_Name;
            public int Device_ID;
            public Guid Identifier;

            public Video_Device(int ID, string Name, Guid Identity = new Guid())
            {
                Device_ID = ID;
                Device_Name = Name;
                Identifier = Identity;
            }

            public override string ToString()
            {
                return String.Format("[{0}] {1}: {2}", Device_ID, Device_Name, Identifier);
            }
        } 

        
        private Capture camStreamCapture = null;        //the current OpenCV capture stream
        //private Capture cscLeft = null;                  //left camera capture
        //private Capture cscRight = null;                 //right camera capture
        //private Capture cscCentre = null;                //centre camera capture 
        private Overlay ol_mark;                //object for the overlay of points

        protected string latitudeOutOfRangeOverlayMessage = "";      //strings for the overlay class to write into
        protected string longitudeOutOfRangeOverlayMessage = "";     //if a variable is offscreen

        //-----------------Capture setup object----------------
        public CameraBoundsSetup setup;                 //uninitialized cam bounds setup object

        //-----------------Video writer object for logging----------------
        public VideoWriter videoWriterOutput;
        public string videoLogFilename;
        private static int initialSetup = 0;        //ints to determine videoWriterSetup behaviour
        private static int fromButtonRepress = 1;
        private static int autoFileSave = 2;

        //-----------------User options and parameters---------
        public int captureChoice;              //user's selection of which capture to use
        public int drawMode_Overlay;
        private string fileName;

        //-----------------Status flags and parameters----------
        protected bool capStartSuccess;           //whether the capture was opened OK
        protected bool isStreaming;               //whether stream is in progress
        protected bool randomSim;                 //using random simulation mode or ordered
        protected bool valHasChanged;             //for updating the marker
        protected bool stillSwitchingCams = false;        //to ensure no crashing when switching camera
        public bool grabResult;                  //result of grabbing a frame, for disconnection detection
        int disconnectCounter = 0;                  //check if disconnected and if so do a full reset
        public bool logCamSwitchToDb = false;       //for signalling a write required to the DB
        bool vidLogWriteOK;                       //for signalling the video writer

        static EventWaitHandle _waitCameraRightSwitch = new AutoResetEvent(true);       //wait handle to prevent threads switching cameras rapidly
        static EventWaitHandle _waitCameraLeftSwitch = new AutoResetEvent(true);

        private System.Threading.Semaphore uiUpdateSemaphone = new Semaphore(1,3); //mutex lock to prevent UI update race conditions
        private Object uiWriteLock = new object();
        bool writingToUi = false;

        //-----------------Settings parameters------------------
        protected int vidPixelWidth;              //video dimensions
        protected int vidPixelHeight;

        //-----------------Camera GPS boundary variables--------
        //struct allows upperLeftBound & outerLimitBound to exist for all camera frames
        //Choose number of cameras below
        public static int totalCameraNumber = 5;

        struct camBound     //struct for one camera
        {
            public double[] upperLeftBound;         //[0] = latitude top left; [1] = longitude top left
            public double[] outerLimitBound;        //[0] = longitude top right; [1] = latitude bottom left
            public double delta_y;                                  //range of latitude in camera frame
            public double delta_x;                                  //range of longitude in camera frame

            //constructor to setup camBound structs
            public camBound(double olb, double ulb)
            {
                upperLeftBound = new double[2];
                outerLimitBound = new double[2];
                delta_x = 0.0;
                delta_y = 0.0;
            }

        }

        public Mat transformMatrix;                 //the matrix to transform the points from the world to image planes

        public int currentlyActiveCamera = -1;

        /*array to hold all camera structs, add new ones for expansion later
            currently 3 cameras 0=left 1=centre 2=right*/
        private camBound[] camBoundArray;


        //-----------------Delegate for thread safe controls----
        public delegate void SetTextCallback(string message);   //to set the Video UI elements with thread safe operations
        private int type = -1;                                  //to call teh correct invoke to put the data on the UI in a thread safe way
        private int type_2 = -1;
        //-------SIMULATION
        public static int DRAW_MODE_RANDOM = 0;
        public static int DRAW_MODE_ORDERED = 1;
        public static int DRAW_MODE_TRACKING = 2;
        public static int DRAW_MODE_REVOBJTRACK = 3;

        #endregion

        #region Initializer and Setup
        public VideoOutputWindow()                              //this is called before the window is even launched
        {
            InitializeComponent();
            //initialise the bounds and capture arrays - prevent NullReference Exceptions later    
            //# cameras can be altered in code if needed     
           // camBoundArray = new camBound[totalCameraNumber];
           // camStreamCaptureArray = new Capture[totalCameraNumber];

            
        }

        private void VideoOutputWindow_Load(object sender, EventArgs e)
        {
            //wait for user to setup bounds before allowing capture to start
            startCaptureButton.Enabled = false;
            status1TextBox.Text = "Click 'Setup Capture' to begin.";
            overlayVideoFramesBox.Visible = false;
            setupInstructLabel.Visible = true;
            pausedCaptureLabel.Visible = false;

        }
        #endregion

        #region Update Info from bounds setup form

        private bool receiveSetupInfo()
        {
            /*the setup parameters have been set by the CameraBOunds Setup window 
                *The parameters are copied across before the window is disposed.
                *Any further use of the variables are the ones local to this class
            New parameters can be requested anytime by reopening the window and re-copying the data
             
             */

            CvInvoke.UseOpenCL = false;
            try
            {
                //---------get capture properties------
                drawMode_Overlay = setup.drawMode_CB;
                captureChoice = setup.videoSource_CB;
                fileName = setup.filenameToOpen_CB;

                //---------get video writer properties------
                setupVideoWriter(initialSetup);

                //---------get capture objects---------
                //get as many capture objects as cameras active

                //----------------TEST CODE--------------------
                WebCams = setup.camStreamCaptureArray_CB;
                _SystemCameras = setup._SystemCameras_CB;
                CameraDevice = setup.CameraDevice_CB;
                _capture = setup._capture_CB;

                camBoundArray = new camBound[WebCams.Length];
                for (int i = 0; i < camBoundArray.Length; i++)
                {
                    camBoundArray[i] = new camBound(0.0, 0.0);
                }

                //Test code configuration steps with current code
                camStreamCapture = _capture;
                //----------------END TEST CODE----------------

                //----------------ORIGINAL CODE----------------
                //for (int i = 0; i < totalCameraNumber; i++)
                //{
                //    camStreamCaptureArray[i] = setup.camStreamCaptureArray_CB[i];
                //}

                //camStreamCapture = setup.currentCamStreamCapture_CB;
                //---------------END ORIGINAL CODE-------------

                //---------get capture extents---------
                // get by default from centre cam (camArray[1])
                for (int i = 0; i <= 1; i++)
                {
                    camBoundArray[1].upperLeftBound[i] = setup.upperLeftCoordsTransformed_CB[i];
                    camBoundArray[1].outerLimitBound[i] = setup.outerLimitCoordsTransformed_CB[i];
                }

                //bring across the transformation matrix to do the point plane conversion (gps real world to image plane)
                transformMatrix = setup.transformMatrix_CB;

                //---------update the UI initially with info--
                //by default first camera is the centre (camBound[1])
                camBoundUIDisplaySetup(1);
                currentlyActiveCamera = 1;

                //use the class-local methods now
                camStreamCapture.ImageGrabbed += parseFrames;   //the method for new frames
                webcamVid = new Mat();                          //create the webcam mat object

                return true;
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show("A capture open error occurred: " + excpt.Message);
                return false;
            }
            
        }

        private void setupVideoWriter(int mode)
        {
            //this method is being called at form initialisation, so get data from bounds window
            if(mode == initialSetup)
            {
                videoLogFilename = setup.videoLogFilename_CB;
                videoWriterOutput = setup.videoWriterOutput_CB;
            }
            else if (mode == fromButtonRepress) //this method was called by repeated pressed of Stop/Start after capture was setup so re-init writer
            {
                if (videoLogFilename!= null) //account for initial "start" press when object already setup
                {
                    return;
                }
                else //create a whole new object since user has pressed stop before
                {
                    videoLogFilename = "videoLogFile" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    videoWriterOutput = new VideoWriter(videoLogFilename + ".mkv", //File name
                                            VideoWriter.Fourcc('M', 'P', '4', '2'), //Video format
                                            20, //FPS
                                            new Size(640, 480), //frame size
                                            true); //Color
                }
            }
            else if (mode == autoFileSave) //the auto file save option
            {
                videoWriterOutput.Dispose();        //this closes the video file & makes it playable
                videoLogFilename = "videoLogFile" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                videoWriterOutput = new VideoWriter(videoLogFilename + ".mkv", //File name
                                        VideoWriter.Fourcc('M', 'P', '4', '2'), //Video format
                                        20, //FPS
                                        new Size(640, 480), //frame size
                                        true); //Color
            }
        }

        #endregion

        #region Frame and Overlay Methods
        //this method is called every time the IsGrabbed event is raised i.e. every time a new frame is captured
        private void parseFrames(object sender, EventArgs arg)
        {
            //Mat webcamVid = new Mat();
            try
            {
                if (!rawVideoFramesBox.IsDisposed && isStreaming)      //make sure not to grab a frame if the window is closig
                {
                    //set camStreamCapture (the central selected camera) to the correct cam (switch if needed) based on co-ord bound checks
                    //only switch capture objects if a GPS value has been updated!
                    if (valHasChanged)
                    {
                        switch (ol_mark.camSwitchStatus)
                        {
                            case -1: break;     //the feed need not be changed from the current one (since an error has occurred / value wasn't changed from default)
                            case 0: logCamSwitchToDb = true;  currentlyActiveCamera = screenStateSwitch(0, currentlyActiveCamera); webcamVid = new Mat(); break;  //switch the current camera to the left
                            case 1: logCamSwitchToDb = true; currentlyActiveCamera = screenStateSwitch(1, currentlyActiveCamera); webcamVid = new Mat(); break; //switch the current camera to the right
                            case 2: break;      //the feed need not be changed from the current one (since the value is in this screen)
                            default:
                                break;
                        }
                        ol_mark.camSwitchStatus = 2;        //reset to stay on current cam
                    }

                    grabResult = camStreamCapture.Retrieve(webcamVid, 0);
                    //rawVideoFramesBox.Image = webcamVid;                                       

                    //Now draw the markers on the overlay and display 
                    //this method sends a FALSE since the image updates 24+ times per second but the data only arrives ~once per second from GPS
                    //This means the check and update of marker is only done on the TIMER TICK every 500ms, otherwise the overlay is just redrawn
                    bool returnVal = false;
                    if (drawMode_Overlay != DRAW_MODE_REVOBJTRACK)
                    {
                        returnVal = ol_mark.drawMarker(ol_mark.x, ol_mark.y, webcamVid, false, currentlyActiveCamera);     //draw marker from external input of coords
                    }
                    else
                    {
                        if (valHasChanged)                                          //as long as timer tick, update the marker - gives persistence of markers without cluttering screen
                        {
                            returnVal = ol_mark.drawPolygons(webcamVid, true);
                        }
                        else
                        {
                            returnVal = ol_mark.drawPolygons(webcamVid, false);                                //draw marker from onscreen tracking
                        }
                    } 


                    if (returnVal == true)
                    {
                        try
                        {
                            overlayVideoFramesBox.Image = ol_mark.overlayGrid;
                           // FOR DEBUGGING MEMORY EXCEPTIONS ThrowMemoryException("Memory");
                        }
                        catch (OutOfMemoryException)
                        {
                            //videoWriterOutput.Dispose();
                            MessageBox.Show("Error! Your system has run out of memory.\n" +
                                "Close some programs and try capturing again.\n" +
                                "Don't worry, video was logged right up to this point and has been saved.\n" +
                                "To restart capture, close any memry-intensive programs, and click 'Stop Capture' once.","Capture halted",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            isStreaming = false;
                        }
                       
                        
                        if (isStreaming /*&& vidLogWriteOK*/) //the commented part allows for dropped frames for condensed filesizes
                        {
                            vidLogWriteOK = false;
                            try
                            {
                                videoWriterOutput.Write(ol_mark.overlayGrid);         //write that frame to the video log file
                                // FOR DEBUGGING ONLY ThrowGenericException("Error");
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.InnerException.ToString() + "\n Try restarting capture.", "Something Happened! Video was saved up until this point", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                        }
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Capturing failed. Reason: " + e.Message, "Something happened!", MessageBoxButtons.OK, MessageBoxIcon.Error,MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                isStreaming = false;
                startCaptureButton.Text = "(Re)Start Capture";
                ReleaseData();      //something failed so release the data
                return;
            }

        }

        private void SetupCapture(int Camera_Identifier)
        {
            //update the selected device
            CameraDevice = Camera_Identifier;
            if (captureChoice == 0)     //this capture method is for the webcams
            {
                                                // Dispose of Capture if it was created before
                if (camStreamCapture != null) camStreamCapture.Stop(); camStreamCapture.Dispose();
                try
                {
                    //Set up capture device
                    camStreamCapture = new Capture(CameraDevice);
                    camStreamCapture.ImageGrabbed += parseFrames;   //the method for new frames
                    camStreamCapture.Start();
                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
                catch (Exception e)
                {
                    //try avoid the fast-switch issue
                    camStreamCapture.Stop(); camStreamCapture.Dispose();
                    camStreamCapture = null;
                    status1TextBox.Text = "Error initialising camera...Trying again...";
                    Thread.Sleep(500);
                    SetupCapture(CameraDevice);
                }
            }
            else if (captureChoice == 1)    //this methd is for the IP cameras
            {
                // Dispose of Capture if it was created before
                if (camStreamCapture != null) camStreamCapture.Stop(); camStreamCapture.Dispose();
                try
                {                   
                    //Set up capture device
                    camStreamCapture = new Capture(WebCams[CameraDevice].Device_Name);  //we want to open a capture by URL which is stored with the cam URL
                    camStreamCapture.ImageGrabbed += parseFrames;   //the method for new frames
                    camStreamCapture.Start();
                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
                catch (Exception e)
                {
                    //try avoid the fast-switch issue
                    camStreamCapture.Stop(); camStreamCapture.Dispose();
                    camStreamCapture = null;
                    status1TextBox.Text = "Error initialising camera...Trying again...";
                    Thread.Sleep(500);
                    SetupCapture(CameraDevice);
                }
            }
            
          //  stillSwitchingCams = true;   

            
           // Thread.Sleep(1500);
           // stillSwitchingCams = false;
        }

        private int screenStateSwitch(int switchCase, int activeCamLocal)
        {
            if (isStreaming)
            {
                //switchCase = 0 => switch left; switchCase = 1 => switch right

                if (switchCase == 0)        //means a switch left is requested
                {
                    //switch coordinates
                    if (activeCamLocal - 1 >= 0)       //don't go left if you can't
                    {
                        
                        double dx_test = camBoundArray[activeCamLocal].delta_x;
                        //------------modify camera GPS bounds-----------
                        //setup left cam's upper right longitude bound
                        camBoundArray[activeCamLocal - 1].outerLimitBound[0] =
                            camBoundArray[activeCamLocal].upperLeftBound[1];

                        //setup left cam's latitude (no change)
                        camBoundArray[activeCamLocal - 1].outerLimitBound[1] =
                            camBoundArray[activeCamLocal].outerLimitBound[1];

                        //setup upper left latitude (no change)
                        camBoundArray[activeCamLocal - 1].upperLeftBound[0] =
                            camBoundArray[activeCamLocal].upperLeftBound[0];

                        //setup upper left longitude
                        camBoundArray[activeCamLocal - 1].upperLeftBound[1] =
                            camBoundArray[activeCamLocal].upperLeftBound[1] 
                            - dx_test/*camBoundArray[currentlyActiveCamera].delta_x*/;

                        //------------modify actual capture objects---------
                        //make the camera one screen to the left the new active camera
                        //----------------Capture object switch--------------
                        //halt fast switching threads
                        _waitCameraRightSwitch.WaitOne(5000);
                        SetupCapture(activeCamLocal - 1);
                        _waitCameraLeftSwitch.Set();
                        //----------------------END TEST CODE-----------
                        //----------------------ORIGINAL CODE COMMENTED OUT------
                        //camStreamCaptureArray[currentlyActiveCamera] = camStreamCapture;
                        //camStreamCaptureArray[currentlyActiveCamera - 1].Start();
                        //camStreamCapture = camStreamCaptureArray[currentlyActiveCamera - 1];
                        //----------------Coordinate bounds switch-----------

                        activeCamLocal--;            //switch camera index one left
                        camBoundUIDisplaySetup(activeCamLocal);  //call UI update to show new bounds onscreen

                        ////halt all other camera frames for resource purposes
                        //for (int i = 0; i < camStreamCaptureArray.Length; i++)
                        //{
                        //    if (i != currentlyActiveCamera - 1)
                        //    {
                        //        camStreamCaptureArray[i].Pause();
                        //    }
                        //}
                        //----------------------END ORIGINAL CODE OUT
                        return activeCamLocal;
                    }
                    else if (activeCamLocal - 1 == 0)   //we're already on the leftmost camera
                    {
                        return activeCamLocal;
                    }
                    
                    ////capture object comparison and switch

                    //if (camStreamCapture.Equals(cscLeft))    //if the current frame is already set to the left
                    //{
                    //    cscCentre.Stop();
                    //    cscRight.Stop();
                    //    return;     //keep leftmost camera frame
                    //}
                    //else if (camStreamCapture.Equals(cscCentre)) //if the current frame is set to the centre camera
                    //{
                    //    //----------------Capture object switch--------------
                    //    cscCentre = camStreamCapture;       //set the current stream to centre
                    //    camStreamCapture = cscLeft;         //switch camera left by one screen
                    //    cscCentre.Stop();
                    //    cscRight.Stop();
                    //    cscLeft.Start();
                    //    //----------------Coordinate bounds switch-----------
                    //    currentlyActiveCamera--;            //switch camera index one left
                    //    camBoundUIDisplaySetup(currentlyActiveCamera);  //call UI update to show new bounds onscreen
                    //}
                    //else if (camStreamCapture.Equals(cscRight))
                    //{
                    //    //----------------Capture object switch--------------
                    //    cscRight = camStreamCapture;        //set the current stream to right
                    //    camStreamCapture = cscCentre;       //switch camera left by one screen to centre
                    //    cscLeft.Stop();
                    //    cscRight.Stop();
                    //    cscCentre.Start();
                    //    //----------------Coordinate bounds switch-----------
                    //    currentlyActiveCamera--;            //switch camera index one left
                    //    camBoundUIDisplaySetup(currentlyActiveCamera);  //call UI update to show new bounds onscreen
                    //}

                }
                else if (switchCase == 1)       //means a switch right is requested
                {
                    //switch coordinates
                    if (activeCamLocal + 1 <= camBoundArray.Length)       //don't go right if you can't
                    {
                        //setup left cam's upper right longitude bound
                        camBoundArray[activeCamLocal + 1].outerLimitBound[0] =
                            camBoundArray[activeCamLocal].outerLimitBound[0] +
                            camBoundArray[activeCamLocal].delta_x;

                        //setup left cam's latitude (no change)
                        camBoundArray[activeCamLocal + 1].outerLimitBound[1] =
                            camBoundArray[activeCamLocal].outerLimitBound[1];

                        //setup upper left latitude (no change)
                        camBoundArray[activeCamLocal + 1].upperLeftBound[0] =
                            camBoundArray[activeCamLocal].upperLeftBound[0];

                        //setup upper left longitude
                        camBoundArray[activeCamLocal + 1].upperLeftBound[1] =
                            camBoundArray[activeCamLocal].outerLimitBound[0];

                        //------------modify actual capture objects---------
                        //make the camera one screen to the right the new active camera

                        //--------------TEST CODE-------
                        _waitCameraLeftSwitch.WaitOne(5000);
                        SetupCapture(activeCamLocal + 1);
                        _waitCameraRightSwitch.Set();
                        //--------------END TEST CODE-----
                        //-------------ORIGINAL CODE COMMENTED OUT------------
                        //camStreamCaptureArray[currentlyActiveCamera] = camStreamCapture;
                        //camStreamCapture = camStreamCaptureArray[currentlyActiveCamera + 1];
                        //camStreamCaptureArray[currentlyActiveCamera + 1].Start();

                        //halt all other camera frames for resource purposes
                        //for (int i = 0; i < camStreamCaptureArray.Length; i++)
                        //{
                        //    if (i != currentlyActiveCamera + 1)
                        //    {
                        //        camStreamCaptureArray[i].Pause();
                        //    }
                        //}
                        //-----------------END ORIGINAL CODE--------------
                        activeCamLocal++;                        //indicate we've switched one camera right                   
                        camBoundUIDisplaySetup(activeCamLocal);  //call UI update to show new bounds onscreen
                        return activeCamLocal;
                    }
                    else if (activeCamLocal + 1 > camBoundArray.Length)   //we're already on the leftmost camera
                    {
                        return activeCamLocal;
                    }

                    //if (camStreamCapture.Equals(cscRight))    //if the current frame is already set to the right
                    //{
                    //    cscCentre.Stop();
                    //    cscLeft.Stop();
                    //    return;     //keep rightmost camera frame
                    //}
                    //else if (camStreamCapture.Equals(cscCentre)) //if the current frame is set to the centre camera
                    //{
                    //    //----------------Capture object switch--------------
                    //    cscCentre = camStreamCapture;       //set current stream to centre
                    //    camStreamCapture = cscRight;         //switch camera right by one screen
                    //    //cscCentre.Stop();
                    //    //cscLeft.Stop();
                    //    //cscRight.Start();
                    //    //camStreamCapture.Start();

                    //    //----------------Coordinate bounds switch-----------
                    //    currentlyActiveCamera++;            //switch camera index one right
                    //    camBoundUIDisplaySetup(currentlyActiveCamera);  //call UI update to show new bounds onscreen
                    //}
                    //else if (camStreamCapture.Equals(cscLeft)) //if curent frame is set to the left
                    //{
                    //    //----------------Capture object switch--------------
                    //    cscLeft = camStreamCapture;
                    //    camStreamCapture = cscCentre;       //switch camera right by one screen to centre
                    //    //cscLeft.Stop();
                    //    //cscCentre.Start();
                    //    //camStreamCapture.Start();
                    //    //cscRight.Stop();

                    //    //----------------Coordinate bounds switch-----------
                    //    currentlyActiveCamera++;            //switch camera index one right
                    //    camBoundUIDisplaySetup(currentlyActiveCamera);  //call UI update to show new bounds onscreen
                    //}


                }
            }
            switchCase = 2;        //set back to "current"
            return activeCamLocal;
        }

        #endregion

        #region Timer ticks
        //checks to see if the frames are different to each other 10 seconds ago, if so, it's still moving LOL
        private void cameraDisconnectCheck_Tick(object sender, EventArgs e)
        {
            if (isStreaming)
            {
                if (grabResult == false)        //something has failed with the capture
                {
                    camDisconnectedWarningLabel.Visible = true;
                    grabResult = true;
                    SetupCapture(currentlyActiveCamera);
                    switch (ol_mark.camSwitchStatus)
                    {
                        case -1: break;     //the feed need not be changed from the current one (since an error has occurred / value wasn't changed from default)
                        case 0: logCamSwitchToDb = true; currentlyActiveCamera = screenStateSwitch(0, currentlyActiveCamera); webcamVid = new Mat(); break;  //switch the current camera to the left
                        case 1: logCamSwitchToDb = true; currentlyActiveCamera = screenStateSwitch(1, currentlyActiveCamera); webcamVid = new Mat(); break; //switch the current camera to the right
                        case 2: break;      //the feed need not be changed from the current one (since the value is in this screen)
                        default:
                            break;
                    }
                    ol_mark.camSwitchStatus = 2;        //reset to stay on current cam

                }
                else
                {
                    camDisconnectedWarningLabel.Visible = false;
                }
                if (disconnectCounter > 1) //this covers the case where the parse method does not run, in which case grabResult isn't avaialble
                {
                    disconnectCounter = 0;
                    return;                 //nothing to do, capture is OK
                }
                //else if (disconnectCounter < 1)
                //{
                   // camDisconnectedWarningLabel.Visible = true;
                    //SetupCapture(currentlyActiveCamera);
                    //switch (ol_mark.camSwitchStatus)
                    //{
                    //    case -1: break;     //the feed need not be changed from the current one (since an error has occurred / value wasn't changed from default)
                    //    case 0: logCamSwitchToDb = true; currentlyActiveCamera = screenStateSwitch(0, currentlyActiveCamera); webcamVid = new Mat(); break;  //switch the current camera to the left
                    //    case 1: logCamSwitchToDb = true; currentlyActiveCamera = screenStateSwitch(1, currentlyActiveCamera); webcamVid = new Mat(); break; //switch the current camera to the right
                    //    case 2: break;      //the feed need not be changed from the current one (since the value is in this screen)
                    //    default:
                    //        break;
                    //}
                    //ol_mark.camSwitchStatus = 2;        //reset to stay on current cam


                    //disconnectCounter = 0;
                //}

            }
            else
            {
                camDisconnectedWarningLabel.Visible = false;
            }

            
        }
        //checks to see if the camera (or any others) have been disconnected
        private void videoSaveTimer_Tick(object sender, EventArgs e)
        {
            vidLogWriteOK = true;
            if (isStreaming)
            {
                setupVideoWriter(autoFileSave);         //save current capture file
            }


        //    if (isStreaming)
        //    {
        //        Mat testFrame1 = new Mat();
        //        testFrame1 = camStreamCapture.QueryFrame();
        //        Mat testFrame2 = new Mat(vidPixelHeight, vidPixelWidth, DepthType.Cv8U, 3);
        //        testFrame2 = camStreamCapture.QueryFrame();
        //        if (CvInvoke.Norm(testFrame1, testFrame2) == 0.0)
        //        {
        //            Console.Write("Connection Error on current camera!");
        //        }
        //    }

        }

        //This recalculates the incoming datapoints once every 500ms since GPS data only arrives that often
        private void refreshOverlay_Tick(object sender, EventArgs e)
        {
                overlayTick();      //call the overlay update without passing GPS coords    
        }

        public void overlayTick(double incoming_lat= 0.0, double incoming_long = 0.0)
        {
            if (ol_mark != null)
            {

                if (incoming_lat != 0.0 && incoming_long != 0.0)    //this means that this method was called from the parser not by the timer above (so value has changed)
                {
                    valHasChanged = true;       //we're deceiving new data from the parser
                    bool varsInUse = false;
                    //while (!varsInUse)
                    //{
                        try
                        {
                            varsInUse = ol_mark.setNewCoords(incoming_lat, incoming_long, transformMatrix);      //set coords if not being read from/written to
                        }
                        catch (OverflowException)
                        {
                        type = 4;
                            setTextonVideoUI("Arithmetic error in calculating bounds - did you set them correctly?");       //use thread safe var access
                        type = -1;
                        
                        //return;
                    }
                        
                    //}
                    //now we draw the marker with the updated point coords
                    bool returnVal = false;
                    if (drawMode_Overlay != DRAW_MODE_REVOBJTRACK)
                    {
                        returnVal = ol_mark.drawMarker(ol_mark.x, ol_mark.y, webcamVid, true, currentlyActiveCamera);     //draw marker from external input of coords
                    }
                    if (returnVal == true && isStreaming)              //if the marker routine returned OK, draw the result in the video window
                    {
                            overlayVideoFramesBox.Image = ol_mark.overlayGrid;                       
                    }
                }
                else
                {


                    //this is just a normaltimer tick from the refreshOverlay_Tick method and it's likely values haven't  changed. Thus just redraw the overlay without recalc
                    bool returnVal = false;
                    if (drawMode_Overlay != DRAW_MODE_REVOBJTRACK)
                    {
                        valHasChanged = false;
                        returnVal = ol_mark.drawMarker(ol_mark.x, ol_mark.y, webcamVid, false, currentlyActiveCamera);     //draw marker from external input of coords
                    }
                    else
                    {
                        valHasChanged = !valHasChanged;           //let the polygons redraw every 500ms
                        ////returnVal = ol_mark.drawPolygons(webcamVid, true);                                //draw marker from onscreen tracking
                        ////returnVal = ol_mark.drawMarker(ol_mark.x, ol_mark.y, ol_mark.overlayGrid, false);
                    }

                    if (returnVal == true && isStreaming)              //if the marker routine returned OK, draw the result in the video window
                    {
                        overlayVideoFramesBox.Image = ol_mark.overlayGrid;
                    }
                }

                //prevents the double lat/long variables from being modified
                type = 0;
                double realLat = ol_mark.current_pointGPS_lat;  //gets the actual lat/long as opposed to the scaled versions of pixels for screen
                setTextonVideoUI(realLat.ToString()); //we need to do this to prevent the marshall by ref warning

                type = 1;
                double realLong = ol_mark.current_pointGPS_long;
                setTextonVideoUI(realLong.ToString()); //we need to do this to prevent the marshall by ref warning

                type = 2;
                setTextonVideoUI(ol_mark.latitudeOutOfRangeOverlayMessage);

                type = 3;
                setTextonVideoUI(ol_mark.longitudeOutOfRangeOverlayMessage);
                type = -1;

            }
        }

        #endregion

        #region UI and Button Routines - Setup Button

        private void setupCaptureButton_Click(object sender, EventArgs e)
        {
            //We assume that clicking the setup capture button resets all video preferences

            ReleaseData();              //dispose of any previous capture objects
            isStreaming = false;        

                this.setup = new CameraBoundsSetup(this);      //pass itself as an object to manipulate
                DialogResult setupResult = setup.ShowDialog();             //pass the videoOutput object to allow settings to be set and passed back

                //respond based on the result of the dialog
                if (setupResult == DialogResult.OK || setupResult == DialogResult.Yes)
                {
                    //get the new data from the setup UI
                    bool receivedOK = receiveSetupInfo();
                    if (receivedOK)
                    {
                        startCaptureButton.Enabled = true;
                        setup.Close();
                        setup.Dispose();        //dispose of setup
                    }
                    else
                    {
                        startCaptureButton.Enabled = false;
                        MessageBox.Show("One or more camera setup parameters weren't saved correctly. Please launch setup again.", "Camera setup error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                    
                }
                else if (setupResult == DialogResult.Cancel || setupResult == DialogResult.No)        //if setup process was prematurely cancelled
                {
                    status1TextBox.Clear();
                    status1TextBox.Text = "Setup was cancelled.";
                    startCaptureButton.Enabled = false;         //first time setup, require settings before capture
                    setup.Close();
                    setup.Dispose();                               //clear object because it wasn't setup properly
                }

        }


        #endregion

        #region Setting text on Video UI & capture start

        private void setTextonVideoUI(string text)
        {
            //Update the UI with information -check threadsafe since the other thread might be using it
            // If the calling thread is different from the thread that
            // created the TextBox control, this method creates a
            // SetTextCallback and calls itself asynchronously using the
            // Invoke method.
            if (isStreaming)        //accounts for lat/long setting before streaming starts
            {

                switch (type)
                {
                    case 0:
                        if (this.latitudeLabel.InvokeRequired)
                        {
                            SetTextCallback l = new SetTextCallback(setTextonVideoUI);
                            Invoke(l, new object[] { text });
                        }
                        else
                        {
                                this.latitudeLabel.Text = text;

                        }; break;
                    case 1:
                        if (this.LongitudeLabel.InvokeRequired)
                        {
                            SetTextCallback l = new SetTextCallback(setTextonVideoUI);
                            Invoke(l, new object[] { text });
                        }
                        else {
                                this.LongitudeLabel.Text = text;

                        }; break;
                    case 2:
                        if (this.latOORStatusBox.InvokeRequired)
                        {
                            SetTextCallback l = new SetTextCallback(setTextonVideoUI);
                            Invoke(l, new object[] { text });
                        }
                        else
                        {
                                this.latOORStatusBox.Text = text;
                        }; break;
                    case 3:
                        if (this.longOORTextBox.InvokeRequired)
                        {
                            SetTextCallback l = new SetTextCallback(setTextonVideoUI);
                            Invoke(l, new object[] { text });
                        }
                        else
                        {
                                this.longOORTextBox.Text = text;
                        }; break;
                    case 4:
                        if (this.status1TextBox.InvokeRequired)
                        {
                            SetTextCallback l = new SetTextCallback(setTextonVideoUI);
                            Invoke(l, new object[] { text });
                        }
                        else
                        {
                                this.status1TextBox.Text = text;
                        }; break;

                    default:
                        Console.Write("Couldn't update one or more UI elements with cross-thread call");
                        break;
                }

                type = -1;
            }

            switch (type_2) //this section is used for updating screen bounds display
            {
                case 3:
                    if (this.frameHeightLabel.InvokeRequired)
                    {
                        SetTextCallback l = new SetTextCallback(setTextonVideoUI);
                        Invoke(l, new object[] { text });
                    }
                    else { this.frameHeightLabel.Text = text; }; break;
                case 4:
                    if (this.frameWidthLabel.InvokeRequired)
                    {
                        SetTextCallback l = new SetTextCallback(setTextonVideoUI);
                        Invoke(l, new object[] { text });
                    }
                    else { this.frameWidthLabel.Text = text; }; break;
                case 5:
                    if (this.latTopLeftLabel.InvokeRequired)
                    {
                        SetTextCallback l = new SetTextCallback(setTextonVideoUI);
                        Invoke(l, new object[] { text });
                    }
                    else { this.latTopLeftLabel.Text = text; }; break;
                case 6:
                    if (this.longTopLeftLabel.InvokeRequired)
                    {
                        SetTextCallback l = new SetTextCallback(setTextonVideoUI);
                        Invoke(l, new object[] { text });
                    }
                    else { this.longTopLeftLabel.Text = text; }; break;
                case 7:
                    if (this.longTopRightLabel.InvokeRequired)
                    {
                        SetTextCallback l = new SetTextCallback(setTextonVideoUI);
                        Invoke(l, new object[] { text });
                    }
                    else { this.longTopRightLabel.Text = text; }; break;
                case 8:
                    if (this.latBotLeftLabel.InvokeRequired)
                    {
                        SetTextCallback l = new SetTextCallback(setTextonVideoUI);
                        Invoke(l, new object[] { text });
                    }
                    else { this.latBotLeftLabel.Text = text; }; break;
                default: 
                    break;
            }

        }
                
        private void camBoundUIDisplaySetup(int camScreenNumber)
        {
            /* INFORMATION
             * This method is called once at setup (assuming the middle cam is active for demo)
             * It's recalled each time a screen switch occurs. 
             * If switching left, its DECREMENTED, switch right is INCREMENTED
             * 0 is the LEFTMOST camera (left of initial centre)                
             */

            //the drawMode, fileName and videoSource are copied from the other form
            //evaluates what the user chose from the bounds setup box
            switch (captureChoice)
            {
                case 0: videoModeLabel.Text = "Live Video"; break;
                case 1: videoModeLabel.Text = "Recorded Video"; break;
                default:
                    videoModeLabel.Text = "None Set";
                    break;
            }
            switch (drawMode_Overlay)
            {
                case 0: drawModeLabel.Text = "Random"; drawMode_Overlay = DRAW_MODE_RANDOM; break;
                case 1: drawModeLabel.Text = "Ordered"; drawMode_Overlay = DRAW_MODE_ORDERED; break;
                case 2: drawModeLabel.Text = "Tracking"; drawMode_Overlay = DRAW_MODE_TRACKING; break;
                case 3: drawModeLabel.Text = "Object-Based Tracking"; drawMode_Overlay = DRAW_MODE_REVOBJTRACK; break;
                default:
                    break;
            }

            getVideoInfo();                                         //call video info function


            //display the limits set on previous window for the current camera (assumed to be the centre)
            //upperLeftBound[0] = latitude top left; [1] = longitude top left
            //OuterLimitBound[0] = longitude top right; [1] = latitude bottom left
            //use threadsafe access

            //prevents the double lat/long variables from being modified
            type_2 = 3;
            setTextonVideoUI(vidPixelHeight.ToString());      //get the video parameters

            type_2 = 4;
            setTextonVideoUI(vidPixelWidth.ToString());

            type_2 = 5;
            setTextonVideoUI(camBoundArray[camScreenNumber].upperLeftBound[0].ToString());

            type_2 = 6;
            setTextonVideoUI(camBoundArray[camScreenNumber].upperLeftBound[1].ToString());

            type_2 = 7;
            setTextonVideoUI(camBoundArray[camScreenNumber].outerLimitBound[0].ToString());

            type_2 = 8;
            setTextonVideoUI(camBoundArray[camScreenNumber].outerLimitBound[1].ToString());

            type_2 = -1;

            //calculate the camera frame coordinate range - assume African co-ords; latitude always <0 longitude always >0
            camBoundArray[camScreenNumber].delta_y = Math.Abs(camBoundArray[camScreenNumber].outerLimitBound[1] - camBoundArray[camScreenNumber].upperLeftBound[0]);
            camBoundArray[camScreenNumber].delta_x = camBoundArray[camScreenNumber].outerLimitBound[0] - camBoundArray[camScreenNumber].upperLeftBound[1];

            if (camStreamCapture != null)
            {
                //ASSIGN THESE LIMITS TO THE OVERLAY CLASS TO WORK WITH THEM THERE
                ol_mark = new Overlay();                            //init the overlay object
                ol_mark.setupOverlay();                             //setup the overlay
                ol_mark.gridWidth = vidPixelWidth;                  //pass these variables to the other class
                ol_mark.gridHeight = vidPixelHeight;
                ol_mark.setupPredictionLimits();                    //setup the prediction point limits
                ol_mark.dx = camBoundArray[camScreenNumber].delta_x;
                ol_mark.dy = camBoundArray[camScreenNumber].delta_y;

                if (drawMode_Overlay == DRAW_MODE_REVOBJTRACK)      //the (x,y) coords aren't drawn onscreen if using the object based tracking
                {
                    ol_mark.displayCoordTextOnscreen = false;
                } 
                else
                {
                    ol_mark.displayCoordTextOnscreen = true;
                }

                for (int i = 0; i <= 1; i++)
                {
                    ol_mark.ulBound[i] = camBoundArray[camScreenNumber].upperLeftBound[i];
                    ol_mark.olBound[i] = camBoundArray[camScreenNumber].outerLimitBound[i];
                }

                valHasChanged = false;

            }
        }

        private void startCaptureButton_Click(object sender, EventArgs e)
        {
            if (isStreaming)
            {  //stop the capture
                isStreaming = false;
                setupCaptureButton.Enabled = true;
                startCaptureButton.Text = "Start Capture";
                startCaptureButton.Enabled = false;     
                pausedCaptureLabel.Visible = true;
                camStreamCapture.Pause();
                ol_mark.clearScreen();      //remove the marker and lines off the screen.
                videoSaveTimer.Stop();        //pause the disconnection check timer                
                videoLogFilename = null;            //resets filename
                videoWriterOutput.Dispose();        //closes the file
                startCaptureButton.Enabled = true;
            }
            else
            {
                //start the capture
                overlayVideoFramesBox.Visible = true;   //show the output frames box
                setupInstructLabel.Visible = false;     //hide the setup instruction label
                pausedCaptureLabel.Visible = false;     //hide the paused label (for if capture was already on)
                setupCaptureButton.Enabled = false;     //don't allow settings to be changed during capture
                videoSaveTimer.Start();           //start the disconnection check timer
                startCaptureButton.Text = "Stop Capture";
                setupVideoWriter(fromButtonRepress);                     //create new video Log file
                try
                {
                  
                   camStreamCapture.Start();
                   isStreaming = true;
                    
                }
                catch (Exception re)
                {
                   MessageBox.Show("Capturing still failed. Try again later. Reason: " + re.Message, "Something happened!", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error,MessageBoxDefaultButton.Button1 ,(MessageBoxOptions)0x40000);
                   isStreaming = false;
                   startCaptureButton.Text = "(Re)Start Capture";
                   ReleaseData();      //something failed so release the data
                   return;
                 }
              }

        }

        #endregion

        #region Close & release methods 
        public void ReleaseData()
        {
            if (camStreamCapture != null) { camStreamCapture.Stop(); camStreamCapture.Dispose();}

            //stop each camera capture instance
            for (int i = 0; i < totalCameraNumber ; i++)
            {
                //this method might be called at the start of execution so check for null references in case
                if (camStreamCaptureArray != null && camStreamCaptureArray[i] != null)
                {
                    camStreamCaptureArray[i].Stop();
                    camStreamCaptureArray[i].Dispose();
                }
            }
            //if (cscLeft != null) { cscLeft.Stop();  cscLeft.Dispose(); }
            //if (cscRight != null) { cscRight.Stop();  cscRight.Dispose(); }
            //if (cscCentre != null) { cscCentre.Stop();  cscCentre.Dispose(); }
            //rawVideoFramesBox.Dispose();
            videoSaveTimer.Stop();
        }

        private void getVideoInfo()            //get the video properties
        {
            vidPixelHeight = (int)camStreamCapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight);
            vidPixelWidth = (int)camStreamCapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth);
        }

        private void VideoOutputWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isStreaming)
            {
                MessageBox.Show("Capture is still in progress. Stop it first then close this window.",
                    "Stop capture first!", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                e.Cancel = true;        //stop the form closing
            }
            else
            {
                isStreaming = false;
                this.Visible = false;
                this.Dispose();
                ReleaseData();      //try to release the capture
            }
            
            
        }

        #endregion

        #region Debug tools
        public static void ThrowMemoryException(string message)
        {
            throw new OutOfMemoryException();
        }

        public static void ThrowNullReference(string message)
        {
            throw new NullReferenceException();
        }

        public static void ThrowGenericException(string message)
        {
            throw new Exception();
        }

        public static void ThrowOverflowException(string message)
        {
            throw new OverflowException();
        }

        #endregion


    }



}
