using AutoMapper;
using CryptoApp.DTOs;
using CryptoApp.Entities;

namespace CryptoApp.AutoMapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRegisterDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<UserUpdateDto, User>();
        }
    }
}
