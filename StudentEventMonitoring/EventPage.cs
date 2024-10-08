using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StudentEventMonitoring.utils;
using MySql.Data.MySqlClient;

namespace StudentEventMonitoring
{
    public partial class EventPage : Form
    {

        private DbCon connection;

        private int selectedEvent = -1;


        public EventPage()
        {
            InitializeComponent();
        }

        public EventPage(int eventId)
        {
            InitializeComponent();
            connection = DbCon.Instance();
            selectedEvent = eventId;

            try
            {

                MySqlDataReader reader = connection.ReadData("events", new Dictionary<string, string>() { { "event_id", eventId.ToString() } });
                reader.Read();
                title.Text = reader["title"].ToString();
                description.Text = reader["description"].ToString();
                start.Value = DateTime.Parse(reader["start_date"].ToString());
                end.Value = DateTime.Parse(reader["end_date"].ToString());

                reader.Close();

            }catch(Exception ex)
            {
                MessageBox.Show("Failed to retrieve event data:\n\nMessage:" + ex.Message);
            }

        }

        private void btnAttendanceList_Click(object sender, EventArgs e)
        {
            AttendanceList attendanceList = new AttendanceList(selectedEvent);
            attendanceList.Show();
            this.Hide();
        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            Attendance attendance = new Attendance(selectedEvent);
            attendance.Show();
            this.Hide();

        }

        private void EventPage_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            new EventList().Show();
        }
    }
}
