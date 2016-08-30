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
using Emgu.CV.Util;

namespace TEST_GPS_Parsing
{
    public partial class VideoOutputWindow : Form
    {
            //privates for status
        private Mat webcamVid;                  //create a Mat object to manipulate
        private Capture camStreamCapture;       //the OpenCV capture stream

        public int captureChoice;              //user's selection of which capture to use
        public int drawMode;
        public string fileName;

        private bool capStartSuccess;           //whether the capture was opened OK
        private bool isStreaming;               //whether stream is in progress
        private bool randomSim;                 //using random simulation mode or ordered
        private bool valHasChanged;             //for updating the marker
        int button;                             //finds out if user has hit ESC

         

            //enums for user requirements
            //source choice
        const int EXTERNAL_WEBCAM = 0;
        const int OFFLINE_FILE = 1;
        //point mapping

        const int RANDOM_POINTS = 0;
        const int ORDERED_POINTS = 1;
        int vidPixelWidth;              //video dimensions
        int vidPixelHeight;

        public VideoOutputWindow()
        {
            InitializeComponent();
            

        }

        private void VideoOutputWindow_Load(object sender, EventArgs e)
        {
            //the drawMode, fileName and videoSource are set by the other form
            //evaluates what the user chose from the bounds setup box
            switch (drawMode)
            {
                case RANDOM_POINTS: randomSim = true; break;
                case ORDERED_POINTS: randomSim = false; break;
                default:
                    Console.Write("Please select a sim mode");
                    break;
            }

            switch (captureChoice)
            {
                case EXTERNAL_WEBCAM: capStartSuccess = doCapture(EXTERNAL_WEBCAM); break;
                case OFFLINE_FILE: capStartSuccess = doCapture(OFFLINE_FILE, fileName); break;
                default:
                    Console.Write("You've made an invalid choice. Try again");
                    break;
            }
            if (capStartSuccess == false)
            {
                throw new Exception();
            }
        }


        public bool doCapture(int choice, string fileName = "")        //perform the capture
        {
            isStreaming = true;
            CvInvoke.NamedWindow("Incoming Video Stream", Emgu.CV.CvEnum.NamedWindowType.AutoSize); //load up a named window
            getVideoInfo();                     //get the extents of the frame
            //webcamVid;

            //overlayMarker.setupOverlay();       //setup the image overlay

            if (camStreamCapture != null) camStreamCapture.Dispose();   //close the instance if it's already open 

            try
            {
                //Set up capture device based on choice
                    switch (choice)
                {
                    case EXTERNAL_WEBCAM: camStreamCapture = new Capture(EXTERNAL_WEBCAM); break;
                    case OFFLINE_FILE: camStreamCapture = new Capture(OFFLINE_FILE); break;
                    default:
                        break;
                }

                camStreamCapture.ImageGrabbed += parseFrames;       //add the image grabbed to the frame
                return true;
            }
            catch (NullReferenceException excpt)
            {
                throw new NullReferenceException();
            }

        }

        private void parseFrames(object sender, EventArgs e)
        {
            webcamVid = camStreamCapture.QueryFrame();
            if (webcamVid == null)                  //quit if no image received
            {
                return;
            }

            CvInvoke.Imshow("Incoming Video Stream", webcamVid);

            button = CvInvoke.WaitKey(30);
            if (button == 27)
            {
                Console.Write("ESC pressed.");
            }
        }


        void getVideoInfo()            //get the video properties
        {
            vidPixelHeight = (int)camStreamCapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight);
            vidPixelWidth = (int)camStreamCapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth);
        }

        public bool streamingInProgress()
        {
            return isStreaming;
        }

    }
}
