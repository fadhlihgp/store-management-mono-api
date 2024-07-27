using Microsoft.AspNetCore.Mvc;
using store_management_mono_api.Modules.AuthModule._Repositories._Interfaces;
using store_management_mono_api.Modules.AuthModule._ViewModels;

namespace store_management_mono_api.Modules.AuthModule._Controllers;

[ApiController]
[Route("api/v1/auth/forget-password")]
public class ForgetPasswordController : ControllerBase
{
    private readonly IForgetPasswordRepository _forgetPasswordRepository;

    public ForgetPasswordController(IForgetPasswordRepository forgetPasswordRepository)
    {
        _forgetPasswordRepository = forgetPasswordRepository;
    }

    [HttpPost("send-otp")]
    public async Task<IActionResult> SendLinkOtpToEmail([FromBody] SendLinkOtpVm sendLinkOtp)
    {
        await _forgetPasswordRepository.SendLinkOtpToEmail(sendLinkOtp);
        return Ok(new
        {
            Message = "Link reset password berhasil dikirim ke email anda"
        });
    }

    [HttpGet("verify-otp/{token}")]
    public async Task<IActionResult> VerifyOtp([FromRoute] string token)
    {
        var getVerificationResponseVm = await _forgetPasswordRepository.VerifyOtp(token);
        return Ok(new
        {
            Message = "Otp berhasil diverifikasi",
            Data = getVerificationResponseVm
        });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestVm resetPasswordVm)
    {
        await _forgetPasswordRepository.ResetPassword(resetPasswordVm);
        return Ok(new
        {
            Message = "Password berhasil diperbarui, silahkan login dengan password baru"
        });
    }
}