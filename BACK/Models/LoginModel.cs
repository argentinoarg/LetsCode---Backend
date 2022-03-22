using System.ComponentModel.DataAnnotations;

namespace BACK.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")] 
        public string Login { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")] 
        public string Password { get; set; }
    }
}
