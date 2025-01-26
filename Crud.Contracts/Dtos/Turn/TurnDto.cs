using MisCanchas.Contracts.Dtos.Client;

namespace MisCanchas.Contracts.Dtos.Turn
{
    public class TurnDto
    {
        public int TurnId { get; set; }
        public DateTime TurnDateTime { get; set; }
        public int ClientId { get; set; }
        public decimal Price { get; set; }
        public bool Paid { get; set; }
        public ClientDto Client { get; set; }
    }
}
