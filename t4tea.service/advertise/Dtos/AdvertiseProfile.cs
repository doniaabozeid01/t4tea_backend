using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using t4tea.data.Entities;
using t4tea.service.benifit.Dtos;

namespace t4tea.service.advertise.Dtos
{
    public class AdvertiseProfile : Profile
    {
        public AdvertiseProfile()
        {
            CreateMap<Advertise, addAdvertise>().ReverseMap();
            CreateMap<Advertise, AdvertiseDto>().ReverseMap();
        }

    }
}
