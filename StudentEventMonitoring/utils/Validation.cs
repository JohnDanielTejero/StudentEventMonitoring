using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StudentEventMonitoring.utils
{
    class Validation
    {

        public static bool isEmpty(string value)
        {
            string pattern = @"^\s*$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(value);
        }

    }
}
