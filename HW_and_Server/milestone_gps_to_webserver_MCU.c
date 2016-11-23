#include "mbed.h"
#include <string>

//------------------------------------------------------------------------------------
/* This example was tested on C027-U20 and C027-G35 with the on board modem. 
   
   Additionally it was tested with a shield where the SARA-G350/U260/U270 RX/TX/PWRON 
   is connected to D0/D1/D4 and the GPS SCL/SDA is connected D15/D15. In this 
   configuration the following platforms were tested (it is likely that others 
   will work as well)
   - U-BLOX:    C027-G35, C027-U20, C027-C20 (for shield set define C027_FORCE_SHIELD)
   - NXP:       LPC1549v2, LPC4088qsb
   - Freescale: FRDM-KL05Z, FRDM-KL25Z, FRDM-KL46Z, FRDM-K64F
   - STM:       NUCLEO-F401RE, NUCLEO-F030R8
                mount resistors SB13/14 1k, SB62/63 0R
*/
#include "GPS.h"
#include "MDM.h"
#include "HTTPClient.h"

//------------------------------------------------------------------------------------
// You need to configure these cellular modem / SIM parameters.
// These parameters are ignored for LISA-C200 variants and can be left NULL.
//------------------------------------------------------------------------------------
//! Set your secret SIM pin here (e.g. "1234"). Check your SIM manual.
#define SIMPIN      "00000"
/*! The APN of your network operator SIM, sometimes it is "internet" check your 
    contract with the network operator. You can also try to look-up your settings in 
    google: https://www.google.de/search?q=APN+list */
#define APN         "afrihost"
//! Set the user name for your APN, or NULL if not needed
#define USERNAME    NULL
//! Set the password for your APN, or NULL if not needed
#define PASSWORD    NULL 
//------------------------------------------------------------------------------------

//#define CELLOCATE
#define MDM_DEBUG

//-------------Socket parameters-----------
//socket parameter - defines the socket ID for this session
int socketID = -1;              //current socket ID
bool socketClosed = false;       //check if the socket is closed
char hostname[100] = "";
//------------------------------------------

int main(void)
{
    int ret;

#ifdef LARGE_DATA
    char buf[2048] = "";
#else
    char buf[512] = "";
#endif

    // Create the GPS object
#if 1   // use GPSI2C class
    GPSI2C gps;
#else   // or GPSSerial class 
    GPSSerial gps; 
#endif
    // Create the modem object
    MDMSerial mdm; // use mdm(D1,D0) if you connect the cellular shield to a C027
    // initialize the modem 
    MDMParser::DevStatus devStatus = {};
    MDMParser::NetStatus netStatus = {};
    MDMParser::IP ip;
    bool mdmOk = mdm.init(SIMPIN, &devStatus);
    
    //print the modem status to the shell
    mdm.dumpDevStatus(&devStatus);
    
    if (mdmOk) {
        // wait until we are connected
        mdmOk = mdm.registerNet(&netStatus);
        mdm.dumpNetStatus(&netStatus);
    }
    if (mdmOk)
    {
        // join the internet connection 
        ip = mdm.join(APN,USERNAME,PASSWORD);
        if (ip == NOIP)
            printf("Not able to join network");
        else
        {
            mdm.dumpIp(ip);           
        }      
    
    }
 
    printf("SMS and GPS Loop\r\n");
    //--------variables for data storage & status--------
    char link[128] = "";
    char devStatusString[100] = "";
    char alt[7] = "";
    char spd[5] = "";
    char lat[10] = "";
    char longi[10] = "";
    unsigned int i = 0xFFFFFFFF;
    const int wait = 100;
    bool abort = false;
    
    //make the socket connection--------------------------
        //open a socket, TCP port 80  (we're using HTTP requests)
        
            socketID = mdm.socketSocket(mdm.IPPROTO_TCP,80);      
            if(socketID == SOCKET_ERROR)
            {
                printf("Socket creation error.");
                socketClosed = mdm.socketFree(socketID);
            }else
            {
            //set the hostname of the server to connect the socket to (the AWS instance)
            sprintf(hostname,"54.244.63.232");        //initial hostname
            
            }
            
    mdm.socketConnect(socketID,hostname, 80);
    
    //---------------Check if server is responsive-----------------
    // Send/receive buffers and data counters
    char sbuffer[512];
    char rbuffer[64];
    int rcount = -1;
    int scount = -1;
    int generatedFieldsCount = 0;       //to craft POST only after certain co-ords found
    
    //send GET to the server
    //sprintf(sbuffer,"GET / HTTP/1.1\r\nHost: 54.244.63.232\r\nConnection: close\r\n\r\n");
    //scount = mdm.socketSend(socketID,sbuffer, sizeof sbuffer);
    //printf("sent %d %.*s\r\n", scount, strstr(sbuffer, "\r\n")-sbuffer, sbuffer);
            
    // Recieve a OK from the server
    //rcount = mdm.socketRecv(socketID, rbuffer, sizeof rbuffer);
    //printf("recv %d [%.*s]\r\n", rcount, strstr(rbuffer, "\r\n")-rbuffer, rbuffer);
    
    if (true/*strstr(rbuffer, "200") && rcount != -1*/)
    {
        abort = false;
        printf("Server connection succeeded - sending GPS\n");
        
        //TEST
       // sprintf(sbuffer,"POST testdata=1.0 HTTP/1.1\r\n\r\n");
       // scount = mdm.socketSend(socketID,sbuffer, sizeof sbuffer);
        printf("\n\nSent test GET");       
    }else
    {
        printf("Didn't receive HTTP OK from server...");
        //keep retrying to contact server
        while (!strstr(rbuffer, "200"))
        {
        scount = mdm.socketSend(socketID, sbuffer, sizeof sbuffer);
        rcount = mdm.socketRecv(socketID, rbuffer, sizeof rbuffer);        
        printf("Server connection failed. Retrying in 5 seconds");
        ::wait_ms(5000);
        sprintf(rbuffer,"");
        rcount = -1;
        scount = -1;
        }
    }
    
    while (!abort) {
    //    led = !led;
    //----------------GPS character update---------------------------------------
        while ((ret = gps.getMessage(buf, sizeof(buf))) > 0)
        {
            //printf("NMEA: %s", buf);
            int len = LENGTH(ret);            
            if ((PROTOCOL(ret) == GPSParser::NMEA) && (len > 6))
            {
                // talker is $GA=Galileo $GB=Beidou $GL=Glonass $GN=Combined $GP=GPS
                if ((buf[0] == '$') || buf[1] == 'G') {
                    #define _CHECK_TALKER(s) ((buf[3] == s[0]) && (buf[4] == s[1]) && (buf[5] == s[2]))
                    if (_CHECK_TALKER("GLL")) {
                        double la = 0, lo = 0;
                        char ch;
                        if (gps.getNmeaAngle(1,buf,len,la) && 
                            gps.getNmeaAngle(3,buf,len,lo) && 
                            gps.getNmeaItem(6,buf,len,ch) && ch == 'A')
                        {
                            sprintf(lat,"%.5f",la); sprintf(longi,"%.5f",lo);                            
                            printf("GPS Location: %.5f %.5f\r\n", la, lo);
                            generatedFieldsCount++; 
                            sprintf(link, "I am here!\n"
                                          "https://maps.google.com/?q=%.5f,%.5f", la, lo);                            
                        }
                    } else if (_CHECK_TALKER("GGA") || _CHECK_TALKER("GNS") ) {
                        double a = 0; 
                        if (gps.getNmeaItem(9,buf,len,a)) // altitude msl [m]
                            printf("GPS Altitude: %.1f\r\n", a); sprintf(alt,"%.2f",a); 
                            generatedFieldsCount++; 
                    } else if (_CHECK_TALKER("VTG")) {
                        double s = 0; 
                        if (gps.getNmeaItem(7,buf,len,s)) // speed [km/h]
                            printf("GPS Speed: %.1f\r\n", s); sprintf(spd,"%.2f", s);
                            generatedFieldsCount++; 
                    } 
                    
                    if (generatedFieldsCount >1)
                    {
                        //----------------craft the HTTP POST request---------
                        generatedFieldsCount = 0;
                      
                        //----------------send on the socket--------------
                        ///receive_gpsData.php?lat=-29.987&long=20.123&numSats=2
                        //TEST
                        socketID = mdm.socketSocket(mdm.IPPROTO_TCP,80); 
                        mdm.socketConnect(socketID,hostname, 80);
                        //char sbuffer[] = "GET /receive_gpsData.php?lat=-29.11111&long=25.000001 HTTP/1.1\r\nHost: 54.244.63.232\r\nConnection: close\r\n\r\n";
                        sprintf(sbuffer, "GET /receive_gpsData.php?lat=%s&long=%s&spd=%s&alt=%s HTTP/1.1\r\nHost: 54.244.63.232\r\n\r\n",lat,longi,spd,alt);           
                        scount = mdm.socketSend(socketID,sbuffer,sizeof sbuffer);
                        printf("sent [%d] %.*s\r\n", scount, strstr(sbuffer, "\r\n\r\n")-sbuffer, sbuffer);
                        
                        // Recieve a OK from the server
                        rcount = mdm.socketRecv(socketID, rbuffer, sizeof rbuffer);
                        printf("recv %d [%.*s]\r\n", rcount, strstr(rbuffer, "\r\n")-rbuffer, rbuffer);
                        
                        socketClosed = mdm.socketFree(socketID);
                        //ENDTEST
                        
                        //sprintf(sbuffer, "GET /receive_gpsData.php?lat=%s&long=%s HTTP/1.1\r\nHost: 54.244.63.232\r\nAccept: text/html\r\nConnection: keep-alive\r\n\r\n",lat,longi);                        
                        //scount = mdm.socketSend(socketID,sbuffer, sizeof sbuffer);
                        //printf("sent [%d] %.*s\r\n", scount, strstr(sbuffer, "\r\n")-sbuffer, sbuffer); 
                        
                        //OTHER DATA
                        //sprintf(sbuffer, "GET /receive_gpsData.php?spd=%s&alt=%s HTTP/1.1\r\nHost: 54.244.63.232\r\nConnection: keep-alive\r\n\r\n",spd,alt);
                        //scount = mdm.socketSend(socketID,sbuffer, sizeof sbuffer);
                                                
                        
                        
                        //scount = mdm.socketSend(socketID,post_string, sizeof post_string);
                             
                    }
                    
                              
            } 
       }
                

    //--------------------------------------------------------------------------
   
    //----------------periodic SMS update and network check----------------------
        if (mdmOk && (i++ == 5000/wait)) {
            i = 0;
            // check the network status
            if (mdm.checkNetStatus(&netStatus)) {
                mdm.dumpNetStatus(&netStatus, fprintf, stdout);
            }
                
            // checking unread sms`
            int ix[8];
            int n = mdm.smsList("REC UNREAD", ix, 8);
            if (8 < n) n = 8;
            while (0 < n--)
            {
                char num[32];
                printf("Unread SMS at index %d\r\n", ix[n]);
                if (mdm.smsRead(ix[n], num, buf, sizeof(buf))) {
                    printf("Got SMS from \"%s\" with text \"%s\"\r\n", num, buf);
                    printf("Delete SMS at index %d\r\n", ix[n]);
                    mdm.smsDelete(ix[n]);
                    // provide a reply
                    const char* reply = "Hello my friend";
                    if (strstr(buf, /*w*/"here are you"))
                        reply = *link ? link : "I don't know"; // reply wil location link
                    else if (strstr(buf, /*s*/"hutdown"))
                        abort = true, reply = "bye bye";
                    else if (strstr(buf, /*S*/"tatus"))
                        reply = devStatusString;                //reply with all data
                    printf("Send SMS reply \"%s\" to \"%s\"\r\n", reply, num);
                    mdm.smsSend(num, reply);
                }
            }
        }
    }
#ifdef RTOS_H
        Thread::wait(wait);
#else
        ::wait_ms(wait);
#endif
    }
    gps.powerOff();
    mdm.powerOff();
    return 0;
}

