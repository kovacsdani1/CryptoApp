using AutoMapper;
using CryptoApp.DTOs;
using CryptoApp.Entities;

namespace CryptoApp.AutoMapperProfiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, TransactionDto>();
            CreateMap<Transaction, TransactionDetailsDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username));
        }
    }
}
