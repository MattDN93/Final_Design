using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
               
        public double[] upperLeftBound = new double[2];       //[0] = latitude top left; [1] = longitude top left
        public double[] outerLimitBound = new double[2];      //[0] = longitude top right; [1] = latitude bottom left


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
                default:
                    break;
            }

            getVideoInfo();                                         //call video info function
            frameHeightLabel.Text = vidPixelHeight.ToString();      //get the video parameters
            frameWidthLabel.Text = vidPixelWidth.ToString();

            //display the limits set on previous window
            //[0] = latitude top left; [1] = longitude top left
            //[0] = longitude top right; [1] = latitude bottom left
            latTopLeftLabel.Text = upperLeftBound[0].ToString();
            longTopLeftLabel.Text = upperLeftBound[1].ToString();
            longTopRightLabel.Text = outerLimitBound[0].ToString();
            latBotLeftLabel.Text = outerLimitBound[1].ToString();

            if (camStreamCapture != null)
            {
                ol_mark = new Overlay();                            //init the overlay object
                ol_mark.setupOverlay();                             //setup the overlay
                ol_mark.gridWidth = vidPixelWidth;
                ol_mark.gridHeight = vidPixelHeight;

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

            if (ol_mark != null)    
            {
                //SIMULATION
                if (isStreaming)
                {
                    ol_mark.generateSimPts();
                }
                
                bool returnVal = ol_mark.drawMarker(ol_mark.x, ol_mark.y, webcamVid, true);

                if (returnVal == true)              //if the marker routine returned OK, draw the result in the video window
                {
                    overlayVideoFramesBox.Image = ol_mark.overlayGrid;
                }

                //Update the UI with information
                latitudeLabel.Text = ol_mark.y.ToString();
                LongitudeLabel.Text = ol_mark.x.ToString();
                latOORStatusBox.Clear();
                latOORStatusBox.Text = ol_mark.latitudeOutOfRangeOverlayMessage;    //pull the status from the checkBounds method
                longOORTextBox.Clear();
                longOORTextBox.Text = ol_mark.longitudeOutOfRangeOverlayMessage;    //pull status from the checkBounds methods

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
