using MySql.Data.MySqlClient;
using StudentEventMonitoring.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace StudentEventMonitoring
{
    public partial class AttendanceList : Form
    {
        public int selectedEvent = -1;
        private DbCon con;

        public AttendanceList()
        {
            InitializeComponent();
        }

        public AttendanceList(int eventId)
        {
            InitializeComponent();
            selectedEvent = eventId;
            this.con = DbCon.Instance();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            EventPage eventPage = new EventPage(selectedEvent);
            eventPage.Show();
            this.Hide();
        }

        private void attendances_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void AttendanceList_Load(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Student Number");
            table.Columns.Add("First Name");
            table.Columns.Add("Last Name");
            table.Columns.Add("Program");
            table.Columns.Add("Year Level");
            table.Columns.Add("Time in");
            table.Columns.Add("Time out");

            string query = @"
                SELECT s.student_number, s.first_name, s.last_name, s.program, s.year_level, a.timein, a.timeout
                FROM students s
                JOIN attendances a ON s.student_number = a.student_number
                WHERE a.event_id = @eventId;";

            try
            {
                using (con.Connection) 
                {
                    con.IsConnect();

                    using (MySqlCommand cmd = new MySqlCommand(query, con.Connection))
                    {
                       
                        Console.WriteLine($"Selected Event ID: {selectedEvent}");
                        cmd.Parameters.AddWithValue("@eventId", selectedEvent);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("No records found for the given event ID.");
                                MessageBox.Show("No records found for this event.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return; 
                            }

                            while (reader.Read())
                            {
                                DataRow row = table.NewRow();
                                row["Student Number"] = reader["student_number"];
                                row["First Name"] = reader["first_name"];
                                row["Last Name"] = reader["last_name"];
                                row["Program"] = reader["program"];
                                row["Year Level"] = reader["year_level"];
                                row["Time in"] = reader["timein"];
                                row["Time out"] = reader["timeout"];
                                table.Rows.Add(row);
                            }
                        }
                    }
                }
                attendances.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
