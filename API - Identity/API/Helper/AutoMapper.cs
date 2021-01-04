using API.Dtos;
using API.Identity;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebAPI.Identity.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserIdentity, UserDto>().ReverseMap();
            CreateMap<UserIdentity, UserLoginDto>().ReverseMap();
        }
    }
}