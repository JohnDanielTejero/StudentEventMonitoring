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
    public partial class Attendance : Form
    {

        int selectedEvent = -1;

        public Attendance()
        {
            InitializeComponent();
        }

        public Attendance(int eventId)
        {
            InitializeComponent();
            selectedEvent = eventId;
        }

        private void Attendance_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
            new EventPage(selectedEvent).Show();
        }
    }
}
