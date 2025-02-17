using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using t4tea.data.Entities;
using t4tea.service.advertise.Dtos;

namespace t4tea.service.images.Dtos
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<Images, AddImage>().ReverseMap();
            CreateMap<Images, ImageDto>().ReverseMap();
        }
    }
}
