using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace TEST_GPS_Parsing
{
    class XMLNodes
    {
        //top-end XML document components
        public XmlDocument databaseDoc;
        public XmlWriter dbWriter;
        public XmlNode rootNode;
        public XmlComment docComment, docComment2;
        public string dbFileName;
        System.IO.StreamWriter dbOutputFile;

        //GPS specific elements
        //Setting up XML Nodes
        XmlNode newElem;

        XmlNode latitude;
        XmlNode longitude;
        XmlNode fixtype;
        XmlNode grspd;
        XmlNode angle;
        XmlNode date;
        XmlNode time;
        XmlNode fixqual;
        XmlNode numsats;
        XmlNode accuracy;
        XmlNode altitude;


        //Assign nodes' attrivutes
        XmlAttribute packetID;
        XmlAttribute grspdType;
        XmlAttribute angleType;



        /// <summary>
        /// 
        /// </summary>
        public void createXmlDbFile()
        {
            
            //databaseDoc = new XmlDocument();
            //rootNode = databaseDoc.CreateElement("GPSLog");

            //databaseDoc.AppendChild(rootNode);
         
            //docComment = databaseDoc.CreateComment("GPS Data Logging Session Start");
            //docComment2 = databaseDoc.CreateComment("Logging started at " + DateTime.Now.ToString());
            //databaseDoc.InsertBefore(docComment, rootNode);
            //databaseDoc.InsertBefore(docComment2, rootNode);

            //create filename and set up stream for XmlWriter to use
            dbFileName = "GPSLogDB" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xml";
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
                MessageBox.Show("An error occured while closing the database. The file may be corrupt. Details: " + e.ToString(), "An error has occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (NullReferenceException n)
            {
                
                MessageBox.Show("An error occured whilst writing to the database. The file may be corrupt. Details: " + n.ToString(),"An error has occurred",MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            addToDb.WriteElementString("GroundSpeed", gpsDataForDB.grspd_k);
            //addToDb.WriteAttributeString("Type", "knots");
            addToDb.WriteElementString("GroundSpeed", gpsDataForDB.grspd_kph);
            //addToDb.WriteAttributeString("Type", "kph");
            addToDb.WriteElementString("Altitude", gpsDataForDB.altitude);
            addToDb.WriteElementString("Angle", gpsDataForDB.cardAngle);
            //addToDb.WriteAttributeString("Type", "Cardinal");
            addToDb.WriteElementString("Angle", gpsDataForDB.trkangle);
            //addToDb.WriteAttributeString("Type", "Degrees");
            addToDb.WriteElementString("Accuracy", gpsDataForDB.accuracy);
            addToDb.WriteElementString("FixType", gpsDataForDB.fixtype_f);
            addToDb.WriteElementString("FixQuality", gpsDataForDB.fixqual_f);
            addToDb.WriteElementString("NumSats", gpsDataForDB.numsats);

            addToDb.WriteEndElement();              //end Packet
            addToDb.Flush();

    
            
        }


    }

    
}
