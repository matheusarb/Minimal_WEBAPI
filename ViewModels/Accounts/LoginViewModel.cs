using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Accounts;

public class LoginViewModel
{
    [Required(ErrorMessage = "O e-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "Endereço de e-mail inválido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória")]
    public string Password { get; set; }
}
