using System;
using System.Collections.Generic;
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

            databaseDoc = createXmlDbStructure(databaseDoc);
            databaseDoc = saveXmlDbFile(databaseDoc, dbFileName);

        }

        public XmlDocument saveXmlDbFile(XmlDocument dbToSave, string fileName)
        {
            dbToSave.Save(fileName);
            return dbToSave;
        }

        public XmlDocument createXmlDbStructure(XmlDocument dbFile)
        {
            //Setting up XML Nodes
            newElem = dbFile.CreateNode("element", "Packet", "");

            latitude = dbFile.CreateNode("element", "Latitude", "");
            longitude = dbFile.CreateNode("element", "Longitude", "");
            fixtype = dbFile.CreateNode("element", "TypeofFix", "");
            grspd = dbFile.CreateNode("element", "GroundSpeed", "");
            angle = dbFile.CreateNode("element", "Heading", "");
            date = dbFile.CreateNode("element", "Date", "");
            time = dbFile.CreateNode("element", "Time", "");
            fixqual = dbFile.CreateNode("element", "FixQuality", "");
            numsats = dbFile.CreateNode("element", "NumOfSats", "");
            accuracy = dbFile.CreateNode("element", "Accuracy", "");
            altitude = dbFile.CreateNode("element", "Altitude", "");


            //Assign nodes' attrivutes
            packetID = dbFile.CreateAttribute("ID");
            grspdType = dbFile.CreateAttribute("Type");
            angleType = dbFile.CreateAttribute("Type");

            //Appending attribute values to elements
            newElem.Attributes.Append(packetID);
            grspd.Attributes.Append(grspdType);
            angle.Attributes.Append(angleType);

            return dbFile;
        }

        public bool populateDbFields(GPSPacket gpsDataForDB)
        {
            //Assigning attribute values
            packetID.Value = gpsDataForDB.ID.ToString();
            grspdType.Value = "KPH";
            angleType.Value = "Cardinal";

            //Populating the actual values into the XML
            latitude.InnerText = gpsDataForDB.latitude;
            longitude.InnerText = gpsDataForDB.longitude;
            fixtype.InnerText = gpsDataForDB.fixtype_f;
            grspd.InnerText = gpsDataForDB.grspd_k;
            angle.InnerText = gpsDataForDB.cardAngle;
            date.InnerText = gpsDataForDB.date;
            time.InnerText = gpsDataForDB.time;
            fixqual.InnerText = gpsDataForDB.fixqual_f;
            numsats.InnerText = gpsDataForDB.numsats;
            accuracy.InnerText = gpsDataForDB.accuracy;
            altitude.InnerText = gpsDataForDB.altitude;

            //append <packet> to root
            rootNode.AppendChild(newElem);

            //append the data as children to the <packet> tag
            newElem.AppendChild(date);
            newElem.AppendChild(time);
            newElem.AppendChild(latitude);
            newElem.AppendChild(longitude);
            newElem.AppendChild(grspd);
            newElem.AppendChild(altitude);
            newElem.AppendChild(angle);
            newElem.AppendChild(accuracy);
            newElem.AppendChild(fixtype);
            newElem.AppendChild(fixqual);
            newElem.AppendChild(numsats);

            databaseDoc = saveXmlDbFile(databaseDoc,dbFileName);        //save the newly created node to the XML stream and release the lock

            return true;
        }


    }

    
}
