using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingAssignment
{
    public class YearlyTotal
    {
        public int Year;
        public decimal AmountWon;
        public decimal AmountLost;

        public override string ToString()
        {
            return string.Format("{0} \t {1:0.00} \t {2:0.00}", Year, AmountWon, AmountLost);

        }
    }
}
