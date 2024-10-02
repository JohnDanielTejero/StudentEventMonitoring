using StudentEventMonitoring.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentEventMonitoring
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Create an instance of db con with proper configurations
            DbCon configuration = DbConf.Instance()
                    .SetServer("localhost")
                    .SetUser("root")    
                    .SetPassword("password")
                    .SetDatabase("wam1_midterm_db")
                    .Build();
            Application.Run(new Form1());
        }
    }
}
