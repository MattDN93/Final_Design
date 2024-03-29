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
#include "Overlay.h"

//testing random co-ord plot below
#include <time.h>


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
		int ptChoice;
		cout << "Enter a point mapping mode (0 = Random points; 1 = Ordered line):";
		cin >> ptChoice;

		switch (ptChoice)
		{
		case 0: randomSim = true; break;
		case 1: randomSim = false; break;
		default: cout << "Please select a sim mode" << endl;
			break;
		}

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
	webcamVid;			//instantiate camCapture object
	Overlay ol_mark;		//overlay instantiation

	ol_mark.setupOverlay();	//setup the marker overlay

	srand(time(NULL));		//seed random number generator
	int randCountTime = 0;	//counter to update co-ords ~1s
	int x = 1, y = 1;

	while (camStreamCapture.isOpened())		//as long as the camera stream is open we want to read off it
	{
		bool frameOK;
		int button;							//checks which keyboard button is pressed for exit routine
		isStreaming = true;					//set flag that streaming in progress
		frameOK = camStreamCapture.read(webcamVid);	//perform the capture and check
		if (!frameOK)								//if frame failed to capture, quit
		{
			break;
		}
		imshow("Incoming Video Stream", webcamVid); //display the video in a GUI window
		
		//TEST with random x and y vars below assuming 640*480; will make this variable
		randCountTime++;
		if ((randCountTime % 20) == 0 )
		{
			if (randomSim == true)
			{
				randCountTime = 0;
				x = rand() % 610;
				y = rand() % 450;
				cout << "x co-ord: " << x + 15 << "; y co-ord: " << y + 15 << endl;
				ol_mark.drawMarker(x, y, webcamVid, true);					//informs routine that previous marker value must be saved for new one
			}
			else if (randomSim == false)					//generate "ordered" list of values that increase as we go along
			{
				x = (rand() % 20);			//x = (-10 to +50)
				y = (rand() % 10);			//y = (-20 to +40)
				ol_mark.orderedPt.x = ol_mark.orderedSimPts.back().x + x;		//add a random x increment to the vector
				ol_mark.orderedPt.y = ol_mark.orderedSimPts.back().y + y;		//add a random y increment to the vector

				ol_mark.orderedSimPts.push_back(ol_mark.orderedPt);				//push onto the new vector and display
				cout << "x co-ord: " << ol_mark.orderedPt.x << "; y co-ord: " << ol_mark.orderedPt.y << endl;
				ol_mark.drawMarker(ol_mark.orderedPt.x, ol_mark.orderedPt.y, webcamVid, true);					//informs routine that previous marker value must be saved for new one
			}

	
		}
		if (randomSim == true)
		{
			ol_mark.drawMarker(x, y, webcamVid, false);					//initiate the overlay draw routine WITHOUT updating the previous marker
		}
		else
		{
			ol_mark.drawMarker(ol_mark.orderedPt.x, ol_mark.orderedPt.y, webcamVid, false);
		}
			


		//end test code


		button = waitKey(30);
		
		//cout << "Frame count:" << camStreamCapture.get(CAP_PROP_FRAME_COUNT) << endl;
		
		if (button == 27)		//if the ESC button is pressed, quit the capture
		{
			break;
		}
	}

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

void setGridExtents(int, int)
{

}
