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
    public partial class CreateEvent : Form
    {
        public CreateEvent()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EventList eventlist = new EventList();
            eventlist.Show();
            this.Hide();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {

        }
    }
}
