using AutoMapper;
using t4tea.data.Entities;

namespace t4tea.service.category.Dtos
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Categories, AddCategoryDto>().ReverseMap();
            CreateMap<Categories, categoryDto>().ReverseMap();
        }
    }
}
