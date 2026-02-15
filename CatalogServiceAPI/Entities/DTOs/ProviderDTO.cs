namespace CatalogServiceAPI.Entities.DTOs
{
    public record CreateProviderDto(
        string Name,
        string Code);    

    public record GetProviderCreatedDto(
        int Id,
        string Name,
        string Code);

    public record UpdateProviderDto(
        string Name);

    public record GetProviderSimpleDto(
        string Name,
        string Code);



    public record GetProviderWithProductsDto(
        string Name,
        string Code,
        IEnumerable<GetProductToSellDto> ProductDto);
}