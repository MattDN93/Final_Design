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
}

camStream::~camStream()				//destructor
{
	camStreamCapture.release();
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
			doCapture();
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
			doCapture();
		}
	}
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

void camStream::doCapture()
{
	namedWindow("Incoming Video Stream", CV_WINDOW_AUTOSIZE);
	Mat webcamVid;			//create a Mat object to manipulate
	while (camStreamCapture.isOpened())
	{
		bool frameOK;
		frameOK = camStreamCapture.read(webcamVid);
		if (!frameOK)
		{
			break;
		}
		imshow("Incoming Video Stream", webcamVid);
		waitKey(30);
		cout << "Current FPS: " << camStreamCapture.get(CAP_PROP_FPS) << endl;
		cout << "Frame count:" << camStreamCapture.get(CAP_PROP_FRAME_COUNT) << endl;
	}
	//camStream::~camStream();		//dispose of the object
}
