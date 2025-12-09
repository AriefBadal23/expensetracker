using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using expensetrackerapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace expensetrackerapi.Controllers
{
    [Route("api/v1/[controller]")]
    public class Transactions : Controller
    {
        private readonly ILogger<Transactions> _logger;
        private readonly ExpenseTrackerContext _db;

        public Transactions(ILogger<Transactions> logger, ExpenseTrackerContext context)
        {
            _logger = logger;
            _db = context;
        }


        [HttpGet]
        public ActionResult Get()
        {
            var transactions = _db.Transactions;

            if (transactions == null) return NotFound("No transactions found.");
            return Ok(transactions);

        }

        [HttpPost]
        public ActionResult Post([FromBody] Transaction transaction)
        {
            if (transaction != null)
            {
                _db.Transactions.Add(transaction);
                _db.SaveChanges();

                return Ok();
            }

            return BadRequest("Invalid Transaction.");

        }
    }
}