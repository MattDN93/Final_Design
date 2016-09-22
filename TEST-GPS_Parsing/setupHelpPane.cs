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
    public partial class setupHelpPane : Form
    {
        int helpRequired = -1;
        public setupHelpPane(int helpType)
        {
            InitializeComponent();
            switch (helpType)
            {
                case 0: helpImage.Image = Image.FromFile("coordsHelp.png");break;
                case 1: helpImage.Image = Image.FromFile("cameraHelp.png");break;
                case 2: 
                default:
                    break;
            }
        }

        private void setupHelpPane_Load(object sender, EventArgs e)
        {
            switch (helpRequired)
            {
                case 0: helpLabel.Text = "entering camera bound co-ordinates:"; break;
                case 1: helpLabel.Text = "choosing camera source:"; break;
                case 2: 
                default:
                    break;
            }
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
