using MySql.Data.MySqlClient;
using StudentEventMonitoring.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentEventMonitoring
{
    public partial class Form1 : Form
    {
        private DbCon connection;
        public Form1()
        {
            InitializeComponent();
            connection = DbCon.Instance();
        }

        private void btnAddStudent_Click(object sender, EventArgs e)
        {

            AddStudent AddStudent = new AddStudent();
            AddStudent.Show();
            this.Hide();
        }

        private void btnStudentList_Click(object sender, EventArgs e)
        {
            StudentList studentList = new StudentList();
            studentList.Show();
            this.Hide();

        }

        private void btnEventList_Click(object sender, EventArgs e)
        {
            EventList eventlist = new EventList();
            eventlist.Show();
            this.Hide();

            //Pantesting para lang maview yung design ng Event Page
            /*EventPage eventPage = new EventPage();
            eventPage.Show();
            this.Hide();*/

        }
    }
}
