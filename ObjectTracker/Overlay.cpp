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
	imshow("loaded img", objectMarker);
	resize(objectMarker, objectMarker, Size(30, 30));				//resize the target reticle 
	
	overlayGrid.zeros(Size(gridWidth, gridHeight), CV_8UC3);		//set up a blank grid matrix
	//resize(overlayGrid, overlayGrid, Size(gridWidth, gridHeight));	//resize the overlay to match the video size DON'T USE
	
}

void Overlay::drawMarker(int x_val, int y_val,Mat webcamVidforOverlay)
{
	overlayGrid = webcamVidforOverlay.clone();
	int cx = (overlayGrid.cols - 30) / 2;
	if (!objectMarker.empty())
	{
		srcBGR = Mat(objectMarker.size(), CV_8UC3);
		//cvtColor(objectMarker, srcBGR, CV_BGR2GRAY);								//CRASHES ATM
		//TEST BELOW
		//Rect markerBoundsBox = Rect(cx, overlayGrid.rows / 2, 30, 30);			//rewrite Rect(x_val+15,y_val+15,30,30)
		Rect markerBoundsBox = Rect(x_val + 15, y_val + 15, 30, 30);

		Mat markerBounds = overlayGrid(markerBoundsBox);
		srcBGR.copyTo(markerBounds);

	}
	imshow("Marker On-screen", overlayGrid);

}

