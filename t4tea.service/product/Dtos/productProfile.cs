using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using t4tea.data.Entities;
using t4tea.service.benifit.Dtos;
using t4tea.service.category.Dtos;

namespace t4tea.service.product.Dtos
{
    public class productProfile : Profile
    { 
        public productProfile()
        {
            CreateMap<Products, addProductDto>().ReverseMap();
            CreateMap<Products, ProductDto>().ReverseMap();
        }
    }
}
