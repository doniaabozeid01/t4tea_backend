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
using t4tea.service.category.Dtos;
using t4tea.service.saveAndDeleteImage;

namespace t4tea.service.advertise
{
    public class AdvertiseServices : IAdvertiseServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISaveAndDeleteImageService _imageService;

        public AdvertiseServices(IUnitOfWork unitOfWork, IMapper mapper, ISaveAndDeleteImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;

        }

        public async Task<AdvertiseDto> AddAdvertise(addAdvertise advertiseDto)
        {
            if (advertiseDto.ImagePath == null || advertiseDto.ImagePath.Length == 0)
            {
                return null;
            }

            string imageUrl = await _imageService.UploadToCloudinary(advertiseDto.ImagePath);

            var advert = new Advertise
            {
                ImagePath = imageUrl
            };

            await _unitOfWork.Repository<Advertise>().AddAsync(advert);
            var status = await _unitOfWork.CompleteAsync();

            if (status == 0)
            {
                return null;
            }

            return new AdvertiseDto
            {
                Id = advert.Id,
                ImagePath = advert.ImagePath
            };
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

        public async Task<AdvertiseDto> UpdateAdvertise(int id, addAdvertise advertiseDto)
        {
            var advert = await _unitOfWork.Repository<Advertise>().GetByIdAsync(id);
            if (advert == null)
            {
                return null;
            }

            if (advertiseDto.ImagePath != null)
            {
                // حذف القديمة من Cloudinary
                if (!string.IsNullOrEmpty(advert.ImagePath))
                {
                    var publicId = _imageService.ExtractPublicIdFromUrl(advert.ImagePath);
                    _imageService.DeleteFromCloudinary(publicId);
                }

                // رفع الجديدة
                advert.ImagePath = await _imageService.UploadToCloudinary(advertiseDto.ImagePath);
            }

            _unitOfWork.Repository<Advertise>().Update(advert);
            var status = await _unitOfWork.CompleteAsync();

            if (status == 0)
            {
                return null;
            }

            return new AdvertiseDto
            {
                Id = advert.Id,
                ImagePath = advert.ImagePath
            };
        }


        public async Task<IReadOnlyList<AdvertiseDto>> GetAllAdvertises()
        {
            var adverts = await _unitOfWork.Repository<Advertise>().GetAllAsync();
            return _mapper.Map<IReadOnlyList<AdvertiseDto>>(adverts);
        }

        public async Task<AdvertiseDto> GetAdvertiseById(int id)
        {
            var advert = await _unitOfWork.Repository<Advertise>().GetByIdAsync(id);
            return _mapper.Map<AdvertiseDto>(advert);
        }

        public async Task<int> DeleteAdvertise(int id)
        {
            var advert = await _unitOfWork.Repository<Advertise>().GetByIdAsync(id);
            if (advert == null)
            {
                return 0;
            }

            if (!string.IsNullOrEmpty(advert.ImagePath))
            {
                var publicId = _imageService.ExtractPublicIdFromUrl(advert.ImagePath);
                _imageService.DeleteFromCloudinary(publicId);
            }

            _unitOfWork.Repository<Advertise>().Delete(advert);
            return await _unitOfWork.CompleteAsync();
        }



    }
}
