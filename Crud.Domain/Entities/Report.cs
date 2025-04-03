
namespace MisCanchas.Domain.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public decimal In { get; set; }
        public decimal Out { get; set; }
        public int Canceled { get; set; }
        public int Booking { get; set; }

    }
}
