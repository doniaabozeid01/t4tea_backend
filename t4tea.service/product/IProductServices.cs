using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using t4tea.service.product.Dtos;

namespace t4tea.service.product
{
    public interface IProductServices
    {
        Task<IReadOnlyList<ProductDto>> GetAllProducts();
        Task<ProductDto> UpdateProduct(int id, addProductDto prodDto);
        Task<ProductDto> AddProduct(addProductDto productDto);
        Task<ProductDto> GetProductById(int id);
        Task<int> DeleteProduct(int id);
        //Task<IReadOnlyList<getOffers>> GetAllOffers(int type);
        Task<ProductDto> changeDiscount(int id, decimal discount);


        Task<IReadOnlyList<ProductDto>> GetAllProductWithOriginalOffer();
        Task<IReadOnlyList<ProductDto>> GetAllProductWithVIPOffer();

    }
}
