using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using t4tea.data.Entities;

namespace t4tea.service.Order.Dtos
{
    public class OrderProfile : Profile
    {

        public OrderProfile()
        {
            CreateMap<addOrder, OrderRequest>().ReverseMap();


            CreateMap<GetOrderRequest, OrderRequest>().ReverseMap();


            CreateMap<OrderItem, GetOrderItems>().ReverseMap();
        }
    }

}

