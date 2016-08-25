#ifndef CAMSTREAM_H
#include "camStream.h"
#endif
#ifndef OVERLAY_H
#include "Overlay.h"
#endif // !OVERLAY_H

using namespace cv;
using namespace std;

int main(int argc, char** argv)
{
	camStream cs;		//instatiate a camStream object
	Overlay ol;			//instantiate overlay object and pass it the video dimensions

	//----------------Sample of arguments if the application is called via the other UI------------
	if (argc == 1)	//the console is called normally, ask the user what to do
	{
		cout << "Console launched independently. Going to user query mode..." << endl;
		cout << argv[0];
		auto callUserInput = async(launch::async, std::bind(&camStream::userInputQuery, &cs));	//ask user which input to use & run the detection (asynchronously)

																								//wait for user to make choice
		if (callUserInput.valid())
		{
			callUserInput.get();
		}

	}
	else //this console has been called from the C# instance
	{
		/*parse through the arguments from the other app
		Arg[0] = filename
		Arg[1] = lat-topLeft
		Arg[2] = long-topLeft
		Arg[3] = long-topRight
		Arg[4] = lat-botLeft
		Arg[5] = Picture mode; (0 = Internal Webcam; 1 = specify a file)
		Arg[6] = Draw mode; (0 = Random points; 1 = Ordered line)
		*/
		cout << "Console launched via other app - parsing arguments now...." <<endl;
		ol.TopLeftCoords[0] = atof(argv[1]);
		ol.TopLeftCoords[1] = atof(argv[2]);
		ol.OuterLimitsCoords[0] = atof(argv[3]);
		ol.OuterLimitsCoords[1] = atof(argv[4]);

		cs.captureChoice = atoi(argv[5]);	//if capture = 0 just open webcam with method call, if 1 pass the method the filename

		bool camCaptureReturnSuccess;					//variable to indicate if camCapture returned OK
		switch (cs.captureChoice)
		{
		case 0: camCaptureReturnSuccess = cs.camCapture(0,"false");	break; //open webcam (don't pass the filename)
		case 1: camCaptureReturnSuccess = cs.camCapture(1,argv[7]); break; //open file and send filename
		default: cout << "camCapture method returned failed.";
			break;
		}

		switch (atoi(argv[6]))
		{
		case 0: cs.randomSim = true; break;
		case 1: cs.randomSim = false; break;
		default: cout << "Drawing of points mode selection failed - using random points";
			cs.randomSim = true;
			break;
		}

		//evaluate this special call to see if it's succeeded
		if (camCaptureReturnSuccess == true)
		{
			cout << "prelims from main app call completed successfully!" <<endl;
		}
		else
		{
			cout << "Something failed whilst processing arguments from the main app." << endl;
		}
	}

	
	if (cs.captureOpenedOK() == true)
	{
		auto waitStreamCompletion = async(launch::async, std::bind(&camStream::doCapture, &cs));

		//user has chosen, now open the stream and wait for it to finish	
		if (waitStreamCompletion.valid())
		{
			//waitKey(500);
			if (cs.streamingInProgress() == true)
			{
				cs.getVideoInfo();		//display the resolution of the display if ESC is pressed
				ol.getVideoInfo(cs.vidPixelWidth,cs.vidPixelHeight);		//update the overlay grid with the correct dimensions as well
			}
			waitStreamCompletion.get();
		}
	}

	//ol.setupOverlay();					//load up the grid and reticle
	//ol.drawMarker(0,0);

	cout << "Done with processing the video!"; 


	waitKey(0); // Wait for a keystroke in the window
	return 0;
}

