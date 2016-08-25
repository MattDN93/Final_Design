/*
CamStream Header File
-----------------------------------------------------
Contains the class definition for the CamStream class
Related to streaming the video from a specific source
-----------------------------------------------------
*/

#ifndef CAMSTREAM_H 
#define CAMSTREAM_H

#ifndef OPENCV_HEAD
#define OPENCV_HEAD

#include <opencv2/core.hpp>
#include <opencv2/imgcodecs.hpp>
#include <opencv2/highgui.hpp>
#include <opencv2/video.hpp>
#include <opencv2/videoio.hpp>

#endif // !OPENCV_HEAD

//Standard includes
#include <iostream>
#include <string>
#include <sstream>
#include <future>
#include <vector>


using namespace cv;
using namespace std;

class camStream
{
public:
	camStream();
	~camStream();

	int vidPixelWidth;				//video dimensions
	int vidPixelHeight;

	friend class Overlay;

	bool valHasChanged;				//for updating the marker
	bool camCapture(int,string);			//method to initiate capture (string for C# arg pass)
	void userInputQuery();			//ask user which way they want to open the video
	void doCapture();				//perform the capture
	void getVideoInfo();			//get the video properties
	bool streamingInProgress();		//lets non-class object query streaming status
	bool captureOpenedOK();			//initial flag set when user opens files

	bool randomSim;					//using random simulation mode or ordered
	int captureChoice;				//user's selection of which capture to use

private:
	Mat webcamVid;					//create a Mat object to manipulate
	VideoCapture camStreamCapture;	//the OpenCV video capture object

	bool capStartSuccess;			//whether the capture was opened OK
	bool isStreaming;				//whether stream is in progress



};

#pragma endregion







#endif // !CAMSTREAM 