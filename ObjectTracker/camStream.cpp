/*
		CamStream File
		-----------------------------------------------------
		Contains the member functions for the CamStream class
		Related to streaming the video from a specific source
		-----------------------------------------------------
		-Open the camera capture on the OpenCV side
		-Ask the user which capture type they want
		- do the actual capture 
		- dispose of the onbject afterwards

*/
#ifndef CAMSTREAM_H
#include "camStream.h"
#endif

camStream::camStream()				//constructor
{
	VideoCapture camStreamCapture = VideoCapture();
	captureChoice = -1;
	capStartSuccess = false;
	isStreaming = false;
}

camStream::~camStream()				//destructor
{
	camStreamCapture.release();
}

void camStream::userInputQuery()
{

	while (capStartSuccess == false)
	{
		cout << "Enter an interface to capture (0 = Internal Webcam; 1 = specify a file): ";
		cin >> captureChoice;

		switch (captureChoice)
		{
		case 0: capStartSuccess = camCapture(0); break;
		case 1: capStartSuccess = camCapture(1); break;
		default: cout << "You've made an invalid choice. Try again" << endl;
			break;
		}

	}
}

bool camStream::camCapture(int choice)
{
	if (choice == 0)
	{
		camStreamCapture.open(0);

		if (!camStreamCapture.isOpened())
		{
			cout << "Failed to open the stream!";
			return false;
		}
		else
		{
			isStreaming = true;
			return true;
			//doCapture();
		}
	}
	else if (choice == 1)
	{
		string fileName;
		cout << "Enter a filename to open, fully qualified: ";
		cin >> fileName;

		camStreamCapture.open(fileName);		//pass 0 for a webcam

		if (!camStreamCapture.isOpened())
		{
			cout << "Failed to open the stream!";
			return false;
		}
		else
		{
			isStreaming = true;
			return true; // doCapture();
		}
	}
}



void camStream::doCapture()
{
	isStreaming = true;
	namedWindow("Incoming Video Stream", CV_WINDOW_AUTOSIZE);
	Mat webcamVid;			//create a Mat object to manipulate
	while (camStreamCapture.isOpened())
	{
		bool frameOK;
		int button;
		isStreaming = true;
		frameOK = camStreamCapture.read(webcamVid);
		if (!frameOK)
		{
			break;
		}
		imshow("Incoming Video Stream", webcamVid);
		button = waitKey(30);
		//cout << "Current FPS: " << camStreamCapture.get(CAP_PROP_FPS) << endl;
		//cout << "Frame count:" << camStreamCapture.get(CAP_PROP_FRAME_COUNT) << endl;
		
		if (button == 27)		//if the ESC button is pressed, quit the capture
		{
			break;
		}
	}

	//camStream::~camStream();		//dispose of the object
}

void camStream::getVideoInfo()
{
	if (camStreamCapture.isOpened())
	{
		vidPixelHeight = camStreamCapture.get(CAP_PROP_FRAME_HEIGHT);
		vidPixelWidth = camStreamCapture.get(CAP_PROP_FRAME_WIDTH);
	}
	cout << "Video height = " << vidPixelHeight << endl;
	cout << "Video width = " << vidPixelWidth << endl;
	//isStreaming = false;
}

bool camStream::streamingInProgress()
{
	return isStreaming;
}

bool camStream::captureOpenedOK()
{
	if (capStartSuccess)
	{
		return true;
	}
	else
	{
		return false;
	}
	
}
