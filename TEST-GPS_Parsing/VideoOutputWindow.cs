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

namespace TEST_GPS_Parsing
{
    public partial class VideoOutputWindow : Form
    {
        #region Declaration Objects and Vars
        //-----------------Matrix and Capture Objects----------
        public Mat webcamVid;                  //create a Mat object to manipulate
        public Mat overlayVid;
        private Capture camStreamCapture = null;        //the current OpenCV capture stream
        public Capture cscLeft = null;                  //left camera capture
        public Capture cscRight = null;                 //right camera capture
        public Capture cscCentre = null;                //centre camera capture 
        private Overlay ol_mark;                //object for the overlay of points

        protected string latitudeOutOfRangeOverlayMessage = "";      //strings for the overlay class to write into
        protected string longitudeOutOfRangeOverlayMessage = "";     //if a variable is offscreen

        //-----------------Capture setup object----------------
        public CameraBoundsSetup setup;                 //uninitialized cam bounds setup object


        //-----------------User options and parameters---------
        public int captureChoice;              //user's selection of which capture to use
        public int drawMode_Overlay;
        public string fileName;

        //-----------------Status flags and parameters----------
        protected bool capStartSuccess;           //whether the capture was opened OK
        protected bool isStreaming;               //whether stream is in progress
        protected bool randomSim;                 //using random simulation mode or ordered
        protected bool valHasChanged;             //for updating the marker

        //-----------------Settings parameters------------------
        protected int vidPixelWidth;              //video dimensions
        protected int vidPixelHeight;

        //-----------------Camera GPS boundary variables--------
        //vector of vectors allows upperLeftBound & outerLimitBound to exist for all camera frames

        public double[] upperLeftBound = new double[2];         //[0] = latitude top left; [1] = longitude top left
        public double[] outerLimitBound = new double[2];        //[0] = longitude top right; [1] = latitude bottom left
        public double delta_y;                                  //range of latitude in camera frame
        public double delta_x;                                  //range of longitude in camera frame

        //-----------------Delegate for thread safe controls----
        public delegate void SetTextCallback(string message);   //to set the Video UI elements with thread safe operations
        private int type = -1;                                  //to call teh correct invoke to put the data on the UI in a thread safe way

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
        }

        public void initCamStreams()
        {
            //tries to initialise the default camera and the first adjacent left and right

            try
            {
                //initialise the centre, left and right camera objects - set as needed
                cscCentre = new Capture(2);                                     //CENTRE CAMERA FRAME
                cscLeft = new Capture(1);                                       //LEFT CAMERA FRAME
                cscRight = new Capture(0);                                      //RIGHT CAMERA FRAME               

                if (cscCentre.GetCaptureProperty(CapProp.FrameHeight) != 0)
                {
                    camStreamCapture = cscCentre;                   //initially set the centre cam to the current
                }
                else if (cscLeft.GetCaptureProperty(CapProp.FrameHeight) != 0)
                {
                    camStreamCapture = cscLeft;                     //set initial frame to left cam if centre is not setup
                }
                else if (cscRight.GetCaptureProperty(CapProp.FrameHeight) != 0)
                {
                    camStreamCapture = cscRight;                    //set initial frame to the right camera if centre & left not setup
                }

                camStreamCapture.ImageGrabbed += parseFrames;   //the method for new frames
                webcamVid = new Mat();                          //create the webcam mat object

            }
            catch (NullReferenceException nr)
            {

                throw nr;
            }

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

        private void receiveSetupInfo()
        {
            CvInvoke.UseOpenCL = false;
            try
            {
                if (camStreamCapture == null)
                {
                    initCamStreams();
                }

            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show("A capture open error occurred: " + excpt.Message);
                return;
            }

            //recreate the objects if they were disposed by the last call
            if (camStreamCapture == null || webcamVid == null)
            {
                initCamStreams();
            }

            //Checks if the minimum number of cameras are available
            if (cscLeft.GetCaptureProperty(CapProp.FrameHeight) == 0 ||
                cscRight.GetCaptureProperty(CapProp.FrameHeight) == 0 ||
                cscCentre.GetCaptureProperty(CapProp.FrameHeight) == 0)
            {
                DialogResult continueAnyway = MessageBox.Show("Error - at least 3 cameras must be operational for this program to work optimally. If you choose to continue, you might encounter unexpected problems. Continue without 3 cameras?", "Camera Load error!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (continueAnyway == DialogResult.No)
                {
                    isStreaming = false;
                    this.Visible = false;
                    this.Dispose();
                }

            }


            //the drawMode, fileName and videoSource are set by the other form
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
            frameHeightLabel.Text = vidPixelHeight.ToString();      //get the video parameters
            frameWidthLabel.Text = vidPixelWidth.ToString();

            //display the limits set on previous window for the current camera (assumed to be the centre)
            //upperLeftBound[0] = latitude top left; [1] = longitude top left
            //OuterLimitBound[0] = longitude top right; [1] = latitude bottom left
            latTopLeftLabel.Text = upperLeftBound[0].ToString();
            longTopLeftLabel.Text = upperLeftBound[1].ToString();
            longTopRightLabel.Text = outerLimitBound[0].ToString();
            latBotLeftLabel.Text = outerLimitBound[1].ToString();

            //calculate the camera frame coordinate range - assume African co-ords; latitude always <0 longitude always >0
            delta_y = Math.Abs(outerLimitBound[1] - upperLeftBound[0]);
            delta_x = outerLimitBound[0] - upperLeftBound[1];

            if (camStreamCapture != null)
            {
                //ASSIGN THESE LIMITS TO THE OVERLAY CLASS TO WORK WITH THEM THERE
                ol_mark = new Overlay();                            //init the overlay object
                ol_mark.setupOverlay();                             //setup the overlay
                ol_mark.gridWidth = vidPixelWidth;                  //pass these variables to the other class
                ol_mark.gridHeight = vidPixelHeight;
                ol_mark.dx = delta_x;
                ol_mark.dy = delta_y;

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
                    ol_mark.ulBound[i] = upperLeftBound[i];
                    ol_mark.olBound[i] = outerLimitBound[i];
                }

                camStreamCapture.Start();                           //immediately start capturing
                isStreaming = true;
                startCaptureButton.Text = "Stop Capture";

                valHasChanged = false;
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
                            case 0: screenStateSwitch(0); webcamVid = new Mat(); break;  //switch the current camera to the left
                            case 1: screenStateSwitch(1); webcamVid = new Mat(); break; //switch the current camera to the right
                            case 2: break;      //the feed need not be changed from the current one (since the value is in this screen)
                            default:
                                break;
                        }
                        ol_mark.camSwitchStatus = 2;        //reset to stay on current cam
                    }
                    

                    camStreamCapture.Retrieve(webcamVid, 0);    //grab a frame and store to webcamVid matrix
                    rawVideoFramesBox.Image = webcamVid;        //display on-screen

                    //Now draw the markers on the overlay and display 
                    //this method sends a FALSE since the image updates 24+ times per second but the data only arrives ~once per second from GPS
                    //This means the check and update of marker is only done on the TIMER TICK every 500ms, otherwise the overlay is just redrawn
                    bool returnVal = false;
                    if (drawMode_Overlay != DRAW_MODE_REVOBJTRACK)
                    {
                        returnVal = ol_mark.drawMarker(ol_mark.x, ol_mark.y, webcamVid, false);     //draw marker from external input of coords
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
                        overlayVideoFramesBox.Image = ol_mark.overlayGrid;
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

        private void screenStateSwitch(int switchCase)
        {
            if (isStreaming)
            {
                //switchCase = 0 => switch left; switchCase = 1 => switch right

                if (switchCase == 0)        //means a switch left is requested
                {
                    if (camStreamCapture.Equals(cscLeft))    //if the current frame is already set to the left
                    {
                        return;     //keep leftmost camera frame
                    }
                    else if (camStreamCapture.Equals(cscCentre)) //if the current frame is set to the centre camera
                    {
                        //----------------Capture object switch--------------
                        cscCentre = camStreamCapture;       //set the current stream to centre
                        camStreamCapture = cscLeft;         //switch camera left by one screen
                        
                        //----------------Coordinate bounds switch-----------

                    }
                    else if (camStreamCapture.Equals(cscRight))
                    {
                        //----------------Capture object switch--------------
                        cscRight = camStreamCapture;        //set the current stream to right
                        camStreamCapture = cscCentre;       //switch camera left by one screen to centre

                        //----------------Coordinate bounds switch-----------

                    }




                }
                else if (switchCase == 1)       //means a switch right is requested
                {

                    if (camStreamCapture.Equals(cscRight))    //if the current frame is already set to the right
                    {
                        return;     //keep rightmost camera frame
                    }
                    else if (camStreamCapture.Equals(cscCentre)) //if the current frame is set to the centre camera
                    {
                        //----------------Capture object switch--------------
                        cscCentre = camStreamCapture;       //set current stream to centre
                        camStreamCapture = cscRight;         //switch camera right by one screen

                        //----------------Coordinate bounds switch-----------

                    }
                    else if (camStreamCapture.Equals(cscLeft)) //if curent frame is set to the left
                    {
                        //----------------Capture object switch--------------
                        cscLeft = camStreamCapture;
                        camStreamCapture = cscCentre;       //switch camera right by one screen to centre

                        //----------------Coordinate bounds switch-----------

                    }


                }
            }
            switchCase = 2;        //set back to "current"
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
                    while (!varsInUse)
                    {
                        try
                        {
                            varsInUse = ol_mark.setNewCoords(incoming_lat, incoming_long);      //set coords if not being read from/written to
                        }
                        catch (OverflowException)
                        {
                            type = 4;
                            setTextonVideoUI("Arithmetic error in calculating bounds - did you set them correctly?");       //use thread safe var access
                            type = -1;
                            return;
                        }
                        
                    }
                    //now we draw the marker with the updated point coords
                    bool returnVal = false;
                    if (drawMode_Overlay != DRAW_MODE_REVOBJTRACK)
                    {
                        returnVal = ol_mark.drawMarker(ol_mark.x, ol_mark.y, webcamVid, true);     //draw marker from external input of coords
                    }
                    if (returnVal == true && isStreaming)              //if the marker routine returned OK, draw the result in the video window
                    {
                            overlayVideoFramesBox.Image = ol_mark.overlayGrid;                       
                    }
                }
                else
                {
                    valHasChanged = false;
                    //this is just a normaltimer tick from the refreshOverlay_Tick method and it's likely values haven't  changed. Thus just redraw the overlay without recalc
                    bool returnVal = false;
                    if (drawMode_Overlay != DRAW_MODE_REVOBJTRACK)
                    {
                        returnVal = ol_mark.drawMarker(ol_mark.x, ol_mark.y, webcamVid, false);     //draw marker from external input of coords
                    }
                    else
                    {
                        ////returnVal = ol_mark.drawPolygons(webcamVid, true);                                //draw marker from onscreen tracking
                        ////returnVal = ol_mark.drawMarker(ol_mark.x, ol_mark.y, ol_mark.overlayGrid, false);
                    }

                    if (returnVal == true && isStreaming)              //if the marker routine returned OK, draw the result in the video window
                    {
                        overlayVideoFramesBox.Image = ol_mark.overlayGrid;
                    }
                }

                ol_mark.usingCoords = true;             //prevents the double lat/long variables from being modified
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
                ol_mark.usingCoords = false;            //unlocks double lat/long vars
                type = -1;
            }
        }
        #endregion

        #region UI and Button Routines

        private void setupCaptureButton_Click(object sender, EventArgs e)
        {
            if (setup == null)      //this is the first time the setup window has been opened
            {
                setup = new CameraBoundsSetup(this);      //pass itself as an object to manipulate
                DialogResult setupResult = setup.ShowDialog();             //pass the videoOutput object to allow settings to be set and passed back

                //respond based on the result of the dialog
                if (setupResult == DialogResult.OK)
                {
                    //TODO: Implement setup routines and UI update
                    startCaptureButton.Enabled = true;
                }
                else if (setupResult == DialogResult.Cancel)        //if setup process was prematurely cancelled
                {
                    status1TextBox.Clear();
                    status1TextBox.Text = "Setup was cancelled.";
                    startCaptureButton.Enabled = false;         //first time setup, require settings before capture
                    setup = null;                               //clear object because it wasn't setup properly
                }
            }
            else
            {
                //this isn't the first time this session that the user is opening the setting form
                DialogResult setupResult = setup.ShowDialog();
                if (setupResult == DialogResult.OK)
                {
                    //TODO: Implement setup routines and UI update
                    startCaptureButton.Enabled = true;
                }
                else if (setupResult == DialogResult.Cancel)        //if setup process was prematurely cancelled
                {
                    status1TextBox.Clear();
                    status1TextBox.Text = "Setup was cancelled. Using previous settings.";
                    startCaptureButton.Enabled = true;              //we'll use previously entered settings
                }

            }
        }

        private void setTextonVideoUI(string text)
        {
            //Update the UI with information -check threadsafe since the other thread might be using it
            // If the calling thread is different from the thread that
            // created the TextBox control, this method creates a
            // SetTextCallback and calls itself asynchronously using the
            // Invoke method.
            if (isStreaming)
            {

                switch (type)
                {
                    case 0:
                        if (this.latitudeLabel.InvokeRequired)
                        {
                            SetTextCallback l = new SetTextCallback(setTextonVideoUI);
                            Invoke(l, new object[] { text });

                        }
                        else { this.latitudeLabel.Text = text; }; break;
                    case 1:
                        if (this.LongitudeLabel.InvokeRequired)
                        {
                            SetTextCallback l = new SetTextCallback(setTextonVideoUI);
                            Invoke(l, new object[] { text });
                        }
                        else { this.LongitudeLabel.Text = text; }; break;
                    case 2:
                        if (this.latOORStatusBox.InvokeRequired)
                        {
                            SetTextCallback l = new SetTextCallback(setTextonVideoUI);
                            Invoke(l, new object[] { text });
                        }
                        else { this.latOORStatusBox.Text = text; }; break;
                    case 3:
                        if (this.longOORTextBox.InvokeRequired)
                        {
                            SetTextCallback l = new SetTextCallback(setTextonVideoUI);
                            Invoke(l, new object[] { text });
                        }
                        else { this.status1TextBox.Text = text; }; break;
                    case 4:
                        if (this.status1TextBox.InvokeRequired)
                        {
                            SetTextCallback l = new SetTextCallback(setTextonVideoUI);
                            Invoke(l, new object[] { text });
                        }
                        else { this.status1TextBox.Text = text; }; break;
                    default:
                        Console.Write("Couldn't update one or more UI elements with cross-thread call");
                        break;
                }
            }

        }

        private void startCaptureButton_Click(object sender, EventArgs e)
        {
                if (isStreaming)
                {  //stop the capture
                    isStreaming = false;
                    startCaptureButton.Text = "Start Capture";
                pausedCaptureLabel.Visible = true;
                    camStreamCapture.Pause();
                    cscLeft.Pause();
                    cscRight.Pause();
                    cscCentre.Pause();
                    ol_mark.clearScreen();      //remove the marker and lines off the screen.
                    
                }
                else
                {
                //start the capture

                overlayVideoFramesBox.Visible = true;   //show the output frames box
                setupInstructLabel.Visible = false;     //hide the setup instruction label
                pausedCaptureLabel.Visible = false;     //hide the paused label (for if capture was already on)
                    startCaptureButton.Text = "Stop Capture";
                    try
                    {
                    camStreamCapture.Start();
                    cscLeft.Start();
                    cscRight.Start();
                    cscCentre.Start();
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

        public void ReleaseData()
        {
            if (camStreamCapture != null) { camStreamCapture.Dispose();}
            if (cscLeft != null) { cscLeft.Dispose(); }
            if (cscRight != null) { cscRight.Dispose(); }
            if (cscCentre != null) { cscCentre.Dispose(); }
            rawVideoFramesBox.Dispose();
                
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
                this.Visible = false;
                this.Dispose();
                //ReleaseData();      //try to release the capture
            }
            
            
        }
        #endregion


    }



}
