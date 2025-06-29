using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using t4tea.data.Context;
using t4tea.data.Entities;
using t4tea.repository.Interfaces;

namespace t4tea.repository.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly T4teaDbContext _context;

        public GenericRepository(T4teaDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }


        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }


        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public async Task<IReadOnlyList<Products>> GetAllProductsAsync()
        {
            return await _context.Set<Products>().Include(y => y.category).Include(y => y.images).Include(z => z.benifits).Include(x => x.reviews).ToListAsync();
        }

        
        public async Task<Products> GetProductByIdAsync(int id)
        {
            return await _context.Set<Products>().Include(y => y.category).Include(y => y.images).Include(z => z.benifits).Include(x => x.reviews).FirstOrDefaultAsync(x=>x.Id == id);
        }









        public async Task<IReadOnlyList<Categories>> GetAllCategoriesAsync()
        {
            return await _context.Set<Categories>().Include(x=>x.Products).ThenInclude(y => y.images).Include(x=>x.Products).ThenInclude(z=>z.benifits).ToListAsync();
        }

        public async Task<Categories> GetCategoryByIdAsync(int id)
        {
            return await _context.Set<Categories>().Include(x => x.Products).ThenInclude(y => y.images).Include(x => x.Products).ThenInclude(z => z.benifits).FirstOrDefaultAsync(x => x.Id == id); ;
        }












        public async Task<IReadOnlyList<FavouriteProducts>> GetProductFavouriteByUserId(string id)
        {
            return await _context.Set<FavouriteProducts>().Where(x=>x.UserId == id).Include(x => x.Product).ThenInclude(y => y.images).Include(x => x.User).ToListAsync() ;
        }














        public async Task<IReadOnlyList<CartItems>> GetAllCartsAsync()
        {
            return await _context.Set<CartItems>().Include(x => x.User).Include(x => x.Product).ToListAsync();
        }

        public async Task<CartItems> GetCartByIdAsync(int id)
        {
            return await _context.Set<CartItems>().Include(x => x.User).Include(x => x.Product).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<CartItems>> GetByUserIdAsync(string id)
        {
            return await _context.Set<CartItems>().Include(x => x.Product).ThenInclude(x=>x.images).Include(x => x.User).Where(x => x.UserId == id).ToListAsync();
        }










        public async Task<TEntity> GetByIdAsync(string id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }









        public async Task<IReadOnlyList<OrderRequest>> GetOrdersByUserId(string id)
        {
            return await _context.Set<OrderRequest>().Where(x => x.UserId == id).ToListAsync();
        }



        public async Task<IReadOnlyList<OrderItem>> GetOrderItemsByOrderRequestId(string id)
        {
            return await _context.Set<OrderItem>().Include(x => x.Product).Include(x => x.User).Where(x => x.OrderRequestId == id).ToListAsync();
        }




        //public async Task<IReadOnlyList<Offer>> GetAllOffersBasedOnType(int type)
        //{
        //    return await _context.Set<Offer>().Where(x=> x.Type.Equals(type)).Include(x => x.Products).ThenInclude(y => y.images).Include(x => x.Products).ThenInclude(z => z.benifits).ToListAsync();
        //}





        public async Task<IReadOnlyList<Products>> GetAllProductWithOriginalOffer()
        {
            return await _context.Set<Products>().Where(x => x.Discount != 0 && x.Discount < 15).Include(y => y.images).Include(z => z.benifits).Include(x => x.reviews).ToListAsync();
        }


        public async Task<IReadOnlyList<Products>> GetAllProductWithVIPOffer()
        {
            return await _context.Set<Products>().Where(x => x.Discount != 0 && x.Discount >= 15).Include(y => y.images).Include(z => z.benifits).Include(x=>x.reviews).ToListAsync();
        }





        public async Task<IReadOnlyList<Reviews>> GetAllReviewsAsync()
        {
            return await _context.Set<Reviews>().Include(x => x.User).ToListAsync();
        }








        public async Task<IReadOnlyList<Reviews>> GetReviewsByProductId(int id)
        {
            return await _context.Set<Reviews>().Where(x=>x.ProductId == id).ToListAsync();
        }






    }
}
