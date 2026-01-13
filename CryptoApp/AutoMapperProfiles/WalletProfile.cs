using AutoMapper;
using CryptoApp.DTOs;
using CryptoApp.Entities;

namespace CryptoApp.AutoMapperProfiles
{
    public class WalletProfile : Profile
    {
        public WalletProfile()
        {
            CreateMap<Wallet, WalletDto>();
            CreateMap<Portfolio, PortfolioDto>();
        }
    }
}
