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

namespace StudentEventMonitoring
{
    public partial class CreateEvent : Form
    {
        private DbCon connection;
        public CreateEvent()
        {
            InitializeComponent();
            connection = DbCon.Instance();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EventList eventlist = new EventList();
            eventlist.Show();
            this.Hide();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (formInvalid()) throw new Exception("Invalid Data Detected");
                Dictionary<string, string> eventData = new Dictionary<string, string>()
                {
                    { "title", eventTitle.Text },
                    { "description", description.Text },
                    { "start_date", startDate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")},
                    { "end_date", endDate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")}
                };

                if (!connection.InsertData("events", eventData)) throw new Exception("Failed Create Event");

                MessageBox.Show("Event Created");
                this.Dispose();
                new EventList().Show();
            }catch(Exception ex)
            {
                MessageBox.Show("Something went worng\n\nMessage: " + ex.Message);
            }
        }

        private bool formInvalid()
        {
            DateTime start = startDate.Value;
            DateTime end = endDate.Value;
            return Validation.isEmpty(eventTitle.Text) || Validation.isEmpty(description.Text) || start > end;
        }

        private void CreateEvent_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
