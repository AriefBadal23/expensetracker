using expensetrackerapi.Contracts;
using expensetrackerapi.DTO;
using expensetrackerapi.Models;
using Microsoft.AspNetCore.Mvc;
using expensetrackerapi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace expensetrackerapi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class Transactions : Controller
    {

        private readonly IExpenseService _expenseExpenseService;
        private readonly ILogger<IExpenseService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public Transactions(IExpenseService expenseService, ILogger<IExpenseService> logger, UserManager<ApplicationUser> userManager)
        {
            _expenseExpenseService = expenseService;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet("details")]
        public async Task<ActionResult> GetTransactionById([FromQuery] int id)
        {
            var transaction = await _expenseExpenseService.GetTransactionByID(id);
            
            if (transaction.Value == null)
            {
                return NotFound(transaction);
            }

            return Ok(transaction);
        }


        [HttpGet]
        public async Task<ActionResult> Get(
            [FromQuery] int? month, int? year,
            [FromQuery] int? bucket,
            int pageNumber = 1, int pageSize = 3)
        {
            var id = _userManager.GetUserId(User);
            var transactions = await _expenseExpenseService.GetTransactions(id, month, year, bucket, pageNumber, pageSize);
            if (!transactions.IsSuccess)
            {
                return NotFound();
            }
            return Ok(transactions);

        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] RequestTransactionDto transaction)
        {
            var userId = _userManager.GetUserId(User);
            var transactionCreated = await _expenseExpenseService.CreateTransaction(userId,transaction);
            if (transactionCreated.Value == null) return BadRequest();
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
            if (transaction.Value != null)
            {
                return Ok(transaction);
            }
            return NotFound();
        }
    }
}