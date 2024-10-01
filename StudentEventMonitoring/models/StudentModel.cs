using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentEventMonitoring.models
{
    class StudentModel
    {
        string studentNumber;
        string lastName;
        string firstName;
        string program;
        int yearlevel;

        public StudentModel() { }
        public StudentModel(string studentNumber, string lastName, string firstName, string program, int yearlevel)
        {
            this.studentNumber = studentNumber;
            this.lastName = lastName;
            this.firstName = firstName;
            this.program = program;
            this.yearlevel = yearlevel;
        }

        public string StudentNumber { get => studentNumber; set => studentNumber = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string Program { get => program; set => program = value; }
        public int Yearlevel { get => yearlevel; set => yearlevel = value; }
    }
}
