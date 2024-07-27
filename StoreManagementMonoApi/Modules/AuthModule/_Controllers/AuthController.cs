using Microsoft.AspNetCore.Mvc;
using store_management_mono_api.Modules.AuthModule._Repositories._Interfaces;
using store_management_mono_api.Modules.AuthModule._ViewModels;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Modules.AuthModule._Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private IAuthRepository _authRepository;
    
    public AuthController(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestVm loginRequest)
    {
        var data = await _authRepository.Login(loginRequest);
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil login",
            Data = data
        });
    }
}