using AutoMapper;
using CS_APIServerProject.DTO;
using CS_APIServerProject.Models;
namespace CS_APIServerProject.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Product, ProductCreateDTO>();
            CreateMap<Product, CharacteristicsDTO>();

            CreateMap<ProductCreateDTO, Product>().ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Characteristics,
                opt => opt.MapFrom(s => s.Characteristics ?? new CharacteristicsDTO()));

            CreateMap<ProductUpdateDTO, Product>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Characteristics,
                opt => opt.MapFrom(s => s.Characteristics ?? new CharacteristicsDTO()));

            //For User
            CreateMap<CharacteristicsDTO, Characteristics>();
            CreateMap<User,  UserCreateDTO>();

            //Tying user Create DTO method with User
            CreateMap<UserCreateDTO, User>()
                .ForMember(d => d.Id, opt => opt.Ignore());

            //Tying user Update DTO method with User
            CreateMap<UserUpdateDTO, User>()
                .ForMember(d => d.Id, opt => opt.Ignore());

            //For Order
            CreateMap<Order, OrderCreateDTO>();
            CreateMap<Order, Product>();

            //Tying order Create DTO method with Order and user 
            CreateMap<OrderCreateDTO, Order>()
                .ForMember(d => d.Id,
                opt => opt.MapFrom(s => s.FK_Products ?? new List<Product>()));

            //Tying order Update DTO method with Order and user
            CreateMap<OrderUpdateDTO, Order>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.FK_Products, 
                opt => opt.MapFrom(s => s.FK_Products ?? new List<Product>()));




        }


    }
}
