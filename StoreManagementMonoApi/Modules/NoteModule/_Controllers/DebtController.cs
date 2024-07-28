using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using store_management_mono_api.Modules.NoteModule._Repositories._Interfaces;
using store_management_mono_api.Modules.NoteModule._ViewModels;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Modules.NoteModule._Controllers;

[ApiController]
[Authorize]
[Route("api/v1/note/debt")]
public class DebtController : ControllerBase
{
    private readonly IDebtRepository _debtRepository;

    public DebtController(IDebtRepository debtRepository)
    {
        _debtRepository = debtRepository;
    }

    private CredentialReq GetCredentialReq()
    {
        return new CredentialReq
        {
            StoreId = User.FindFirst("StoreId")?.Value,
            AccountId = User.FindFirst("AccountId")?.Value
        };
    }
    
    [HttpGet]
    public async Task<IActionResult> GetDebtList()
    {
        var data = await _debtRepository.GetDebts(GetCredentialReq().StoreId);
        return Ok(new MultipleDataResponse
        {
            Message = "Berhasil mendapatkan data hutang",
            Data = data,
            TotalData = data.Count()
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDebtById([FromRoute] string id)
    {
        var data = await _debtRepository.GetDebtById(id);
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil mendapatkan data detail",
            Data = data
        });
    }

    [HttpGet("detail/{id}")]
    public async Task<IActionResult> GetDebtDetailById([FromRoute] string id)
    {
        var data = await _debtRepository.GetDebtDetailById(id);
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil mendapatkan data detail hutang",
            Data = data
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateDebt([FromBody] DebtRequestVm debtRequestVm)
    {
        await _debtRepository.CreateDebt(GetCredentialReq(), debtRequestVm);
        return Created("api/v1/debt", new NoDataResponse
        {
            Message = "Berhasil menambah data hutang"
        });
    }
    
    [HttpPost("detail")]
    public async Task<IActionResult> CreateDebtDetail([FromBody] DebtDetailRequestVm debtRequestVm)
    {
        await _debtRepository.CreateDebtOne(GetCredentialReq(), debtRequestVm);
        return Created("api/v1/debt", new NoDataResponse
        {
            Message = "Berhasil menambah data hutang"
        });
    }

    [HttpPut("detail/{id}")]
    public async Task<IActionResult> UpdateDebtDetail([FromRoute] string id, [FromBody] DebtDetailRequestVm debtDetailRequestVm)
    {
        await _debtRepository.UpdateDebtDetails(id, GetCredentialReq(), debtDetailRequestVm);
        return Ok(new NoDataResponse
        {
            Message = "Berhasil memperbarui data hutang"
        });
    }

    [HttpDelete("detail/{id}")]
    public async Task<IActionResult> DeleteDebtDetail([FromRoute] string id)
    {
        await _debtRepository.DeleteDebtDetails(id);
        return Ok(new NoDataResponse
        {
            Message = "Berhasil menghapus data hutang"
        });
    }
}