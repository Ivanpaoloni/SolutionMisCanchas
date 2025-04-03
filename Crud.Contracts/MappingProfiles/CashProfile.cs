using AutoMapper;
using MisCanchas.Contracts.Dtos.Cash;
using MisCanchas.Domain.Entities;

namespace MisCanchas.Contracts.MappingProfiles
{
    public class CashProfile : Profile
    {
        public CashProfile()
        {
            CreateMap<Cash, CashDto>().ReverseMap();

            CreateMap<Cash, CashUpdateDto>().ReverseMap();
            CreateMap<CashDto, CashUpdateDto>().ReverseMap();
        }
    }
}
