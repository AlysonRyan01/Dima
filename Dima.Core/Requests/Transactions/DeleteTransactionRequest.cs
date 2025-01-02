using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Requests.Transactions;

public class DeleteTransactionRequest : BaseRequest
{
    [Required(ErrorMessage = "O Id deve ser informado")]
    public long Id { get; set; }
}