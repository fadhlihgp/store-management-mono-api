namespace store_management_mono_api.Modules.AuthModule._ViewModels;

public class SendLinkOtpVm
{
    public string Email { get; set; }
}

public class GetVerificationResponseVm
{
    public string Email { get; set; }
    public bool IsSuccess { get; set; }
}

public class ResetPasswordRequestVm
{
    public string Token { get; set; }
    public string NewPassword { get; set; }
}