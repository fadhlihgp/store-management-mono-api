using store_management_mono_api.Modules.OtherModule._ViewModels;

namespace store_management_mono_api.Modules.OtherModule._Repositories._Interfaces;

public interface IDashboardRepository
{
    Task<DashboardResponseVm> GetDashboard(string storeId, int? year);
}