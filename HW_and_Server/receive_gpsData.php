<?php

//setup database and options
$hostname = 'localhost';  //the MySQL db on the AWS instance is on localhost 
$dbname = 'mysql';	//connect to the db
$username = 'root';
$password = 'toor';

//now connect to the database
$dbconnection = mysqli_connect($hostname, $username, $password) or die('Connection to database failed - is the MySQL service running on the server?');

//choose the database
$selDb = mysqli_select_db($dbconnection,$dbname) or die('Database connection failed. Not available');

//receive the incoming GPS variables and data
//Will ignore any packets that aren't sent
//into packetID, place the varibale called 'pid' in GET
$packetID = (isset($_GET['pid']))? $_GET['pid']: false;
$sessionID = (isset($_GET['sid']))? $_GET['sid']: false;
$date = (isset($_GET['date']))? $_GET['date']: false;
$time = (isset($_GET['time']))? $_GET['time']: false;
$latitude = (isset($_GET['lat']))? $_GET['lat']: false;
$longitude = (isset($_GET['long']))? $_GET['long']: false;
$grSpdKph = (isset($_GET['spd']))? $_GET['spd']: false;
$altitude = (isset($_GET['alt']))? $_GET['alt']: false;
$angleCard = (isset($_GET['ac']))? $_GET['ac']: false;
$angleDeg = (isset($_GET['ad']))? $_GET['ad']: false;
$accuracy = (isset($_GET['acc']))? $_GET['acc']: false;
$fixType = (isset($_GET['ft']))? $_GET['ft']: false;
$fixQual = (isset($_GET['fq']))? $_GET['fq']: false;
$numSats = (isset($_GET['ns']))? $_GET['ns']: false;
$checksum = (isset($_GET['ns']))? $_GET['ns']: false;

//insert the incoming data into the MySQL database
//first braket is MySQL headers - VALUES is the value name above
$sql = "INSERT INTO `gpsData`(`packetID`, `sessionID`, `date`, `time`, `latitude`, `longitude`, `grSpdKph`, `altitude`, `angleCard`, `angleDeg`, `accuracy`, `fixType`, `fixQual`, `numSats`, `checksum`) VALUES ('$packetID','$sessionID',NOW(),NOW(),'$latitude','$longitude','$grSpdKph','$altitude','$angleCard','$angleDeg' ,'$accuracy' ,'$fixType','$fixQual','$numSats','$checksum')";

$insert = mysqli_query($dbconnection, $sql) or die('SQL write error: '. mysql_error()); // insert MySQL request;

if($insert) {
 echo'MySQL database write OK';
}

mysqli_close($dbconnection);

?>
