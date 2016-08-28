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

	void checkBounds(int , int, Mat);		//checks the current point for a match with the edges
	
	int marker_x;					//current centre x-position of overlay marker
	int marker_y;					//current centre y-position of overlay marker
	
	Mat overlayGrid;		//"background" grid overlay matrix
	Mat srcBGR;	

	Mat videoFrame;

	Point orderedPt;				//point object for simulation
	vector<Point> orderedSimPts;	//list of points to simulate

	double TopLeftCoords[2];		//Marks upper left limit of current frame (0=lat; 1=long)
	double OuterLimitsCoords[2];	//Marks 0=topright_long and 1=bottomleft_lat points




private:
	int gridWidth;					//specifies the overlay extents
	int gridHeight;

	Point current_point;			//OpenCV Point struct for the current marker
	Point prev_point;				//OpenCV Point struct for the previous marker

	vector<Point> points;			//vector of the last 10 points generated
	vector<Point>::iterator pt_it;	//iterator for the vector above

	ostringstream curr_positionInfoSS;		//string stream for the current position marker
	string curr_posInfoString;

	ostringstream LimitExceededLat_SS;		//stringstream for lat exceeded
	ostringstream LimitExceededLong_SS;	//stringstream for long exceeded
	string limitExceededLat;
	string limitExceededLong;
};


#endif
