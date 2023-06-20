using System.ComponentModel.DataAnnotations;

namespace MisCanchas.Models
{
    public class AdminFieldViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Horario de apertura")]
        [Range(0, 24, ErrorMessage = "El valor ingresado debe ser una hora valida")]
        public int OpenHour { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Horario de cierre")]
        [Range(0,24, ErrorMessage ="El valor ingresado debe ser una hora valida")]
        public int CloseHour { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nombre de la cancha")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Precio")]
        public decimal Price { get; set; }
    }
}
