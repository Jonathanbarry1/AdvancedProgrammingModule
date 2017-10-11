using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingAssignment
{
    [Serializable]
    public class Bet
    {
        public string Course;
        public string Horse;
        public DateTime Date;
        public decimal Amount;
        public bool Won;

        public Bet(string course, DateTime date, decimal amount, bool won)
        {
            Course = course;
            if (date > DateTime.Now) //makes sure that the bet happened in the past! 
                throw new ArgumentOutOfRangeException();
            Date = date;
            Amount = amount;
            Won = won;            
        }

        public Bet(string course, string horse, DateTime date, decimal amount, bool won)
        {
            Course = course;
            Horse = horse;
            if (date > DateTime.Now)
                throw new ArgumentOutOfRangeException();
            Date = date;
            Amount = amount;
            Won = won;
        }

        public override string ToString()
        {
            
            if (Horse == null)
            {
                return string.Format("The race course is {0}, the date was {1}, the amount was {2:0.00}, and it was a {3}.", Course, Date.ToShortDateString(), Amount, (Won) ? "victory" : "loss");
            }
            else
            {
                return string.Format("The race course is {0}, the date was {1}, the amount was {2:0.00}, and it was a {3}. The horse's name was {4}", Course, Date.ToShortDateString(), Amount, (Won) ? "victory" : "loss", Horse);
            }
            
        }
    }
}