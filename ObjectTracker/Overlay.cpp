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
	objectMarker = imread("C:\\Users\\Matt\\Documents\\Source\\Final_Design\\ObjectTracker\\editmarker1.png", CV_LOAD_IMAGE_UNCHANGED);					//load objectmarker into matrix
	resize(objectMarker, objectMarker, Size(30, 30));				//resize the target reticle 
	
	overlayGrid.zeros(Size(gridWidth, gridHeight), CV_8UC1);		//set up a blank grid matrix
	//resize(overlayGrid, overlayGrid, Size(gridWidth, gridHeight));	//resize the overlay to match the video size
	
}

void Overlay::drawMarker(int x_val, int y_val,Mat webcamVidforOverlay)
{
	if (!objectMarker.empty())
	{
		srcBGR = Mat(objectMarker.size(), CV_8UC3);
		//cvtColor(objectMarker, objectMarker, CV_GRAY2BGR);
		overlayGrid = webcamVidforOverlay.clone();
		int cx = (overlayGrid.cols - 30) / 2;
		Rect markerBoundsBox = Rect(cx, overlayGrid.rows / 2, 30, 30);
		Mat markerBounds = overlayGrid(markerBoundsBox);
		srcBGR.copyTo(markerBounds);
		imshow("Marker On-screen", overlayGrid);
	}

}

