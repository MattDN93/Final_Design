using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST_GPS_Parsing
{
    class GPSPacket
    {
        //vars for GPS data
public 
        int ID;
        //incoming GPS data
        //from GPRMC
        double latitutde;
        double longitude;
        char fixtype;
        double grspd_k;
        double trkangle;
        string date;
        //from GRGSV
        int fixqual;
        int numsats;
        double accuracy;
        //from GPVTG
        double altitude;
        double grspd_kph;

        //friendly status flags return
        string fixtype_f;
        string fixqual_f;

        //Default constructor
        public GPSPacket()
        {
            int ID = 0;
            //incoming GPS data
            //from GPRMC
            double latitutde = 0.0;
            double longitude = 0.0;
            char fixtype = ' ';
            double grspd_k = 0.0;
            double trkangle = 0.0;
            string date = "1/1/2016";
            //from GRGSV
            int fixqual = -1;
            int numsats = -1;
            double accuracy = 0.0;
            //from GPVTG
            double altitude = 0.0;
            double grspd_kph = 0.0;

            //friendly status flags return
            string fixtype_f = "";
            string fixqual_f = "";
        }

        //function to return friendly string from flags
        public void friendlyFlagString(char fixtype, int fixqual)
        {
            switch (fixtype)
            {
                case 'A': fixtype_f = "Active"; break;
                case 'V': fixtype_f = "Void"; break;
                default: fixtype_f = "Fix Error";
                    break;
            }

            switch (fixqual)
            {
                case 1: fixqual_f = "GPS fix"; break;
                case 2: fixqual_f = "DGPS fix"; break;
                case 3: fixqual_f = "PPS fix"; break;
                case 4: fixqual_f = "Realtime Kinematic"; break;
                case 5: fixqual_f = "Float Realtime Kinematic"; break;
                case 6: fixqual_f = "Dead reckoning"; break;
                case 7: fixqual_f = "Manual input"; break;
                case 8: fixqual_f = "Simulation"; break;
                default: fixqual_f = "Fix Quality Error";
                    break;
            }
        }

        public string getFriendlyFixType()
        {
            return fixtype_f;
        }

        public string getFriendlyFixQual()
        {
            return fixqual_f;
        }


    }
}
