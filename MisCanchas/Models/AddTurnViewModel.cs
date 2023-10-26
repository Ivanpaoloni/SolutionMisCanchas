using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MisCanchas.Models
{
    public class AddTurnViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha y Hora")]
        [DataType(DataType.DateTime)]
        public DateTime TurnDateTime { get; set; } = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH tt"));
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Cliente")]
        public int ClientId { get; set; }
        public IEnumerable<SelectListItem>? Clients { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Precio")]
        public decimal Price { get; set; }
        public bool Paid { get; set; }
    }
}
