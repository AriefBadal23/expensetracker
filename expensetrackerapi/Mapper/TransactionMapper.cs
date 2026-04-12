using expensetrackerapi.DTO;
using expensetrackerapi.Models;
using Riok.Mapperly.Abstractions;

namespace expensetrackerapi.Mapper;

[Mapper]
public partial class TransactionMapper
{
    // Security: Avoiding having the UserID in the requestDTO or responseDTO.
    [MapperIgnoreTarget(nameof(Transaction.Id))]
     [MapperIgnoreTarget(nameof(Transaction.ApplicationUserId))]
    public partial Transaction TransactionDtoToRequestTransaction(RequestTransactionDto dto);
    
    [MapperIgnoreSource(nameof(Transaction.ApplicationUserId))]
    public partial ResponseTransactionDTo TransactionToResponseTransaction(Transaction? transaction);
}