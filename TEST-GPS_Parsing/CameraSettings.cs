using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST_GPS_Parsing
{
    class CameraSettings
    {
        //video parameters
        //The index of video mode defines the selected item. 0=Video on PC 1=Webcam
        //The index of draw mode defines the selected item. 0=Random 1=Ordered 2=Tracking
        private int videoSource;
        private int drawMode;
        private string filenameToOpen;

        //video GPS coordinate extents
        double[] upperLeftCoords = new double[2];       //[0] = latitude top left; [1] = longitude top left
        double[] outerLimitCoords = new double[2];      //[0] = longitude top right; [1] = latitude bottom left

        public bool setCameraCoordBounds(string _upperLeft0, string _upperLeft1, string _upperRight0, string _bottomLeft1)
        {
            //save the co-ordinate limits to the relative variables
            bool parseSuccess = false;
            int parseSuccessCount = 0;

            //convert the text to double and check
            parseSuccess = double.TryParse(_upperLeft0,System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out upperLeftCoords[0]);
            if (parseSuccess) { parseSuccessCount++; }else { parseSuccess = false; }
            parseSuccess = double.TryParse(_upperLeft1, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out upperLeftCoords[1]);
            if (parseSuccess) { parseSuccessCount++; } else { parseSuccess = false; }
            parseSuccess = double.TryParse(_upperRight0, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out outerLimitCoords[0]);
            if (parseSuccess) { parseSuccessCount++; } else { parseSuccess = false; }
            parseSuccess = double.TryParse(_bottomLeft1, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out outerLimitCoords[1]);
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
                videoSource = _videoSource;
                drawMode = _drawMode;
            }
            else
            {
                Console.Write("Failed to set draw mode choice.");
                return;
            }


            //only set the filename if the source is PC video
            if (_videoSource == 1)
            {
                filenameToOpen = _filenameToOpen;
            }
            else
            {
                filenameToOpen = "false";
            }
            
            Console.Write("Set video parameters successfully.");
        }

        public void startNewVideoConsole()
        {
            //now actually launch the process!
            Process videoCaptureProcess = new Process();
            videoCaptureProcess.StartInfo.FileName = "C:\\Users\\Matt\\Documents\\Source\\Final_Design\\x64\\Debug\\VideoStreamCPlusPlus.exe";

            /*parse through the arguments from the other app
            Arg[0] = process filename
            Arg[1] = lat-topLeft
            Arg[2] = long-topLeft
            Arg[3] = long-topRight
            Arg[4] = lat-botLeft
            Arg[5] = Picture mode; (0 = Internal Webcam; 1 = specify a file)
            Arg[6] = Draw mode; (0 = Random points; 1 = Ordered line)
            Arg[7] = file to open
            */
            videoCaptureProcess.StartInfo.Arguments = upperLeftCoords[0].ToString() + " " +
                                                      upperLeftCoords[1].ToString() + " " +
                                                      outerLimitCoords[0].ToString() + " " +
                                                      outerLimitCoords[1].ToString() + " " +
                                                      videoSource.ToString() + " " +
                                                      drawMode.ToString() + " " +
                                                      filenameToOpen;
            videoCaptureProcess.Start();

        }
    }
}
