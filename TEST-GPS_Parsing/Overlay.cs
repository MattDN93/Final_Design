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
        #region Declaration Object and Vars
        //---------marker data vars FOR DISPLAY----------
        //OpenCV requires the marker and point objects onscreen to be of INT type
        //The raw GPS coordinates are rounted to a int pixel value and stored in these 
        //Raw GPS data in double form is stored below

        int marker_x;                   //current centre x-position of overlay marker
        int marker_y;                   //current centre y-position of overlay marker
        public int gridWidth;          //specifies the overlay extents
        public int gridHeight;

        //---------Point structures FOR DISPLAY----------
        public Point orderedPt;                // [SIMULATION]point object for simulation
        protected List<Point> orderedSimPts;      // [SIMULATION]list of points to simulate  
        Point current_point;            //OpenCV Point struct for the current marker
        Point prev_point;               //OpenCV Point struct for the previous marker
        List<Point> points;           //vector of the last 10 points generated by data

        //---------Point structures for incoming co-ord calculations---
        //Performing the bound calculations requires doubles, which are later rounded to 
        //ints for display onscreen.

        public double current_pointGPS_lat;        //the decimal floating pt rep for the GPS coords
        public double current_pointGPS_long;
        public double dx;                          //delta_X storage from VideoOutput class
        public double dy;                          //delta_Y from Video output class
        public double[] ulBound = new double[2];         //[0] = latitude top left; [1] = longitude top left
        public double[] olBound = new double[2];        //[0] = longitude top right; [1] = latitude bottom left
        private double p;                               //(p,q) is the (lat,long) pair when scaling up onscreen coords with object track mode
        private double q;
         
        //---------overlay structures--------
        public Mat overlayGrid;        //"background" grid overlay matrix


        //---------strings for OSD------------
        string curr_posInfoString;
        public string LimitExceededLong { get; private set; }
        public string LimitExceededLat { get; private set; }

        //------------Simulation vars-------------------
        public int randCountTime { get; private set; }
        public int x { get; private set; }
        public int y { get; private set; }
        public bool displayCoordTextOnscreen { get; set; }

        Random randPointGenerator;

        //------------Usage flags------------------------
        public bool usingCoords;

        #endregion

        #region Setup and Initialization
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

            usingCoords = false;
        }
        #endregion

        #region Setting and Scaling Co-ords
        //a method to allow the parser class to access the coords vars without race conditions
        public bool setNewCoords(double lat_forDisplay, double long_forDisplay)
        {
            if (usingCoords == true) //make thread wait until we're done using the data
            {
                return false;
            }
            else
            {
                current_pointGPS_lat = lat_forDisplay;
                current_pointGPS_long = long_forDisplay;

                try
                {
                    scaleGpsCoordsToDisplayBounds(lat_forDisplay, long_forDisplay);
                }
                catch (OverflowException e)
                {
                    throw e;
                }
                
                return true;
            }
        }



        //this method clears the overlay object and removes the displayed points from the screen
        public void clearScreen()
        {
            points.Clear();
            current_point.X = 0;
            current_point.Y = 0;
            marker_x = 0;
            marker_y = 0;
            //overlayGrid.Dispose();
        }

        //This method takes the lat/long limits of the camera frame, calculates an offset based on
        //where the given GPS coords are, and then scales it to a pixel value to be overlayed onscreen
        public void scaleGpsCoordsToDisplayBounds(double incoming_lat,double incoming_long)
        {
            /* DEFINITIONS:
             * UpperLeftBound[0] = latitude top left; [1] = longitude top left
            outerLimitBound[0] = longitude top right; [1] = latitude bottom left
            
            Formulae:   given a current (incoming_lat, incoming_long) in (p,q)
                        latTopLeft = b; longTopLeft = a
                        delta(y) = latitude range of camera frame (calculated onetime)
                        delta(x) = longitude range of camera framse (calculated onetime)
                        y_pixels = image frame height; x_pixels = image frame width
                        we want an output scaled point (r,s)

                        s = ((|p-b|)/delta(y)) * y_pixels
                        r = ((q-a)/delta(x)) * x_pixels
                        
                        (r,s) is then scaled down to an integer

                        here,   s = marker_y or y
                                r = marker_x or x
             */

                y = Convert.ToInt32(Math.Round((Math.Abs(incoming_lat - ulBound[0]) / dy) * gridHeight));
                x = Convert.ToInt32(Math.Round((((incoming_long - ulBound[1]) / dx) * gridWidth)));


        }

        /*this method takes the onscreen location of the object in question and translates it to a lat/long 
          based on the lat/long limits specified by the user.
             */
        public void scaleDisplayCoordsToGpsBounds(int pix_x, int pix_y)
        {
            /* DEFINITIONS:
            * UpperLeftBound[0] = latitude top left; [1] = longitude top left
           outerLimitBound[0] = longitude top right; [1] = latitude bottom left

           Formulae:   given a current (pixel x, pixel y) in (pix_x,pix_y)
                       latTopLeft = b; longTopLeft = a
                       delta(y) = latitude range of camera frame (calculated onetime)
                       delta(x) = longitude range of camera framse (calculated onetime)
                       y_pixels = image frame height; x_pixels = image frame width
                       we want an output point (p,q) from the scaled screen version

                       p = latitude = (delta(y) * pix_y)/y + b
                       q = longitude = (delta(x) * pix_x)/x + a

                       (p,q) is a (lat,long) pair
                       p, q are doubles
            */

            p = ((dy * pix_y) / gridHeight) + ulBound[0];
            q = ((dx * pix_x) / gridWidth) + ulBound[1];


        }
        #endregion

        #region Drawing Onscreen Marker (Points)
        public bool drawMarker(int current_xval, int current_yval, Mat webcamVidforOverlay, bool valHasChanged)
        {
            try
            {
                if (captureChoice == DRAW_MODE_REVOBJTRACK)
                {
                    current_xval = x;
                    current_yval = y;
                }
                else
                {
                    overlayGrid = webcamVidforOverlay.Clone();      //clone the incoming frame onto the overlay
                }
                

                //write static text over video
                //void putText(Mat& img, const string& text, Point org, int fontFace, double fontScale, new MCvScalar color, int thickness=1, int lineType=8, bool bottomLeftOrigin=false )
                CvInvoke.PutText(overlayGrid, "Capture in progress. Press ESC to end.", new Point(10, 10), Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 0.5, new MCvScalar(0, 0, 255), 1, Emgu.CV.CvEnum.LineType.EightConnected, false);

                usingCoords = true;         //setting this to ensure the x & y values aren't touched by other thread. Bool setting is atomic

                checkBounds(current_xval, current_yval, overlayGrid);       //make sure these points aren't out of bounds

                //prevent a mess onscreen by clearing the markers every 200
                if (points.Count > 200)
                {
                    points.Clear();
                }


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
                    CvInvoke.Circle(overlayGrid, points[i], 2, new MCvScalar(0, 0, 255), -1, Emgu.CV.CvEnum.LineType.EightConnected, 0);

                    //----------------Draw label next to current point
                    //void putText(Mat& img, const string& text, Point org, int fontFace, double fontScale, new MCvScalar color, int thickness=1, int lineType=8, bool bottomLeftOrigin=false )
                    //build a string with the current co-ords

                    
                    if (displayCoordTextOnscreen == true)  //only draw (x,y) co-ords with non-object tracking modes
                    {
                        curr_posInfoString = "";        //clear previous string contents
                        //IMPORTANT: Co-ords are calculated on x and y offset in (x,y) but DISPLAYED as (lat(y),long(x))
                        curr_posInfoString = "Pt:" + i + "(" + points[i].Y + "," + points[i].X + ")";
                        CvInvoke.PutText(overlayGrid, curr_posInfoString, points[i], Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 0.5, new MCvScalar(0, 0, 255), 1, Emgu.CV.CvEnum.LineType.EightConnected, false);
                    }

                        //----------------Draw joining line-----------
                        //Setting the below co-ords means you join lines between the CURRENT x_val and the LAST marker_x 
                        //args (line(Mat& img, Point pt1, Point pt2, const Scalar& color, int thickness=1, int lineType=8, int shift=0))
                        //colour is Scalar(B,G,R)
                        if (i >= 1)
                        {
                            CvInvoke.Line(overlayGrid, points[i], points[i - 1], new MCvScalar(0, 0, 255), 1, Emgu.CV.CvEnum.LineType.EightConnected, 0);
                        }

                    }

                //this is the value stored from the last call
                marker_x = current_xval;
                marker_y = current_yval;
                usingCoords = false;        //release the variables to be modified by the other thread
                return true;
            }
            catch(Exception e)
            {
                //if something went wrong
                return false;
                throw e;
            }


        }
        #endregion

        #region Drawing Onscreen Marker (Polygons)
        public bool drawPolygons(Mat webcamVidForOverlay, bool onscreenPolyhasChanged = false)
        {
            //1. Noise removal
            //Mat processedFrame = new Mat();         //create a new frame to process
            Mat processedFrame = webcamVidForOverlay.Clone();

            CvInvoke.CvtColor(processedFrame, processedFrame, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);     //convert to grayscale
            Mat downSampled = new Mat();            //create a holding mat for downsampling
            CvInvoke.PyrDown(processedFrame, downSampled);  //use pyramid downsample
            CvInvoke.PyrUp(downSampled, processedFrame);     //upsample onto the original again

            //2. Canny Edge detection
            double thresholdLink = 80.0;           //value to force rejection/acceptance if pixel is between upper & lower thresh
            double thresholdLow = 40.0;               //lower brightness threshold 0-> 255 where 255 = white
            double thresholdHigh = 120;             //have a 3:1 upper:lower ratio (per Canny's paper)
            Mat cannyResult = new Mat();            //create a holding Mat for the canny edge result
            CvInvoke.Canny(processedFrame, cannyResult, thresholdLow, thresholdHigh);

            //3. Hough Probabilistic Transform
            //Uses probabilistic methods to determine all the polygons in the image
            //use (rho, theta) polar coordinate plane
            LineSegment2D[] polygonSegment = CvInvoke.HoughLinesP(
                cannyResult,
                1,                  //rho - distance 'vector'
                Math.PI / 45.0,     //theta - angle 'vector'
                20,                 //threshold for definition of 'line'
                30,                 //minimum line width
                10);                // gap between lines

            //4. Find the contours          

            //we draw the result to OverlayGrid which is accessed by the video output methods
            overlayGrid = webcamVidForOverlay.Clone();

            /*this object is a (x0,y0,x1,y1) vector where (x0,y0) and (x1,y1) are the respective
            extremities of a line
             */
            VectorOfPoint approxContour = new VectorOfPoint();
            using (VectorOfVectorOfPoint polyContours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(
                    cannyResult,        //the output of the Canny detector
                    polyContours,       //the 2D vector of line-points
                    null,               //not used since we don't want a hierarchy
                    Emgu.CV.CvEnum.RetrType.List,    //retrieve ALL contours, no hierarchy
                    Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple  //segments are compressed so only endpoints stored
                    );

                int numContours = polyContours.Size;  //add each point to an array to curve approximate


                for (int i = 0; i < numContours; i++)
                {
                    using (VectorOfPoint currentContour = polyContours[i])    //work on inner vector (i.e. the first set of points on the line)
                    { 
                    //Now draw the found rectangles onscreen!
                    CvInvoke.DrawContours(overlayGrid, polyContours, -1, new MCvScalar(150, 105, 0), 1, Emgu.CV.CvEnum.LineType.EightConnected, null);

                    /*use ApproxPoly to find some epsilon to match all points to,
                    Use the Ramer-Douglas-Peuker algo to simplify the currentContour curve and store it in
                    approxContour
                        */
                    CvInvoke.ApproxPolyDP(
                        currentContour,         //the current polygon
                        approxContour,          //the resultant smoothe polygon
                        CvInvoke.ArcLength(currentContour, true) * 0.05, //calculating epsilon by finding the arc length of start -> end
                        true);                  //is the contour closed?

                        if (approxContour.Size == 4)        //there are 4 contours -> rectangle
                        {
                            //for a rectangle, find internal angles, they must be 80 < angle < 100 to be a rectangle
                            bool validRect = true;
                            Point[] pts = approxContour.ToArray();  //make the contour into an array -> polyline
                            LineSegment2D[] polyEdges = PointCollection.PolyLine(pts, true);    //store each polygon edge for testing

                            for (int j = 0; j < polyEdges.Length; j++)  //traverse each edge and check angle
                            {
                                double angle = Math.Abs(
                                   polyEdges[(j + 1) % polyEdges.Length].GetExteriorAngleDegree(polyEdges[j]));
                                if (angle < 80 || angle > 100)
                                {
                                    validRect = false;
                                    break;
                                }

                            }

                            //find the area of the polygon to avoid false positives with very small areas
                            double polygonArea = CvInvoke.ContourArea(approxContour);

                            if (validRect && polygonArea > 250.0)
                            {
                                //if valid, travers the current array of edges and draw them to the screen
                                for (int k = 0; k < pts.Length; k++)
                                {
                                    CvInvoke.Polylines(overlayGrid, pts, true, new MCvScalar(0, 255, 0));
                                }

                                //find the centre of this rectangle & draw it onscreen
                                Point rectCentre = getCentre(approxContour);
                                CvInvoke.Circle(overlayGrid, rectCentre, 5, new MCvScalar(0, 150, 105), -1, Emgu.CV.CvEnum.LineType.AntiAlias);
                                string centreString = "(" + rectCentre.X + "," + rectCentre.Y + ")";
                                CvInvoke.PutText(overlayGrid, centreString, new Point(rectCentre.X + 2,rectCentre.Y + 2), Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 0.5, new MCvScalar(0, 150, 105), 1, Emgu.CV.CvEnum.LineType.EightConnected, false);

                                //pass the centre of the current rectangle's co-ords to the display & scaling methods
                                scaleDisplayCoordsToGpsBounds(rectCentre.X, rectCentre.Y);
                                //this method sets (p,q) as (lat,long) so we allocate these vars to the UI display vars

                                usingCoords = true;             //set this to prevent race conditions
                                current_pointGPS_lat = p;       //set these to separate coords which will be used to display the lat/long on the left UI
                                current_pointGPS_long = q;

                                x = rectCentre.X;               //set so that the drawMarker method can display the trail of points onscreen
                                y = rectCentre.Y;

                                //TEST
                                if (onscreenPolyhasChanged)
                                {
                                    drawMarker(x, y, overlayGrid, true);   //draw this point update it 
                                }
                                else
                                {
                                    drawMarker(x, y, overlayGrid, false);   //draw this point but don't update it yet
                                }

                                //END TEST

                                usingCoords = false;

                            }

                            

                        }//approxSize == 4

                    }//vector currentCOntour

                }   //end numContours

            }
            return true;
        

            
            

            
        }

        //finds the centre of the rectangle in question - only called if a rectangle has been found i.e. pts0-4 exist
        private Point getCentre(VectorOfPoint contour)
        {
            ////find the centre of the vector distance between pts 2 and 0, and 1 and 3
            ////this centre is the rectangle centre
            //double centreVal1 = 0.5 * (Math.Sqrt(Math.Pow((pts[2].Y - pts[0].Y), 2) + Math.Pow((pts[2].X - pts[0].X), 2)));
            //double centreVal2 = 0.5 * (Math.Sqrt(Math.Pow((pts[3].Y - pts[1].Y), 2) + Math.Pow((pts[3].X - pts[1].X), 2)));

            //double centreHyp = Math.Round((centreVal1 + centreVal2) / 2.0);

            //get the moments of the contour
            MCvMoments contourMoments =  CvInvoke.Moments(contour);

            /*The moments returned are used to find the centre (x,y) co-ords
                x = m10/m00
                y = m01/m00
             */
            int centre_x = Convert.ToInt32(contourMoments.M10 / contourMoments.M00);
            int centre_y = Convert.ToInt32(contourMoments.M01 / contourMoments.M00);

            Point centre = new Point(centre_x, centre_y);
            return centre;
        }
        #endregion

        #region Checking Bounds
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
        #endregion

        #region For Simulation Only
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
            else if (drawMode_Overlay == DRAW_MODE_TRACKING)
            {
                return;
            }
                //------------------END POINT SIM-------------------------------
          

        }
        #endregion



    }
}
