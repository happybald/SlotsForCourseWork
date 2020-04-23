using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SlotsForCourseWork.DTO
{
    public class TransactionDTO
    {
        public string UserName { get; set; }
        public string Time { get; set; }
        public int Bet { get; set; }
        public int Result { get; set; }
    }
}
