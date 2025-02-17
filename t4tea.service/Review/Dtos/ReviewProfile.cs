using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using t4tea.data.Entities;

namespace t4tea.service.Review.Dtos
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<AddReviewDto, t4tea.data.Entities.Reviews>().ReverseMap();
            CreateMap<GetReviewDto, t4tea.data.Entities.Reviews>().ReverseMap();

            //CreateMap<ProductReviews, AddReviewDto>();
        }
    }
}
