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
        public void parseLatLong(string latitude, string longitude)
        {
            string latTemp, longTemp;
            int pointIndexLat, pointIndexLong;             //make sure 3 digit degree values are accounted for
            //current lat/long in form DDMM.SSS(H)
            //this transforms to DD.MMSSS(H)
            try
            {
                pointIndexLat = latitude.IndexOf('.');
                latTemp = latitude.Insert(pointIndexLat-2, ".");
                latTemp = latTemp.Remove(pointIndexLat+1, 1);
                pointIndexLong = longitude.IndexOf('.');
                longTemp = longitude.Insert(pointIndexLong - 2, ".");
                longTemp = longTemp.Remove(pointIndexLong+1, 1);

                /*pull off headings at the end to modify the lat/long value
                    if N / E = value stays same sign
                    if S / W = value becomes negative
                    Finally we remove the cardinal directions
                 */
                if (latitude.EndsWith("S"))
                {
                    latTemp = latTemp.Insert(0, "-");
                    latTemp = latTemp.TrimEnd('S');
                }
                latTemp = latTemp.TrimEnd('N');

                if (longitude.EndsWith("W"))
                {
                    longTemp = longTemp.Insert(0, "-");
                    longTemp = longTemp.TrimEnd('W');
                }
                longTemp = longTemp.TrimEnd('E');


                /*now convert these to doubles 
                 We assume the decimal point is given by the NMEA strings as a "." and NOT a ","
                 */
                latitudeD = double.Parse(latTemp,System.Globalization.CultureInfo.InvariantCulture);
                longitudeD = double.Parse(longTemp, System.Globalization.CultureInfo.InvariantCulture);
                pointIndexLat = 0;
                pointIndexLong = 0;
                return;

            }
            catch (ArgumentOutOfRangeException a)
            {
                Console.WriteLine("Co-ordinate values were null or not defined properly");
                return;
            }
            catch (System.FormatException f)
            {
                Console.WriteLine("Co-ordinates are in an incorrect format (or blank)");
            }


               
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

