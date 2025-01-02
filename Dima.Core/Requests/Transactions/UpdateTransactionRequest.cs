using System.ComponentModel.DataAnnotations;
using Dima.Core.Enums;

namespace Dima.Core.Requests.Transactions;

public class UpdateTransactionRequest
{
    
    public long Id { get; set; }
    
    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "O tipo da transação é obrigatório.")]
    public ETransactionType Type { get; set; } = ETransactionType.Withdraw;

    [Required(ErrorMessage = "O valor é obrigatório.")]
    [Range(0.0, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "O ID da categoria é obrigatório.")]
    [Range(1, long.MaxValue, ErrorMessage = "O ID da categoria deve ser maior que zero.")]
    public long CategoryId { get; set; }

    [DataType(DataType.Date, ErrorMessage = "A data informada não é válida.")]
    public DateTime? PaidOrReceivedAt { get; set; }
}