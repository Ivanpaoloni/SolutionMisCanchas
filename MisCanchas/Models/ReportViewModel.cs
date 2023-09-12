namespace MisCanchas.Models
{
    public class ReportViewModel
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public decimal Amount { get; set; }
        public decimal In { get; set; }
        public decimal Out { get; set; }
        public int Canceled { get; set; }
        public int Booking { get; set; }
    }
}
