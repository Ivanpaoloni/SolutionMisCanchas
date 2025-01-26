using MisCanchas.Contracts.Dtos.Turn;

namespace MisCanchas.Contracts.Dtos.Client
{
    public class ClientDto
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientTelephone { get; set; }
        public int NationalIdentityDocument { get; set; }
        public ICollection<TurnDto> Turns { get; set; }
    }
}
