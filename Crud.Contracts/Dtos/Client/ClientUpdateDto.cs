namespace MisCanchas.Contracts.Dtos.Client
{
    public class ClientUpdateDto
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientTelephone { get; set; }
        public int NationalIdentityDocument { get; set; }
    }
}
