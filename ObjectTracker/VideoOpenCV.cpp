#ifndef CAMSTREAM_H
#include "camStream.h"
#endif


using namespace cv;
using namespace std;

int main(int argc, char** argv)
{
	camStream cs;		//instatiate a camStream object
	auto callUserInput = async(launch::async, std::bind(&camStream::userInputQuery, &cs));	//ask user which input to use & run the detection (asynchronously)

	//wait for user to make choice
	if (callUserInput.valid())
	{
		callUserInput.get();
	}
	
	if (cs.captureOpenedOK() == true)
	{
		auto waitStreamCompletion = async(launch::async, std::bind(&camStream::doCapture, &cs));
		
		if (waitStreamCompletion.valid())
		{
			waitKey(500);
			while (cs.streamingInProgress() == true)
			{
				cs.getVideoInfo();		//display the resolution of the display if ESC is pressed
			}
			waitStreamCompletion.get();
		}
	}
	//user has chosen, now open the stream and wait for it to finish


	

	cout << "Done with processing the video!"; 


	waitKey(0); // Wait for a keystroke in the window
	return 0;
}

