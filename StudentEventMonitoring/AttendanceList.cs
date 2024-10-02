using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentEventMonitoring
{
    public partial class AttendanceList : Form
    {
        public AttendanceList()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            EventPage eventPage = new EventPage();
            eventPage.Show();
            this.Hide();
        }
    }
}
