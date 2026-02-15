namespace CatalogServiceAPI.Entities.DTOs
{

    //To administrators

    public record CreateProductDto(            
        string ProviderCode,        
        string ProductCode,
        int CategoryId,              
        string Name,               
        string? Description,                 
        decimal Price,      
        int Stock,
        IFormFile? Image);

    public record CreatedProductDto(
        int Id,
        string ProviderCode,
        string ProductCode,
        GetCategorySimpleByClientDto Category,
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

    public record GetProductToListByAdminDto(
        string ProviderCode,
        string ProductCode,
        GetCategorySimpleByClientDto CategoryNameDto,
        string Name,
        decimal Price,
        int Stock);

    public record GetProductToCheckStockDto(
        string ProviderCode,
        string ProductCode,
        string Name,
        int Stock);

    public record GetProductToSellDto(
        string ProviderCode,
        string ProductCode,
        string Name,
        decimal Price);


    //To clients

    public record GetProductToListByClientDto(
        string ProviderCode,
        string ProductCode,
        GetCategorySimpleByClientDto CategoryNameDto,       
        string Name,
        string? Description,
        decimal Price,
        string? UrlPhoto);

    public record GetProductToOrderClientDto(
        string ProviderCode,
        string ProductCode,
        string Name,
        decimal Price);
        
}

