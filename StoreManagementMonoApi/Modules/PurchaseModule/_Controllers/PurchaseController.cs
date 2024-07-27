using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using store_management_mono_api.Modules.PurchaseModule._Repositories._Interfaces;
using store_management_mono_api.Modules.PurchaseModule._ViewModels;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Modules.PurchaseModule._Controllers;

[ApiController]
[Authorize]
[Route("api/v1/purchase")]
public class PurchaseController : ControllerBase
{
    private readonly IPurchaseRepository _purchaseRepository;

    public PurchaseController(IPurchaseRepository purchaseRepository)
    {
        _purchaseRepository = purchaseRepository;
    }

    private CredentialReq GetCredentialReq()
    {
        return new CredentialReq
        {
            StoreId = User.Claims.First(c => c.Type == "StoreId").Value,
            AccountId = User.Claims.First(c => c.Type == "AccountId").Value
        };
    }
    
    [HttpPost("{status}")]
    public async Task<IActionResult> CreatePurchase([FromRoute] string status, [FromBody] PurchaseRequestVM purchaseRequestVm)
    {
        var save = await _purchaseRepository.CreatePurchase(GetCredentialReq(), purchaseRequestVm, status);
        return Created("api/v1/purchase", new SingleDataResponse
        {
            Message = "Berhasil membuat transaksi",
            Data = save
        });
    }

    [HttpGet]
    public IActionResult GetAllPurchases([FromQuery] string? status, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var data = _purchaseRepository.GetAllPurchases(GetCredentialReq(), status, startDate, endDate);
        return Ok(new MultipleDataResponse
        {
            Message = "Berhasil mendapatkan data transaksi",
            Data = data,
            TotalData = data.Count()
        });
    }
    
    [HttpGet("{id}")]

    public IActionResult GetPurchaseById([FromRoute] string id)
    {
        var purchase = _purchaseRepository.GetPurchaseById(id);
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil mendapatkan data transaksi",
            Data = purchase
        });
    }

    [HttpPost("pay-debt")]
    public async Task<IActionResult> PayDebt([FromBody] PurchaseDebtRequestVM purchaseDebtRequestVm)
    {
        var purchaseDebt = await _purchaseRepository.PayDebt(GetCredentialReq(), purchaseDebtRequestVm);
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil membayar hutang",
            Data = purchaseDebt
        });
    }
}