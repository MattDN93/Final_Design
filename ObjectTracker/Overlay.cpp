#include "Overlay.h"


Overlay::Overlay()
{
	
}

Overlay::~Overlay()
{
}

//inherited method to set the dimensions of the grid to those of the video
void Overlay::getVideoInfo(int wdth, int hgt)
{
	gridHeight = hgt;
	gridWidth = wdth;
}

void Overlay::setupOverlay()
{
	//objectMarker = imread("C:\\Users\\Matt\\Documents\\Source\\Final_Design\\ObjectTracker\\mark.png", CV_LOAD_IMAGE_ANYCOLOR);					//load objectmarker into matrix
	//imshow("loaded img", objectMarker);
	//resize(objectMarker, objectMarker, Size(30, 30));				//resize the target reticle 
	
	overlayGrid.zeros(Size(gridWidth, gridHeight), CV_8UC3);		//set up a blank grid matrix
	
	videoFrame.zeros(Size(gridWidth, gridHeight), CV_8UC3);

	//initialise the point element for the line-drawing
	prev_point.x = 0;
	prev_point.y = 0;
	marker_x = 1;
	marker_y = 1;

	if (randomSim == false)			//generate ordered points to sim
	{
		orderedPt.x = 50;
		orderedPt.y = 50;
		orderedSimPts.push_back(orderedPt);	//push back first ordered point

	}


	
}

void Overlay::drawMarker(int current_xval, int current_yval,Mat webcamVidforOverlay, bool valHasChanged)
{
		//videoFrame = webcamVidforOverlay.clone();
		overlayGrid = webcamVidforOverlay.clone();

		//write static text over video
		putText(overlayGrid, "Capture in progress. Press ESC to end.", Point(10,10) , FONT_HERSHEY_COMPLEX_SMALL, 0.5, Scalar(0, 0, 255), 1, 8, false);

		checkBounds(current_xval, current_yval, overlayGrid);

			if (valHasChanged == true)
			{

				//set up the Point structs for markers
				current_point.x = current_xval;
				current_point.y = current_yval;
				prev_point.x = marker_x;
				prev_point.y = marker_y;

				pt_it = points.begin();					//reallocate the iterator to vector start
				points.push_back(current_point);			//push back the current point into the vector

			}

//			srcBGR = Mat(objectMarker.size(), CV_8UC3);
//			Rect markerBoundsBox = Rect(current_xval, current_yval, 30, 30);

		
			/*This loop does all of the above:
				-draws the circle for each point in the vector
				-draws the point co-ordinates in text
				-draws a line from the points
				-stores points in a vector
			
			*/
			for (size_t i = 0; i < points.size(); i++)			//use the iterator to traverse the points array
			{
				

				//----------------Draw circle marker--------
				//void circle(Mat& img, Point center, int radius, const Scalar& color, int thickness = 1, int lineType = 8, int shift = 0)
				circle(overlayGrid, points[i], 5, Scalar(0, 0, 255), -1, 8, 0);				

				//----------------Draw label next to current point
				//void putText(Mat& img, const string& text, Point org, int fontFace, double fontScale, Scalar color, int thickness=1, int lineType=8, bool bottomLeftOrigin=false )
				//build a string with the current co-ords

				curr_posInfoString.clear();
				curr_positionInfoSS.str("");
				curr_positionInfoSS << "Pt:" << i << "(" << points[i].x << "," << points[i].y << ")";
				curr_posInfoString = curr_positionInfoSS.str();

				putText(overlayGrid, curr_posInfoString, points[i], FONT_HERSHEY_COMPLEX_SMALL, 0.5, Scalar(0, 0, 255), 1, 8, false);
				
				//----------------Draw joining line-----------
				//Setting the below co-ords means you join lines between the CURRENT x_val and the LAST marker_x 
				//args (line(Mat& img, Point pt1, Point pt2, const Scalar& color, int thickness=1, int lineType=8, int shift=0))
				//colour is Scalar(B,G,R)
				if (i >=1)
				{
					line(overlayGrid, points[i], points[i - 1], Scalar(0, 0, 255), 2, 8, 0);
				}
				
			}

//			Mat markerBounds = overlayGrid(markerBoundsBox);
///			srcBGR.copyTo(markerBounds);

			//this is the value stored from the last call
			marker_x = current_xval;
			marker_y = current_yval;

		

		//show the actual frames in a GUI
		imshow("Marker On-screen", overlayGrid);
///		//overlayGrid.release();
	

}

void Overlay::checkBounds(int curr_x, int curr_y, Mat overlay) 
{
	//the overlay grid gives us the size of the video so that we can determine where the limits are
	//Overlay.cols = number of x-colums (pixels) in the image
	//Overlay.rows = number of y-rows (pixels) in the image
	int xLimit = overlay.cols;
	int yLimit = overlay.rows;

	if (curr_x < 0 || curr_x > xLimit)
	{
		limitExceededLong.clear();
		LimitExceededLong_SS.str("");
		LimitExceededLong_SS << "Longitude offscreen: (" << curr_x << "," << curr_y << ")";
		limitExceededLong = LimitExceededLong_SS.str();
		putText(overlayGrid, limitExceededLong, Point(10, 20), FONT_HERSHEY_COMPLEX_SMALL, 0.5, Scalar(255, 0, 0), 1, 8, false);
	}
	if (curr_y < 0 || curr_y > yLimit)
	{
		limitExceededLat.clear();
		LimitExceededLat_SS.str("");
		LimitExceededLat_SS << "Latitude offscreen: (" << curr_x << "," << curr_y << ")";
		limitExceededLat = LimitExceededLat_SS.str();
		putText(overlayGrid, limitExceededLat, Point(10, 30), FONT_HERSHEY_COMPLEX_SMALL, 0.5, Scalar(255, 0, 0), 1, 8, false);
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

	
	if (curr_x < 0)		//narrow to A,B,C
	{
		if (curr_y<0)
		{
			//draw line to corner of A
			arrowedLine(overlayGrid, Point(50, 50), Point(0, 0), Scalar(255, 0, 0), 0.5, CV_AA, 0, 0.1);
			putText(overlayGrid, "Offscreen here", Point(40, 40), FONT_HERSHEY_COMPLEX_SMALL, 0.5, Scalar(255, 0, 0), 1, 8, false);
		}
		else if (curr_y > 0 && curr_y < yLimit)
		{
			//draw line to the y-part of B
			arrowedLine(overlayGrid, Point(50, curr_y), Point(0, curr_y), Scalar(255, 0, 0), 0.5, CV_AA, 0, 0.1);
			putText(overlayGrid, "Offscreen here", Point(40, curr_y), FONT_HERSHEY_COMPLEX_SMALL, 0.5, Scalar(255, 0, 0), 1, 8, false);
		}
		else if (curr_y > yLimit)
		{
			//draw line to point of C
			arrowedLine(overlayGrid, Point(0, yLimit), Point(50, yLimit - 50), Scalar(255, 0, 0), 0.5, CV_AA, 0, 0.1);
			putText(overlayGrid, "Offscreen here", Point(40, yLimit - 40), FONT_HERSHEY_COMPLEX_SMALL, 0.5, Scalar(255, 0, 0), 1, 8, false);
		}
		
	}
	else if (curr_x > 0 && curr_x < xLimit)		//narrow to D or E
	{
		if (curr_y < 0)
		{
			//draw D
			arrowedLine(overlayGrid, Point(curr_x, 50), Point(curr_x, 0), Scalar(255, 0, 0), 0.5, CV_AA, 0, 0.1);
			putText(overlayGrid, "Offscreen here", Point(curr_x, 40), FONT_HERSHEY_COMPLEX_SMALL, 0.5, Scalar(255, 0, 0), 1, 8, false);
		}
		else if (curr_y > yLimit)
		{
			//draw E
			arrowedLine(overlayGrid, Point(curr_x, yLimit - 60), Point(curr_x, yLimit), Scalar(255, 0, 0), 0.5, CV_AA, 0, 0.1);
			putText(overlayGrid, "Offscreen here", Point(curr_x, yLimit - 40), FONT_HERSHEY_COMPLEX_SMALL, 0.5, Scalar(255, 0, 0), 1, 8, false);
		}
	}
	else if (curr_x > xLimit)					//narrow to F,G,H
	{
		if (curr_y < 0)
		{
			//draw F
			arrowedLine(overlayGrid, Point(xLimit - 80, 50), Point(xLimit, 0), Scalar(255, 0, 0), 0.5, CV_AA, 0, 0.1);
			putText(overlayGrid, "Offscreen here", Point(xLimit - 100, 50), FONT_HERSHEY_COMPLEX_SMALL, 0.5, Scalar(255, 0, 0), 1, 8, false);
		}
		else if (curr_y > 0 && curr_y < yLimit)
		{
			//draw G
			arrowedLine(overlayGrid, Point(xLimit - 80, curr_y), Point(xLimit, curr_y), Scalar(255, 0, 0), 0.5, CV_AA, 0, 0.1);
			putText(overlayGrid, "Offscreen here", Point(xLimit - 100, curr_y), FONT_HERSHEY_COMPLEX_SMALL, 0.5, Scalar(255, 0, 0), 1, 8, false);
		}
		else if (curr_y > yLimit)
		{
			//draw H
			arrowedLine(overlayGrid, Point(xLimit - 80, yLimit - 80), Point(xLimit,yLimit), Scalar(255, 0, 0), 0.5, CV_AA, 0, 0.1);
			putText(overlayGrid, "Offscreen here", Point(xLimit - 100, yLimit - 40), FONT_HERSHEY_COMPLEX_SMALL, 0.5, Scalar(255, 0, 0), 1, 8, false);
		}

	}
	
}

