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
    public partial class EventList : Form
    {
        private Timer debounceTimer;
        private const int debounceInterval = 300;

        private DbCon connection;
        public EventList()
        {
            InitializeComponent();
            connection = DbCon.Instance();
            populateTable();
            debounceTimer = new Timer();
            debounceTimer.Interval = debounceInterval;
            debounceTimer.Tick += DebounceTimer_Tick;
        }

        private void DebounceTimer_Tick(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            string searchInput = search.Text.Trim();

            if (string.IsNullOrEmpty(searchInput))
            {
                populateTable();
                return;
            }

            MySqlDataReader reader = null;

            DataTable table = new DataTable();
            table.Columns.Add("ID");
            table.Columns.Add("Title");
            table.Columns.Add("Start");
            table.Columns.Add("End");
            table.Columns.Add("Description");

            try
            {
                var parameters = new Dictionary<string, string>
                {
                    { "title", $"%{searchInput}%" },
                    { "description", $"%{searchInput}%" },
                };

                if (connection.Connection.State == System.Data.ConnectionState.Open && reader != null)
                {
                    reader.Close();
                }

                reader = connection.ReadMatchData("events", parameters);

                while (reader.Read())
                {

                    DataRow row = table.NewRow();
                    row["ID"] = reader["event_id"];
                    row["Title"] = reader["title"];
                    row["Start"] = reader["start_date"];
                    row["End"] = reader["end_date"];
                    row["Description"] = reader["description"];
                    table.Rows.Add(row);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{ex}. Please try again.",
                    "Error!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
            }

            events.DataSource = table;
        }

        private void populateTable()
        {
            MySqlDataReader records = connection.ReadData("events", new Dictionary<string, string>());
            DataTable table = new DataTable();
            table.Columns.Add("ID");
            table.Columns.Add("Title");
            table.Columns.Add("Start");
            table.Columns.Add("End");
            table.Columns.Add("Description");

            while (records.Read())
            {
                DataRow row = table.NewRow();
                row["ID"] = records["event_id"];
                row["Title"] = records["title"];
                row["Start"] = records["start_date"];
                row["End"] = records["end_date"];
                row["Description"] = records["description"];
                table.Rows.Add(row);
            }
            records.Close();
            events.DataSource = table;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Form1 landingpage = new Form1();
            landingpage.Show();
            this.Hide();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            CreateEvent createEvent = new CreateEvent();
            createEvent.Show();
            this.Hide();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (events.CurrentCell != null)
                {
                    var selectedCell = events.Rows[events.CurrentCell.RowIndex].Cells["ID"].Value;

                    if (selectedCell != null && int.TryParse(selectedCell.ToString(), out int id))
                    {
                        new CreateEvent(id).Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("No valid record selected.");
                    }
                }
                else
                {
                    MessageBox.Show("No cell is currently selected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            try
            {
                if (events.CurrentCell != null)
                {
                    var selectedCell = events.Rows[events.CurrentCell.RowIndex].Cells["ID"].Value;

                    if (selectedCell != null && int.TryParse(selectedCell.ToString(), out int id))
                    {
                        if (!connection.DeleteData("events", new Dictionary<string, string>() { { "event_id", id.ToString() } }))
                        {
                            throw new Exception("Failed to Delete");
                        }

                        MessageBox.Show("Record Deleted");
                        this.Dispose(); 
                        new EventList().Show();
                    }
                    else
                    {
                        MessageBox.Show("No valid record selected.");
                    }
                }
                else
                {
                    MessageBox.Show("No cell is currently selected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete\n\nMessage: " + ex.Message);
            }
        }

        private void EventList_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void events_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.Hide();
            new EventPage(Convert.ToInt32(events.Rows[events.CurrentCell.RowIndex].Cells["ID"].Value)).Show();
        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
            
        }
    }

}
