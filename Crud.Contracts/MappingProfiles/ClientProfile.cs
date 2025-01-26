using AutoMapper;
using MisCanchas.Contracts.Dtos.Client;
using MisCanchas.Domain.Entities;

namespace MisCanchas.Contracts.MappingProfiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, ClientDto>().ReverseMap();

            CreateMap<Client, ClientCreateDto>().ReverseMap();
            CreateMap<Client, ClientUpdateDto>().ReverseMap();
        }
    }
}
