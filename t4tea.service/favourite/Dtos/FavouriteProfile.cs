using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using t4tea.data.Entities;
using t4tea.service.category.Dtos;

namespace t4tea.service.favourite.Dtos
{
    public class FavouriteProfile : Profile
    {
        public FavouriteProfile()
        {
            CreateMap<FavouriteProducts, AddFavouriteDto>().ReverseMap();
            CreateMap<FavouriteProducts, FavouriteDto>().ReverseMap();
        }
    }
}
