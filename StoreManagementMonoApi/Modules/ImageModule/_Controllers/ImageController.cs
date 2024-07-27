using Microsoft.AspNetCore.Mvc;
using store_management_mono_api.Modules.ImageModule._Repositories._Interfaces;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Modules.ImageModule._Controllers;

[ApiController]
[Route("api/v1/image")]
public class ImageController : ControllerBase
{
    private IImageRepository _imageRepository;

    public ImageController(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile image)
    {
        var saveImage = await _imageRepository.CreateImage(image);
        return Created("api/v1/image", new SingleDataResponse
        {
            Message = "Berhasil menyimpan gambar",
            Data = saveImage
        });
    }

    [HttpDelete("{imageId}")]
    public async Task<IActionResult> DeleteImage([FromRoute] string imageId)
    {
        var data = await _imageRepository.DeleteImage(imageId);
        return Ok(data);
    }
}