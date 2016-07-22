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
        public int ID;
        //incoming GPS data
        //from GPRMC
        public double latitude;
        public double longitude;
        public char fixtype;
        public double grspd_k;
        public double trkangle;
        public string date;
        public string time;
        //from GRGSV
        public int fixqual;
        public int numsats;
        public double accuracy;
        //from GPVTG
        public double altitude;
        public double grspd_kph;

        //friendly status flags return
        public string fixtype_f;
        public string fixqual_f;

        //Default constructor
        public GPSPacket()
        {
            ID = 5;
            //incoming GPS data
            //from GPRMC
            latitude = 0.0;
            longitude = 0.0;
            fixtype = ' ';
            grspd_k = 0.0;
            trkangle = 0.0;
            date = "1/1/2016";
            time = "23:59.59";
            //from GRGSV
            fixqual = -1;
            numsats = -1;
            accuracy = 0.0;
            //from GPVTG
            altitude = 0.0;
            grspd_kph = 0.0;

            //friendly status flags return
            fixtype_f = "Not Set";
            fixqual_f = "Not Set";
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
