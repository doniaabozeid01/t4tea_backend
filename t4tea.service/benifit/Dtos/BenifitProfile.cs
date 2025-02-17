using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using t4tea.data.Entities;
using t4tea.service.category.Dtos;

namespace t4tea.service.benifit.Dtos
{
    public class BenifitProfile : Profile
    {
        public BenifitProfile()
        {
            CreateMap<Benifits, AddBenifits>().ReverseMap();
            CreateMap<Benifits, BenifitDto>().ReverseMap();
        }
    }
}
