using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System;
using System.ComponentModel.DataAnnotations;
using SlotsForCourseWork.Models;

namespace SlotsForCourseWork.DTO
{
    public class TransactionDto
    {
        public string UserName { get; }
        public string Time { get; }
        public int Bet { get; }
        public int Result { get; }
        
        public TransactionDto (string userName, string time, int bet,int result)
        {
            UserName = userName;
            Time = time;
            Bet = bet;
            Result = result;
        }
    }
}
