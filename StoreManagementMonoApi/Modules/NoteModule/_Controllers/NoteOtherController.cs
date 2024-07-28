using Microsoft.AspNetCore.Mvc;
using store_management_mono_api.Modules.NoteModule._Repositories._Interfaces;
using store_management_mono_api.Modules.NoteModule._ViewModels;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Modules.NoteModule._Controllers;

[ApiController]
[Route("api/v1/note/other")]
public class NoteOtherController : ControllerBase
{
    private readonly INoteOtherRepository _noteOtherRepository;

    public NoteOtherController(INoteOtherRepository noteOtherRepository)
    {
        _noteOtherRepository = noteOtherRepository;
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
    
    [HttpPost]
    public async Task<IActionResult> CreateNoteOther([FromBody] NoteOtherRequestVm noteOtherRequestVm)
    {
        var noteOtherResponseVm = await _noteOtherRepository.CreateNoteOther(GetCredential(), noteOtherRequestVm);
        return Created("api/v1/note/other", new SingleDataResponse
        {
            Message = "Berhasil membuat data catatan",
            Data = noteOtherResponseVm
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllNotesOther()
    {
        var response = await _noteOtherRepository.GetNoteOthers(GetCredential().StoreId);
        return Ok(new MultipleDataResponse
        {
            Message = "Berhasil mendapat data catatan",
            Data = response,
            TotalData = response.Count()
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNoteById([FromRoute] string id)
    {
        var data = await _noteOtherRepository.GetNoteOtherById(id);
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil mendapat data catatan detail",
            Data = data
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNoteOther([FromRoute] string id,
        [FromBody] NoteOtherRequestVm noteOtherRequestVm)
    {
        var update = await _noteOtherRepository.UpdateNoteOther(id, GetCredential(), noteOtherRequestVm);
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil memperbarui data catatan",
            Data = update
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNoteOther([FromRoute] string id)
    {
        await _noteOtherRepository.DeleteNoteOther(id);
        return Ok(new NoDataResponse
        {
            Message = "Berhasil menghapus catatan"
        });
    }
}