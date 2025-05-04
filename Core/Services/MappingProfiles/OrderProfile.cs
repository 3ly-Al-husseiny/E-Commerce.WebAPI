using AutoMapper;
using Domain.Models.Orders;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderResultDto>()
                .ForMember(d => d.PaymentStatus, O => O.MapFrom(S => S.PaymentStatus.ToString()))
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ToString()))
                .ForMember(d => d.Total, O => O.MapFrom(S => S.SubTotal + S.DeliveryMethod.Cost));


            CreateMap<DeliveryMethod, DeliveryMethodDto>();
            
            
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, O => O.MapFrom(S => S.ProductInOrderItem.ProductId))
                .ForMember(d => d.ProductName, O => O.MapFrom(S => S.ProductInOrderItem.ProductName))
                .ForMember(d => d.PictureUrl, O => O.MapFrom(S => S.ProductInOrderItem.PictureUrl))
                ;

            CreateMap<Address , AddressDto>().ReverseMap();
                
        }
    }
}
