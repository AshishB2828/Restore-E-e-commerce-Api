using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReactShope.Entity;
using ReactShope.Entity.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactShope.Data
{
    public class StoreContext : IdentityDbContext<User, Role, int>
    {
        public StoreContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Order> Orders { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>().HasData(
               
                 new Product
                 {  
                     Id=1,
                     Brand = "HP",
                     Description = "HP Chromebook 14 Intel Celeron N4020-4GB S… HPHP",
                     Name = "HP Chromebook 14",
                     PictureUrl = "https://images-eu.ssl-images-amazon.com/images/I/513LujQCoXL._AC_SX184_.jpg",
                     Price = 300,
                     QuantityInStock = 18,
                     Type = "Laptop"
                 },
                 new Product
                 {
                     Id = 2,
                     Brand = "boat",
                     Description = "boAt Rockerz 255 Pro in-Ear Earphones with 10… boAtboAt",
                     Name = "boAt Rockerz 255 Pro",
                     PictureUrl = "https://images-eu.ssl-images-amazon.com/images/I/31PU4kWou+L._AC_SX184_.jpg",
                     Price = 1200,
                     QuantityInStock = 20,
                     Type = "HeadPhone"
                 },
                 new Product
                 {
                     Id = 3,
                     Brand = "Nicon",
                     Description = "Nikon Z7 Mirrorless Camera Body with 24-… NikonNikon",
                     Name = "Nikon Z7",
                     PictureUrl = "https://images-eu.ssl-images-amazon.com/images/I/41ecM6cGpxL._AC_SX184_.jpg",
                     Price = 1200,
                     QuantityInStock = 20,
                     Type = "HeadPhone"
                 },
                  new Product
                  {
                      Id = 4,
                      Brand = "LG",
                      Description = "LG Gram Intel Evo 11th Gen Core i7 17 inches Ultra-Light Laptop (16 GB RAM, 512 GB SSD, New Windows 11 Home Preload, Iris Xe Graphics, USC -C x 2 (with Power), 1.35 kg, 17Z90P-G.AH85A2, Black)LG Gram Intel Evo 11th Gen Core i7",
                      Name = "LG Gram ",
                      PictureUrl = "https://images-eu.ssl-images-amazon.com/images/I/416Am14drmL._AC_SX184_.jpg",
                      Price = 200,
                      QuantityInStock = 10,
                      Type = "Laptop"
                  }

                );

            //Adding Roles

            builder.Entity<Role>()
                .HasData(
                    new Role { Id=1, Name = "Member", NormalizedName = "MEMBER" },
                    new Role { Id=2, Name="Admin", NormalizedName="ADMIN"}
                );
            builder.Entity<User>()
                .HasOne(a => a.Address)
                .WithOne()
                .HasForeignKey<UserAddress>(a => a.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
