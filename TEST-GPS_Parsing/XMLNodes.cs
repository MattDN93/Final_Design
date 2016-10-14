using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace TEST_GPS_Parsing
{
    class XMLNodes
    {
        //top-end XML document components
        public XmlWriter dbWriter;
        public string dbFileName;
        public static string dbFileToOpenName;
        System.IO.StreamWriter dbOutputFile;

        /// <summary>
        /// 
        /// </summary>
        public void createXmlDbFile()
        {

            //create filename and set up stream for XmlWriter to use
            dbFileName = "GPSLogDB" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xml";
            getDbFilePath(dbFileName);
            dbOutputFile = new System.IO.StreamWriter(dbFileName);

            //Create the Xml Doc
            dbWriter = XmlWriter.Create(dbOutputFile);
            dbWriter.WriteStartDocument();
            dbWriter.WriteComment("GPS Data Logging Session Start");
            dbWriter.WriteComment("Logging started at " + DateTime.Now.ToString());
            dbWriter.WriteRaw("<GPSLog>");
            //dbWriter.WriteEndElement();
            //dbWriter.WriteEndDocument();
            dbWriter.Flush();
            //databaseDoc = createXmlDbStructure(databaseDoc);
            saveXmlDbFile();
        }

        public static string getDbFilePath(string dbFN)
        {
            //This method returns the filename to the UI thread to open the XML database from the app.
            dbFileToOpenName = dbFN;
            return dbFileToOpenName;
        }

        public void saveXmlDbFile()
        {
            //databaseDoc.Save(dbOutputFile);

        }

        public bool closeXmlDbFile()
        {

            //dbWriter.WriteEndElement();
            //dbWriter.WriteEndDocument();
            try
            {
                dbWriter.Flush();
                dbWriter.WriteRaw("</GPSLog>");
                dbWriter.Close();
            }
            catch (InvalidOperationException e)
            {
                MessageBox.Show("An error occured while closing the database. The file may be corrupt. Details: " + e.Message, "An error has occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (NullReferenceException n)
            {
                
                MessageBox.Show("An error occured whilst writing to the database. The file may be corrupt. Details: " + n.Message,"An error has occurred",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
             

            return true;
        }

        public XmlWriter createXmlDbStructure()
        {
            //Setting up XML Nodes
            XmlWriterSettings writerSetting = new XmlWriterSettings();
            writerSetting.ConformanceLevel = ConformanceLevel.Auto;
            writerSetting.WriteEndDocumentOnClose = true;
            writerSetting.Indent = true;
            //writerSetting.NewLineOnAttributes = true;
            XmlWriter addToDb = null;
            StreamWriter fileWriter = dbOutputFile;
            addToDb = XmlWriter.Create(fileWriter, writerSetting);

            //XmlNode date = databaseDoc.FirstChild;

            return addToDb;
        }

        public void populateDbFields(GPSPacket gpsDataForDB)
        {
            //create a document fragment that will be appended to the existing XML
            XmlWriter addToDb = createXmlDbStructure();

            addToDb.WriteStartElement("Packet");    //start packet

            addToDb.WriteAttributeString("ID", gpsDataForDB.packetID.ToString());
            addToDb.WriteElementString("Date", gpsDataForDB.date);
            addToDb.WriteElementString("Time", gpsDataForDB.time);
            addToDb.WriteElementString("Latitude",gpsDataForDB.latitude);
            addToDb.WriteElementString("Longitude", gpsDataForDB.longitude);

            addToDb.WriteStartElement("GroundSpeed");
            addToDb.WriteAttributeString("Type", "knots");
            addToDb.WriteValue(gpsDataForDB.grspd_k);
            addToDb.WriteEndElement();

            addToDb.WriteStartElement("GroundSpeed");
            addToDb.WriteAttributeString("Type", "kph");
            addToDb.WriteValue(gpsDataForDB.grspd_kph);
            addToDb.WriteEndElement();

            addToDb.WriteElementString("Altitude", gpsDataForDB.altitude);

            addToDb.WriteStartElement("Angle");
            addToDb.WriteAttributeString("Type", "Cardinal");
            addToDb.WriteValue(gpsDataForDB.cardAngle);
            addToDb.WriteEndElement();

            addToDb.WriteStartElement("Angle");
            addToDb.WriteAttributeString("Type", "Degrees");
            addToDb.WriteValue(gpsDataForDB.trkangle);
            addToDb.WriteEndElement();

            addToDb.WriteElementString("Accuracy", gpsDataForDB.accuracy);
            addToDb.WriteElementString("FixType", gpsDataForDB.fixtype_f);
            addToDb.WriteElementString("FixQuality", gpsDataForDB.fixqual_f);
            addToDb.WriteElementString("NumSats", gpsDataForDB.numsats);

            addToDb.WriteEndElement();              //end Packet
            addToDb.Flush();

    
            
        }


    }

    
}
