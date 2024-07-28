using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using store_management_mono_api.Modules.NoteModule._Repositories._Interfaces;
using store_management_mono_api.Modules.NoteModule._ViewModels;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Modules.NoteModule._Controllers;

[ApiController]
[Authorize]
[Route("api/v1/note/income-expense")]
public class NoteIncomeExpenseController : ControllerBase
{
    private readonly INoteIncomeExpenseRepository _incomeExpenseRepository;

    public NoteIncomeExpenseController(INoteIncomeExpenseRepository incomeExpenseRepository)
    {
        _incomeExpenseRepository = incomeExpenseRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetIncomeExpenseList()
    {
        var data = await _incomeExpenseRepository.GetIncomeExpenseList(GetCredential().StoreId);
        return Ok(new MultipleDataResponse
        {
            Message = "Berhasil mendapatkan data",
            Data = data,
            TotalData = data.Count()
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetIncomeExpenseById([FromRoute] string id)
    {
        var data = await _incomeExpenseRepository.GetIncomeExpenseDetail(id);
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil mendapatkan data",
            Data = data
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateIncomeExpense([FromForm] IncomeExpenseRequestVm incomeExpenseRequestVm)
    {
        var data = await _incomeExpenseRepository.CreateIncomeExpense(GetCredential(), incomeExpenseRequestVm);
        string info = data.Type ? "Pemasukan" : "Pengeluaran";
        return Created("api/v1/note/income-expense", new SingleDataResponse
        {
            Message = $"Berhasil menambah data {info}",
            Data = data
        });
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIncomeExpense([FromRoute] string id, [FromForm] IncomeExpenseRequestVm incomeExpenseRequestVm)
    {
        var data = await _incomeExpenseRepository.UpdateIncomeExpense(GetCredential(), id, incomeExpenseRequestVm);
        string info = data.Type ? "Pemasukan" : "Pengeluaran";
        return Ok(new SingleDataResponse
        {
            Message = $"Berhasil memperbarui data {info}",
            Data = data
        });
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIncomeExpense([FromRoute] string id)
    {
        await _incomeExpenseRepository.DeleteIncomeExpense(id);
        return Ok(new NoDataResponse
        {
            Message = "Berhasil menghapus data"
        });
    }
    
    private CredentialReq GetCredential()
    {
        var accountId = User.FindFirst("AccountId")?.Value;
        var storeId = User.FindFirst("StoreId")?.Value;
        return new CredentialReq
        {
            StoreId = storeId,
            AccountId = accountId,
        };
    }
}