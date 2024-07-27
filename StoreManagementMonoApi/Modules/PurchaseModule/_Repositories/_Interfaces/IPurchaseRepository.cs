using store_management_mono_api.Modules.PurchaseModule._ViewModels;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Modules.PurchaseModule._Repositories._Interfaces;

public interface IPurchaseRepository
{
    public Task<PurchaseResponseListVM> CreatePurchase(CredentialReq credentialReq, PurchaseRequestVM purchaseRequestVm,
        string status);
    public IEnumerable<PurchaseResponseListVM> GetAllPurchases(CredentialReq credentialReq, string? status,
        DateTime? startDate, DateTime? endDate);
    public PurchaseResponseVM GetPurchaseById(string id);

    public Task<PurchaseResponseListVM> PayDebt(CredentialReq credentialReq,
        PurchaseDebtRequestVM purchaseDebtRequestVm);
}