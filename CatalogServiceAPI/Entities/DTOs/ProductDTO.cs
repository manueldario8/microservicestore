namespace CatalogServiceAPI.Entities.DTOs
{
    public class ProductDTO
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
    }
}
