#pragma once
#ifndef OVERLAY_H 
#define OVERLAY_H

#ifndef OPENCV_HEAD
#define OPENCV_HEAD

#include <opencv2/core.hpp>
#include <opencv2/imgcodecs.hpp>
#include <opencv2/highgui.hpp>
#include <opencv2/video.hpp>
#include <opencv2/videoio.hpp>

#endif // !OPENCV_HEAD

#ifndef CAMSTREAM_H
#include "camStream.h"
#endif // !CAMSTREAM_H

class Overlay : public camStream
{
public:
	Overlay();
	~Overlay();

	void getVideoInfo(int,int);		//get the dimensions of the video file 
	void setupOverlay();			//setup the matrices for the overlay
	void drawMarker(int,int,Mat,bool);		//function to place the marker on the video at (x,y)
	
	int marker_x;					//current centre x-position of overlay marker
	int marker_y;					//current centre y-position of overlay marker
	
	Mat objectMarker;		//matrix to hold object marker 
	Mat overlayGrid;		//"background" grid overlay matrix
	Mat srcBGR;				


private:
	int gridWidth;					//specifies the overlay extents
	int gridHeight;


	Point current_point;			//OpenCV Point struct for the current marker
	Point prev_point;				//OpenCV Point struct for the previous marker

};


#endif
