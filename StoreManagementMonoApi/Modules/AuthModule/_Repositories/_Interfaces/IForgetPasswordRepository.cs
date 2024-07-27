using store_management_mono_api.Modules.AuthModule._ViewModels;

namespace store_management_mono_api.Modules.AuthModule._Repositories._Interfaces;

public interface IForgetPasswordRepository
{
    public Task SendLinkOtpToEmail(SendLinkOtpVm sendLinkOtp);
    public Task<GetVerificationResponseVm> VerifyOtp(string token);
    public Task ResetPassword(ResetPasswordRequestVm resetPasswordVm);
}