using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlotsForCourseWork.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using SlotsForCourseWork.Services.Contracts;
using SlotsForCourseWork.Models;

namespace SlotsForCourseWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionAPIController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ApplicationContext _context;
    
        public TransactionAPIController(ApplicationContext db, ITransactionService transactionService)
        {
            _context = db;
            _transactionService = transactionService;
        }

        [HttpGet]
        public IEnumerable Get()
        {
            var transactions = _transactionService.GetAllTransactions();
            return transactions;
        }
    }
}