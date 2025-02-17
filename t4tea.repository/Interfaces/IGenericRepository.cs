using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using t4tea.data.Context;
using t4tea.data.Entities;

namespace t4tea.repository.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task AddAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(int id);

        void Update(TEntity entity);

        Task<IReadOnlyList<TEntity>> GetAllAsync();

        void Delete(TEntity entity);

        Task<IReadOnlyList<Products>> GetAllProductsAsync();
        Task<Products> GetProductByIdAsync(int id);


        Task<IReadOnlyList<Categories>> GetAllCategoriesAsync();

        Task<Categories> GetCategoryByIdAsync(int id);




        Task<IReadOnlyList<FavouriteProducts>> GetProductFavouriteByUserId(string id);










        Task<IReadOnlyList<CartItems>> GetAllCartsAsync();
        Task<CartItems> GetCartByIdAsync(int id);
        Task<IReadOnlyList<CartItems>> GetByUserIdAsync(string id);














        Task<TEntity> GetByIdAsync(string id);


        Task<IReadOnlyList<OrderRequest>> GetOrdersByUserId(string id);

        Task<IReadOnlyList<OrderItem>> GetOrderItemsByOrderRequestId(string id);








        //Task<IReadOnlyList<Offer>> GetAllOffersBasedOnType(int type);
        Task<IReadOnlyList<Products>> GetAllProductWithOriginalOffer();
        Task<IReadOnlyList<Products>> GetAllProductWithVIPOffer();









        Task<IReadOnlyList<Reviews>> GetAllReviewsAsync();

        Task<IReadOnlyList<Reviews>> GetReviewsByProductId(int id);



    }
}
