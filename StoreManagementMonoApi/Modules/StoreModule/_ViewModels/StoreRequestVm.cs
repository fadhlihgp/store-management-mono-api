namespace store_management_mono_api.Modules.StoreModule._ViewModels;

public class StoreRequestVm
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? BusinessType { get; set; }
    public DateTime? EstablishDate { get; set; }
}