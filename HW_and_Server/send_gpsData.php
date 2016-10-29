<?php
//this script takes in requests from the software for GPS co-ordinates and returns them using an appropriate HTTP response. 

//setup database and options
$hostname = 'localhost';  //the MySQL db on the AWS instance is on localhost 
$dbname = 'mysql';	//connect to the db
$username = 'root';
$password = 'toor';

//now connect to the database
$dbconnection = mysqli_connect($hostname, $username, $password) or die('SQL_ConnectDb_Fail');

//choose the database
$selDb = mysqli_select_db($dbconnection,$dbname) or die('SQL_SelectDb_Fail');

//receive the GET request from the software , make a request to the DB for the data and return the info

$latestRow = "SELECT * FROM `gpsData` WHERE `packetID` = (SELECT MAX(`packetID`) FROM `gpsData`)";
$sqlAccess = mysqli_query($dbconnection,$latestRow) or die('SQL_SelectEntry_Fail'.mysql_error());

$fetchResult = mysqli_fetch_array($sqlAccess);	//get the array of data

if ($sqlAccess)
{
	echo printf ("%s; %s; %s; %s; %s; %s; %s; %s", $fetchResult[0], $fetchResult[1],$fetchResult[2], $fetchResult[3],$fetchResult[4], $fetchResult[5],$fetchResult[6],$fetchResult[7]);
}else{
	echo 'RowPrint_Fail';
}

mysqli_close($dbconnection);

?>

