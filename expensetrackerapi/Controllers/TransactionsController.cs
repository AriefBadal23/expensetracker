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
        private readonly UserManager<ApplicationUser> _userManager;

        public Transactions(IExpenseService expenseService, UserManager<ApplicationUser> userManager)
        {
            _expenseExpenseService = expenseService;
            _userManager = userManager;
        }

        [HttpGet("details")]
        public async Task<ActionResult> GetTransactionById([FromQuery] int id)
        {
            var transaction = await _expenseExpenseService.GetTransactionByID(id);
            
            if (transaction.Value == null)
            {
                // TODO: De transaction kon niet gevonden worden.
                return NotFound(transaction);
            }

            return Ok(transaction);
        }


        [HttpGet]
        public async Task<ActionResult> GetUserTransactions(
            [FromQuery] int? month, int? year,
            [FromQuery] int? bucket,
            int pageNumber = 1, int pageSize = 3)
        {
            var id = _userManager.GetUserId(User);
            var transactions = await _expenseExpenseService.GetTransactions(id, month, year, bucket, pageNumber, pageSize);
            if (!transactions.IsSuccess)
            {
                // TODO: user transaction could not be found.
                return NotFound();
            }
            return Ok(transactions);

        }

        [HttpPost]
        public async Task<ActionResult> CreateTransaction([FromBody] RequestTransactionDto transaction)
        {
            var userId = _userManager.GetUserId(User);
            
            if (userId is null) return Unauthorized(); // TODO: UserId is null, log it.
            
            var transactionCreated = await _expenseExpenseService.CreateTransaction(userId,transaction);
            
            if (transactionCreated.Value == null) return BadRequest();
            return Ok(transactionCreated);
            


        }

        [HttpDelete("{transactionId:int}")]
        public async Task<ActionResult> DeleteTransaction(int transactionId)
        {
            
            var userId = _userManager.GetUserId(User);
            if (userId is null) return Unauthorized(); //TODO: UserId is null, log it.
            var isDeleted = await _expenseExpenseService.DeleteTransaction(userId, transactionId);
            if (!isDeleted.IsSuccess)
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateTransactionDto updatedTransaction)
        {
            var userId = _userManager.GetUserId(User);
            if (userId is null) return Unauthorized();
            
            var transaction = await _expenseExpenseService.UpdateTransaction(userId,id, updatedTransaction);
            if (transaction.Value != null)
            {
                return Ok(transaction);
            }
            return NotFound();
        }
    }
}