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
        public int selectedEvent = -1;

        public AttendanceList()
        {
            InitializeComponent();
        }

        public AttendanceList(int eventId)
        {
            InitializeComponent();
            selectedEvent = eventId;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            EventPage eventPage = new EventPage(selectedEvent);
            eventPage.Show();
            this.Hide();
        }
    }
}
