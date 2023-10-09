using AutoMapper;
using MeBay.Models;

namespace MeBay.Services
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<User, UserResponseDto>().ReverseMap();
            CreateMap<User, UserRegisterDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
            CreateMap<User, UserItemDto>().ReverseMap();
            CreateMap<User, UserLoginResponseDto>().ReverseMap();
            CreateMap<Item, ItemDto>().ReverseMap();
            CreateMap<Item, ItemSellDto>().ReverseMap();
        }
    }
}
