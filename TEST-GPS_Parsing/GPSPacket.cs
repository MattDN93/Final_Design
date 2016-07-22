using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST_GPS_Parsing
{
    class GPSPacket
    {
        //incoming GPS data
        //from GPRMC
        int ID;
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

        //function to return friendly string from flags
        void friendlyFlagString(char fixtype, int fixqual)
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
                case 1: fixqual_f = "GPS fix";break;
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


    }
}
