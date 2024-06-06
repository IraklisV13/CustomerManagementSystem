namespace CustomerManagementSystem.Models
{
    public class ProductCatalog
    {
        public int ProductCatalogId { get; set; }
        public string SourceName { get; set; }
        public string SourceUrl { get; set; }
        public DateTime LastSyncDate { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
