namespace MisCanchas.Contracts.Dtos.Turn
{
    public class TurnUpdateDto
    {
        public int TurnId { get; set; }
        public DateTime TurnDateTime { get; set; }
        public int ClientId { get; set; }
        public decimal Price { get; set; }
        public bool Paid { get; set; }
    }
}
