using store_management_mono_api.Modules.StoreModule._ViewModels;

namespace store_management_mono_api.Modules.StoreModule._Repositories._Interfaces;

public interface IStoreRepository
{
    public Task CreateStore(StoreRequestVm storeRequestVm);
    public Task EditStore(string storeId, StoreRequestVm storeRequestVm);
    public Task<IEnumerable<StoreResponseVm>> GetAllStore();
    public Task<StoreResponseVm> GetStoreById(string storeId);
    public void DeleteStore(string storeId);
}