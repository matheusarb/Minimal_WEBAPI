using Blog.Models;
using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels;

public class EditorCategoryViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(40, MinimumLength = 3, ErrorMessage ="Este campo deve conter no mínimo 3 e no máximo 40 caracteres")]
    public string Name { get; set; }
    [Required(ErrorMessage = "O slug é obrigatório")]
    public string Slug { get; set; }
}
