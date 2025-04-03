using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MisCanchas.Domain.Entities
{
    public class Turn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TurnId { get; set; }
        public DateTime TurnDateTime { get; set; }
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        public decimal Price { get; set; }
        public bool Paid { get; set; }
    }
}
