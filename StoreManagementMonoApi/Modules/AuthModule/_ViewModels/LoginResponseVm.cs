namespace store_management_mono_api.Modules.AuthModule._ViewModels;

public class LoginResponseVm
{
    public UserLogin User { get; set; }
    public string Token { get; set; }
}

public class UserLogin
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string RoleId { get; set; }
    public string? Role { get; set; }
}