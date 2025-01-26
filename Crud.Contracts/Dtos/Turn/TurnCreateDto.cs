using MisCanchas.Contracts.Dtos.Client;

namespace MisCanchas.Contracts.Dtos.Turn
{
    public class TurnCreateDto
    {
        public DateTime TurnDateTime { get; set; }
        public int ClientId { get; set; }
        public decimal Price { get; set; }
        public bool Paid { get; set; }
    }
}
