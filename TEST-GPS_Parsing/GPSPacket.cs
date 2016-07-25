using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST_GPS_Parsing
{
    class GPSPacket
    {
        public string gpsLogfilename;
        //vars for GPS data
        public int ID;
        //incoming GPS data
        //from GPRMC
        public string latitude;
        public string longitude;
        public string fixtype;
        public string grspd_k;
        public string trkangle;
        public string cardAngle;
        public string date;
        public string time;
        //from GRGSV
        public string fixqual;
        public string numsats;
        public string accuracy;
        //from GPVTG
        public string altitude;
        public string grspd_kph;

        //friendly status flags return
        public string fixtype_f;
        public string fixqual_f;

        //Default constructor
        public GPSPacket()
        {
            ID = 5;
            //incoming GPS data
            //from GPRMC
            latitude = "";
            longitude = "";
            fixtype = "";
            grspd_k = "";
            trkangle = "";
            date = "";
            time = "";
            //from GRGSV
            fixqual = "";
            numsats = "";
            accuracy = "";
            //from GPVTG
            altitude = "";
            grspd_kph = "";

            //friendly status flags return
            fixtype_f = "";
            fixqual_f = "";
        }

        //function to return friendly string from flags
        public void friendlyFlagString(string fixtype, string fixqual)
        {
            switch (fixtype)
            {
                case "A": fixtype_f = "Active"; break;
                case "V": fixtype_f = "Void"; break;
                default: fixtype_f = "Fix Error";
                    break;
            }

            switch (fixqual)
            {
                case "0": fixqual_f = "Invalid fix"; break;
                case "1": fixqual_f = "GPS fix"; break;
                case "2": fixqual_f = "DGPS fix"; break;
                case "3": fixqual_f = "PPS fix"; break;
                case "4": fixqual_f = "Realtime Kinematic"; break;
                case "5": fixqual_f = "Float Realtime Kinematic"; break;
                case "6": fixqual_f = "Dead reckoning"; break;
                case "7": fixqual_f = "Manual input"; break;
                case "8": fixqual_f = "Simulation"; break;
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
