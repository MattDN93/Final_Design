using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace TEST_GPS_Parsing
{
    class MySQLInterface
    {
        string dbPassword = "";
        string dbUsername = "";
        string dbName = "";
        MySqlConnection conn;
        MySqlDataReader rdr;
        MySqlCommand cmd;
        object result;
        int r;

        string sql;

        //connect to the database file
        public void loginProcess()
        {
            dbUsername = "root"; /*usernameTextbox.Text;*/
            dbPassword = "W1nd0w5L1v3//";/*passwordTextbox.Text;*/
            dbName = "dbtest";

            string connStr = "server=localhost;user=" + dbUsername + ";database=" + dbName +";port=3306;password=" + dbPassword + ";";
            conn = new MySqlConnection(connStr);

            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("Done.");
        }

        public void createDbStructure()
        {
            try
            {
                //create the table for GPS data
                sql = @"create table gpsData(
                PacketID int,
                SessionPacketID int,
                Date char(20),
                Time char(20),
                Latitude char(20),
                Longitude char(20),
                GrSpdKnots char(10),
                GrSpdKph char(5),
                Altitude char(10),
                AngleCard char(1),
                AngleDeg char(10),
                Accuracy char(10),
                FixType char(50),
                FixQual char(50),
                NumSats char(5),
                Checksum char(50),
                Primary key(PacketID)
                ); ";

                cmd = new MySqlCommand(sql, conn);
                result = cmd.ExecuteScalar();

                //created table for the video output Log
                sql = @"Create table videoLog(
                eventID int,
                Latitude real,
                Longitude real,
                currentCamNum tinyint,
                latUpLeft real,
                longUpLeft real,
                longUpRight real,
                latBotLeft real,
                eventDesc char(50),
                smsNum int,
                Primary key(eventID)
                );";

                cmd = new MySqlCommand(sql, conn);
                result = cmd.ExecuteScalar();
            }
            catch (Exception ex )
            {

                throw ex;
            }


        }

        //insert the data into the table
        public void populateDbFieldsGPS(GPSPacket gpsDataForDB)
        {
            //check where to append from - if table data already exists
            sql = "SELECT COUNT(*) FROM gpsData";
            cmd = new MySqlCommand(sql, conn);
            result = cmd.ExecuteScalar();
            if (result != null)
            {
                r = Convert.ToInt32(result);
            }
            else
            {
                r = 2;
            }

            //append to the table if already existing
            result = null;
            //test writing new tables

            sql = @"insert into gpsData values("
            + (r + gpsDataForDB.ID) + ","                               //this is the overall DB ID
            + gpsDataForDB.ID + ",'"                 //note this ID is the packet per session
            + gpsDataForDB.date + "','" 
            + gpsDataForDB.time + "','"
            + gpsDataForDB.latitude + "','"
            + gpsDataForDB.longitude + "','"
            + gpsDataForDB.grspd_k + "','"
            + gpsDataForDB.grspd_kph + "','"
            + gpsDataForDB.altitude + "','"
            + gpsDataForDB.cardAngle + "','"
            + gpsDataForDB.trkangle + "','"
            + gpsDataForDB.accuracy + "','"
            + gpsDataForDB.fixtype_f + "','"
            + gpsDataForDB.fixqual_f + "','"
            + gpsDataForDB.numsats + "','"
            + gpsDataForDB.checksumResultStatusForDisplay +
            "');";

            cmd = new MySqlCommand(sql, conn);
            result = cmd.ExecuteScalar();
        }

        public void populateDbFieldsVideo(GPSPacket gpsDataDb, VideoOutputWindow voForDb)
        {
            
        }
    }


}
