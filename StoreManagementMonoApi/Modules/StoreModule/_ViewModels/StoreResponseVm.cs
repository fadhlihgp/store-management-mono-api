namespace store_management_mono_api.Modules.StoreModule._ViewModels;

public class StoreResponseVm
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public DateTime RegisterDate { get; set; }
    public string? BusinessType { get; set; }
    public DateTime? EstablishDate { get; set; }
    public DateTime LastEdited { get; set; }
}