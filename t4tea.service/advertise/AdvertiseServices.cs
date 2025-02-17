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

namespace t4tea.service.advertise
{
    public class AdvertiseServices : IAdvertiseServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdvertiseServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AdvertiseDto> AddAdvertise(addAdvertise advertiseDto)
        {
            if (advertiseDto.ImagePath == null || advertiseDto.ImagePath.Length == 0)
            {
                return null; // التأكد من أن الصورة موجودة
            }

            string imagePath = await SaveImage(advertiseDto.ImagePath); // حفظ الصورة واسترجاع مسارها

            var advert = new Advertise
            {
                ImagePath = imagePath // تخزين المسار فقط
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

        public async Task<AdvertiseDto> UpdateAdvertise(int id, addAdvertise advertiseDto)
        {
            var advert = await _unitOfWork.Repository<Advertise>().GetByIdAsync(id);

            if (advert == null)
            {
                return null;
            }

            // إذا كان هناك صورة جديدة، احذف القديمة ثم احفظ الجديدة
            if (advertiseDto.ImagePath != null)
            {
                DeleteImage(advert.ImagePath); // حذف الصورة القديمة
                advert.ImagePath = await SaveImage(advertiseDto.ImagePath); // حفظ الصورة الجديدة
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

            // حذف الصورة قبل حذف الإعلان
            DeleteImage(advert.ImagePath);

            _unitOfWork.Repository<Advertise>().Delete(advert);
            return await _unitOfWork.CompleteAsync();
        }

    }
}
