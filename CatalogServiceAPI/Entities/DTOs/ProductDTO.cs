namespace CatalogServiceAPI.Entities.DTOs
{
    public record CreateProductDto(            
            string ProviderCode,                 
            int CategoryId,              
            string ProductCode,              
            string Name,               
            string? Description,                 
            decimal Price,      
            int Stock,    
            string? UrlPhoto);

    public record UpdateProductDto(
        string Name,
        string? Description,
        decimal Price,
        int Stock,
        string? UrlPhoto);
}

