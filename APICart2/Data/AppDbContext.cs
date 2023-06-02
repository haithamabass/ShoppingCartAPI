using APICart2.Models;
using APICart2.Models.AuthModels;
using Microsoft.EntityFrameworkCore;

namespace APICart2.Data
{
    public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {

            }

            public DbSet<Cart> Carts { get; set; }
            public DbSet<CartItem> CartItems { get; set; }
            public DbSet<Product> Products { get; set; }
            public DbSet<ProductCategory> ProductCategories { get; set; }
            public DbSet<Invoice> Invoices { get; set; }
            public DbSet<InvoiceItem> InvoiceItems { get; set; }

            public DbSet<Order> Orders { get; set; }



            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                #region Add Products

            //Products
            //Beauty Category
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 1,
                Name = "Glossier - Beauty Kit",
                Description = "A kit provided by Glossier, containing skin care, hair care and makeup products",
                ImageURL = "/Images/Beauty/Beauty1.png",
                BarCode = "PB-11",
                Price = 100,
                Quantity = 100,
                CategoryId = 1

                });
                modelBuilder.Entity<Product>().HasData(new Product
                {
                    ProductId = 2,
                    Name = "Curology - Skin Care Kit",
                    Description = "A kit provided by Curology, containing skin care products",
                    ImageURL = "/Images/Beauty/Beauty2.png",
                    BarCode = "PB-12",
                    Price = 50,
                    Quantity = 45,
                    CategoryId = 1

                });
                modelBuilder.Entity<Product>().HasData(new Product
                {
                    ProductId = 3,
                    Name = "Cocooil - Organic Coconut Oil",
                    Description = "A kit provided by Curology, containing skin care products",
                    ImageURL = "/Images/Beauty/Beauty3.png",
                    BarCode = "PB-13",
                    Price = 20,
                    Quantity = 30,
                    CategoryId = 1

                });
                modelBuilder.Entity<Product>().HasData(new Product
                {
                    ProductId = 4,
                    Name = "Schwarzkopf - Hair Care and Skin Care Kit",
                    Description = "A kit provided by Schwarzkopf, containing skin care and hair care products",
                    ImageURL = "/Images/Beauty/Beauty4.png",
                    BarCode = "PB-14",
                    Price = 50,
                    Quantity = 60,
                    CategoryId = 1

                });
                modelBuilder.Entity<Product>().HasData(new Product
                {
                    ProductId = 5,
                    Name = "Skin Care Kit",
                    Description = "Skin Care Kit, containing skin care and hair care products",
                    ImageURL = "/Images/Beauty/Beauty5.png",
                    BarCode = "PB-15",
                    Price = 30,
                    Quantity = 85,
                    CategoryId = 1

                });
                //Electronics Category
                modelBuilder.Entity<Product>().HasData(new Product
                {
                    ProductId = 6,
                    Name = "Air Pods",
                    Description = "Air Pods - in-ear wireless headphones",
                    ImageURL = "/Images/Electronic/Electronics1.png",
                    BarCode = "PE-31",
                    Price = 100,
                    Quantity = 120,
                    CategoryId = 3

                });
                modelBuilder.Entity<Product>().HasData(new Product
                {
                    ProductId = 7,
                    Name = "On-ear Golden Headphones",
                    Description = "On-ear Golden Headphones - these headphones are not wireless",
                    ImageURL = "/Images/Electronic/Electronics2.png",
                    BarCode = "PE-32",
                    Price = 40,
                    Quantity = 200,
                    CategoryId = 3

                });
                modelBuilder.Entity<Product>().HasData(new Product
                {
                    ProductId = 8,
                    Name = "On-ear Black Headphones",
                    Description = "On-ear Black Headphones - these headphones are not wireless",
                    ImageURL = "/Images/Electronic/Electronics3.png",
                    BarCode = "PE-33",
                    Price = 40,
                    Quantity = 300,
                    CategoryId = 3

                });
                modelBuilder.Entity<Product>().HasData(new Product
                {
                    ProductId = 9,
                    Name = "Sennheiser Digital Camera with Tripod",
                    Description = "Sennheiser Digital Camera - High quality digital camera provided by Sennheiser - includes tripod",
                    ImageURL = "/Images/Electronic/Electronic4.png",
                    BarCode = "PE-34",
                    Price = 600,
                    Quantity = 20,
                    CategoryId = 3

                });
                modelBuilder.Entity<Product>().HasData(new Product
                {
                    ProductId = 10,
                    Name = "Canon Digital Camera",
                    Description = "Canon Digital Camera - High quality digital camera provided by Canon",
                    ImageURL = "/Images/Electronic/Electronic5.png",
                    BarCode = "PE-35",
                    Price = 500,
                    Quantity = 15,
                    CategoryId = 3

                });
                modelBuilder.Entity<Product>().HasData(new Product
                {
                    ProductId = 11,
                    Name = "Nintendo Gameboy",
                    Description = "Gameboy - Provided by Nintendo",
                    ImageURL = "/Images/Electronic/technology6.png",
                    BarCode = "PE-36",
                    Price = 100,
                    Quantity = 60,
                    CategoryId = 3
                });
                //Furniture Category
                modelBuilder.Entity<Product>().HasData(new Product
                {
                    ProductId = 12,
                    Name = "Black Leather Office Chair",
                    Description = "Very comfortable black leather office chair",
                    ImageURL = "/Images/Furniture/Furniture1.png",
                    BarCode = "PF-21",
                    Price = 50,
                    Quantity = 212,
                    CategoryId = 2
                });

                modelBuilder.Entity<Product>().HasData(new Product
                {
                    ProductId = 13,
                    Name = "Pink Leather Office Chair",
                    Description = "Very comfortable pink leather office chair",
                    ImageURL = "/Images/Furniture/Furniture2.png",
                    BarCode = "PF-22",
                    Price = 50,
                    Quantity = 112,
                    CategoryId = 2
                });
                modelBuilder.Entity<Product>().HasData(new Product
                {
                    ProductId = 14,
                    Name = "Lounge Chair",
                    Description = "Very comfortable lounge chair",
                    ImageURL = "/Images/Furniture/Furniture3.png",
                    BarCode = "PF-23",
                    Price = 70,
                    Quantity = 90,
                    CategoryId = 2
                });
                modelBuilder.Entity<Product>().HasData(new Product
                {
                    ProductId = 15,
                    Name = "Silver Lounge Chair",
                    Description = "Very comfortable Silver lounge chair",
                    ImageURL = "/Images/Furniture/Furniture4.png",
                    BarCode = "PF-24",
                    Price = 120,
                    Quantity = 95,
                    CategoryId = 2
                });
                modelBuilder.Entity<Product>().HasData(new Product
                {
                    ProductId = 16,
                    Name = "Porcelain Table Lamp",
                    Description = "White and blue Porcelain Table Lamp",
                    ImageURL = "/Images/Furniture/Furniture6.png",
                    BarCode = "PF-25",
                    Price = 15,
                    Quantity = 100,
                    CategoryId = 2
                });
                modelBuilder.Entity<Product>().HasData(new Product
                {
                    ProductId = 17,
                    Name = "Office Table Lamp",
                    Description = "Office Table Lamp",
                    ImageURL = "/Images/Furniture/Furniture7.png",
                    BarCode = "PF-26",
                    Price = 20,
                    Quantity = 73,
                    CategoryId = 2
                });

                //Shoes Category
                modelBuilder.Entity<Product>().HasData(new Product
                {
                    ProductId = 18,
                    Name = "Puma Sneakers",
                    Description = "Comfortable Puma Sneakers in most sizes",
                    ImageURL = "/Images/Shoes/Shoes1.png",
                    BarCode = "PC-41",
                    Price = 100,
                    Quantity = 50,
                    CategoryId = 4
                });
                modelBuilder.Entity<Product>().HasData(new Product
                {
                    ProductId = 19,
                    Name = "Colorful Trainers",
                    Description = "Colorful trainsers - available in most sizes",
                    ImageURL = "/Images/Shoes/Shoes2.png",
                    BarCode = "PC-42",
                    Price = 150,
                    Quantity = 60,
                    CategoryId = 4
                });
                modelBuilder.Entity<Product>().HasData(new Product
                {
                    ProductId = 20,
                    Name = "Blue Nike Trainers",
                    Description = "Blue Nike Trainers - available in most sizes",
                    ImageURL = "/Images/Shoes/Shoes3.png",
                    BarCode = "PC-43",
                    Price = 200,
                    Quantity = 70,
                    CategoryId = 4
                });
                modelBuilder.Entity<Product>().HasData(new Product
                {
                    ProductId = 21,
                    Name = "Colorful Hummel Trainers",
                    Description = "Colorful Hummel Trainers - available in most sizes",
                    ImageURL = "/Images/Shoes/Shoes4.png",
                    BarCode = "PC-44",
                    Price = 120,
                    Quantity = 120,
                    CategoryId = 4
                });
                modelBuilder.Entity<Product>().HasData(new Product
                {
                    ProductId = 22,
                    Name = "Red Nike Trainers",
                    Description = "Red Nike Trainers - available in most sizes",
                    ImageURL = "/Images/Shoes/Shoes5.png",
                    BarCode = "PC-45",
                    Price = 200,
                    Quantity = 100,
                    CategoryId = 4
                });
                modelBuilder.Entity<Product>().HasData(new Product
                {
                    ProductId = 23,
                    Name = "Birkenstock Sandles",
                    Description = "Birkenstock Sandles - available in most sizes",
                    ImageURL = "/Images/Shoes/Shoes6.png",
                    BarCode = "PC-46",
                    Price = 50,
                    Quantity = 150,
                    CategoryId = 4
                });

                #endregion

                #region Create Shopping Cart for Users
                //modelBuilder.Entity<Cart>().HasData(new Cart
                //{
                //    CartId = 1,
                //    UserId = 1

                //});
                //modelBuilder.Entity<Cart>().HasData(new Cart
                //{
                //    CartId = 2,
                //    UserId = 2

                //});
                #endregion

                #region Add Product Categories
                modelBuilder.Entity<ProductCategory>().HasData(new ProductCategory
                {
                    CategoryId = 1,
                    Name = "Beauty",
                });
                modelBuilder.Entity<ProductCategory>().HasData(new ProductCategory
                {
                    CategoryId = 2,
                    Name = "Furniture",
                });
                modelBuilder.Entity<ProductCategory>().HasData(new ProductCategory
                {
                    CategoryId = 3,
                    Name = "Electronics",
                });
                modelBuilder.Entity<ProductCategory>().HasData(new ProductCategory
                {
                    CategoryId = 4,
                    Name = "Shoes",
                });
            #endregion

                #region Seed Users
            //Add users
            //modelBuilder.Entity<User>().HasData(new User
            //{
            //    UserId = 1,
            //    UserName = "Bob"

            //});
            //modelBuilder.Entity<User>().HasData(new User
            //{
            //    UserId = 2,
            //    UserName = "Sarah"

            //});
            #endregion

                modelBuilder.Ignore<AppUser>();
                modelBuilder.Ignore<User>();




        }

    }

}
