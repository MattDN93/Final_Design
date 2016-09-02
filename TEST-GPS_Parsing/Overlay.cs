using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;

namespace TEST_GPS_Parsing
{
    class Overlay : VideoOutputWindow
    {
        int marker_x;                   //current centre x-position of overlay marker
        int marker_y;                   //current centre y-position of overlay marker

        Mat objectMarker;       //matrix to hold object marker 
        public Mat overlayGrid;        //"background" grid overlay matrix
        Mat srcBGR;

        Point orderedPt;                //point object for simulation
        List<Point> orderedSimPts;    //list of points to simulate
        
	    private int gridWidth;                  //specifies the overlay extents
        private int gridHeight;


        Point current_point;            //OpenCV Point struct for the current marker
        Point prev_point;               //OpenCV Point struct for the previous marker

        List<Point> points;           //vector of the last 10 points generated

        private string curr_posInfoString;      //string for the current position label

        public void setupOverlay()             //called once to initialize the overlay of points
        {
            overlayGrid = new Mat();     //sets up blank matrix of size of the video 
            orderedSimPts = new List<Point>();  //set up new list for simulated points
            points = new List<Point>();         //set up main points to display list
                
            //initialise the point element for the line-drawing
            prev_point.X = 0;
            prev_point.Y = 0;
            marker_x = 1;
            marker_y = 1;

            if (randomSim == false)         //generate ordered points to sim
            {
                orderedPt.X = 50;
                orderedPt.Y = 50;
                orderedSimPts.Add(orderedPt); //push back first ordered point

            }

        }

        public bool drawMarker(int current_xval, int current_yval, Mat webcamVidforOverlay, bool valHasChanged)
        {
            try
            {
                overlayGrid = webcamVidforOverlay.Clone();      //clone the incoming frame onto the overlay

                //write static text over video
                //void putText(Mat& img, const string& text, Point org, int fontFace, double fontScale, Scalar color, int thickness=1, int lineType=8, bool bottomLeftOrigin=false )
                CvInvoke.PutText(overlayGrid, "Capture in progress. Press ESC to end.", new Point(10, 10), Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 0.5, new MCvScalar(0, 0, 255), 1, Emgu.CV.CvEnum.LineType.EightConnected, false);

                ///checkBounds(current_xval, current_yval, overlayGrid);       //make sure these points aren't out of bounds

                if (valHasChanged == true)
                {

                    //set up the Point structs for markers
                    current_point.X = current_xval;
                    current_point.Y = current_yval;
                    prev_point.X = marker_x;
                    prev_point.Y = marker_y;

                    //pt_it = points.begin();                 //reallocate the iterator to vector start
                    points.Add(current_point);            //push back the current point into the vector

                }


                /*This loop does all of the above:
                    -draws the circle for each point in the vector
                    -draws the point co-ordinates in text
                    -draws a line from the points
                    -stores points in a vector

                */
                for (int i = 0; i < points.Count(); i++)          //use the iterator to traverse the points array
                {


                    //----------------Draw circle marker--------
                    //void circle(Mat& img, Point center, int radius, const Scalar& color, int thickness = 1, int lineType = 8, int shift = 0)
                    CvInvoke.Circle(overlayGrid, points[i], 5, new MCvScalar(0, 0, 255), -1, Emgu.CV.CvEnum.LineType.EightConnected, 0);

                    //----------------Draw label next to current point
                    //void putText(Mat& img, const string& text, Point org, int fontFace, double fontScale, Scalar color, int thickness=1, int lineType=8, bool bottomLeftOrigin=false )
                    //build a string with the current co-ords

                    curr_posInfoString = "";        //clear previous string contents
                    curr_posInfoString = "Pt:" + i + "(" + points[i].X + "," + points[i].Y + ")";
                    CvInvoke.PutText(overlayGrid, curr_posInfoString, points[i], Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 0.5, new MCvScalar(0, 0, 255), 1, Emgu.CV.CvEnum.LineType.EightConnected, false);

                    //----------------Draw joining line-----------
                    //Setting the below co-ords means you join lines between the CURRENT x_val and the LAST marker_x 
                    //args (line(Mat& img, Point pt1, Point pt2, const Scalar& color, int thickness=1, int lineType=8, int shift=0))
                    //colour is Scalar(B,G,R)
                    if (i >= 1)
                    {
                        CvInvoke.Line(overlayGrid, points[i], points[i - 1], new MCvScalar(0, 0, 255), 2, Emgu.CV.CvEnum.LineType.EightConnected, 0);
                    }

                }

                //this is the value stored from the last call
                marker_x = current_xval;
                marker_y = current_yval;
                return true;
            }
            catch(Exception e)
            {
                //if something went wrong
                return false;
                throw e;
            }


        }





    }
}
