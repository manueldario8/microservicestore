namespace CatalogServiceAPI.Interfaces
{
    public interface IImageService
    {
        Task<string> UploadImage(Stream file, string name);
    }
}
