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
        public Mat webcamVid;                  //create a Mat object to manipulate
        private Capture camStreamCapture = null;       //the OpenCV capture stream

        public int captureChoice;              //user's selection of which capture to use
        public int drawMode;
        public string fileName;

        private bool capStartSuccess;           //whether the capture was opened OK
        private bool isStreaming;               //whether stream is in progress
        private bool randomSim;                 //using random simulation mode or ordered
        private bool valHasChanged;             //for updating the marker
        int button;                             //finds out if user has hit ESC

        int vidPixelWidth;              //video dimensions
        int vidPixelHeight;

        public VideoOutputWindow()
        {
            InitializeComponent();
            CvInvoke.UseOpenCL = false;
            try
            {
                camStreamCapture = new Capture();               //instantiate new capture object
                camStreamCapture.ImageGrabbed += parseFrames;   //the method for new frames
                webcamVid = new Mat(vidPixelHeight, vidPixelWidth, DepthType.Cv8U, 3);      //create the webcam mat object
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
            switch (drawMode)
            {
                case 0: drawModeLabel.Text = "Random";break;
                case 1: drawModeLabel.Text = "Ordered";break;
                default:
                    break;
            }

            getVideoInfo();                                         //call video info function
            frameHeightLabel.Text = vidPixelHeight.ToString();      //get the video parameters
            frameWidthLabel.Text = vidPixelWidth.ToString();

            if (camStreamCapture != null)
            {
                camStreamCapture.Start();                           //immediately start capturing
                isStreaming = true;
                startCaptureButton.Text = "Stop Capture";
            }

        }

        private void parseFrames(object sender, EventArgs arg)
        {
            //Mat webcamVid = new Mat();
            try
            {
                if (!rawVideoFramesBox.IsDisposed)      //make sure not to grab a frame if the window is closig
                {
                    camStreamCapture.Retrieve(webcamVid, 0);
                    rawVideoFramesBox.Image = webcamVid;
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
