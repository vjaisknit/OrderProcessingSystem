using AutoMapper;
using Domain.Data;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using ViewModel.Auth;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterVM, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username));

            // Domain → ViewModel
            CreateMap<Order, OrderVM>()
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => Enum.Parse<PaymentStatus>(src.PaymentStatus, true)))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => Enum.Parse<PaymentMethod>(src.PaymentMethod, true)))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => Enum.Parse<OrderStatus>(src.OrderStatus, true)))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<OrderItem, OrderItemVM>();

            // ViewModel → Domain
            CreateMap<OrderVM, Order>()
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString()))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus.ToString()))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<OrderItemVM, OrderItem>();
        }
    }
}
