using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TEST_GPS_Parsing
{
    class XMLNodes
    {
        //top-end XML document components
        public XmlDocument databaseDoc;
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



        public void createXmlDbFile()
        {
            databaseDoc = new XmlDocument();
            rootNode = databaseDoc.CreateElement("GPSLog");

            databaseDoc.AppendChild(rootNode);
         
            docComment = databaseDoc.CreateComment("GPS Data Logging Session Start");
            docComment2 = databaseDoc.CreateComment("Logging started at " + DateTime.Now.ToString());
            databaseDoc.InsertBefore(docComment, rootNode);
            databaseDoc.InsertBefore(docComment2, rootNode);

            dbFileName = "GPSLogDB" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xml";
            
            dbOutputFile = new System.IO.StreamWriter(dbFileName);

            //databaseDoc = createXmlDbStructure(databaseDoc);
            saveXmlDbFile();
        }

        public void saveXmlDbFile()
        {
            databaseDoc.Save(dbOutputFile);

        }

        public void closeXmlDbFile()
        {
            XmlWriter finalDbAppend = createXmlDbStructure();
            finalDbAppend.WriteEndDocument();
            finalDbAppend.Flush();
            finalDbAppend.Close();
        }

        public XmlWriter createXmlDbStructure()
        {
            //Setting up XML Nodes
            XmlWriterSettings writerSetting = new XmlWriterSettings();
            writerSetting.ConformanceLevel = ConformanceLevel.Auto;
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
