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
using System.Text.RegularExpressions;

namespace StudentEventMonitoring
{
    public partial class AddStudent : Form
    {
        DbCon con;
        string regexPattern = @"^\d{3}-\d{4}$";
        public AddStudent(
            string studentNumber = "", 
            string firstName = "", 
            string lastName = "", 
            string program = "", 
            string yearLevel = "")
        {
            InitializeComponent();
            con = DbCon.Instance();

            if (!string.IsNullOrEmpty(studentNumber))
            {
                tbStudentNumber.Text = studentNumber;
                tbFirstName.Text = firstName;
                tbLastName.Text = lastName;
                cbProgram.SelectedItem = program;
                cbYear.SelectedItem = yearLevel;
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbStudentNumber.Text.Trim().Length == 0 || tbFirstName.Text.Trim().Length == 0 || tbLastName.Text.Trim().Length == 0 || cbProgram.SelectedIndex < 0|| cbYear.SelectedIndex < 0) {
                MessageBox.Show("Form incomplete. Please try again.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
                if (Regex.IsMatch(tbStudentNumber.Text.Trim(), regexPattern)) {
                    try {
                        var studentData = new Dictionary<string, string>()
                        {
                            { "student_number", tbStudentNumber.Text.Trim() },
                            { "first_name", tbFirstName.Text.Trim() },
                            { "last_name", tbLastName.Text.Trim() },
                            { "program", cbProgram.Text },
                            { "year_level", cbYear.Text }
                        };

                        var conditions = new Dictionary<string, string> { { "student_number", tbStudentNumber.Text.Trim() } };

                        using (var reader = con.ReadData("students", conditions))
                        {
                            if (reader.HasRows) 
                            {
                                reader.Close();
                                DialogResult dialogResult = MessageBox.Show(
                                        "Student number already exists. Do you still want to continue? By clicking Yes, student details will be updated aside from the student number.", 
                                        "Confirmation", 
                                        MessageBoxButtons.YesNo, 
                                        MessageBoxIcon.Question
                                );

                                if (dialogResult == DialogResult.Yes)
                                {
                                    con.UpdateData("students", studentData, conditions);
                                    MessageBox.Show("Student details updated successfully.");
                                }
                                else
                                {
                                    MessageBox.Show("Student update canceled.");
                                }
                            }
                            else 
                            {
                                reader.Close();
                                con.InsertData("students", studentData); 
                                MessageBox.Show("Student successfully added.");
                            }
                        }

                        tbStudentNumber.Text = "";
                        tbFirstName.Text = "";
                        tbLastName.Text = "";
                        cbProgram.SelectedIndex = -1;
                        cbYear.SelectedIndex = -1;

                    }
                    catch (Exception ex) {
                        MessageBox.Show($"{ex.Message}. Please try again.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else {
                    MessageBox.Show("Invalid Student ID. Please try again.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Form1 landingpage = new Form1();
            landingpage.Show();
            this.Hide();

        }
    }
}
