using CustomerManagementSystem.DBContexts;
using CustomerManagementSystem.Models;
using CustomerManagementSystem.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CustomerManagementSystem.Services
{
    public class SynchronizationService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public SynchronizationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CMSContext>();
                var productCatalogs = await context.ProductCatalogs.ToListAsync(cancellationToken);

                foreach (var catalog in productCatalogs)
                {
                    await SyncProductsAsync(catalog.ProductCatalogId);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public async Task SyncProductsAsync(int catalogId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CMSContext>();
                var catalog = await context.ProductCatalogs.FindAsync(catalogId);
                if (catalog == null) throw new Exception("Catalog not found");

                var products = await FetchProductsFromSource(catalog.SourceUrl);

                foreach (var productDto in products)
                {
                    var existingProduct = await context.Products
                        .FirstOrDefaultAsync(p => p.ExternalId == productDto.Id && p.ProductCatalogId == catalogId);

                    if (existingProduct == null)
                    {
                        var product = new Product
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
                            ProductCatalogId = catalogId,
                            ExternalId = productDto.Id
                        };

                        context.Products.Add(product);
                    }
                    else
                    {
                        existingProduct.Title = productDto.Title;
                        existingProduct.Description = productDto.Description;
                        existingProduct.Price = productDto.Price;
                        existingProduct.Category = productDto.Category;
                        existingProduct.Image = productDto.Image;
                        existingProduct.Rating = new Rating
                        {
                            Rate = productDto.Rating.Rate,
                            Count = productDto.Rating.Count
                        };
                        context.Products.Update(existingProduct);
                    }
                }

                catalog.LastSyncDate = DateTime.Now;
                await context.SaveChangesAsync();
            }
        }

        private async Task<List<ProductDto>> FetchProductsFromSource(string sourceUrl)
        {
            if (string.IsNullOrWhiteSpace(sourceUrl))
            {
                sourceUrl = "https://fakestoreapi.com/products/";
            }

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync(sourceUrl);
                var products = JsonConvert.DeserializeObject<List<ProductDto>>(response);
                return products;
            }
        }
    }
}
