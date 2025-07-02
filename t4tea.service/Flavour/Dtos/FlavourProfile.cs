using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using t4tea.data.Entities;
using t4tea.service.Review.Dtos;

namespace t4tea.service.Flavour.Dtos
{
    public class FlavourProfile : Profile
    {

        public FlavourProfile()
        {
            CreateMap<addFlavourDto, Flavours>().ReverseMap();
            CreateMap<GetFlavourDto, Flavours>().ReverseMap();
        }

    }
}
