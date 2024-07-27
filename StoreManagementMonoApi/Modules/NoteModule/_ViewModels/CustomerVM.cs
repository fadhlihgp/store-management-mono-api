namespace store_management_mono_api.Modules.NoteModule._ViewModels;

public class CustomerRequestVm
{
    public string FullName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
}

public class CustomerResponseVm
{
    public string Id { get; set; }
    public string FullName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime EditedAt { get; set; }
    public string EditedBy { get; set; } = null!;
}