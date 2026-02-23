using expensetrackerapi.DTO;
using expensetrackerapi.Models;
using Riok.Mapperly.Abstractions;

namespace expensetrackerapi.Mapper;

[Mapper]
public partial class TransactionMapper
{
    [MapperIgnoreTarget(nameof(Transaction.Id))]
    public partial Transaction TransactionDtoToRequestTransaction(RequestTransactionDto dto);
    
    public partial ResponseTransactionDTo TransactionToResponseTransaction(Transaction? transaction);
}