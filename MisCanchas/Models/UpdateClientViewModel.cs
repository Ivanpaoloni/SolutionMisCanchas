using System.ComponentModel.DataAnnotations;

namespace MisCanchas.Models
{
    public class UpdateClientViewModel
    {
        public required int ClientId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(maximumLength: 50)]
        [Display(Name = "Nombre")]
        public required string ClientName { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Email")]
        public required string ClientEmail { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Telefono")]
        public required string ClientTelephone { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nro de Documento")]
        public required int NationalIdentityDocument { get; set; }
    }
}
