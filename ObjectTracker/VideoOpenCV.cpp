#ifndef CAMSTREAM_H
#include "camStream.h"
#endif


using namespace cv;
using namespace std;

int main(int argc, char** argv)
{
	camStream cs;		//instatiate a camStream object
	cs.userInputQuery();	//ask user which input to use & run the detection
	
	waitKey(0); // Wait for a keystroke in the window
	return 0;
}

