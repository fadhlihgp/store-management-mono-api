namespace store_management_mono_api.Modules.ImageModule._ViewModels;

public class ImageRequestVm
{
    public string? PhotoUrl { get; set; }
    public string? PublicId { get; set; }
}

public class ImageDeleteResponse
{
    public string Message { get; set; }
    public bool IsSuccess { get; set; }
}