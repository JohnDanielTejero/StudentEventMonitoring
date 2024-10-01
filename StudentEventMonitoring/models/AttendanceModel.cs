using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentEventMonitoring.models
{
    class AttendanceModel
    {
        string studentnumber;
        int eventid;
        DateTime timein;
        DateTime timeout;

        public AttendanceModel() { }
        public AttendanceModel(string studentnumber, int eventid, DateTime timein, DateTime timeout)
        {
            this.Studentnumber = studentnumber;
            this.Eventid = eventid;
            this.Timein = timein;
            this.Timeout = timeout;
        }

        public string Studentnumber { get => studentnumber; set => studentnumber = value; }
        public int Eventid { get => eventid; set => eventid = value; }
        public DateTime Timein { get => timein; set => timein = value; }
        public DateTime Timeout { get => timeout; set => timeout = value; }
    }
}
