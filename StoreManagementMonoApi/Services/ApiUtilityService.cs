using System.Text;
using System.Text.Json;
using store_management_mono_api.Exceptions;
using store_management_mono_api.Services.Interfaces;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Services;

public class ApiUtilityService : IApiUtilityService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ApiUtilityService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task SendEmail(EmailVm emailVm)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiUtility");

            var requestBody = new StringContent(JsonSerializer.Serialize(emailVm), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("email-utility/send", requestBody);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<CloudinaryDeleteImageResponseVm?> DeleteImageCloudinary(CloudinaryDeleteImageRequestVm deleteImageRequestVm)
    {
        try
        {
            // Create client
            var client = _httpClientFactory.CreateClient("ApiUtility");
        
            // Make object for request body
            var jsonContent = new StringContent(JsonSerializer.Serialize(deleteImageRequestVm), Encoding.UTF8, "application/json");
        
            // Hit Request and get response
            var response = await client.PutAsync("cloudinary/v1/delete-photo", jsonContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CloudinaryDeleteImageResponseVm>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public async Task<CloudinaryUploadImageResponseVm?> UploadImageCloudinary(IFormFile image)
    {
        try
        {
            // Create client
            var client = _httpClientFactory.CreateClient("ApiUtility");

            // Prepare data for sent to API
            var sendFile = new MultipartFormDataContent();
            if (image.Length > 0)
            {
                var fileStream = image.OpenReadStream();
                sendFile.Add(new StreamContent(fileStream), "image", image.FileName);
            }
            else
            {
                throw new BadRequestException("Tidak ada file yang di unggah!");
            }

            // Hit api with data 
            var response = await client.PostAsync("cloudinary/v1/upload-photo/KelolaWarung", sendFile);
            response.EnsureSuccessStatusCode();

            // Get the response
            var responseClient = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CloudinaryUploadImageResponseVm>(responseClient, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}