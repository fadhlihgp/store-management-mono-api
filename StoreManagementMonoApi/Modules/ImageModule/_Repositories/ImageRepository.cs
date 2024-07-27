using store_management_mono_api.Context;
using store_management_mono_api.Entities;
using store_management_mono_api.Exceptions;
using store_management_mono_api.Modules.ImageModule._Repositories._Interfaces;
using store_management_mono_api.Modules.ImageModule._ViewModels;
using store_management_mono_api.Services.Interfaces;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Modules.ImageModule._Repositories;

public class ImageRepository : IImageRepository
{
    private readonly AppDbContext _context;
    private readonly IApiUtilityService _apiUtilityService;

    public ImageRepository(AppDbContext context, IApiUtilityService apiUtilityService)
    {
        _context = context;
        _apiUtilityService = apiUtilityService;
    }

    public async Task<Image> CreateImage(IFormFile imageFile)
    {
        try
        {
            var cloudinaryUploadImageResponseVm = await _apiUtilityService.UploadImageCloudinary(imageFile);
            var saveImage = new Image
            {
                Id = Guid.NewGuid().ToString(),
                PhotoUrl = cloudinaryUploadImageResponseVm.Data.PhotoUrl,
                PublicId = cloudinaryUploadImageResponseVm.Data.PublicId
            };
            var response = await _context.Images.AddAsync(saveImage);
            await _context.SaveChangesAsync();
            return response.Entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ImageDeleteResponse> DeleteImage(string imageId)
    {
        // Find image by id, if not found throw error
        var findImage = await _context.Images.FindAsync(imageId);
        if (findImage == null) throw new NotFoundException("Gambar tidak ditemukan");

        try
        {
            // If publicId not null, hit ApiUtility for deleting cloudinary image
            if (findImage.PublicId != null)
            {
                var responseApiUtility = await _apiUtilityService.DeleteImageCloudinary(
                    new CloudinaryDeleteImageRequestVm { PublicId = findImage.PublicId });
                if (responseApiUtility?.Data.Error == null)  _context.Images.Remove(findImage);
                await _context.SaveChangesAsync();
               return new ImageDeleteResponse
               {
                   Message = "Berhasil menghapus gambar",
                   IsSuccess = true
               };
            }
            else
            {
                throw new NotFoundException("Public id tidak ditemukan");
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}