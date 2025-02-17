using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using t4tea.data.Entities;
using t4tea.repository.Interfaces;
using t4tea.service.advertise.Dtos;
using t4tea.service.images.Dtos;

namespace t4tea.service.images
{
    public class ImagesServices : IImagesServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ImagesServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ImageDto> AddProductImage(AddImage imageDto)
        {
            if (imageDto.ImagePath == null || imageDto.ImagePath.Length == 0)
            {
                return null; // التأكد من أن الصورة موجودة
            }

            string imagePath = await SaveImage(imageDto.ImagePath); // حفظ الصورة واسترجاع مسارها

            var img = new Images
            {
                ImagePath = imagePath, // تخزين المسار فقط
                ProductId = imageDto.ProductId,
            };

            await _unitOfWork.Repository<Images>().AddAsync(img);
            var status = await _unitOfWork.CompleteAsync();

            if (status == 0)
            {
                return null;
            }

            return new ImageDto
            {
                Id = img.Id,
                ImagePath = img.ImagePath,
                ProductId = img.ProductId,
            };
        }


        public async Task<string> SaveImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return null;
            }

            try
            {
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
            catch (Exception ex)
            {
                // يمكن تسجيل الخطأ هنا باستخدام Logger
                return null;
            }
        }

        public void DeleteImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return;
            }

            try
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            catch (Exception ex)
            {
                // يمكن تسجيل الخطأ هنا باستخدام Logger
            }
        }

        public async Task<ImageDto> UpdateImage(int id, AddImage imageDto)
        {
            var img = await _unitOfWork.Repository<Images>().GetByIdAsync(id);

            if (img == null)
            {
                return null;
            }

            // إذا كان هناك صورة جديدة، احذف القديمة ثم احفظ الجديدة
            if (imageDto.ImagePath != null)
            {
                DeleteImage(img.ImagePath); // حذف الصورة القديمة
                img.ImagePath = await SaveImage(imageDto.ImagePath); // حفظ الصورة الجديدة
            }

            _unitOfWork.Repository<Images>().Update(img);
            var status = await _unitOfWork.CompleteAsync();

            if (status == 0)
            {
                return null;
            }

            return new ImageDto
            {
                Id = img.Id,
                ImagePath = img.ImagePath,
                ProductId = img.ProductId,
            };
        }


        public async Task<IReadOnlyList<ImageDto>> GetAllProductsImages()
        {
            var imgs = await _unitOfWork.Repository<Images>().GetAllAsync();
            return _mapper.Map<IReadOnlyList<ImageDto>>(imgs);
        }

        public async Task<ImageDto> GetProductImageById(int id)
        {
            var img = await _unitOfWork.Repository<Images>().GetByIdAsync(id);
            return _mapper.Map<ImageDto>(img);
        }

        public async Task<int> DeleteImage(int id)
        {
            var img = await _unitOfWork.Repository<Images>().GetByIdAsync(id);

            if (img == null)
            {
                return 0;
            }

            // حذف الصورة قبل حذف الإعلان
            DeleteImage(img.ImagePath);

            _unitOfWork.Repository<Images>().Delete(img);
            return await _unitOfWork.CompleteAsync();
        }
    }
}
