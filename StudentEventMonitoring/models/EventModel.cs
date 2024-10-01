using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentEventMonitoring.models
{
    class EventModel
    {
        int eventid;
        string title;
        string description;
        DateTime startdate;
        DateTime enddate;
        string venue;
        string status;

        public EventModel() { }
        public EventModel(int eventid, string title, string description, DateTime startdate, DateTime enddate, string venue, string status)
        {
            this.eventid = eventid;
            this.title = title;
            this.description = description;
            this.startdate = startdate;
            this.enddate = enddate;
            this.venue = venue;
            this.status = status;
        }

        public int Eventid { get => eventid; set => eventid = value; }
        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        public DateTime Startdate { get => startdate; set => startdate = value; }
        public DateTime Enddate { get => enddate; set => enddate = value; }
        public string Venue { get => venue; set => venue = value; }
        public string Status { get => status; set => status = value; }
    }
}
