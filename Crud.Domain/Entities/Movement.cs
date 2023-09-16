using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisCanchas.Domain.Entities
{
    public class Movement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int MovementTypeId { get; set; }
        public virtual MovementType MovementType { get; set; }

    }
}
