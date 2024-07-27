using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using store_management_mono_api.Modules.AccountModule._Interfaces;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Modules.AccountModule;

[Authorize]
[ApiController]
[Route("api/v1/account")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;

    public AccountController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAccounts()
    {
        var roleId = User.FindFirst("RoleId")?.Value;
        var storeId = User.FindFirst("StoreId")?.Value;
        var accountResponseVms = await _accountRepository.GetAllAccounts(roleId, storeId);

        return Ok(new MultipleDataResponse
        {
            Message = "Berhasil mengambil data akun",
            Data = accountResponseVms,
            TotalData = accountResponseVms.Count()
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccountById([FromRoute] string id)
    {
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil mengambil data detail akun",
            Data = await _accountRepository.GetAccountById(id)
        });
    }
    
    [HttpGet("current-account")]
    public async Task<IActionResult> GetAccountByLogin()
    {
        var id = User.FindFirst("AccountId")?.Value;
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil mengambil data detail akun",
            Data = await _accountRepository.GetAccountById(id)
        });
    }

    [HttpPut("update-profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] AccountRequestVm accountRequestVm)
    {
        var id = User.FindFirst("AccountId")?.Value;
        await _accountRepository.EditAccount(id, accountRequestVm);

        return Ok(new NoDataResponse
        {
            Message = "Berhasil memperbarui profile"
        });
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateAccount([FromBody] AccountRequestVm accountRequestVm)
    {
        var roleId = User.FindFirst("RoleId")?.Value;
        var storeId = User.FindFirst("StoreId")?.Value;
        
        await _accountRepository.CreateAccount(roleId, storeId, accountRequestVm);
        
        return Created("create", new NoDataResponse
        {
            Message = "Berhasil membuat akun"
        });
    }
    
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateAccount([FromRoute] string id, [FromBody] AccountRequestVm accountRequestVm)
    {
        await _accountRepository.EditAccount(id, accountRequestVm);

        return Ok(new NoDataResponse
        {
            Message = "Berhasil memperbarui akun"
        });
    }

    [HttpPut("delete/{id}")]
    public async Task<IActionResult> DeleteAccount([FromRoute] string id)
    {
        await _accountRepository.DeleteAccount(id);
        return Ok(new NoDataResponse
        {
            Message = "Berhasil menghapus data akun"
        });
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordVm changePasswordVm)
    {
        var id = User.FindFirst("AccountId")?.Value;
        await _accountRepository.ChangePassword(id, changePasswordVm);
        return Ok(new NoDataResponse
        {
            Message = "Password berhasil diperbarui"
        });
    }
}