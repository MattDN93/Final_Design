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

	auto callUserInput = async(launch::async, std::bind(&camStream::userInputQuery, &cs));	//ask user which input to use & run the detection (asynchronously)

	//wait for user to make choice
	if (callUserInput.valid())
	{
		callUserInput.get();
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

