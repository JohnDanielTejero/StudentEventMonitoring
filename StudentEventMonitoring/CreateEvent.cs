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
    public partial class CreateEvent : Form
    {
        private DbCon connection;

        private int selectedEvent = -1;

        public CreateEvent()
        {
            InitializeComponent();
            connection = DbCon.Instance();
        }

        public CreateEvent(int eventID)
        {
            InitializeComponent();
            connection = DbCon.Instance();
            this.Text = "Edit Event";
            label1.Text = "Edit Event";
            btnCreate.Visible = false;
            saveEdit.Visible = true;

            try
            {
                MySqlDataReader reader = connection.ReadData("events", new Dictionary<string, string>() { { "event_id", eventID.ToString() } });
                reader.Read();

                eventTitle.Text = reader["title"].ToString();
                description.Text = reader["description"].ToString();
                startDate.Value = DateTime.Parse(reader["start_date"].ToString());
                endDate.Value = DateTime.Parse(reader["end_date"].ToString());
                reader.Close();
                selectedEvent = eventID;
            }catch (Exception ex)
            {
                MessageBox.Show("Could not retrive event data\n\nMessage: " + ex.Message);
            }
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
                Dictionary<string, string> eventData = createModel();

                if (!connection.InsertData("events", eventData)) throw new Exception("Failed Create Event");

                MessageBox.Show("Event Created");
                this.Dispose();
                new EventList().Show();
            }catch(Exception ex)
            {
                MessageBox.Show("Something went worng\n\nMessage: " + ex.Message);
            }
        }

        private Dictionary<string, string> createModel()
        {
            return new Dictionary<string, string>()
                {
                    { "title", eventTitle.Text },
                    { "description", description.Text },
                    { "start_date", startDate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")},
                    { "end_date", endDate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")}
                };
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

        private void saveEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (formInvalid()) throw new Exception("Invalid Data Detected");

                Dictionary<string, string> eventDetails = createModel();
                Dictionary<string, string> condition = new Dictionary<string, string>() { { "event_id", selectedEvent.ToString() } };

                if(!connection.UpdateData("events", eventDetails, condition))
                {
                    throw new Exception("Something went wrong when attempting to update");
                }

                MessageBox.Show("Event has been updated");
                this.Hide();
                new EventList().Show();
                
            }catch(Exception ex)
            {
                MessageBox.Show("Failed to update event details\n\nMessage: " + ex.Message);
            }
        }
    }
}
