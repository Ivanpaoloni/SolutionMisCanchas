using System.ComponentModel.DataAnnotations;

namespace MisCanchas.Models
{
    public class AddClientViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(maximumLength: 50)]
        [Display(Name = "Nombre")]
        public string ClientName { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Email")]
        public string ClientEmail { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Telefono")]
        public string ClientTelephone { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nro de Documento")]
        public int NationalIdentityDocument { get; set; }
        public string UrlRetorno { get; set; }
    }
}
