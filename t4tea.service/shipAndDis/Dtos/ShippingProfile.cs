using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using t4tea.data.Entities;
using t4tea.service.Review.Dtos;

namespace t4tea.service.shipAndDis.Dtos
{
    public class ShippingProfile : Profile
    {
        public ShippingProfile()
        {
            CreateMap<AddShippingAndDiscountDto, ShippingAndDiscount>().ReverseMap();
            CreateMap<GetShippingAndDiscountDto, ShippingAndDiscount>().ReverseMap();
        }
    }
}
