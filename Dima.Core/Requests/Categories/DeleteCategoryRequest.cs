using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Requests.Categories;

public class DeleteCategoryRequest : BaseRequest
{
    [Required(ErrorMessage = "O ID e obrigatorio")]
    public long Id { get; set; }
}