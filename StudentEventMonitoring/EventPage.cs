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
    public partial class EventPage : Form
    {
        public EventPage()
        {
            InitializeComponent();
        }

        private void btnAttendanceList_Click(object sender, EventArgs e)
        {
            AttendanceList attendanceList = new AttendanceList();
            attendanceList.Show();
            this.Hide();
        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            Attendance attendance = new Attendance();
            attendance.Show();
            this.Hide();

        }
    }
}
