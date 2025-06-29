using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using t4tea.data.Context;
using t4tea.data.Entities;
using t4tea.repository.Interfaces;
using t4tea.service.category.Dtos;
using t4tea.service.saveAndDeleteImage;

namespace t4tea.service.category
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISaveAndDeleteImageService _imageService;

        public CategoryServices(IUnitOfWork unitOfWork, IMapper mapper, ISaveAndDeleteImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper; 
            _imageService = imageService;

        }


        public async Task<categoryDto> AddCategory(AddCategoryDto categoryDto, IFormFile image)
        {
            var imageUrl = image != null ? await _imageService.UploadToCloudinary(image) : null;

            var category = new Categories
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                ImageUrl = imageUrl
            };

            await _unitOfWork.Repository<Categories>().AddAsync(category);
            var status = await _unitOfWork.CompleteAsync();

            return status == 0 ? null : _mapper.Map<categoryDto>(category);
        }





        //public async Task<string> SaveImage(IFormFile image)
        //{
        //    if (image == null || image.Length == 0)
        //    {
        //        return null;
        //    }

        //    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
        //    if (!Directory.Exists(uploadsFolder))
        //    {
        //        Directory.CreateDirectory(uploadsFolder);
        //    }

        //    var uniqueFileName = $"{Guid.NewGuid()}_{image.FileName}";
        //    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await image.CopyToAsync(stream);
        //    }

        //    return $"/images/{uniqueFileName}"; // مسار الصورة لتخزينه في قاعدة البيانات
        //}




        //public void DeleteImage(string imagePath)
        //{
        //    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));

        //    if (System.IO.File.Exists(fullPath))
        //    {
        //        System.IO.File.Delete(fullPath);
        //    }
        //}


        public async Task<categoryDto> UpdateCategory(int id, AddCategoryDto categoryDto, IFormFile newImage)
        {
            var category = await _unitOfWork.Repository<Categories>().GetByIdAsync(id);
            if (category == null)
                return null;

            if (newImage != null)
            {
                // حذف القديمة
                if (!string.IsNullOrEmpty(category.ImageUrl))
                {
                    var publicId = _imageService.ExtractPublicIdFromUrl(category.ImageUrl);
                    _imageService.DeleteFromCloudinary(publicId);
                }

                category.ImageUrl = await _imageService.UploadToCloudinary(newImage);
            }

            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;

            _unitOfWork.Repository<Categories>().Update(category);
            var status = await _unitOfWork.CompleteAsync();

            return status == 0 ? null : _mapper.Map<categoryDto>(category);
        }




        public async Task<IReadOnlyList<categoryDto>> GetAllCategoriesWithInclude()
        {
            var category = await _unitOfWork.Repository<Categories>().GetAllCategoriesAsync();
            var mappedCategory = _mapper.Map<IReadOnlyList<categoryDto>>(category);
            return mappedCategory;
        }

        public async Task<categoryDto> GetCategoryByIdWithInclude(int id)
        {
            var category = await _unitOfWork.Repository<Categories>().GetCategoryByIdAsync(id);
            var mappedCategory = _mapper.Map<categoryDto>(category);
            return mappedCategory;
        }


        public async Task<IReadOnlyList<categoryDto>> GetAllCategories()
        {
            var category = await _unitOfWork.Repository<Categories>().GetAllAsync();
            var mappedCategory = _mapper.Map<IReadOnlyList<categoryDto>>(category);
            return mappedCategory;
        }


        public async Task<categoryDto> GetCategoryById(int id)
        {
            var category = await _unitOfWork.Repository<Categories>().GetByIdAsync(id);
            var mappedCategory = _mapper.Map<categoryDto>(category);
            return mappedCategory;
        }










        public async Task<int> DeleteCategory(int id)
        {
            var category = await _unitOfWork.Repository<Categories>().GetByIdAsync(id);
            if (category == null)
                return 0;

            if (!string.IsNullOrEmpty(category.ImageUrl))
            {
                var publicId = _imageService.ExtractPublicIdFromUrl(category.ImageUrl);
                _imageService.DeleteFromCloudinary(publicId);
            }

            _unitOfWork.Repository<Categories>().Delete(category);
            return await _unitOfWork.CompleteAsync();
        }


    }
}
