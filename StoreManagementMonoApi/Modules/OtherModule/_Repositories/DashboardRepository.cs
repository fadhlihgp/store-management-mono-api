using Microsoft.EntityFrameworkCore;
using store_management_mono_api.Context;
using store_management_mono_api.Modules.OtherModule._Repositories._Interfaces;
using store_management_mono_api.Modules.OtherModule._ViewModels;

namespace store_management_mono_api.Modules.OtherModule._Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly AppDbContext _appDbContext;

    public DashboardRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<DashboardResponseVm> GetDashboard(string storeId, int? year)
    {
        year ??= DateTime.Now.Year;
        
        var data = await _appDbContext.Stores
            .Include(p => p.Purchases)
            .ThenInclude(pp => pp.PurchaseDetails)
            .Where(s => s.Id.Equals(storeId)).Select(s => new
            {
                customerCount = s.Customers.Count(c => !c.IsDeleted),
                productCount = s.Products.Count(p => !p.IsDeleted),
                purchaseSum = s.Purchases.Where(p => p.Status.ToLower().Equals("paid"))
                    .Select(p => p.PurchaseDetails.Sum(pd => pd.Total)),
                debtSum = s.Debts.Select(d => d.DebtDetails.Where(dd => !dd.IsPaid).Sum(dd => dd.PriceTotal)),
                purchases = s.Purchases.Where(p => p.Status.ToLower().Equals("paid") && p.Date.Year == year)
            }).AsNoTracking().FirstOrDefaultAsync();
            
        var months = Enumerable.Range(1, 12);
        var totals = months
            .Select(month => new
            {
                Month = month,
                Value = data.purchases
                    .Where(p => p.Date.Month == month)
                    .SelectMany(p => p.PurchaseDetails)
                    .Sum(pd => pd.Total)
            })
            .ToList();
        
        return new DashboardResponseVm
        {
            Stats = new List<Stat>
            {
                new Stat
                {
                    Message = "Jumlah Pelanggan",
                    Value = data.customerCount
                },
                new Stat
                {
                    Message = "Jumlah Produk",
                    Value = data.productCount
                },
                new Stat
                {
                    Message = "Total Transaksi ",
                    Value = data.purchaseSum.Sum()
                },
                new Stat
                {
                    Message = "Total Hutang",
                    Value = data.debtSum.Sum()
                }
            },
            TransactionTotals = totals
        };
        // var totals =  _appDbContext
    }
}