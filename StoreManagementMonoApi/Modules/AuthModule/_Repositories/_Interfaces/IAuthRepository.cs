using store_management_mono_api.Modules.AuthModule._ViewModels;

namespace store_management_mono_api.Modules.AuthModule._Repositories._Interfaces;

public interface IAuthRepository
{
    public Task<LoginResponseVm> Login(LoginRequestVm loginRequestVm);
    // Register
    // ForgotPassword
}