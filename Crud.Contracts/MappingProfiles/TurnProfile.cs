using AutoMapper;
using MisCanchas.Contracts.Dtos.Turn;
using MisCanchas.Domain.Entities;

namespace MisCanchas.Contracts.MappingProfiles
{
    public class TurnProfile : Profile
    {
        public TurnProfile()
        {
            CreateMap<Turn, TurnDto>().ReverseMap();

            CreateMap<Turn, TurnCreateDto>().ReverseMap();
            CreateMap<Turn, TurnUpdateDto>().ReverseMap();
        }
    }
}
