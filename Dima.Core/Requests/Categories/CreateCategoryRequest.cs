using System.ComponentModel.DataAnnotations;
using Dima.Core.Requests;

public class CreateCategoryRequest : BaseRequest
{
    [Required(ErrorMessage = "Título é obrigatório.")]
    [MaxLength(80, ErrorMessage = "O título deve conter no máximo 80 caracteres.")]
    [MinLength(3, ErrorMessage = "O título deve conter pelo menos 3 caracteres.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Descricao é obrigatória.")]
    [MaxLength(200, ErrorMessage = "A descrição deve conter no máximo 200 caracteres.")]
    public string Description { get; set; } = string.Empty;
}