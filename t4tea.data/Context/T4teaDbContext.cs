using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using t4tea.data.Entities;

namespace t4tea.data.Context
{
    public class T4teaDbContext : IdentityDbContext<ApplicationUser>
    {
        public T4teaDbContext(DbContextOptions<T4teaDbContext> options) : base(options)
        {

        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // تأكد من استدعاء الأساس لمنع الأخطاء

            // إذا كنت تحتاج لتعريف المفاتيح المركبة يدويًا:
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(x => new { x.LoginProvider, x.ProviderKey });
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(x => new { x.UserId, x.LoginProvider, x.Name });
        }




        public DbSet<Categories> Category { get; set; }
        public DbSet<Products> Product { get; set; }
        public DbSet<Advertise> Advertise { get; set; }
        public DbSet<Benifits> Benifit { get; set; }
        public DbSet<Images> Image { get; set; }
        public DbSet<FavouriteProducts> FavouriteProducts { get; set; }
        public DbSet<CartItems> Cart { get; set; }
        public DbSet<OrderRequest> orderRequest { get; set; }
        public DbSet<OrderItem> orderItems { get; set; }
        //public DbSet<Offer> Offers { get; set; }
        public DbSet<Reviews> Review { get; set; }
        public DbSet<ShippingAndDiscount> ShippingAndDiscount { get; set; }


    }
}
