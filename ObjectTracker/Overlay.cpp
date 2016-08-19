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
	objectMarker = imread("C:\\Users\\Matt\\Documents\\Source\\Final_Design\\ObjectTracker\\mark.png", CV_LOAD_IMAGE_ANYCOLOR);					//load objectmarker into matrix
	//imshow("loaded img", objectMarker);
	resize(objectMarker, objectMarker, Size(30, 30));				//resize the target reticle 
	
	overlayGrid.zeros(Size(gridWidth, gridHeight), CV_8UC3);		//set up a blank grid matrix

	//initialise the point element for the line-drawing
	prev_point.x = 0;
	prev_point.y = 0;
	marker_x = 1;
	marker_y = 1;
	
}

void Overlay::drawMarker(int current_xval, int current_yval,Mat webcamVidforOverlay, bool valHasChanged)
{

		overlayGrid = webcamVidforOverlay.clone();
		if (!objectMarker.empty())
		{
			//set up the Point structs for markers
			current_point.x = current_xval + 15;
			current_point.y = current_yval + 15;

			if (valHasChanged == true)
			{
				prev_point.x = marker_x;
				prev_point.y = marker_y;
			}

//			srcBGR = Mat(objectMarker.size(), CV_8UC3);
//			Rect markerBoundsBox = Rect(current_xval, current_yval, 30, 30);

			//----------------Draw circle marker--------
			//void circle(Mat& img, Point center, int radius, const Scalar& color, int thickness = 1, int lineType = 8, int shift = 0)
			circle(overlayGrid, current_point, 12, Scalar(0, 0, 255), -1, 8, 0);		//current point
			circle(overlayGrid, prev_point, 12, Scalar(0, 0, 255), -1, 8, 0);			//last point

			//----------------Draw label next to current point
			//void putText(Mat& img, const string& text, Point org, int fontFace, double fontScale, Scalar color, int thickness=1, int lineType=8, bool bottomLeftOrigin=false )
			//build a string with the current co-ords
			std::ostringstream curr_positionInfoSS, prev_positionInfoSS;
			curr_positionInfoSS << "Current: (" << current_point.x << "," << current_point.y << ")";
			prev_positionInfoSS << "Previous: (" << prev_point.x << "," << prev_point.y << ")";
			string curr_posInfoString = curr_positionInfoSS.str();
			string prev_posInfoString = prev_positionInfoSS.str();
			
			putText(overlayGrid, curr_posInfoString, current_point,FONT_HERSHEY_PLAIN,1,Scalar(0,0,255),2,8,false );
			putText(overlayGrid, prev_posInfoString, prev_point, FONT_HERSHEY_PLAIN, 1, Scalar(0, 0, 255), 2, 8, false);

			//----------------Draw joining line-----------
			//Setting the below co-ords means you join lines between the CURRENT x_val and the LAST marker_x 
			//args (line(Mat& img, Point pt1, Point pt2, const Scalar& color, int thickness=1, int lineType=8, int shift=0))
			//colour is Scalar(B,G,R)
			line(overlayGrid, current_point, prev_point, Scalar(0, 0, 255), 2, 8, 0);

//			Mat markerBounds = overlayGrid(markerBoundsBox);
//			srcBGR.copyTo(markerBounds);

			//this is the value stored from the last call
			marker_x = current_xval + 15;
			marker_y = current_yval + 15;

		}
		imshow("Marker On-screen", overlayGrid);
		//overlayGrid.release();
	

}

