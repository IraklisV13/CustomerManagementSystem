namespace CustomerManagementSystem.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public Rating Rating { get; set; }
        public int? ProductCatalogId { get; set; }
        public int ExternalId { get; set; }

        public ProductCatalog? ProductCatalog { get; set; }
    }

    public class Rating
    {
        public int RatingId { get; set; }
        public double Rate { get; set; }
        public int Count { get; set; }
    }
}
