﻿using System;
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
        //---------marker data vars----------
        int marker_x;                   //current centre x-position of overlay marker
        int marker_y;                   //current centre y-position of overlay marker
        public int gridWidth;          //specifies the overlay extents
        public int gridHeight;

        //---------overlay structures--------
        public Mat overlayGrid;        //"background" grid overlay matrix

        //---------Point structures----------
        public Point orderedPt;                // [SIMULATION]point object for simulation
        protected List<Point> orderedSimPts;      // [SIMULATION]list of points to simulate  
        Point current_point;            //OpenCV Point struct for the current marker
        Point prev_point;               //OpenCV Point struct for the previous marker
        List<Point> points;           //vector of the last 10 points generated by data

        //---------strings for OSD------------
        string curr_posInfoString;
        public string LimitExceededLong { get; private set; }
        public string LimitExceededLat { get; private set; }

        //------------Simulation vars-------------------
        public int randCountTime { get; private set; }
        public int x { get; private set; }
        public int y { get; private set; }
        Random randPointGenerator;

        public void setupOverlay()             //called once to initialize the overlay of points
        {
            overlayGrid = new Mat();     //sets up blank matrix of size of the video 
            orderedSimPts = new List<Point>();  //set up new list for simulated points
            points = new List<Point>();         //set up main points to display list

            randPointGenerator = new Random();                  //[SIMULATION]seed random number

            //initialise the point element for the line-drawing

            prev_point.X = 0;
            prev_point.Y = 0;
            marker_x = 1;
            marker_y = 1;

            if (drawMode_Overlay == DRAW_MODE_ORDERED)         //generate ordered points to sim
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
                //void putText(Mat& img, const string& text, Point org, int fontFace, double fontScale, new MCvScalar color, int thickness=1, int lineType=8, bool bottomLeftOrigin=false )
                CvInvoke.PutText(overlayGrid, "Capture in progress. Press ESC to end.", new Point(10, 10), Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 0.5, new MCvScalar(0, 0, 255), 1, Emgu.CV.CvEnum.LineType.EightConnected, false);

                checkBounds(current_xval, current_yval, overlayGrid);       //make sure these points aren't out of bounds

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
                    //void putText(Mat& img, const string& text, Point org, int fontFace, double fontScale, new MCvScalar color, int thickness=1, int lineType=8, bool bottomLeftOrigin=false )
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

        //This method checks the location of the points on the grid and displays markers if it exceeds these points
        public void checkBounds(int curr_x, int curr_y, Mat overlay)
        {
            //the overlay grid gives us the size of the video so that we can determine where the limits are
            //Overlay.cols = number of x-colums (pixels) in the image
            //Overlay.rows = number of y-rows (pixels) in the image
            int xLimit = overlay.Cols;
            int yLimit = overlay.Rows;

            if (curr_x < 0 || curr_x > xLimit)
            {
                LimitExceededLong = "Longitude offscreen: (" + curr_x + "," + curr_y + ")";
                longitudeOutOfRangeOverlayMessage = LimitExceededLong;      //write this message to the UI class
                CvInvoke.PutText(overlayGrid, LimitExceededLong, new Point(10, 20), Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 0.5, new MCvScalar(255, 0, 0), 1, Emgu.CV.CvEnum.LineType.EightConnected, false);
            }
            else
            {
                longitudeOutOfRangeOverlayMessage = "";     //clear out of bounds message
            }
            if (curr_y < 0 || curr_y > yLimit)
            {
                LimitExceededLat = "";
                LimitExceededLat = "Latitude offscreen: (" + curr_x + "," + curr_y + ")";
                latitudeOutOfRangeOverlayMessage = LimitExceededLat;        //write this message to the UI class
                CvInvoke.PutText(overlayGrid, LimitExceededLat, new Point(10, 30), Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 0.5, new MCvScalar(255, 0, 0), 1, Emgu.CV.CvEnum.LineType.EightConnected, false);
                
            }
            else
            {
                latitudeOutOfRangeOverlayMessage = "";          //clear out of bounds message
            }

            //systematic check to determine where the offscreen point is so that we can draw an arrow to it
            /*
                    The letters correspond to the zones we check in order shown
                    The top-left corner is the origin of the video frame
                    _A_|________D_____|_F_
                     B |	onscreen  | G
                    __ |_____________ |__
                     C |		E	  | H

                     Drawing arrow:
                     arrowedLine(InputOutputArray img, Point pt1, Point pt2, const Scalar& color, int thickness=1, int lineType=8, int shift=0, double tipLength=0.1)

            */


            if (curr_x < 0)     //narrow to A,B,C
            {
                if (curr_y < 0)
                {
                    //draw line to corner of A
                    CvInvoke.ArrowedLine(overlayGrid, new Point(50, 50), new Point(0, 0), new MCvScalar(255, 0, 0), 1, Emgu.CV.CvEnum.LineType.AntiAlias, 0, 0.1);
                    CvInvoke.PutText(overlayGrid, "Offscreen here", new Point(40, 40), Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 0.5, new MCvScalar(255, 0, 0), 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
                    latitudeOutOfRangeOverlayMessage = "Latitude Out Of Range: " + curr_y.ToString();
                    longitudeOutOfRangeOverlayMessage = "Longitude Out Of Range:" + curr_x.ToString();
                }
                else if (curr_y > 0 && curr_y < yLimit)
                {
                    //draw line to the y-part of B
                    CvInvoke.ArrowedLine(overlayGrid, new Point(50, curr_y), new Point(0, curr_y), new MCvScalar(255, 0, 0), 1, Emgu.CV.CvEnum.LineType.AntiAlias, 0, 0.1);
                    CvInvoke.PutText(overlayGrid, "Offscreen here", new Point(40, curr_y), Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 0.5, new MCvScalar(255, 0, 0), 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
                    latitudeOutOfRangeOverlayMessage = "Latitude in range - Switch Cam Left";
                    longitudeOutOfRangeOverlayMessage = "Longitude " + curr_x.ToString() + " OOR - Switch Cam Left";
                }
                else if (curr_y > yLimit)
                {
                    //draw line to point of C
                    CvInvoke.ArrowedLine(overlayGrid, new Point(0, yLimit), new Point(50, yLimit - 50), new MCvScalar(255, 0, 0), 1, Emgu.CV.CvEnum.LineType.AntiAlias, 0, 0.1);
                    CvInvoke.PutText(overlayGrid, "Offscreen here", new Point(40, yLimit - 40), Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 0.5, new MCvScalar(255, 0, 0), 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
                    latitudeOutOfRangeOverlayMessage = "Latitude Out Of Range: " + curr_y.ToString();
                    longitudeOutOfRangeOverlayMessage = "Longitude Out Of Range:" + curr_x.ToString();
                }

            }
            else if (curr_x > 0 && curr_x < xLimit)     //narrow to D or E
            {
                if (curr_y < 0)
                {
                    //draw D
                    CvInvoke.ArrowedLine(overlayGrid, new Point(curr_x, 50), new Point(curr_x, 0), new MCvScalar(255, 0, 0), 1, Emgu.CV.CvEnum.LineType.AntiAlias, 0, 0.1);
                    CvInvoke.PutText(overlayGrid, "Offscreen here", new Point(curr_x, 40), Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 0.5, new MCvScalar(255, 0, 0), 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
                    latitudeOutOfRangeOverlayMessage = "Latitude out of range";
                    longitudeOutOfRangeOverlayMessage = "Longitude " + curr_y.ToString() + " in range above";
                }
                else if (curr_y > yLimit)
                {
                    //draw E
                    CvInvoke.ArrowedLine(overlayGrid, new Point(curr_x, yLimit - 60), new Point(curr_x, yLimit), new MCvScalar(255, 0, 0), 1, Emgu.CV.CvEnum.LineType.AntiAlias, 0, 0.1);
                    CvInvoke.PutText(overlayGrid, "Offscreen here", new Point(curr_x, yLimit - 40), Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 0.5, new MCvScalar(255, 0, 0), 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
                    latitudeOutOfRangeOverlayMessage = "Latitude out of range";
                    longitudeOutOfRangeOverlayMessage = "Longitude " + curr_y.ToString() + " in range below";
                }
            }
            else if (curr_x > xLimit)                   //narrow to F,G,H
            {
                if (curr_y < 0)
                {
                    //draw F
                    CvInvoke.ArrowedLine(overlayGrid, new Point(xLimit - 80, 50), new Point(xLimit, 0), new MCvScalar(255, 0, 0), 1, Emgu.CV.CvEnum.LineType.AntiAlias, 0, 0.1);
                    CvInvoke.PutText(overlayGrid, "Offscreen here", new Point(xLimit - 100, 50), Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 0.5, new MCvScalar(255, 0, 0), 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
                    latitudeOutOfRangeOverlayMessage = "Latitude Out Of Range: " + curr_y.ToString();
                    longitudeOutOfRangeOverlayMessage = "Longitude Out Of Range:" + curr_x.ToString();
                }
                else if (curr_y > 0 && curr_y < yLimit)
                {
                    //draw G
                    CvInvoke.ArrowedLine(overlayGrid, new Point(xLimit - 80, curr_y), new Point(xLimit, curr_y), new MCvScalar(255, 0, 0), 1, Emgu.CV.CvEnum.LineType.AntiAlias, 0, 0.1);
                    CvInvoke.PutText(overlayGrid, "Offscreen here", new Point(xLimit - 100, curr_y), Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 0.5, new MCvScalar(255, 0, 0), 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
                    latitudeOutOfRangeOverlayMessage = "Latitude in range - Switch Cam Right";
                    longitudeOutOfRangeOverlayMessage = "Longitude " + curr_x.ToString() + " OOR - Switch Cam Right";
                }
                else if (curr_y > yLimit)
                {
                    //draw H
                    CvInvoke.ArrowedLine(overlayGrid, new Point(xLimit - 80, yLimit - 80), new Point(xLimit, yLimit), new MCvScalar(255, 0, 0), 1, Emgu.CV.CvEnum.LineType.AntiAlias, 0, 0.1);
                    CvInvoke.PutText(overlayGrid, "Offscreen here", new Point(xLimit - 100, yLimit - 40), Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 0.5, new MCvScalar(255, 0, 0), 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
                    latitudeOutOfRangeOverlayMessage = "Latitude Out Of Range: " + curr_y.ToString();
                    longitudeOutOfRangeOverlayMessage = "Longitude Out Of Range:" + curr_x.ToString();
                }
                else
                {
                    //clear the out of range messages since nothing was out of range
                    longitudeOutOfRangeOverlayMessage = "";
                    latitudeOutOfRangeOverlayMessage = ""; 
                }

            }

        }

        public void generateSimPts()
        {
            //-----------------SIMULATION OF POINTS-------------------------
                if (drawMode_Overlay == DRAW_MODE_RANDOM)
                {
                    randCountTime = 0;
                    x = randPointGenerator.Next(2000);
                    y = randPointGenerator.Next(1000);
                    //drawMarker(x, y, webcamVid, true);                  //informs routine that previous marker value must be saved for new one
                }
                else if (drawMode_Overlay == DRAW_MODE_ORDERED)                    //generate "ordered" list of values that increase as we go along
                {
                    x = randPointGenerator.Next(-5, 55);          //x = (-10 to +50)
                    y = randPointGenerator.Next(-8, 40);          //y = (-20 to +40)

                    orderedPt.X = orderedSimPts.ElementAt(orderedSimPts.Count() - 1).X + x;       //add a random x increment to the vector
                    orderedPt.Y = orderedSimPts.ElementAt(orderedSimPts.Count() - 1).Y + y;       //add a random y increment to the vector

                    orderedSimPts.Add(orderedPt);             //push onto the new vector and display
                    //drawMarker(orderedPt.X, orderedPt.Y, webcamVid, true);                  //informs routine that previous marker value must be saved for new one
                }
                //------------------END POINT SIM-------------------------------
          

        }




        }
}
