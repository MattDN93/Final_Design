/*
CamStream Header File
-----------------------------------------------------
Contains the class definition for the CamStream class
Related to streaming the video from a specific source
-----------------------------------------------------
*/

#ifndef CAMSTREAM_H 
#define CAMSTREAM_H

#pragma Includes

#ifndef OPENCV_HEAD
#define OPENCV_HEAD

#include <opencv2/core.hpp>
#include <opencv2/imgcodecs.hpp>
#include <opencv2/highgui.hpp>
#include <opencv2/video.hpp>
#include <opencv2/videoio.hpp>

#endif // !OPENCV_HEAD

#include <iostream>
#include <string>
#include <sstream>
#include <future>
#pragma endregion

using namespace cv;
using namespace std;

#pragma classDecs
class camStream
{
public:
	camStream();
	~camStream();

	int vidPixelWidth;				//video dimensions
	int vidPixelHeight;

	friend class Overlay;

	bool valHasChanged;				//for updating the marker
	bool camCapture(int);			//method to initiate capture 
	void userInputQuery();			//ask user which way they want to open the video
	void doCapture();				//perform the capture
	void getVideoInfo();			//get the video properties
	bool streamingInProgress();		//lets non-class object query streaming status
	bool captureOpenedOK();			//initial flag set when user opens files

private:
	Mat webcamVid;					//create a Mat object to manipulate
	VideoCapture camStreamCapture;	//the OpenCV video capture object
	int captureChoice;				//user's selection of which capture to use
	bool capStartSuccess;			//whether the capture was opened OK
	bool isStreaming;				//whether stream is in progress


};

#pragma endregion







#endif // !CAMSTREAM 