using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MailKit;
using Microsoft.AspNetCore.Http;
using t4tea.data.Entities;
using t4tea.repository.Interfaces;
using t4tea.service.advertise.Dtos;
using t4tea.service.images.Dtos;
using t4tea.service.saveAndDeleteImage;

namespace t4tea.service.images
{
    public class ImagesServices : IImagesServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISaveAndDeleteImageService _imageService;


        public ImagesServices(IUnitOfWork unitOfWork, IMapper mapper, ISaveAndDeleteImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<ImageDto> AddProductImage(AddImage imageDto)
        {
            if (imageDto.ImagePath == null || imageDto.ImagePath.Length == 0)
                return null;

            string imageUrl = await _imageService.UploadToCloudinary(imageDto.ImagePath);

            var img = new Images
            {
                ImagePath = imageUrl,
                ProductId = imageDto.ProductId,
            };

            await _unitOfWork.Repository<Images>().AddAsync(img);
            var status = await _unitOfWork.CompleteAsync();

            if (status == 0)
                return null;

            return _mapper.Map<ImageDto>(img);
        }


        //public async Task<string> SaveImage(IFormFile image)
        //{
        //    if (image == null || image.Length == 0)
        //    {
        //        return null;
        //    }

        //    try
        //    {
        //        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
        //        if (!Directory.Exists(uploadsFolder))
        //        {
        //            Directory.CreateDirectory(uploadsFolder);
        //        }

        //        var uniqueFileName = $"{Guid.NewGuid()}_{image.FileName}";
        //        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await image.CopyToAsync(stream);
        //        }

        //        return $"/images/{uniqueFileName}"; // مسار الصورة لتخزينه في قاعدة البيانات
        //    }
        //    catch (Exception ex)
        //    {
        //        // يمكن تسجيل الخطأ هنا باستخدام Logger
        //        return null;
        //    }
        //}

        //public void DeleteImage(string imagePath)
        //{
        //    if (string.IsNullOrEmpty(imagePath))
        //    {
        //        return;
        //    }

        //    try
        //    {
        //        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));
        //        if (System.IO.File.Exists(fullPath))
        //        {
        //            System.IO.File.Delete(fullPath);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // يمكن تسجيل الخطأ هنا باستخدام Logger
        //    }
        //}







        public async Task<ImageDto> UpdateImage(int id, AddImage imageDto)
        {
            var img = await _unitOfWork.Repository<Images>().GetByIdAsync(id);
            if (img == null)
                return null;

            if (imageDto.ImagePath != null)
            {
                var publicId = _imageService.ExtractPublicIdFromUrl(img.ImagePath);
                _imageService.DeleteFromCloudinary(publicId); // حذف القديمة
                img.ImagePath = await _imageService.UploadToCloudinary(imageDto.ImagePath); // رفع الجديدة
            }

            _unitOfWork.Repository<Images>().Update(img);
            var status = await _unitOfWork.CompleteAsync();

            return status == 0 ? null : _mapper.Map<ImageDto>(img);
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
                return 0;

            if (!string.IsNullOrEmpty(img.ImagePath))
            {
                var publicId = _imageService.ExtractPublicIdFromUrl(img.ImagePath);
                _imageService.DeleteFromCloudinary(publicId);
            }

            _unitOfWork.Repository<Images>().Delete(img);
            return await _unitOfWork.CompleteAsync();
        }

    }
}
