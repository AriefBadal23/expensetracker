using expensetrackerapi.Contracts;
using expensetrackerapi.DTO;
using expensetrackerapi.Models;
using Microsoft.AspNetCore.Mvc;
using expensetrackerapi.Services;
using Microsoft.AspNetCore.Authorization;

namespace expensetrackerapi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class Transactions : Controller
    {

        private readonly IExpenseService _expenseExpenseService;
        private readonly ILogger<IExpenseService> _logger;

        public Transactions(IExpenseService expenseService, ILogger<IExpenseService> logger)
        {
            _expenseExpenseService = expenseService;
            _logger = logger;
        }

        [HttpGet("details")]
        public async Task<ActionResult> GetTransactionById([FromQuery] int id)
        {
            var transaction = await _expenseExpenseService.GetTransactionByID(id);
            
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get(
            [FromQuery] int? month, int? year,
            [FromQuery] int? bucket,
            int pageNumber = 1, int pageSize = 3)
        {
            var transactions = await _expenseExpenseService.GetTransactions(month, year, bucket, pageNumber, pageSize);
            if (!transactions.IsSuccess)
            {
                return NotFound();
            }
            return Ok(transactions);

        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] RequestTransactionDto transaction)
        {

            var transactionCreated = await _expenseExpenseService.CreateTransaction(transaction);
            if (transactionCreated == null) return BadRequest();
            return Ok(transactionCreated);

        }

        [HttpDelete("{transactionID}")]
        public async Task<ActionResult> Delete(int transactionID)
        {
            var isDeleted = await _expenseExpenseService.DeleteTransaction(transactionID);
            if (!isDeleted.IsSuccess)
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] Transaction updatedTransaction)
        {
            var transaction = await _expenseExpenseService.UpdateTransaction(updatedTransaction);
            if (transaction != null)
            {
                return Ok(transaction);
            }
            return NotFound();
        }
    }
}