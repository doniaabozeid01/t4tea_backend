using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using t4tea.service.product.Dtos;
using t4tea.service.Review.Dtos;

namespace t4tea.service.Review
{
    public interface IReviewsServices
    {
        Task<GetReviewDto> AddReview(AddReviewDto reviewDto);
        Task<GetReviewDto> UpdateReview(int id, AddReviewDto reviewDto);
        Task<int> DeleteReview(int id);
        Task<IReadOnlyList<GetReviewDto>> GetAllReviews();
        Task<GetReviewDto> GetReviewById(int id);
        Task<ProductDto> UpdateRateOfProduct(int productId);
        Task<float?> GetReviewsByProductId(int id);

    }
}
