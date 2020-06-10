using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SlotsForCourseWork.DTO
{
    public class UserDTO
    {
        public string UserName { get; }
        public int BestScore { get; }

        public UserDTO(string userName, int bestScore)
        {
            UserName = userName;
            BestScore = bestScore;
        }
    }
}
