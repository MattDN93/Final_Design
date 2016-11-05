using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TEST_GPS_Parsing
{
    public partial class dbViewer : Form
    {
        MySQLInterface reviewSql;
        bool loggedInOk = false;

        public dbViewer()
        {
            InitializeComponent();
        }

        private void doMySqlQuery(string queryToExec)
        {
            MySqlCommand reviewCmd = new MySqlCommand(queryToExec, reviewSql.conn);
            DataTable dataTable = new DataTable();

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                dataTable.Columns.Add(col.Name);
                col.DataPropertyName = col.Name;
            }

            MySqlDataAdapter da = new MySqlDataAdapter(reviewCmd);
            da.Fill(dataTable);

            dataGridView1.DataSource = dataTable;
        }

        private void gpsDataRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (gpsDataRadioButton.Checked == true)
            {
                //disable some options not applicable to this db
                eventSearchCombobox.Enabled = false;
            }

        }

        private void showAllRecordsRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (showAllRecordsRadio.Checked == true)
            {
                dateTimeForSearch.Enabled= false;
                eventSearchCombobox.Enabled = false;
                specDateSearchRadioButton.Enabled = false;
                specEventSearchRadioButton.Enabled = false;
            }else if (showSpecificRecordsRadio.Checked == true)
            {
                dateTimeForSearch.Enabled = true;
                eventSearchCombobox.Enabled = true;
                specEventSearchRadioButton.Enabled = true;
                specDateSearchRadioButton.Enabled = true;
            }
        }

        private void showSpecificRecordsRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (showSpecificRecordsRadio.Checked == true)
            {
                showAllRecordsRadio.Checked = false;
                dateTimeForSearch.Enabled = true;
                eventSearchCombobox.Enabled = true;
                specEventSearchRadioButton.Enabled = true;
                specDateSearchRadioButton.Enabled = true;
            }
            else if (showAllRecordsRadio.Checked == true)
            {
                dateTimeForSearch.Enabled = false;
                eventSearchCombobox.Enabled = false;
                specDateSearchRadioButton.Enabled = false;
                specEventSearchRadioButton.Enabled = false;
            }
        }

        private void dbViewer_Load(object sender, EventArgs e)
        {
            eventSearchCombobox.Enabled = false;
            specDateSearchRadioButton.Enabled = false;
            specEventSearchRadioButton.Enabled = false;
            reviewSql = new MySQLInterface(); //setup a database object to use
        }

        private void startQueryButton_Click(object sender, EventArgs e)
        {
            if (!loggedInOk)
            {
                MessageBox.Show("Please login to the DB using the left fields first.", "Login first", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (gpsDataRadioButton.Checked == true) //connect to the gpsData table
            {
                if (showAllRecordsRadio.Checked == true && reviewSql.conn != null)    //show all of GPSDATA
                {
                    string reviewQuery = "SELECT * FROM `gpsData`;";
                    doMySqlQuery(reviewQuery);  //do the query to the db
                }

                //handle the specific date queries
                else if (showSpecificRecordsRadio.Checked == true && reviewSql.conn != null)
                {
                    specDateSearchRadioButton.Enabled = true;
                    specEventSearchRadioButton.Enabled = true;
                    //don't consider the events since those are in the other table
                    if (dateTimeForSearch.Checked)
                    {
                        // = dateTimeForSearch
                    }
                }
            }
            //deal with the same just 
            else if (videoLogRadioButton.Checked == true)
            {
                if (showAllRecordsRadio.Checked == true && reviewSql.conn != null)    //show all of GPSDATA
                {
                    string reviewQuery = "SELECT * FROM `videoLog`;";
                    doMySqlQuery(reviewQuery);  //execute the query

                }

                //handle the specific date queries
                else if ((showSpecificRecordsRadio.Checked == true || specDateSearchRadioButton.Checked == true || specEventSearchRadioButton.Checked == true) && reviewSql.conn != null)
                {
                    //first determine the query since it will be requested from the DB
                    string reviewQuery = "";
                    if (specDateSearchRadioButton.Checked == true)
                    {
                        string theDate = dateTimeForSearch.Value.ToString("yyyy-MM-dd");    //get the selected date
                        reviewQuery = "SELECT * FROM `videoLog` WHERE `Date` = '" + theDate +"';";
                    }
                    else if (specEventSearchRadioButton.Checked == true)
                        {
                        switch (eventSearchCombobox.SelectedIndex)
                        {
                            case 0: reviewQuery = "SELECT * FROM `videoLog` WHERE `eventDesc` = 'camera_Switch';"; break;
                            case 1: reviewQuery = "SELECT * FROM `videoLog` WHERE `eventDesc` = 'connection_Fail';"; break;
                            default:
                                break;
                        }
                    }

                    //now the query has been determined, execute it
                    doMySqlQuery(reviewQuery); //execute the mysql query
                }
            }


        }

        private void loginToDbButton_Click(object sender, EventArgs e)
        {
            try
            {
                //try logon to the DB with the user credentials
                reviewSql.loginProcess(usernameTextbox.Text, passwordTextbox.Text);
                usernameTextbox.Clear();
                passwordTextbox.Clear();
                usernameTextbox.BackColor = System.Drawing.Color.LightGreen;
                passwordTextbox.BackColor = System.Drawing.Color.LightGreen;
                loginToDbButton.Text = "OK!";
                loginToDbButton.Enabled = false;
                usernameTextbox.Enabled = false;
                passwordTextbox.Enabled = false;
                loggedInOk = true;
            }
            catch (Exception)
            {
                usernameTextbox.BackColor = System.Drawing.Color.PaleVioletRed;
                passwordTextbox.BackColor = System.Drawing.Color.PaleVioletRed;
                return;
            }
            
        }

        private void clearFieldsButton_Click(object sender, EventArgs e)
        {
            eventSearchCombobox.SelectedIndex = -1;
        }

        private void eventSearchCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

   
    }
}
