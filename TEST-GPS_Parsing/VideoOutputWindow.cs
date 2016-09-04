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
        //-----------------Matrix and Capture Objects----------
        public Mat webcamVid;                  //create a Mat object to manipulate
        public Mat overlayVid;
        private Capture camStreamCapture = null;       //the OpenCV capture stream
        private Overlay ol_mark;                //object for the overlay of points

        protected string latitudeOutOfRangeOverlayMessage = "";      //strings for the overlay class to write into
        protected string longitudeOutOfRangeOverlayMessage = "";     //if a variable is offscreen

        //-----------------User options and parameters---------
        public int captureChoice;              //user's selection of which capture to use
        public int drawMode_Overlay;
        public string fileName;

        //-----------------Status flags and parameters----------
        protected bool capStartSuccess;           //whether the capture was opened OK
        protected bool isStreaming;               //whether stream is in progress
        protected bool randomSim;                 //using random simulation mode or ordered
        protected bool valHasChanged;             //for updating the marker
        int button;                             //finds out if user has hit ESC

        //-----------------Settings parameters------------------
        protected int vidPixelWidth;              //video dimensions
        protected int vidPixelHeight;

        //-----------------Camera GPS boundary variables--------
               
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

        public VideoOutputWindow()
        {
            InitializeComponent();
            CvInvoke.UseOpenCL = false;
            try
            {
                camStreamCapture = new Capture();               //instantiate new capture object
                camStreamCapture.ImageGrabbed += parseFrames;   //the method for new frames
                webcamVid = new Mat();                          //create the webcam mat object
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
        }

        private void VideoOutputWindow_Load(object sender, EventArgs e)
        {
            //the drawMode, fileName and videoSource are set by the other form
            //evaluates what the user chose from the bounds setup box
            switch (captureChoice)
            {
                case 0: videoModeLabel.Text = "Live Video";break;
                case 1: videoModeLabel.Text = "Recorded Video";break;
                default: videoModeLabel.Text = "None Set";
                    break;
            }
            switch (drawMode_Overlay)
            {
                case 0: drawModeLabel.Text = "Random"; drawMode_Overlay = DRAW_MODE_RANDOM; break;
                case 1: drawModeLabel.Text = "Ordered";drawMode_Overlay = DRAW_MODE_ORDERED; break;
                case 2: drawModeLabel.Text = "Tracking";drawMode_Overlay = DRAW_MODE_TRACKING;break;
                default:
                    break;
            }

            getVideoInfo();                                         //call video info function
            frameHeightLabel.Text = vidPixelHeight.ToString();      //get the video parameters
            frameWidthLabel.Text = vidPixelWidth.ToString();

            //display the limits set on previous window
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
                for (int i = 0; i <= 1; i++)
                {
                    ol_mark.ulBound[i] = upperLeftBound[i];
                    ol_mark.olBound[i] = outerLimitBound[i];
                }

                camStreamCapture.Start();                           //immediately start capturing
                isStreaming = true;
                startCaptureButton.Text = "Stop Capture";
            }

        }

        //this method is called every time the IsGrabbed event is raised i.e. every time a new frame is captured
        private void parseFrames(object sender, EventArgs arg)
        {
            //Mat webcamVid = new Mat();
            try
            {
                if (!rawVideoFramesBox.IsDisposed)      //make sure not to grab a frame if the window is closig
                {
                    camStreamCapture.Retrieve(webcamVid, 0);    //grab a frame and store to webcamVid matrix
                    rawVideoFramesBox.Image = webcamVid;        //display on-screen

                    //Now draw the markers on the overlay and display [SIMULATION VALUES HERE]
                    //this method sends a FALSE since the image updates 24+ times per second but the data only 1 per second
                    //This means the check and update of marker is only done on the TIMER TICK every 500ms, otherwise the overlay is just redrawn
                    bool returnVal = ol_mark.drawMarker(ol_mark.x, ol_mark.y, webcamVid, false);
                    if (returnVal == true)
                    {
                        overlayVideoFramesBox.Image = ol_mark.overlayGrid;
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Capturing failed. Reason: " + e.Message, "Something happened!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isStreaming = false;
                startCaptureButton.Text = "(Re)Start Capture";
                ReleaseData();      //something failed so release the data
                return;
            }

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
                //SIMULATION - enable to generate random points
                /*if (isStreaming)
                {
                    ol_mark.generateSimPts();
                }*/

                if (incoming_lat != 0.0 && incoming_long != 0.0)    //this means that this method was called from the parser not by the timer
                {
                    bool varsInUse = false;
                    while (!varsInUse)
                    {
                        varsInUse = ol_mark.setNewCoords(incoming_lat, incoming_long);      //set coords if not being read from/written to
                    }
                    bool returnVal = ol_mark.drawMarker(ol_mark.x, ol_mark.y, webcamVid, true);
                    if (returnVal == true)              //if the marker routine returned OK, draw the result in the video window
                    {
                        overlayVideoFramesBox.Image = ol_mark.overlayGrid;
                    }
                }
                else
                {   //this is just a normaltimer tick and it's likely values haven't  changed. Thus just redraw the overlay without recalc
                    bool returnVal = ol_mark.drawMarker(ol_mark.x, ol_mark.y, webcamVid, false);
                    if (returnVal == true)              //if the marker routine returned OK, draw the result in the video window
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

        private void setTextonVideoUI(string text)
        {
            //Update the UI with information -check threadsafe since the other thread might be using it
            // If the calling thread is different from the thread that
            // created the TextBox control, this method creates a
            // SetTextCallback and calls itself asynchronously using the
            // Invoke method.
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
                    else { this.longOORTextBox.Text = text; }; break;
                default: Console.Write("Couldn't update one or more UI elements with cross-thread call");
                    break;
            }

        }

        private void startCaptureButton_Click(object sender, EventArgs e)
        {
                if (isStreaming)
                {  //stop the capture
                    startCaptureButton.Text = "Start Capture";
                    camStreamCapture.Pause();
                    isStreaming = false;
                }
                else
                {
                    //start the capture
                    startCaptureButton.Text = "Stop Capture";
                    try
                    {
                    camStreamCapture.Start();
                    isStreaming = true;
                    
                    }
                    catch (Exception re)
                    {
                        MessageBox.Show("Capturing still failed. Try again later. Reason: " + re.Message, "Something happened!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        isStreaming = false;
                        startCaptureButton.Text = "(Re)Start Capture";
                        ReleaseData();      //something failed so release the data
                        return;
                    }
                }

        }

        private void ReleaseData()
        {
            if (camStreamCapture != null)
            {
                camStreamCapture.Dispose();
                rawVideoFramesBox.Dispose();
                this.Dispose();
            }
                
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
                MessageBox.Show("Capture is still in progress. Stop it first then close this window.", "Stop capture first!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;        //stop the form closing
            }
            else
            { 
                ReleaseData();      //try to release the capture
            }
            
            
        }


    }



}
