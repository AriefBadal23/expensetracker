using expensetrackerapi.DTO;
using expensetrackerapi.Models;
using Microsoft.AspNetCore.Mvc;
using expensetrackerapi.Services;

namespace expensetrackerapi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class Transactions : Controller
    {

        private readonly IExpenseService _expenseService;

        public Transactions(IExpenseService service)
        {
            _expenseService = service;
        }

        [HttpGet("details")]
        public ActionResult GetTransactionById([FromQuery] int id)
        {
            var transaction = _expenseService.GetTransactionByID(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }
        
        
        [HttpGet]
        public ActionResult<IExpenseService> Get(
            [FromQuery] int? month, int? year,
            [FromQuery] int? bucket,
            int pageNumber = 1, int pageSize = 3)
        {
            var transactions = _expenseService.GetTransactions(month, year, bucket, pageNumber, pageSize);
            if (transactions is null)
            {
                return NotFound();
            }
            return Ok(transactions);

        }

        [HttpPost]
        public ActionResult Post([FromBody] RequestTransactionDto transaction)
        {

            var transactionCreated = _expenseService.CreateTransaction(transaction);
            if (transactionCreated == null) return BadRequest();
            return Ok(transactionCreated);

        }

        [HttpDelete("{transactionID}")]
        public ActionResult Delete(int transactionID)
        {
            var isDeleted = _expenseService.DeleteTransaction(transactionID);
            if (isDeleted)
            {
                return Ok();
            }
            return NotFound();
        }
        
        [HttpPut]
        public ActionResult Update([FromBody]Transaction updatedTransaction)
        {
            var transaction = _expenseService.UpdateTransaction(updatedTransaction);
            if (transaction != null)
            {
                return Ok(transaction);
            }
            return NotFound();
        }
    }
}