using System.ComponentModel.DataAnnotations;

namespace MisCanchas.Models
{
    public class RegistViewModel
    {
        [Display(Name = "Nombre de usuario")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public required string UserName { get; set; }

        [Display(Name = "Correo electronico")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage ="El campo debe ser un correo electronico valido")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
