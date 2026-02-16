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

            //For Characteristics
            CreateMap<CharacteristicsDTO, Characteristics>();
            
            //For User
            CreateMap<User,  UserCreateDTO>();

            //Tying user Create DTO method with User
            CreateMap<UserCreateDTO, User>()
                .ForMember(d => d.Id, opt => opt.Ignore());

            //Tying user Update DTO method with User
            CreateMap<UserUpdateDTO, User>()
                .ForMember(d => d.Id, opt => opt.Ignore());

            //For Order
            CreateMap<Order, OrderCreateDTO>();
            CreateMap<Order, OrderItem>();

            //Tying order Create DTO method with Order and user 
            CreateMap<OrderCreateDTO, Order>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Items,
                opt => opt.MapFrom(s => s.Items ?? new List<OrderItemCreateDTO>()));

            //Tying order Update DTO method with Order and user
            CreateMap<OrderUpdateDTO, Order>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Items, 
                opt => opt.MapFrom(s => s.Items ?? new List<OrderItemUpdateDTO>()));

            //For OrderItem

            CreateMap<OrderItem, OrderItemCreateDTO>();
            CreateMap<OrderItem, Product>();
            //CreateMap<OrderItem, Order>();

            CreateMap<OrderItemCreateDTO, OrderItem>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Product,
                opt => opt.MapFrom(s => s.Product ?? new ProductCreateDTO()))
                .ForMember(d => d.Order,
                opt => opt.MapFrom(s => s.Order ?? new OrderCreateDTO()));

            CreateMap<OrderItemUpdateDTO, OrderItem>()
               .ForMember(d => d.Id, opt => opt.Ignore())
               .ForMember(d => d.Product,
               opt => opt.MapFrom(s => s.Product ?? new ProductUpdateDTO()))
               .ForMember(d => d.Order,
               opt => opt.MapFrom(s => s.Order ?? new OrderUpdateDTO()));
        }


    }
}
