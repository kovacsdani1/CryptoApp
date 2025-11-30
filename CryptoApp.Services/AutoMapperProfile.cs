using AutoMapper;
using CryptoApp.DataContext.Dtos;
using CryptoApp.DataContext.Entities;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // User mappings
        CreateMap<User, UserDto>();
        CreateMap<UserRegisterDto, User>();

        // Wallet mappings
        CreateMap<Wallet, WalletDto>();

        // Crypto holdings mappings
        CreateMap<CryptoHolding, CryptoHoldingDto>()
            .ForMember(dest => dest.CryptoName, opt => opt.MapFrom(src => src.Cryptocurrency.Name))
            .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.Cryptocurrency.Symbol))
            .ForMember(dest => dest.CurrentPrice, opt => opt.MapFrom(src => src.Cryptocurrency.CurrentPrice));

        // Cryptocurrency mappings
        CreateMap<Cryptocurrency, CryptocurrencyDto>().ReverseMap();

        // Transaction mappings
        CreateMap<Transaction, TransactionDto>()
            .ForMember(dest => dest.CryptoName, opt => opt.MapFrom(src => src.Cryptocurrency.Name))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
    }
}