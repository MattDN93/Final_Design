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
        //--these are all strings for UI display---
        public int timeElapsed;
        //vars for GPS data
        public int ID;
        public int packetID = 0;               //session-based packet ID, one packet is equivalent to a set of NMEA strings starting with GPRMC
        public int deltaCount = 0;                    //checks the delta of packet ID so we can update only when ID has changed
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

        //--these are specific format objects for calculation--
        public DateTime dt;

        //friendly status flags return
        public string fixtype_f;
        public string fixqual_f;

        //checksum for integrity
        public string checksumGPRMC;
        public string checksumGPVTG;
        public string checksumGPGGA;
        public string checksumResultStatusForDisplay;

        //Default constructor
        public GPSPacket()
        {
            ID = 0;
            //incoming GPS data
            //from GPRMC
            latitude = "";
            longitude = "";
            fixtype = "";
            grspd_k = "";
            trkangle = "";
            cardAngle = "";
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
            checksumGPRMC = "";
            checksumGPGGA = "";
            checksumGPVTG = "";
            checksumResultStatusForDisplay = "";
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
        
        //Make sure the checksum is correct by generating a checksum from the string and comparing
        public GPSPacket getChecksum(string sentence, GPSPacket updatedGpsData, int sentenceType)
        {
          string checksum = "", checksumStr = "";
          //Start with first Item
          int checksumCalc= Convert.ToByte(sentence[sentence.IndexOf('$')+1]);
          // Loop through all chars to get a checksum
          for (int i=sentence.IndexOf('$')+2 ; i<sentence.IndexOf('*') ; i++)
          {
            // No. XOR the checksum with this character's value
            checksumCalc^=Convert.ToByte(sentence[i]);              
          }
          //convert the integer value to a 2-digit hex (in string representation)
            checksumStr = checksumCalc.ToString("X2");
            switch (sentenceType)
            {
                //case 0 = GPRMC
                case 0: checksum = checksumGPRMC; break;
                case 1: checksum = checksumGPGGA; break;
                case 2: checksum = checksumGPVTG; break;
                default:
                    break;
            }

            // Check what was parsed from the NMEA string and compare to the raw calculation
            if (checksumStr == checksum)
          {
                //checksum passed, return object
              return updatedGpsData;
          }else 
          {
                //the checksum has failed, note that in the field
                switch (sentenceType)
                {
                    //case 0 = GPRMC
                    case 0: checksumGPRMC = "FAIL"; break;
                    //case 1 = GPGGA
                    case 1: checksumGPGGA = "FAIL"; break;
                    //case 2 = GTVTG
                    case 2: checksumGPVTG = "FAIL"; break;
                    default:
                        break;
                }
                return updatedGpsData;
          }

        }

                //-------------------------PARSING MEMBER METHODS----------------------------

        public GPSPacket parseSelection(string sentenceBuffer, GPSPacket gpsData)
        {
            GPSPacket updatedGpsData = gpsData; //sets up a new object which methods return with updated values
            if (!(sentenceBuffer.StartsWith("$")))
            {
                return updatedGpsData;     //the sentence is invalid 
            }

            if (sentenceBuffer.Contains("GPRMC")) //We assume that each "packet" of sentences begins with a GPRMC hence update the packet ID each time a GPRMC is found!
            {
                packetID++;
                updatedGpsData.ID = packetID;              
                updatedGpsData = parseGPRMC(sentenceBuffer, gpsData);
                updatedGpsData = getChecksum(sentenceBuffer, updatedGpsData, 0);    //checksum compute with GPRMC

                //do some last-minute formatting to the fields based on known issues
                updatedGpsData.date = updatedGpsData.date.TrimEnd('/'); //remove trailing "/"
                if (updatedGpsData.time.StartsWith("0") || updatedGpsData.longitude.StartsWith("0") || updatedGpsData.latitude.StartsWith("0"))
                {
                    //updatedGpsData.time = updatedGpsData.time.TrimStart('0'); //remove leading zeroes
//                    updatedGpsData.latitude = updatedGpsData.latitude.TrimStart('0');
//                    updatedGpsData.longitude = updatedGpsData.longitude.TrimStart('0');
                }

                //convert the track angle in degrees true to a cardinal heading
                trackToCardinal(updatedGpsData);

                //convert the time from a horrible string to something nice
                timeNiceDisplay(updatedGpsData);

                //convert the latitude and longitude from DDMM.MMM(H) to decimal (for Maps and calculations)
                ///updatedGpsData = latLongConvertToDbl(updatedGpsData);

                return updatedGpsData;
            }
            else if (sentenceBuffer.Contains("GPGGA"))
            {
                updatedGpsData = parseGPGGA(sentenceBuffer, gpsData);
                updatedGpsData = getChecksum(sentenceBuffer, updatedGpsData, 1);    //checksum compute with GPGGA
                return updatedGpsData;
            }
            else if (sentenceBuffer.Contains("GPVTG"))
            {
                updatedGpsData = parseGPVTG(sentenceBuffer, gpsData);
                updatedGpsData = getChecksum(sentenceBuffer, updatedGpsData, 2);    //checksum compute with GPVTG
                //trim off leading zeroes from the speed
                if (updatedGpsData.grspd_kph.StartsWith("0"))
                {
                    updatedGpsData.grspd_kph = updatedGpsData.grspd_kph.TrimStart('0');
                }
                return updatedGpsData;
            }
            else
            {
                return updatedGpsData;     //we don't consider all the other NMEA strings
            }

        }

        /*----------------Time and Date Parsing Method 
         Input: GPSData object
         Output: Time and date parsed correctly 
         Objective: parse time and date from raw numbers and letters to a nice format & to computable values. Assumed time HHMMSS.SSS and date DD/MM/YY
             */

        private void timeNiceDisplay(GPSPacket updatedGpsData)
        {
            string newDateTime; //immutable string so make a new one to copy into
            string fixTime;     //new temp string for date
            string format = "dd/MM/yyyy HH:mm:ss.fff"; //set date time format

            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture; //provider for display of date and time SA style

            newDateTime = updatedGpsData.date.Insert(6, "20");  //assume 2 digit date >2000
            //---for UI
            updatedGpsData.date = newDateTime;
            fixTime = updatedGpsData.time.Insert(2, ":").Insert(5, ":");
            updatedGpsData.time = fixTime;
            //--end UI formatting

            //now format to a DateTime object for later calc
            newDateTime += " " + fixTime;

            updatedGpsData.dt = DateTime.ParseExact(newDateTime, format, provider); //this will be used for computation
        }

        //parses the lat and long to a decimal format from DDMM.MMM format
        //conversion method in the Mapping class
        private GPSPacket latLongConvertToDbl(GPSPacket updatedGpsDataForLatLong)
        {
            Mapping mapObjectTemp = new Mapping();      //used just for the lat/long parsing method
            mapObjectTemp.parseLatLong(updatedGpsDataForLatLong.latitude, updatedGpsDataForLatLong.longitude);
            updatedGpsDataForLatLong.latitude = mapObjectTemp.latitudeD.ToString();
            updatedGpsDataForLatLong.longitude = mapObjectTemp.longitudeD.ToString();
            return updatedGpsDataForLatLong;
        }

        private GPSPacket trackToCardinal(GPSPacket updatedGpsData)
        {
            //each degree direction corresponds to some cardinal direction
            //first convert trkangle to a double to evaluate it
            double trkAngle_double;
            bool result = Double.TryParse(updatedGpsData.trkangle, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out trkAngle_double);

            if (result == true)
            {
                if ((348.75 >= trkAngle_double && trkAngle_double <= 360) || ((0 >= trkAngle_double && trkAngle_double <= 33.75)))
                {
                    updatedGpsData.cardAngle = "N";
                }
                else if ((33.75 > trkAngle_double && trkAngle_double <= 78.75))
                {
                    updatedGpsData.cardAngle = "NE";
                }
                else if ((78.75 > trkAngle_double && trkAngle_double <= 101.25))
                {
                    updatedGpsData.cardAngle = "E";
                }
                else if ((101.25 > trkAngle_double && trkAngle_double <= 168.75))
                {
                    updatedGpsData.cardAngle = "SE";
                }
                else if ((168.75 > trkAngle_double && trkAngle_double <= 213.75))
                {
                    updatedGpsData.cardAngle = "S";
                }
                else if ((213.75 > trkAngle_double && trkAngle_double <= 258.75))
                {
                    updatedGpsData.cardAngle = "SW";
                }
                else if ((258.75 > trkAngle_double && trkAngle_double <= 303.75))
                {
                    updatedGpsData.cardAngle = "W";
                }
                else if ((303.75 > trkAngle_double && trkAngle_double < 348.75))
                {
                    updatedGpsData.cardAngle = "NW";
                }

            }

            return updatedGpsData;
        }

        private GPSPacket parseGPVTG(string sentenceBuffer, GPSPacket gpsData)
        {
            int sectionCount = 0;
            string subField = "";
            foreach (var item in sentenceBuffer)
            {
                if (item.ToString() != "*")      //the asterisk specifies the end of line
                {
                    if (item.ToString() == ",")  //increment counter for next segment
                    {
                        sectionCount++;
                        subField = "";
                    }

                    switch (sectionCount)       //store the fields based on the expected section
                    {
                        //GPVTG section 7 is speed in KPH; we need the speed in KPH
                        case 7:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.grspd_kph = subField; //parse double to a string for display
                            }
                            break;

                        default: break;
                    }
                }
                else 
                {
                    break;
                }
            }
            //after the asterisk there's a few chars of checksum
            //checksum is the first char after the asterisk to the end
            checksumGPVTG = sentenceBuffer.Substring(sentenceBuffer.IndexOf('*') + 1);
            return gpsData;
        }

        private GPSPacket parseGPGGA(string sentenceBuffer, GPSPacket gpsData)
        {
            int sectionCount = 0;
            string subField = "";
            foreach (var item in sentenceBuffer)
            {
                if (item.ToString() != "*")      //the asterisk specifies the end of line
                {
                    if (item.ToString() == ",")  //increment counter for next segment
                    {
                        sectionCount++;
                        subField = "";
                    }

                    switch (sectionCount)       //store the fields based on the expected section
                    {
                        //GPGGA section 5 is is the fix quality
                        case 6:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.fixqual = subField; //add the cardinal heading to the latitude
                                gpsData.friendlyFlagString(gpsData.fixtype, gpsData.fixqual); //get the friendly name of the fix quality
                            }
                            break;
                        //GGA section 7 is the number of satellites in view
                        case 7:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.numsats = subField;
                            }
                            break;
                        //section 8 is the accuracy of the GPS fix
                        case 8:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.accuracy = subField;
                            }
                            break;
                        //section 9 is the altitude ASL
                        case 9:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.altitude = subField;
                            }
                            break;
                        default: break;
                    }
                }
                else
                {
                    break;
                }

            }
            //after the asterisk there's a few chars of checksum
            //checksum is the first char after the asterisk to the end
            checksumGPGGA = sentenceBuffer.Substring(sentenceBuffer.IndexOf('*') + 1);
            return gpsData;
        }

        private GPSPacket parseGPRMC(string sentenceBuffer, GPSPacket gpsData)
        {
            int sectionCount = 0;                 //count for subsections of the GPRMC string
            int dateCtr = 0;                    //for formatting the date nicely
            gpsData.date = "";
            string subField = "";
            foreach (var item in sentenceBuffer)
            {
                if (item.ToString() != "*")      //the asterisk specifies the end of line
                {
                    if (item.ToString() == ",")  //increment counter for next segment
                    {
                        sectionCount++;
                        subField = "";
                    }

                    switch (sectionCount)       //store the fields based on the expected section
                    {
                        case 0: break;
                        //GPRMC section 1 = time in HHMMSS
                        case 1:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.time = subField;
                            }
                            break;
                        //GPRMC section 2 = fix, A or V
                        case 2:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.fixtype = subField;    //pick the 2nd char ",A" for example
                                gpsData.friendlyFlagString(gpsData.fixtype, gpsData.fixqual);   //get the friendly string for the fix
                            }
                            break;
                        //GPRMC section 3 = Latitude DDMMSS.SSS
                        case 3:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.latitude = subField; //parse double to a string for display
                            }
                            break;
                        //GPRMC section 4 = Latitude Cardinal Heading
                        case 4:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.latitude += subField; //add the cardinal heading to the latitude
                            }
                            break;
                        //GPRMC section 5 = Longitude DDMMSS.SSS
                        case 5:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.longitude = subField; //parse double to a string for display
                            }
                            break;
                        //GPRMC section 6 = Longitude Cardinal Heading
                        case 6:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.longitude += subField; //add the cardinal heading to the longitude
                            }
                            break;
                        //GPRMC section 7 = Ground speed in Knots
                        case 7:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.grspd_k = subField;
                            }
                            break;
                        //GPRMC section 8 = Track angle in degrees true
                        case 8:
                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                subField += item;
                                gpsData.trkangle = subField;
                            }
                            break;
                        //GPRMC section 9 = Date ,DD/MM/YY
                        case 9:

                            if (item.ToString() != ",")  //make sure the comma isn't included
                            {
                                dateCtr++;
                                if (dateCtr % 2 == 0)
                                {
                                    subField = item.ToString();
                                    gpsData.date += subField;
                                    gpsData.date += "/"; //format the date nicely 
                                }
                                else
                                {
                                    subField = item.ToString();
                                    gpsData.date += subField;

                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    break;
                }

            }
            //after the asterisk there's a few chars of checksum
            //checksum is the first char after the asterisk to the end
            checksumGPRMC = sentenceBuffer.Substring(sentenceBuffer.IndexOf('*') + 1);
            //return completed packet
            return gpsData;

        }

        //------------------------------------------------------------------




    }
}
