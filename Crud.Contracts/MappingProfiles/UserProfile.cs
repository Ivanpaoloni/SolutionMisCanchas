using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MisCanchas.Contracts.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisCanchas.Contracts.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<IdentityUser, UserDto>().ReverseMap();
            CreateMap<IdentityUser, UserCreateDto>().ReverseMap();
        }
    }
}
