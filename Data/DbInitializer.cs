using CustomerManagementSystem.DBContexts;
using CustomerManagementSystem.Models;
using CustomerManagementSystem.Services;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace CustomerManagementSystem.Data
{
    public class DbInitializer
    {
        private SynchronizationService synchronizationService;

        public static void Initialize(CMSContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context), "Database context is null. Cannot initialize the database.");
            }

            // Check if database already exists else create it and initialize with data
            if (!(context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
            {
                // Ensure the database is created
                context.Database.EnsureCreated();

                // Seed the database with initial data
                SeedDatabase(context).GetAwaiter().GetResult();
            }
        }

        private static async Task SeedDatabase(CMSContext context)
        {
            var url = "https://fakestoreapi.com/products/";
            var products = await synchronizationService.FetchProductsFromSource(url);
            var initialProducts = products.Select(productDto => new Product
            {
                Title = productDto.Title,
                Description = productDto.Description,
                Price = productDto.Price,
                Category = productDto.Category,
                Image = productDto.Image,
                Rating = new Rating
                {
                    Rate = productDto.Rating.Rate,
                    Count = productDto.Rating.Count
                },
                ProductCatalogId = 1, // Assuming a default ProductCatalogId
                ExternalId = productDto.Id
            }).ToList();

            context.Products.AddRange(initialProducts);
            await context.SaveChangesAsync();
        }
    }
}
