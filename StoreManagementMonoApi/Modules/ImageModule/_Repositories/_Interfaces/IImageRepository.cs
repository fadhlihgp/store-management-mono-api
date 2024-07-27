using store_management_mono_api.Entities;
using store_management_mono_api.Modules.ImageModule._ViewModels;

namespace store_management_mono_api.Modules.ImageModule._Repositories._Interfaces;

public interface IImageRepository
{
    Task<Image> CreateImage(IFormFile imageFile);
    Task<ImageDeleteResponse> DeleteImage(string imageId);
}