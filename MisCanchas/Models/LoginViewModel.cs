using System.ComponentModel.DataAnnotations;

namespace MisCanchas.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name ="UserName")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        [Display(Name = "Recuerdame")]
        public bool RememberMe { get; set; }
    }
}
