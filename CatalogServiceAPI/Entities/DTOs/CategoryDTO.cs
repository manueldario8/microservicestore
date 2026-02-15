namespace CatalogServiceAPI.Entities.DTOs
{

    //DTOs to administrators
    public record CreateCategoryDto(
        int Id,
        string Name);

    public record UpdateCategoryDto(
        string Name);

    public record GetCategorySimpleByAdminDto(
        int Id,
        string Name);

    public record GetCategoryWithProductsByAdminDto(
        string Name,
        IEnumerable<GetProductToSellDto> Products);

    //DTOs to clients 
    public record GetCategorySimpleByClientDto(
        string Name);

    public record GetCategoryWithProductsByClientDto(
        string Name,
        IEnumerable<GetProductToSellDto> Products);

}