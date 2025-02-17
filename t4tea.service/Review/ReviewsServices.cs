using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using t4tea.data.Entities;
using t4tea.repository.Interfaces;
using t4tea.service.product.Dtos;
using t4tea.service.Review.Dtos;

namespace t4tea.service.Review
{
    public class ReviewsServices : IReviewsServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewsServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<GetReviewDto> AddReview(AddReviewDto reviewDto)
        {
            if (reviewDto != null)
            {
                var mappedReview = _mapper.Map<Reviews>(reviewDto);
                await _unitOfWork.Repository<Reviews>().AddAsync(mappedReview);
                var status = await _unitOfWork.CompleteAsync();

                if (status == 0)
                {
                    return null;
                }

                // تحديث التقييم للمنتج بعد إضافة التقييم
                await UpdateRateOfProduct(reviewDto.ProductId);

                return new GetReviewDto
                {
                    Id = mappedReview.Id,
                    Comment = reviewDto.Comment,
                    CreatedAt = mappedReview.CreatedAt,
                    Rating = reviewDto.Rating,
                    UserId = reviewDto.UserId,
                    ProductId = reviewDto.ProductId,
                };
            }
            return null;
        }






        public async Task<int> DeleteReview(int id)
        {
            var review = await _unitOfWork.Repository<Reviews>().GetByIdAsync(id);
            if (review != null)
            {
                _unitOfWork.Repository<Reviews>().Delete(review);
                var status = await _unitOfWork.CompleteAsync();

                if (status == 0)
                {
                    return 0;
                }
                else
                {
                    return status;
                }
            }
            return 0;
        }

        public async Task<IReadOnlyList<GetReviewDto>> GetAllReviews()
        {
            var reviews = await _unitOfWork.Repository<Reviews>().GetAllReviewsAsync();
            if (reviews.Any()) // يعني فيه عالاقل تقييم واحد او بمعني تاني فيه تقييمات 
            {
                var mappedReviews = _mapper.Map<IReadOnlyList<GetReviewDto>>(reviews);
                return mappedReviews;
            }
            return null;
        }

        public async Task<GetReviewDto> GetReviewById(int id)
        {
            var review = await _unitOfWork.Repository<Reviews>().GetByIdAsync(id);
            if (review != null) // يعني فيه عالاقل تقييم واحد او بمعني تاني فيه تقييمات 
            {
                var mappedReview = _mapper.Map<GetReviewDto>(review);
                return mappedReview;
            }
            return null;
        }

        public async Task<GetReviewDto> UpdateReview(int id, AddReviewDto reviewDto)
        {
            var review = await _unitOfWork.Repository<Reviews>().GetByIdAsync(id);
            if (review != null) // يعني فيه عالاقل تقييم واحد او بمعني تاني فيه تقييمات 
            {
                var mappedReview = _mapper.Map<AddReviewDto>(reviewDto);

                review.UserId = mappedReview.UserId;
                review.ProductId = mappedReview.ProductId;
                review.Comment = mappedReview.Comment;
                review.Rating = mappedReview.Rating;


                _unitOfWork.Repository<Reviews>().Update(review);
                var status = await _unitOfWork.CompleteAsync();
                if (status == 0)
                {
                    return null;
                }
                return new GetReviewDto
                {
                    Id = id,
                    UserId = mappedReview.UserId,
                    ProductId = mappedReview.ProductId,
                    Comment = mappedReview.Comment,
                    Rating = mappedReview.Rating,
                    CreatedAt = review.CreatedAt,
                    UpdatedAt = review.UpdatedAt,
                };
            }
            return null;
        }







        public async Task<ProductDto> UpdateRateOfProduct(int productId)
        {
            var product = await _unitOfWork.Repository<Products>().GetByIdAsync(productId);
            if (product != null)
            {
                // الحصول على المعدل الجديد من التقييمات
                product.Rate = await GetReviewsByProductId(productId);

                // تحديث المنتج
                _unitOfWork.Repository<Products>().Update(product);
                var status = await _unitOfWork.CompleteAsync();

                if (status == 0)
                {
                    return null;
                }

                var mappedProduct = _mapper.Map<ProductDto>(product);
                return mappedProduct;
            }
            return null;
        }



        public async Task<float?> GetReviewsByProductId(int id)
        {
            var reviews = await _unitOfWork.Repository<Reviews>().GetReviewsByProductId(id);

            if (reviews == null || reviews.Count == 0) // لا توجد تقييمات، نرجع قيمة 5 كـ Default
            {
                return 5;
            }

            // جمع التقييمات
            float? totalRate = reviews.Sum(r => r.Rating); // نستخدم float? بدلاً من float

            // حساب المتوسط
            float? rate = totalRate / reviews.Count;

            return rate;
        }



    }
}
