using System.Security.Principal;

namespace MisCanchas.Domain.Entities
{
    public class Field
    {
        public int Id { get; set; }
        public int OpenHour { get; set; }
        public int CloseHour { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
