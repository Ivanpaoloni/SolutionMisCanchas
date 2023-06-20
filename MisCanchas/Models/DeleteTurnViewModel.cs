using System.ComponentModel.DataAnnotations;

namespace MisCanchas.Models
{
    public class DeleteTurnViewModel
    {
        [Display(Name = "Id")]
        public int TurnId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha y Hora")]
        [DataType(DataType.DateTime)]
        public DateTime TurnDateTime { get; set; } = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH tt"));
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Cliente")]
        public int ClientId { get; set; }
        public decimal Price { get; set; }
    }
}
