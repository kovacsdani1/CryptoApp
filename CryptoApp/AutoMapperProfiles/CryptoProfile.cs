using AutoMapper;
using CryptoApp.DTOs;
using CryptoApp.Entities;

namespace CryptoApp.AutoMapperProfiles
{
    public class CryptoProfile : Profile
    {
        public CryptoProfile()
        {
            CreateMap<Crypto, CryptoDto>();
            CreateMap<CryptoDto, Crypto>();
            CreateMap<PostCryptoDto, Crypto>();
            CreateMap<PriceHistory, PriceHistoryDto>();
        }
    }
}
