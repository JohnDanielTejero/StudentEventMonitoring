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
        public AddStudent()
        {
            InitializeComponent();
            con = DbCon.Instance();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbStudentNumber.Text.Trim().Length == 0 || tbFirstName.Text.Trim().Length == 0 || tbLastName.Text.Trim().Length == 0 || cbProgram.SelectedIndex < 0|| cbYear.SelectedIndex < 0) {
                MessageBox.Show("Form incomplete. Please try again.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
                if (Regex.IsMatch(tbStudentNumber.Text.Trim(), regexPattern)) {
                    try {
                        if (con.InsertData("students", new Dictionary<string, string>()
                        {
                            { "student_number" , tbStudentNumber.Text.Trim()},
                            { "first_name" , tbFirstName.Text.Trim()},
                            { "last_name" , tbLastName.Text.Trim()},
                            { "program" , cbProgram.Text},
                            { "year_level" , cbYear.Text}
                        })) {
                            MessageBox.Show("Student successfully added.");

                            tbStudentNumber.Text = "";
                            tbFirstName.Text = "";
                            tbLastName.Text = "";
                            cbProgram.SelectedIndex = -1;
                            cbYear.SelectedIndex = -1;
                        }
                    }
                    catch (Exception ex) {
                        MessageBox.Show($"{ex.Message} Please try again.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
