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

namespace t4tea.service.category
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //public async Task<categoryDto> AddCategory(AddCategoryDto addCategory)
        //{
        //    if (addCategory != null)
        //    {
        //        var category = _mapper.Map<Categories>(addCategory);


        //        await _unitOfWork.Repository<Categories>().AddAsync(category);

        //        var status = await _unitOfWork.CompleteAsync();

        //        if (status == 0)
        //        {
        //            return null;
        //        }

        //        var productId = category.Id;

        //        return new categoryDto
        //        {
        //            Id = productId,
        //            Name = addCategory.Name,
        //            Description = addCategory.Description,
        //        };
        //    }

        //    return null; 
        //}


















        public async Task<categoryDto> AddCategory(AddCategoryDto categoryDto, string imagePath)
        {
            var category = new Categories
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                ImageUrl = imagePath // حفظ المسار فقط
            };

            await _unitOfWork.Repository<Categories>().AddAsync(category);
            var status = await _unitOfWork.CompleteAsync();

            if (status == 0)
            {
                return null;
            }

            return new categoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl
            };
        }





        public async Task<string> SaveImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return null;
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{image.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/images/{uniqueFileName}"; // مسار الصورة لتخزينه في قاعدة البيانات
        }




        public void DeleteImage(string imagePath)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }



        //public async Task<categoryDto> UpdateCategory(int id, AddCategoryDto addCategory)
        //{
        //    var category = await _unitOfWork.Repository<Categories>().GetByIdAsync(id);

        //    if (category != null)
        //    {
        //        // استخدام AutoMapper لتحويل الـ DTO إلى الكائن الفعلي (ElsaeedTeaProduct)
        //        var mappedCategory = _mapper.Map<Categories>(addCategory);

        //        // تحديث الكائن بالبيانات الجديدة
        //        category.Name = mappedCategory.Name;
        //        category.Description = mappedCategory.Description;
        //        //productTea.Price = mappedTea.Price;
        //        //productTea.Weight = mappedTea.Weight;

        //        // تحديث الكائن في الـ Repository
        //        _unitOfWork.Repository<Categories>().Update(category);

        //        // حفظ التغييرات في قاعدة البيانات
        //        var status = await _unitOfWork.CompleteAsync();

        //        if (status == 0)
        //        {
        //            return null;
        //        }

        //        var mapToDto = new categoryDto()
        //        {
        //            Id = id,
        //            Name = mappedCategory.Name,
        //            Description = mappedCategory.Description,
        //            //Price = mappedTea.Price,
        //            //Weight = mappedTea.Weight,
        //            //Images = productTea.Images,
        //        };

        //        return mapToDto;
        //    }

        //    //else
        //    //{
        //    //    // إذا لم يتم العثور على المنتج في قاعدة البيانات
        //    //    throw new Exception("Tea product not found.");
        //    //}
        //    return null;
        //}























        public async Task<categoryDto> UpdateCategory(int id, AddCategoryDto categoryDto, string imagePath)
        {
            var category = await _unitOfWork.Repository<Categories>().GetByIdAsync(id);

            if (category == null)
            {
                return null;
            }

            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;
            category.ImageUrl = imagePath; // تحديث الصورة فقط إذا تم تغييرها

            _unitOfWork.Repository<Categories>().Update(category);
            var status = await _unitOfWork.CompleteAsync();

            if (status == 0)
            {
                return null;
            }

            return new categoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl
            };
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



        //public async Task<int> DeleteCategory(int id)
        //{
        //    var category = await _unitOfWork.Repository<Categories>().GetByIdAsync(id);
        //    if (category != null)
        //    {
        //        _unitOfWork.Repository<Categories>().Delete(category);
        //        var status = await _unitOfWork.CompleteAsync();

        //        if (status == 0)
        //        {
        //            return 0;
        //        }
        //        return status;
        //    }
        //    return 0;
        //}

















        public async Task<int> DeleteCategory(int id)
        {
            var category = await _unitOfWork.Repository<Categories>().GetByIdAsync(id);

            if (category == null)
            {
                return 0;
            }

            _unitOfWork.Repository<Categories>().Delete(category);
            return await _unitOfWork.CompleteAsync();
        }

    }
}
