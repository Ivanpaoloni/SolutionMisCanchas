using System.ComponentModel.DataAnnotations;

namespace MisCanchas.Domain.Entities
{
    public class Movement
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int MovementTypeId { get; set; }
        [Required]
        public virtual MovementType MovementType { get; set; }
        public decimal CurrentBalance { get; set; }

    }
}
