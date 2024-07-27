using System.ComponentModel.DataAnnotations;

namespace store_management_mono_api.Modules.AccountModule;

public class AccountRequestVm
{
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    [StringLength(maximumLength:int.MaxValue, MinimumLength = 8)]
    public string? Password { get; set; }
    public bool IsActive { get; set; } = true;
    public string? StoreId { get; set; }
    public string? RoleId { get; set; }
}

public class AccountResponseVm
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public DateTime? LastLogin { get; set; }
    public bool? IsActive { get; set; }
    public string? StoreId { get; set; }
    public string? StoreName { get; set; }
    public string? ImageId { get; set; }
    public string? ImageUrl { get; set; }
    public string RoleId { get; set; } = null!;
    public string RoleName { get; set; }
}

public class ChangePasswordVm
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}