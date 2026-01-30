namespace CatalogServiceAPI.Entities.Models
{
    public class Provider
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; }
        public bool StatusActived { get; set; } = true;
        public ICollection<Product> Products { get; set; } = [];
    }
}
