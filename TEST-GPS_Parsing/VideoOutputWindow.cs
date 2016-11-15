using System;
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
using System.Timers;
using System.Net;

namespace TEST_GPS_Parsing
{
    public partial class VideoOutputWindow : Form
    {
        #region Declaration Objects and Vars
        //-----------------Matrix and Capture Objects----------
        public Mat webcamVid;                  //create a Mat object to manipulate
        public Mat overlayVid;

        public Capture[] camStreamCaptureArray;                //DEPRECATED ARRAY FOR CAPTURE OBJECTS!!

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

        double[] revObj_gpsForDb;        //temp store for real lat/long from object tracker - send to parser for DB
                                         //[0] = lat; [1] = long
        

        //-----------------User options and parameters---------
        public int captureChoice;              //user's selection of which capture to use
        private static int captureChoiceIP = 1;
        private static int captureChoiceLocal = 0;
        public int drawMode_Overlay;
        private string fileName;

        //-----------------Status flags and parameters----------
        protected bool capStartSuccess;           //whether the capture was opened OK
        protected bool isStreaming;               //whether stream is in progress
        protected bool randomSim;                 //using random simulation mode or ordered
        protected bool valHasChanged;             //for updating the marker
        protected bool stillSwitchingCams = false;        //to ensure no crashing when switching camera
        protected bool usingRPT = false;            //flag for using rev persp. transform (pixel to camera perspective scale)
        public int rptClickCounter = 0;               //counts how many points entered
        public int rptCounter = 0;                  //for initial 10 second counting

        public bool grabResult;                  //result of grabbing a frame, for disconnection detection
        int disconnectCounter = 0;                  //check if disconnected and if so do a full reset
        public bool logCamSwitchToDb = false;       //for signalling a write required to the DB
        bool vidLogWriteOK;                       //for signalling the video writer

        System.Timers.Timer camSwitchTimer;         //timer to check when cameras should switch

        static EventWaitHandle _waitCameraRightSwitch = new AutoResetEvent(true);       //wait handle to prevent threads switching cameras rapidly
        static EventWaitHandle _waitCameraLeftSwitch = new AutoResetEvent(true);
        static EventWaitHandle _waitForParsing = new AutoResetEvent(true);
        static EventWaitHandle _waitforSwitchCheck = new AutoResetEvent(true);

        private System.Threading.Semaphore uiUpdateSemaphone = new Semaphore(1, 3); //mutex lock to prevent UI update race conditions
        private Object uiWriteLock = new object();
       

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
        public Mat reverseTransformMatrix;          //matrix to transform pixel to 3D scaled pixel onscreen

        //--------Reverse Transform points------
        private PointF[] rptPixelBounds = new PointF[4];  //bounds for scaled pixel grid onto 3D camera plane

        //---------Current Camera Variable--------
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
            camInitLabel.Visible = false;

            //initialise the camera switch timer
            // Create a timer with a two second interval.
            camSwitchTimer = new System.Timers.Timer(100);
            // Hook up the Elapsed event for the timer. 
            camSwitchTimer.Elapsed += new ElapsedEventHandler(checkCamSwitchNeeded);
            camSwitchTimer.AutoReset = true;
            camSwitchTimer.Enabled = false;
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
                rptClickCounter = 0;     //reset reverse proj transform settings
                rptCounter = 0;         //reset initial counter for rpt setup
                usingRPT = false;

                //----------------TEST CODE--------------------
                WebCams = setup.webCams_CB;
                _SystemCameras = setup._SystemCameras_CB; //DirectShow object - for local cams
                CameraDevice = setup.CameraDevice_CB;
                _capture = setup._capture_CB;

                if (WebCams.Length == 0)
                {
                    MessageBox.Show("No cameras detected. Please plug in a camera now.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw new NullReferenceException();
                }

                camBoundArray = new camBound[WebCams.Length];
                for (int i = 0; i < camBoundArray.Length; i++)
                {
                    camBoundArray[i] = new camBound(0.0, 0.0);
                }

                //---------get capture extents---------
                // get by default from left cam (camArray[0])

                for (int i = 0; i <= 1; i++)
                {
                    camBoundArray[0].upperLeftBound[i] = setup.upperLeftCoordsTransformed_CB[i];
                    camBoundArray[0].outerLimitBound[i] = setup.outerLimitCoordsTransformed_CB[i];
                }

                //Setup all the capture devices and then pause all but the ones we need
                camStreamCaptureArray = new Capture[WebCams.Length];



                if (captureChoice == captureChoiceIP) //means we're dealing with IP cams so set them all up now
                {
                    for (int i = 0; i < WebCams.Length; i++)
                    {
                        camStreamCaptureArray[i] = new Capture(WebCams[i].Device_Name);
                        //camStreamCaptureArray[i].SetCaptureProperty(CapProp.OpenniMaxBufferSize, 10);
                        //camStreamCaptureArray[i].SetCaptureProperty(CapProp.)
                    }
                    //set the centre (or left, if 1 cam) cam as active
                    //--camStreamCapture = camStreamCaptureArray[len];
                }
                else if (captureChoice == captureChoiceLocal) //set up for local cameras
                {
                    //Test code configuration steps with current code
                    camStreamCapture = _capture;

                }
                camInitLabel.Visible = false;

                if (captureChoice == captureChoiceIP)      //ip cams only
                {
                    for (int i = 1; i < WebCams.Length; i++)
                    {                        
                            camStreamCaptureArray[i].Stop();   //this is for IP cams
                    }
                }


                //bring across the transformation matrix to do the point plane conversion (gps real world to image plane)
                transformMatrix = setup.transformMatrix_CB;

                //calculate the pixel-to-3D space reverse transform matrix
                //reverseTransformMatrix = doRevPerspectiveTransform

                //---------update the UI initially with info--
                //by default first camera is the left (camBound[1])
                camBoundUIDisplaySetup(0);
                currentlyActiveCamera = 0;

                //use the class-local methods now
                if(captureChoice == captureChoiceIP) //IP cams
                {
                    camStreamCaptureArray[0].ImageGrabbed += parseFrames;
                }
                else
                {
                    camStreamCapture.ImageGrabbed += parseFrames;   //the method for new frames
                    
                }                

                webcamVid = new Mat();                          //create the webcam mat object

                //we're done editing the camera frames, launch the background switcher thread
                if (!cameraSwitcher.IsBusy)
                {
                    cameraSwitcher.RunWorkerAsync();
                }
                else
                {
                    cameraSwitcher.CancelAsync();
                    if (cameraSwitcher.IsBusy)
                    {
                        Thread.Sleep(500);
                    }
                    
                    cameraSwitcher.RunWorkerAsync();
                }
                

                //setup the object tracker co-ordinatee
                revObj_gpsForDb = new double[2];
                for (int i = 0; i < 2; i++)
                {
                    revObj_gpsForDb[i] = 0.0;
                }

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
            if (mode == initialSetup)
            {
                videoLogFilename = setup.videoLogFilename_CB;
                videoWriterOutput = setup.videoWriterOutput_CB;
            }
            else if (mode == fromButtonRepress) //this method was called by repeated pressed of Stop/Start after capture was setup so re-init writer
            {
                if (videoLogFilename != null) //account for initial "start" press when object already setup
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
        //Called every 100ms to check if a camera switch is needed
        private void checkCamSwitchNeeded(object source, ElapsedEventArgs e)
        {
        }

        public double[] getObjTrackingCoords()
        {
            if (drawMode_Overlay == DRAW_MODE_REVOBJTRACK && valHasChanged)
            {
                return revObj_gpsForDb; //this is to allow gpsParser to request the co-ordinates from the on-screen tracker
            }
            return null;
        }

        //this method is called every time the IsGrabbed event is raised i.e. every time a new frame is captured
        private void parseFrames(object sender, EventArgs arg)
        {
            
            try
            {
                if (!rawVideoFramesBox.IsDisposed && isStreaming)      //make sure not to grab a frame if the window is closig
                {
                    Mat tempFrame = new Mat();
                    _waitforSwitchCheck.WaitOne();  //wait for the switcher thread to finish its work
                    if (captureChoice == captureChoiceIP)
                    {
                       
                        grabResult = camStreamCaptureArray[currentlyActiveCamera].Retrieve(tempFrame);
                        if (grabResult == true)
                        {
                            webcamVid = tempFrame;
                        }
                        else
                        {
                            return; //if th frame was grabbed as null, don't process anymore
                        }
                       
                    }
                    else if (captureChoice == captureChoiceLocal)
                    {
                        grabResult = camStreamCapture.Retrieve(webcamVid);
                    }
                                        
                    _waitForParsing.Reset();

                    //rawVideoFramesBox.Image = webcamVid;        THIS CAUSES CAPTURE FAILURE CRASHES                               

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
                            returnVal = ol_mark.drawPolygons(webcamVid, currentlyActiveCamera, true);
                        }
                        else
                        {
                            returnVal = ol_mark.drawPolygons(webcamVid, currentlyActiveCamera, false);                                //draw marker from onscreen tracking
                        }
                    }


                    if (returnVal == true)
                    {
                        try
                        {
                            overlayVideoFramesBox.Image = ol_mark.overlayGrid;
                           //rawVideoFramesBox.Image = ol_mark.cannyResult_out;
                            // FOR DEBUGGING MEMORY EXCEPTIONS ThrowMemoryException("Memory");
                        }
                        catch (OutOfMemoryException)
                        {
                            //videoWriterOutput.Dispose();
                            MessageBox.Show("Error! Your system has run out of memory.\n" +
                                "Close some programs and try capturing again.\n" +
                                "Don't worry, video was logged right up to this point and has been saved.\n" +
                                "To restart capture, close any memry-intensive programs, and click 'Stop Capture' once.", "Capture halted", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                _waitForParsing.Set();      //unlock the wait on the other thread

            }
            catch (Exception e)
            {
                MessageBox.Show("Capturing failed. Reason: " + e.Message, "Something happened!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                isStreaming = false;
                startCaptureButton.Text = "(Re)Start Capture";
                ReleaseData();      //something failed so release the data
                return;
            }

        }
        #endregion

        #region Rev. Perspective Transform

        //let the user click in the space to set the perspective transform
        private void overlayVideoFramesBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (isStreaming)
            {
                //first left click is left upper pixel bound
                if (rptClickCounter == 0 && e.Button == MouseButtons.Left)    //first point (top left) for scaling
                {
                    rptClickCounter++;
                    rptPixelBounds[0].X = e.X;      //get mouse X co-ordinate
                    rptPixelBounds[0].Y = e.Y;      //get mouse Y co-ordinate
                }
                //next right click is right upper bound
                //NO MORE clicks so store the bottom bounds automatically since they're fixed
                else if (rptClickCounter == 1 && e.Button == MouseButtons.Left)
                {
                    rptClickCounter = -1;
                    rptPixelBounds[1].X = e.X;      //get mouse X co-ordinate
                    rptPixelBounds[1].Y = e.Y;      //get mouse Y co-ordinate
                                                    //now fill in the rest cause they're fixed
                    rptPixelBounds[2].X = 0;                    
                    rptPixelBounds[2].Y = vidPixelHeight;      
                    rptPixelBounds[3].X = vidPixelWidth;       
                    rptPixelBounds[3].Y = vidPixelHeight;      
                    getRevPerspectiveTransformMatrix();
                    usingRPT = true;
                }
                else
                {
                    rptClickCounter = -1;    //set it beyond 0,1 to not trigger bounds
                                     //ignore any more clicks
                    return;
                }
            }

            
        }

        public void getRevPerspectiveTransformMatrix()
        {
            //gets the reverse perspective transform matrix to prevent floating points when putting pixel onscreen
            PointF[] srcPixelBounds = new PointF[4];
            //fill in the normal screen pixel bounds width = 640, height = 480 (common)
            /*Remember pixel vs "geometry" i.e. if 640x480
            * (0,480)   [0]         [1] (640,480)
            * 
            * (0,0)     [2]         [3] (640,0)
            * */
            srcPixelBounds[0].X = srcPixelBounds[0].Y = srcPixelBounds[1].Y = 0;
            srcPixelBounds[1].X = srcPixelBounds[3].X = vidPixelWidth;
            srcPixelBounds[2].Y = srcPixelBounds[3].Y = vidPixelHeight;
            //we want to scale the top 2 points to something else to account for perspective shift
            reverseTransformMatrix = CvInvoke.GetPerspectiveTransform(srcPixelBounds, rptPixelBounds);
        }


        public void doRevPerspectiveTransform()
        {
            
        }
        #endregion 

        #region capture Setup
        private void SetupCapture(int Camera_Identifier, bool reconnecting)
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
                    isStreaming = true;
                    camStreamCapture = new Capture(CameraDevice);
                    camStreamCapture.ImageGrabbed += parseFrames;   //the method for new frames
                    camStreamCapture.Start();
                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
                catch (Exception)
                {
                    //try avoid the fast-switch issue
                    camStreamCapture.Stop(); camStreamCapture.Dispose();
                    camStreamCapture = null;
                    status1TextBox.Text = "Error initialising camera...Trying again...";
                    Thread.Sleep(500);
                    SetupCapture(CameraDevice,true);
                }
            }
            else if (captureChoice == 1)    //this methd is for the IP cameras
            {
                // Dispose of Capture if it was created before
                //--if (camStreamCapture != null) camStreamCapture.Stop();
                if (camStreamCaptureArray[currentlyActiveCamera] != null && reconnecting == false)
                {
                    camStreamCaptureArray[currentlyActiveCamera].Stop();
                }
                try
                {
                    //--camStreamCapture = null;
                    //--camStreamCapture = camStreamCaptureArray[CameraDevice];
                    //Set up capture device
                    if (reconnecting == true)
                    {
                        camStreamCaptureArray[CameraDevice].Dispose();
                        camStreamCaptureArray[CameraDevice] = new Capture(WebCams[CameraDevice].Device_Name);
                        Thread.Sleep(300);
                    }
                    //camStreamCapture = new Capture(WebCams[CameraDevice].Device_Name);  //we want to open a capture by URL which is stored with the cam URL
                    //--camStreamCaptureArray[currentlyActiveCamera].Pause();   //pause the "old" stream
                    //--camStreamCapture.Start();
                    camStreamCaptureArray[CameraDevice].ImageGrabbed += parseFrames;   //the method for new frames
                    camStreamCaptureArray[CameraDevice].Start();
                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
                catch (Exception)
                {
                    //try avoid the fast-switch issue
                    status1TextBox.Text = "Error initialising camera...Trying again...";
                    Thread.Sleep(500);
                    SetupCapture(CameraDevice,true);
                }
            }

            //  stillSwitchingCams = true;   


            // Thread.Sleep(1500);
            // stillSwitchingCams = false;
        }
        #endregion

        #region capture switching
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
                        _waitCameraRightSwitch.WaitOne(2000);
                        SetupCapture(activeCamLocal - 1, false);
                        _waitCameraLeftSwitch.Set();

                        activeCamLocal--;            //switch camera index one left
                        camBoundUIDisplaySetup(activeCamLocal);  //call UI update to show new bounds onscreen


                        return activeCamLocal;
                    }
                    else if (activeCamLocal - 1 < 0)   //we're already on the leftmost camera
                    {
                        return activeCamLocal;
                    }

                    ////capture object comparison and switch

                   
                }
                else if (switchCase == 1)       //means a switch right is requested
                {
                    //switch coordinates
                    if (activeCamLocal + 1 < camBoundArray.Length)       //don't go right if you can't
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

                        _waitCameraLeftSwitch.WaitOne(2000);
                        SetupCapture(activeCamLocal + 1, false);
                        _waitCameraRightSwitch.Set();

                        activeCamLocal++;                        //indicate we've switched one camera right                   
                        camBoundUIDisplaySetup(activeCamLocal);  //call UI update to show new bounds onscreen
                        return activeCamLocal;
                    }
                    else if (activeCamLocal + 1 >= camBoundArray.Length)   //we're already on the rightmost camera
                    {
                        return activeCamLocal;
                    }

                }
            }
            switchCase = 2;        //set back to "current"
            return activeCamLocal;
        }

        #endregion

        #region Background worker - cameraswitcher
        private void cameraSwitcher_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!cameraSwitcher.CancellationPending)
            {
                if (!rawVideoFramesBox.IsDisposed && isStreaming)      //make sure not to grab a frame if the window is closig
                {
                    //Console.Write("Cam Switcher has lock");
                    //set camStreamCapture (the central selected camera) to the correct cam (switch if needed) based on co-ord bound checks
                    //only switch capture objects if a GPS value has been updated!
                    if (valHasChanged)
                    {
                        //_waitforSwitchCheck.Reset();
                        switch (ol_mark.camSwitchStatus)
                        {
                            case -1: break;     //the feed need not be changed from the current one (since an error has occurred / value wasn't changed from default)
                            case 0: _waitforSwitchCheck.Reset(); logCamSwitchToDb = true; currentlyActiveCamera = screenStateSwitch(0, currentlyActiveCamera); webcamVid = new Mat(); break;  //switch the current camera to the left
                            case 1: _waitforSwitchCheck.Reset(); logCamSwitchToDb = true; currentlyActiveCamera = screenStateSwitch(1, currentlyActiveCamera); webcamVid = new Mat(); break; //switch the current camera to the right
                            case 2: break;      //the feed need not be changed from the current one (since the value is in this screen)
                            default:
                                break;
                        }
                        //_waitforSwitchCheck.Set();
                        ol_mark.camSwitchStatus = 2;        //reset to stay on current cam
                    }
                }
                if (camDisconnectedWarningLabel.Visible == true)
                {
                    bool isUp = reconnectIP(WebCams[currentlyActiveCamera].Device_Name);
                    if (isUp)
                    {
                        SetupCapture(currentlyActiveCamera, true); webcamVid = new Mat();
                    }
                }
                //Console.Write("Cam Switcher released lock");
                _waitforSwitchCheck.Set();
            }

        }

        public bool reconnectIP(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as System.Net.HttpWebRequest;
                //Setting the Request method HEAD- we want to check it exists
                request.Method = "HEAD";
                request.Proxy = null;
                //Getting the Web Response.
                request.Timeout = 3000;
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

        private void cameraSwitcher_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            
        }

        private void cameraSwitcher_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Camera switching thread cancelled.");
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
                    disconnectCounter++;
                    if (disconnectCounter > 5)
                    {
                        disconnectCounter = 0;
                        camDisconnectedWarningLabel.Visible = true;                        
                    }
                    else
                    {
                        camDisconnectedWarningLabel.Visible = false;
                    }

                }
                else
                {
                    grabResult = false;
                    camDisconnectedWarningLabel.Visible = false;
                }
                
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

        public void overlayTick(double incoming_lat = 0.0, double incoming_long = 0.0)
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
                        varsInUse = ol_mark.setNewCoords(incoming_lat, incoming_long, transformMatrix, reverseTransformMatrix,usingRPT);      //set coords if not being read from/written to

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
                    if (drawMode_Overlay == DRAW_MODE_REVOBJTRACK)  //write the onscreen co-ords as lat/long back to gpsparser
                    {
                        revObj_gpsForDb[0] = ol_mark.current_pointGPS_lat;
                        revObj_gpsForDb[1] = ol_mark.current_pointGPS_long;
                    }
                    if (drawMode_Overlay != DRAW_MODE_REVOBJTRACK) //draw markers for the tracking-based points on-screen
                    {
                        valHasChanged = false;
                        returnVal = ol_mark.drawMarker(ol_mark.x, ol_mark.y, webcamVid, false, currentlyActiveCamera);     //draw marker from external input of coords
                    }
                    else //let polygons redraw themselves
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
                camInitLabel.Visible = true;
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
                        else
                        {
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
                case 1: videoModeLabel.Text = "IP Video"; break;
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

            getVideoInfo(camScreenNumber);                                         //call video info function


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

            if (camStreamCapture != null || camStreamCaptureArray[camScreenNumber] != null)
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
                if (captureChoice == captureChoiceIP)
                {camStreamCaptureArray[currentlyActiveCamera].Stop();}
                else{camStreamCapture.Pause();}                                
                ol_mark.clearScreen();      //remove the marker and lines off the screen.
                videoSaveTimer.Stop();        //pause the disconnection check timer   
                camSwitchTimer.Stop();
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
                camSwitchTimer.Start();
                startCaptureButton.Text = "Stop Capture";
                setupVideoWriter(fromButtonRepress);                     //create new video Log file
                try
                {

                    if (captureChoice == captureChoiceIP)
                    { camStreamCaptureArray[currentlyActiveCamera].Start(); }
                    else {
                        camStreamCapture.Start();
                    }
                    isStreaming = true;

                }
                catch (Exception re)
                {
                    MessageBox.Show("Capturing still failed. Try again later. Reason: " + re.Message, "Something happened!",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
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
            cameraSwitcher.CancelAsync();   //shut down the switcher thread

            if (camStreamCapture != null) { camStreamCapture.Stop(); camStreamCapture.Dispose(); }

            //stop each camera capture instance
            if (WebCams == null)
            {
                for (int i = 0; i < totalCameraNumber; i++)
                {
                    //this method might be called at the start of execution so check for null references in case
                    if (camStreamCaptureArray != null && camStreamCaptureArray[i] != null)
                    {
                        camStreamCaptureArray[i].Stop();
                        camStreamCaptureArray[i].Dispose();
                    }
                }
            }
            else
            {
                for (int i = 0; i < WebCams.Length; i++)
                {
                    //this method might be called at the start of execution so check for null references in case
                    if (camStreamCaptureArray != null && camStreamCaptureArray[i] != null)
                    {
                        camStreamCaptureArray[i].Stop();
                        camStreamCaptureArray[i].Dispose();
                    }
                }
            }


            //rawVideoFramesBox.Dispose();
            videoSaveTimer.Stop();
            camSwitchTimer.Stop();
        }

        private void getVideoInfo(int whichCamNumber)            //get the video properties
        {
            if (captureChoice == captureChoiceIP) //IP cams
            {
                vidPixelHeight = (int)camStreamCaptureArray[whichCamNumber].GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight);
                vidPixelWidth = (int)camStreamCaptureArray[whichCamNumber].GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth);
            }
            else if (captureChoice == captureChoiceLocal)
            {
                vidPixelHeight = (int)camStreamCapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight);
                vidPixelWidth = (int)camStreamCapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth);

            }
            
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
