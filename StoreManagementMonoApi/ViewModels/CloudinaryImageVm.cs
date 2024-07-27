namespace store_management_mono_api.ViewModels;

public class CloudinaryDeleteImageRequestVm
{
    public string PublicId { get; set; }
}

public class CloudinaryDeleteImageResponseVm
{
    public string Message { get; set; }
    public DataDeleteResponse Data { get; set; }
}

public class CloudinaryUploadImageResponseVm
{
    public string Message { get; set; }
    public DataImageUploadResponse Data { get; set;}
}
public class DataDeleteResponse
{
    public string Result { get; set; }
    public int StatusCode { get; set; }
    public string? Error { get; set; }
}

public class DataImageUploadResponse
{
    public string PublicId { get; set; }
    public string PhotoUrl { get; set; }
}