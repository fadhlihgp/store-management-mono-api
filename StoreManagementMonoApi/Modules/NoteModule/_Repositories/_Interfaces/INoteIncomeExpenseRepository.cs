using store_management_mono_api.Modules.NoteModule._ViewModels;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Modules.NoteModule._Repositories._Interfaces;

public interface INoteIncomeExpenseRepository
{
    Task<IncomeExpenseResponseVm> CreateIncomeExpense(CredentialReq credentialReq,
        IncomeExpenseRequestVm incomeExpenseRequestVm);

    Task<IEnumerable<IncomeExpenseResponseVm>> GetIncomeExpenseList(string storeId);
    Task<IncomeExpenseResponseVm> GetIncomeExpenseDetail(string id);

    Task<IncomeExpenseResponseVm> UpdateIncomeExpense(CredentialReq credentialReq, string id,
        IncomeExpenseRequestVm incomeExpenseRequestVm);

    Task DeleteIncomeExpense(string id);
}