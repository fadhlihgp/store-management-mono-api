using store_management_mono_api.Modules.NoteModule._ViewModels;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Modules.NoteModule._Repositories._Interfaces;

public interface IDebtRepository
{
    public Task CreateDebt(CredentialReq credentialReq, DebtRequestVm debtRequestVm);
    public Task CreateDebtOne(CredentialReq credentialReq, DebtDetailRequestVm debtRequestOneVm);
    public Task<IEnumerable<DebtResponseListVm>> GetDebts(string storeId);
    public Task<DebtResponseVm> GetDebtById(string debtId);
    public Task<DebtDetailResponseVm> GetDebtDetailById(string debtDetailId);
    public Task UpdateDebtDetails(string debtDetailId, CredentialReq credentialReq,
        DebtDetailRequestVm detailRequestVm);
    public Task DeleteDebtDetails(string debtDetailId);
}