using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using t4tea.service.category.Dtos;

namespace t4tea.service.category
{
    public interface ICategoryServices
    {
        //Task<categoryDto> AddCategory(AddCategoryDto addCategory);
        Task<categoryDto> AddCategory(AddCategoryDto categoryDto, IFormFile image);
        //Task<string> SaveImage(IFormFile image);
        //void DeleteImage(string imagePath);
        //Task<categoryDto> UpdateCategory(int id, AddCategoryDto addCategory);
        Task<categoryDto> UpdateCategory(int id, AddCategoryDto categoryDto, IFormFile newImage);

        Task<categoryDto> GetCategoryById(int id);
        Task<IReadOnlyList<categoryDto>> GetAllCategories();
        //Task<int> DeleteCategory(int id);
        Task<int> DeleteCategory(int id);
        Task<categoryDto> GetCategoryByIdWithInclude(int id);
        Task<IReadOnlyList<categoryDto>> GetAllCategoriesWithInclude();



    }
}
