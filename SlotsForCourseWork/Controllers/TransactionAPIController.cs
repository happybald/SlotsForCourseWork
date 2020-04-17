using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using SlotsForCourseWork.Models;

namespace SlotsForCourseWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionAPIController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
    
        public TransactionAPIController(ApplicationContext db, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        struct TrNormalized
        {
            public string DateTime { get; set; }
            public string UserName { get; set; }
            public int Bet { get; set; }
            public int Win { get; set; }

        }

        [HttpGet]
        public IEnumerable Get()
        {
                var transactions = this._context.Transactions.OrderByDescending(u => u.Time).Take(20).ToList<Transaction>();
                var transactListNormalized = new List<TrNormalized>();
                for (int i = 0; i < transactions.Count; i++)
                {
                    transactListNormalized.Add(new TrNormalized { DateTime = transactions[i].Time.ToString("dd/M/yyyy HH:mm:ss"), UserName = transactions[i].UserName, Bet = transactions[i].Bet, Win = transactions[i].Result });
                }
                if (transactions.Count <= 0)
                {
                    return "Error!";
                }
                return transactListNormalized;

        }
    }
}