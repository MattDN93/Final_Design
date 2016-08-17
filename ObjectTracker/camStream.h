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
#include <opencv2/core.hpp>
#include <opencv2/imgcodecs.hpp>
#include <opencv2/highgui.hpp>
#include <opencv2/video.hpp>
#include <opencv2/videoio.hpp>
#include <iostream>
#include <string>
#pragma endregion

using namespace cv;
using namespace std;

#pragma classDecs
class camStream
{
public:
	camStream();
	~camStream();

	bool camCapture(int);			//method to initiate capture 
	void userInputQuery();			//ask user which way they want to open the video
	void doCapture();				//perform the capture

private:
	VideoCapture camStreamCapture;	//the OpenCV video capture object
	int captureChoice;				//user's selection of which capture to use
	bool capStartSuccess;			//whether the capture was opened OK
};

#pragma endregion







#endif // !CAMSTREAM 