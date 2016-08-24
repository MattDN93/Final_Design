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
    public partial class CameraBoundsSetup : Form
    {
        public CameraBoundsSetup()
        {
            InitializeComponent();
        }

        private void setExtentsButton_Click(object sender, EventArgs e)
        {

        }

        private void clearFieldsButton_Click(object sender, EventArgs e)
        {
            latBottomLeft.Clear();
            latUpperLeft.Clear();
            latUpperRight.Clear();
            longBottomLeft.Clear();
            longUpperLeft.Clear();
            longUpperRight.Clear();

        }

        private void goBackButton_Click(object sender, EventArgs e)
        {
            checkFieldsTimer.Stop();        //stop and dispose the timer
            checkFieldsTimer.Dispose();
            this.Close();
        }

        private void CameraBoundsSetup_Load(object sender, EventArgs e)
        {
            //checkFieldsTimer = new Timer(); //instantiate the timer
            checkFieldsTimer.Start();       //start the timer to check status of the entry of points
        }

        private void checkFieldsTimer_Tick(object sender, EventArgs e)
        {
            
            if (longUpperLeft.Text == "" || latUpperLeft.Text == "" || latBottomLeft.Text == "" || longUpperRight.Text == "" )
            {
                setExtentsButton.Enabled = false;      //keep disabled until all fields are filled so they can't start prematurely
                camViewStatusTextBox.Clear();           //clears from last tick
                camViewStatusTextBox.AppendText("Fields empty:");   //append which fields are still empty until they're fixed
                if (longUpperLeft.Text == "")
                {
                    camViewStatusTextBox.AppendText(" Long. Step 1;");
                }
                if (latUpperLeft.Text == "")
                {
                    camViewStatusTextBox.AppendText(" Lat. Step 1;");
                }
                if (longUpperRight.Text == "")
                {
                    camViewStatusTextBox.AppendText(" Long. Step 2;");
                }
                if (latBottomLeft.Text == "")
                {
                    camViewStatusTextBox.AppendText(" Lat. Step 3;");
                }
            }
            else
            {
                camViewStatusTextBox.Clear();
                camViewStatusTextBox.AppendText("Fields filled. Ready to start video.");
                setExtentsButton.Enabled = true;        //keep start button enabled if fields contain something
            }

            latUpperRight.Text = latUpperLeft.Text;
            longBottomLeft.Text = longUpperLeft.Text;
        }
    }
}
