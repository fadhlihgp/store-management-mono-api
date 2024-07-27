using store_management_mono_api.Modules.AuthModule._ViewModels;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Services.Interfaces;

public interface IApiUtilityService
{
    public Task SendEmail(EmailVm emailVm);
    Task<CloudinaryDeleteImageResponseVm?> DeleteImageCloudinary(CloudinaryDeleteImageRequestVm deleteImageRequestVm);
    Task<CloudinaryUploadImageResponseVm?> UploadImageCloudinary(IFormFile image);
}