using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlotsForCourseWork.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime Time { get; set; }
        public int Bet { get; set; }
        public int Result { get; set; }
    }
}
