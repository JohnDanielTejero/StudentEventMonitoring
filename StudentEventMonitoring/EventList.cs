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

        private DbCon connection;
        public EventList()
        {
            InitializeComponent();
            connection = DbCon.Instance();
            populateTable();
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
            this.Hide();
            new CreateEvent(Convert.ToInt32(events.Rows[events.CurrentCell.RowIndex].Cells["ID"].Value)).Show();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(events.Rows[events.CurrentCell.RowIndex].Cells["ID"].Value);
                if (!connection.DeleteData("events", new Dictionary<string, string>() { { "event_id", id.ToString() } })) throw new Exception("Failed to Delete");

                MessageBox.Show("Record Deleted");
                this.Dispose();
                new EventList().Show();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed to delete\n\nMessage:" + ex.Message);
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
    }
}
