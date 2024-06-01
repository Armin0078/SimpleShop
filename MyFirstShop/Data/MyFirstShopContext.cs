using Microsoft.EntityFrameworkCore;
using MyFirstShop.Models;
using System.Drawing;

namespace MyFirstShop.Data
{
	public class MyFirstShopContext:DbContext
	{
        public MyFirstShopContext(DbContextOptions<MyFirstShopContext> options):base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryToProduct> CategoryToProducts { get; set; }

        public DbSet<Product>  Products { get; set; }

        public DbSet<Item>  Items { get; set; }

		public DbSet<Users> Users { get; set; }

		public DbSet<Order> orders { get; set; }

		public DbSet<OrderDetial> ordersDetial { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CategoryToProduct>().HasKey(
				d => new {d.CategoryId , d.ProductId }
				);
			
			#region Seed Data Category
			modelBuilder.Entity<Category>().HasData(
				new Category { Id = 1,Name = "لپتاپ",Description= "لپتاپ" },
				new Category { Id = 2,Name = "موبایل", Description = "موبایل" },
				new Category { Id = 3, Name = "تلوزیون", Description = "تلوزیون" }
			);

			modelBuilder.Entity<Item>().HasData(
				new Item() { Id =1 , price = 2562 , quantityInStock = 1 },
				new Item() { Id =2 , price = 3769 , quantityInStock = 4},
				new Item() { Id =3, price = 4897, quantityInStock =0 }
				);

			modelBuilder.Entity<Product>().HasData(
				new Product() { Id=1 , Name = "ریش تراش" , ItemId =1 , Description = "یکی  بخر دو  تا ببر"},
				new Product() { Id=2 , Name = "پیتزا" , ItemId = 2, Description = "با پیتزا روزتو شروع کن"},
				new Product() { Id=3 , Name = "آرمین" , ItemId = 3, Description = "بهترین دوست دنیا"}
				);

			modelBuilder.Entity<CategoryToProduct>().HasData(
				new CategoryToProduct() { CategoryId =1 , ProductId =1 },
				new CategoryToProduct() { CategoryId =2 , ProductId =1 },
				new CategoryToProduct() { CategoryId =3, ProductId =1 },
				new CategoryToProduct() { CategoryId =1, ProductId =2 },
				new CategoryToProduct() { CategoryId =2, ProductId =2 },
				new CategoryToProduct() { CategoryId =3, ProductId =2 },
				new CategoryToProduct() { CategoryId =1, ProductId =3 },
				new CategoryToProduct() { CategoryId =2, ProductId =3 },
				new CategoryToProduct() { CategoryId =3, ProductId =3 }
				);
			#endregion
			
			base.OnModelCreating(modelBuilder);
		}
	}
}
