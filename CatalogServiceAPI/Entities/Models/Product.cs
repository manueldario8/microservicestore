using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CatalogServiceAPI.Entities.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string ProviderCode { get; set; }
        public required string ProductCode { get; set; }


        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }
        public required int Stock { get; set; }
        public bool StatusActive { get; set; } = true;

        [Display(Name = "Image")]
        public string? UrlPhoto { get; set; }


        [JsonIgnore]
        public Provider Provider { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}
