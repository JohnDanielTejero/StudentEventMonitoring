using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using StudentEventMonitoring.models;
using StudentEventMonitoring.utils;

namespace StudentEventMonitoring
{
    public partial class StudentList : Form
    {
        DbCon con;
        public StudentList()
        {
            InitializeComponent();
            con = DbCon.Instance();
        }

        private void LoadStudentsData() 
        {
            try
            {
                MySqlDataReader reader = con.ReadData("students", new Dictionary<string, string>() { });
                DataTable table = new DataTable();
                table.Columns.Add("Student Number");
                table.Columns.Add("First Name");
                table.Columns.Add("Last Name");
                table.Columns.Add("Program");
                table.Columns.Add("Year");

                while (reader.Read())
                {
                    DataRow row = table.NewRow();
                    row["Student Number"] = reader["student_number"];
                    row["First Name"] = reader["first_name"];
                    row["Last Name"] = reader["last_name"];
                    row["Program"] = reader["program"];
                    row["Year"] = reader["year_level"];

                    table.Rows.Add(row);
                }
                reader.Close();
                studentsTable.DataSource = table;

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
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Form1 landingpage = new Form1();
            landingpage.Show();
            this.Hide();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddStudent addStudent = new AddStudent();
            addStudent.Show();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            LoadStudentsData();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void StudentList_Load(object sender, EventArgs e)
        {
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (studentsTable.SelectedRows.Count > 0)
            {
                var selectedRow = studentsTable.SelectedRows[0];

                if (selectedRow.Cells["Student Number"] != null)
                {
                    string studentNumber = (string)selectedRow.Cells["Student Number"].Value;

                    DialogResult result = MessageBox.Show(
                        $"Are you sure you want to delete the student with ID: {studentNumber}?",
                        "Confirm Delete",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning
                    );

                    if (result == DialogResult.OK)
                    {
                        con.DeleteData("students", new Dictionary<string, string>{{ "student_number", studentNumber }});
                        LoadStudentsData();
                        MessageBox.Show("Student deleted successfully!", "Operation Successfull!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Deletion canceled.", "Operation Canceled!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Column 'Student Number' not found.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select the student's row that you want to delete.", "No row selected!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
