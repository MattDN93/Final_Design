using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;

namespace TEST_GPS_Parsing
{
    class Mapping
    {
        public double latitudeD;
        public double longitudeD;
        public int mapMarkerCount = 0;

        //method to make doubles out of the latitude and longitude to enter on map
        //stores the return values in the class variables latitudeD and longitudeD
        public void parseLatLong(string latitude, string longitude, bool usingWebLogging)
        {
            //updated includes methods for fallback calculatons
            if (usingWebLogging)
            {
                try
                {
                    latitudeD = double.Parse(latitude, System.Globalization.CultureInfo.InvariantCulture);
                    longitudeD = double.Parse(longitude, System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Co-ordinate values were null or not defined properly");
                    return;
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("Co-ordinates are in an incorrect format (or blank)");
                }
                
            }
            else
            {
                string latTemp, longTemp, degTempLat, minTempLat, minTempLong, degTempLong;
                int pointIndexLat, pointIndexLong;             //make sure 3 digit degree values are accounted for
                double degLat, minLat, degLong, minLong;         //store the minute component of the latitude/longitude

                //current lat/long in form DDMM.SSS(H)
                //this transforms to DD.MMSSS(H)
                try
                {

                    /*pull off headings at the end to modify the lat/long value
                    if N / E = value stays same sign
                    if S / W = value becomes negative
                    Finally we remove the cardinal directions
                    */


                    if (latitude.EndsWith("S"))
                    {
                        latTemp = latitude.Insert(0, "-");
                        latTemp = latTemp.TrimEnd('S');
                    }
                    else
                    {
                        latTemp = latitude.TrimEnd('N');
                    }


                    if (longitude.EndsWith("W"))
                    {
                        longTemp = longitude.Insert(0, "-");
                        longTemp = longTemp.TrimEnd('W');
                    }
                    else
                    {
                        longTemp = longitude.TrimEnd('E');
                    }

                    /* Conversion from DDMM.MMM to decimal
                     * We assume the decimal point is given by the NMEA strings as a "." and NOT a ","
                     *To account for weirdness with the 2/3 digits, negatives and leading zeroes. we reverse the strings first
                     * i.e 3654.928 ; 07702.694
                     * 
                     */
                    latTemp = ReverseString(latTemp);
                    longTemp = ReverseString(longTemp);


                    pointIndexLat = latTemp.IndexOf('.');
                    if (latTemp.EndsWith("-"))
                    {
                        minTempLat = ReverseString(latTemp.Substring(0, 6));    //store the reversed version of the minutes portion of latitude [MMM.MM]DD                  
                        minLat = double.Parse(minTempLat, System.Globalization.CultureInfo.InvariantCulture);       //bring back to normal MM.MMM and convert to a double

                        degTempLat = ReverseString(latTemp.Substring(pointIndexLat + 3));    //store the reversed version of the degrees portion of latitude MMM.MM[DDX] account for 3rd digit at X
                        degLat = double.Parse(degTempLat, System.Globalization.CultureInfo.InvariantCulture);       //bring back to normal XDD (X is 3rd digit or sign) and convert to a double

                        latitudeD = System.Math.Abs(degLat) + (minLat / 60.0);
                        latitudeD = System.Math.Abs(latitudeD) * (-1);
                    }
                    else
                    {
                        minTempLat = ReverseString(latTemp.Substring(0, 6));    //store the reversed version of the minutes portion of latitude [MMM.MM]DD                    
                        minLat = double.Parse(minTempLat, System.Globalization.CultureInfo.InvariantCulture);       //bring back to normal MM.MMM and convert to a double

                        degTempLat = ReverseString(latTemp.Substring(pointIndexLat + 3));   //store the reversed version of the degrees portion of latitude MMM.MM[DDX] account for 3rd digit at X
                        degLat = double.Parse(degTempLat, System.Globalization.CultureInfo.InvariantCulture);       //bring back to normal XDD (X is 3rd digit or sign) and convert to a double

                        latitudeD = System.Math.Abs(degLat) + (minLat / 60.0);                          //construct the decimal equivalent of the DDMM.MMM string as XDD.MMMMM
                    }


                    pointIndexLong = longTemp.IndexOf('.');
                    if (longTemp.EndsWith("-"))
                    {
                        minTempLong = ReverseString(longTemp.Substring(0, 6));    //store the reversed version of the degrees portion of latitude                   
                        minLong = double.Parse(minTempLong, System.Globalization.CultureInfo.InvariantCulture);       //pull of the rest for the co-ord dd[MM.MMMM]

                        degTempLong = ReverseString(longTemp.Substring(pointIndexLat + 3));    //store the reversed version of the degrees portion of latitude
                        degLong = double.Parse(degTempLong, System.Globalization.CultureInfo.InvariantCulture);       //pull off the degree part of the co-ord [DD]MM.MMMM

                        longitudeD = System.Math.Abs(degLong) + (minLong / 60.0);
                        longitudeD = System.Math.Abs(longitudeD) * (-1);
                    }
                    else
                    {
                        minTempLong = ReverseString(longTemp.Substring(0, 6));    //store the reversed version of the degrees portion of latitude 
                        minLong = double.Parse(minTempLong, System.Globalization.CultureInfo.InvariantCulture);       //pull of the rest for the co-ord dd[MM.MMMM]

                        degTempLong = ReverseString(longTemp.Substring(pointIndexLat + 3));    //store the reversed version of the degrees portion of latitude
                        degLong = double.Parse(degTempLong, System.Globalization.CultureInfo.InvariantCulture);       //pull off the degree part of the co-ord [DD]MM.MMMM

                        longitudeD = System.Math.Abs(degLong) + (minLong / 60.0);
                    }


                    /* Convert DDMM.MMM to Decimal
                        do MM.MMM / 60 and add to the minute
                        degLat + (minLat / 60)
                     */



                    /*now convert these to doubles 
                    We assume the decimal point is given by the NMEA strings as a "." and NOT a ","
                    */
                    ///latitudeD = double.Parse(latTemp, System.Globalization.CultureInfo.InvariantCulture);
                    ///longitudeD = double.Parse(longTemp, System.Globalization.CultureInfo.InvariantCulture);


                    pointIndexLat = 0;
                    pointIndexLong = 0;
                    degLat = degLong = 0;
                    return;

                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Co-ordinate values were null or not defined properly");
                    return;
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("Co-ordinates are in an incorrect format (or blank)");
                }
            }



               
        }

        //string reversal code http://stackoverflow.com/questions/228038/best-way-to-reverse-a-string
        public static string ReverseString(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }


        public GMapOverlay plotOnMap(GMapOverlay gmo)
        {

            //only send back data if nonzerp
            if (latitudeD != 0 && longitudeD != 0)
            {
                mapMarkerCount++;
                GMarkerGoogle locationMarker = new GMarkerGoogle(new PointLatLng(latitudeD, longitudeD),GMarkerGoogleType.arrow);
                gmo.Markers.Add(locationMarker);

                if (mapMarkerCount > 20)
                {
                    mapMarkerCount = 0;
                    gmo.Markers.Clear();    //prevent perforance issues by clearing every 10 markers
                }
                
            }

            return gmo;
        }

        }

        
}

