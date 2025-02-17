using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using t4tea.data.Entities;

namespace t4tea.service.Cart.Dtos
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CartItems, AddCart>().ReverseMap();

            CreateMap<CartItems, GetCart>().ReverseMap();
        }
    }
}
