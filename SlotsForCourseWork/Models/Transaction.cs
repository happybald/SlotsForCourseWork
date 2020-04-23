﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SlotsForCourseWork.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Time { get; set; }
        [Required]
        public int Bet { get; set; }
        [Required]
        public int Result { get; set; }
    }
}
