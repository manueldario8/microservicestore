namespace CatalogServiceAPI.Entities.Models
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool StatusActived { get; set; } = true;
        public ICollection<Product> Products { get; set; } = [];
    }
}
