using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using store_management_mono_api.Exceptions;
using store_management_mono_api.Modules.StoreModule._Repositories._Interfaces;
using store_management_mono_api.Modules.StoreModule._ViewModels;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Modules.StoreModule._Controllers;

[Authorize]
[Controller]
[Route("api/v1/store")]
public class StoreController : ControllerBase
{
    private IStoreRepository _storeRepository;

    public StoreController(IStoreRepository storeRepository)
    {
        _storeRepository = storeRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateStore([FromBody] StoreRequestVm storeRequestVm)
    {
        var roleId = User.FindFirst("RoleId")?.Value;
        if (roleId != "1") throw new ForbiddenException("Akses dilarang");
        
        await _storeRepository.CreateStore(storeRequestVm);
        return Created("api/store", new NoDataResponse
        {
            Message = "Berhasil menambah toko baru"
        });
    }

    [HttpPut("{storeId}")]
    public async Task<IActionResult> UpdateStore([FromRoute] string storeId, [FromBody] StoreRequestVm storeRequestVm)
    {
        await _storeRepository.EditStore(storeId, storeRequestVm);
        return Ok(new NoDataResponse
        {
            Message = "Berhasil memperbarui data toko"
        });
    }

    [HttpPut("update-store")]
    public async Task<IActionResult> UpdateStoreByLogin([FromBody] StoreRequestVm storeRequestVm)
    {
        var storeId = User.FindFirst("StoreId")?.Value;
        await _storeRepository.EditStore(storeId, storeRequestVm);
        return Ok(new NoDataResponse
        {
            Message = "Berhasil memperbarui data toko"
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStores()
    {
        var storeResponseVms = await _storeRepository.GetAllStore();
        return Ok(new MultipleDataResponse
        {
            Message = "Berhasil mendapatkan data toko",
            Data = storeResponseVms,
            TotalData = storeResponseVms.Count()
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStoreById([FromRoute] string id)
    {
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil mengambil data",
            Data = await _storeRepository.GetStoreById(id)
        });
    }
    
    [HttpGet("current-store")]
    public async Task<IActionResult> GetCurrentStoreByLogin()
    {
        var storeId = User.FindFirst("StoreId")?.Value;
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil mengambil data",
            Data = await _storeRepository.GetStoreById(storeId)
        });
    }

    [HttpPut("delete/{id}")]
    public Task<IActionResult> DeleteStore([FromRoute] string id)
    {
        _storeRepository.DeleteStore(id);
        return Task.FromResult<IActionResult>(Ok(new NoDataResponse
        {
            Message = "Berhasil menghapus data toko"
        }));
    }
}