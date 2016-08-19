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
	prev_point.x = 1;
	prev_point.y = 1;
	
}

void Overlay::drawMarker(int current_xval, int current_yval,Mat webcamVidforOverlay)
{

		overlayGrid = webcamVidforOverlay.clone();
		if (!objectMarker.empty())
		{
			//set up the Point structs for markers
			current_point.x = current_xval + 15;
			current_point.y = current_yval + 15;

//			srcBGR = Mat(objectMarker.size(), CV_8UC3);
//			Rect markerBoundsBox = Rect(current_xval, current_yval, 30, 30);

			//----------------Draw circle marker--------
			//void circle(Mat& img, Point center, int radius, const Scalar& color, int thickness = 1, int lineType = 8, int shift = 0)
			circle(overlayGrid, current_point, 12, Scalar(0, 0, 255), -1, 8, 0);

			//----------------Draw joining line-----------
			//Setting the below co-ords means you join lines between the CURRENT x_val and the LAST marker_x 
			//args (line(Mat& img, Point pt1, Point pt2, const Scalar& color, int thickness=1, int lineType=8, int shift=0))
			//colour is Scalar(B,G,R)
			line(overlayGrid, current_point, prev_point, Scalar(0, 0, 255), 2, 8, 0);

//			Mat markerBounds = overlayGrid(markerBoundsBox);
//			srcBGR.copyTo(markerBounds);



			marker_x = current_xval + 15;
			marker_y = current_yval + 15;

			//load the previous point into the database
			prev_point.x = marker_x;
			prev_point.y = marker_y;
		}
		imshow("Marker On-screen", overlayGrid);
		//overlayGrid.release();
	

}

