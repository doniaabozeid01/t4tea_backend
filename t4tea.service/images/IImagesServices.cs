using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using t4tea.service.images.Dtos;

namespace t4tea.service.images
{
    public interface IImagesServices
    {
        Task<ImageDto> AddProductImage(AddImage imageDto);
        //Task<string> SaveImage(IFormFile image);
        //void DeleteImage(string imagePath);
        Task<ImageDto> UpdateImage(int id, AddImage imageDto);
        Task<IReadOnlyList<ImageDto>> GetAllProductsImages();
        Task<ImageDto> GetProductImageById(int id);
        Task<int> DeleteImage(int id);
    }
}
