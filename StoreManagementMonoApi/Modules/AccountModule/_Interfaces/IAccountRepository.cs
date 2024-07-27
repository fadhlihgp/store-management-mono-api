namespace store_management_mono_api.Modules.AccountModule._Interfaces;

public interface IAccountRepository
{
    public Task CreateAccount(string roleId, string storeId, AccountRequestVm requestVm);
    public Task EditAccount(string accountId, AccountRequestVm editRequestVm);
    public Task<IEnumerable<AccountResponseVm>> GetAllAccounts(string roleId, string storeId);
    public Task<AccountResponseVm> GetAccountById(string accountId);
    public Task DeleteAccount(string accountId);
    public Task ChangePassword(string accountId, ChangePasswordVm changePasswordVm);
}