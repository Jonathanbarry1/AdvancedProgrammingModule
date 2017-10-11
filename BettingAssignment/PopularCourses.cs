using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingAssignment
{
    public class PopularCourses
    {
        public string CourseName { get; set; }
        public int NumberOfTimes { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, and it was bet on {1} times.", CourseName, NumberOfTimes);            
           

        }
    }
}
