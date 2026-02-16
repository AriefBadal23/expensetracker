using expensetrackerapi.Models;
using Microsoft.AspNetCore.Mvc;
using expensetrackerapi.Services;

namespace expensetrackerapi.Controllers
{
    [Route("api/v1/[controller]")]
    public class Transactions : Controller
    {
        private readonly ILogger<Transactions> _logger;
        private readonly ExpenseTrackerContext _db;

        private readonly IExpenseService _expenseService;

        public Transactions(ILogger<Transactions> logger, ExpenseTrackerContext context, IExpenseService service)
        {
            _logger = logger;
            _db = context;
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
        public ActionResult Post([FromBody] CreateTransactionDto dto)
        {

            var TransactionCreated = _expenseService.CreateTransaction(dto);
            if (!TransactionCreated) return BadRequest();
            return Created();

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
        public ActionResult Update([FromBody]Transaction UpdatedTransaction)
        {
            var transaction = _expenseService.UpdateTransaction(UpdatedTransaction);
            if (transaction != null)
            {
                return Ok(transaction);
            }
            return NotFound();
        }
    }
}