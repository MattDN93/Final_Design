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
        string dbName = "";
        MySqlConnection conn;
        MySqlDataReader rdr;
        MySqlCommand cmd;
        object result;
        int r;

        string sql;

        //connect to the database file
        public void loginProcess(string dbUsername, string dbPassword)
        {
            dbName = "dbtest";
            string connStr = "server=localhost;user=" + dbUsername + ";database=" + dbName +";port=3306;password=" + dbPassword + ";";
            conn = new MySqlConnection(connStr);

            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
            }
            catch (Exception open_ex)
            {
                Console.WriteLine(open_ex.ToString());
                throw open_ex;
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
                        Date char(20),
                        timestamp char(20),
                        Latitude char(20),
                        Longitude char(20),
                        currentCamNum tinyint,
                        vidLogFile char(50),
                        eventDesc char(50),
                        smsNum bigint);
                        ";

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
            try
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
                    r = 1;
                }

                //append to the table if already existing
                result = null;

                //test writing new tables

                sql = @"insert into gpsData values("
                + (r++) + ","                               //this is the overall DB ID
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
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void populateDbFieldsVideo(GPSPacket gpsDataDb, VideoOutputWindow voForDb, string logReason)
        {
            try
            {
                //check where to append from - if table data already exists
                sql = "SELECT COUNT(*) FROM videoLog";
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

                sql = @"insert into videoLog values("
                + (r++) + ",'"                                    //this is the overall DB ID
                + gpsDataDb.date + "','"
                + gpsDataDb.time + "','"
                + gpsDataDb.latitude + "','"
                + gpsDataDb.longitude + "','"
                + voForDb.currentlyActiveCamera + "','"
                + voForDb.videoLogFilename + "','"                
                + logReason + "','"                       //event descriptions
                + "27760934353" +    //sms number
                "');";

                cmd = new MySqlCommand(sql, conn);
                result = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


}
