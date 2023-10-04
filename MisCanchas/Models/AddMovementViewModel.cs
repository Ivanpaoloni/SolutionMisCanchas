using Microsoft.AspNetCore.Mvc.Rendering;
using MisCanchas.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace MisCanchas.Models
{
    public class AddMovementViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha y Hora")]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Monto a registrar")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Descripcion")]
        public string Description { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Tipo de movimiento")]
        public int MovementTypeId { get; set; }
        public IEnumerable<SelectListItem>? MovementTypes { get; set; }
    }
}
