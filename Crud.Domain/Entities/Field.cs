using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace MisCanchas.Domain.Entities
{
    public class Field
    {
        public int Id { get; set; }
        [Required]
        public int OpenHour { get; set; }
        [Required]
        public int CloseHour { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
