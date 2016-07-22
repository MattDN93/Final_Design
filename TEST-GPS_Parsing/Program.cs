using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TEST_GPS_Parsing
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
                 
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            //set up a new packet object with default constructor
            GPSPacket gpsData = new GPSPacket();

            //for program init always set first packet to 1
            gpsData.ID = 1;

            //"flush" the UI with the default values
            Program.resetUI();
        }


        public void resetUI()
        {

        }
    }

}



