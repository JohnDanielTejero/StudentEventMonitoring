using MySql.Data.MySqlClient;
using StudentEventMonitoring.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentEventMonitoring
{
    public partial class Attendance : Form
    {

        int selectedEvent = -1;
        private DbCon con;
        public Attendance()
        {
            InitializeComponent();
            this.con = DbCon.Instance();
        }

        public Attendance(int eventId)
        {
            InitializeComponent();
            selectedEvent = eventId;
            this.con = DbCon.Instance();
        }

        private void Attendance_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
            new EventPage(selectedEvent).Show();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            string regexPattern = @"^\d{3}-\d{4}$";

            if (string.IsNullOrWhiteSpace(studentidinput.Text))
            {
                MessageBox.Show("Field cannot be empty.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Regex.IsMatch(studentidinput.Text.Trim(), regexPattern))
            {
                MessageBox.Show("Incorrect student number.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MySqlDataReader reader = null; 

            try
            {
                reader = con.ReadData("attendances", new Dictionary<string, string>() {
                    { "student_number", studentidinput.Text.Trim() },
                    { "event_id", selectedEvent.ToString() }
                });

                if (reader != null && reader.Read())
                {
                    // Check for timeout
                    if (reader["timeout"] == DBNull.Value)
                    {
                        reader.Close();

                        con.UpdateData("attendances", new Dictionary<string, string>() {
                            { "timeout", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") }
                        }, new Dictionary<string, string>() {
                            { "student_number", studentidinput.Text.Trim() },
                            { "event_id", selectedEvent.ToString() }
                        });

                        MessageBox.Show("Time out success!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.studentidinput.Text = "";
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Student already logged in for this event.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        reader.Close(); 
                        return;
                    }
                }
                reader?.Close();
                con.InsertData("attendances", new Dictionary<string, string>() {
                    { "student_number", studentidinput.Text.Trim() },
                    { "event_id", selectedEvent.ToString() },
                    { "timein", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") }
                });

                MessageBox.Show("Attendance record created!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.studentidinput.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred: {ex.Message}. Please try again.",
                    "Error!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                reader?.Close();
            }
        }

    }
}
