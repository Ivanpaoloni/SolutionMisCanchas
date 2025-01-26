namespace MisCanchas.Contracts.Dtos.Client
{
    public class ClientCreateDto
    {
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientTelephone { get; set; }
        public int NationalIdentityDocument { get; set; }
    }
}
