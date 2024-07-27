using Microsoft.EntityFrameworkCore;
using store_management_mono_api.Context;
using store_management_mono_api.Entities;
using store_management_mono_api.Exceptions;
using store_management_mono_api.Modules.PurchaseModule._Repositories._Interfaces;
using store_management_mono_api.Modules.PurchaseModule._ViewModels;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Modules.PurchaseModule._Repositories;

public class PurchaseRepository : IPurchaseRepository
{
    private readonly AppDbContext _appDbContext;

    public PurchaseRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    private async Task<string> CreateRandomInvoice(string storeId, string purchaseType)
    {
        string purchaseTypeStr = purchaseType.Equals("1") ? "OF" : (purchaseType.Equals("2") ? "DB" : "DV");
        int count = await _appDbContext.Purchases.CountAsync(p =>
            p.StoreId.Equals(storeId) && p.Status.Equals("Paid") && p.Date.Equals(DateTime.Today));
        string countStr = (count + 1).ToString("000");
        
        return $"INV-{purchaseTypeStr}{DateTime.Today:yyMMdd}{countStr}";
    }
    
    public async Task<PurchaseResponseListVM> CreatePurchase(CredentialReq credentialReq, PurchaseRequestVM purchaseRequestVm, string status)
    {
        // Create Invoices
        string invoice = status.ToLower().Equals("paid") ? await CreateRandomInvoice(credentialReq.StoreId, purchaseRequestVm.PurchaseTypeId) : "-";

        try
        {
            await _appDbContext.Database.BeginTransactionAsync();

            // Fetch all product prices in one query
            var productIds = purchaseRequestVm.PurchaseDetails.Select(pd => pd.ProductId).ToList();
            var unitPriceIds = purchaseRequestVm.PurchaseDetails.Select(pd => pd.UnitPriceId).ToList();
            var productPrices = await _appDbContext.ProductPrices
                .Where(pp => productIds.Contains(pp.ProductId) && unitPriceIds.Contains(pp.UnitPriceId))
                .ToDictionaryAsync(pp => new { pp.ProductId, pp.UnitPriceId });

            // Fetch all products in one query
            var products = await _appDbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            var savePurchase = new Purchase
            {
                Id = Guid.NewGuid().ToString(),
                CustomerId = purchaseRequestVm.CustomerId,
                Date = purchaseRequestVm.Date,
                Invoice = invoice,
                CreatedAt = DateTime.Now,
                CreatedBy = credentialReq.AccountId,
                Note = purchaseRequestVm.Note,
                PurchaseTypeId = purchaseRequestVm.PurchaseTypeId,
                Status = "Paid",
                Payment = purchaseRequestVm.Payment,
                Money = purchaseRequestVm.Money,
                StoreId = credentialReq.StoreId,
                PurchaseDetails = new List<PurchaseDetail>()
            };

            foreach (var pd in purchaseRequestVm.PurchaseDetails)
            {
                if (productPrices.TryGetValue(new { ProductId = pd.ProductId, pd.UnitPriceId  }, out var productPrice) &&
                    products.TryGetValue(pd.ProductId, out var product))
                {
                    var purchaseDetail = new PurchaseDetail
                    {
                        Id = Guid.NewGuid().ToString(),
                        Price = pd.Price,
                        Qty = pd.Qty,
                        UnitProductId = pd.UnitPriceId,
                        Total = pd.Qty * pd.Price,
                        ProductId = pd.ProductId,
                    };
                    savePurchase.PurchaseDetails.Add(purchaseDetail);

                    // Update stock in memory
                    product.Stock -= (productPrice.QtyPcs * pd.Qty);
                }
            }

            await _appDbContext.Purchases.AddAsync(savePurchase);
            _appDbContext.Products.UpdateRange(products.Values);
            await _appDbContext.SaveChangesAsync();
            await _appDbContext.Database.CommitTransactionAsync();

            return new PurchaseResponseListVM
            {
                Id = savePurchase.Id,
                Status = savePurchase.Status,
                PurchaseType = savePurchase.PurchaseTypeId,
                Invoice = invoice,
                Customer = savePurchase.CustomerId ?? "-",
                Date = savePurchase.Date,
                PurchaseTotal = purchaseRequestVm.PurchaseDetails.Sum(p => p.Price * p.Qty),
                Note = savePurchase.Note
            };
        }
        catch (Exception e)
        {
            await _appDbContext.Database.RollbackTransactionAsync();
            Console.WriteLine(e);
            throw;
        }
    }


    public IEnumerable<PurchaseResponseListVM> GetAllPurchases(CredentialReq credentialReq, string? status, DateTime? startDate, DateTime? endDate)
    {
        var purchases =
            _appDbContext.Purchases.AsNoTracking()
                .Where(p =>
                    p.StoreId.Equals(credentialReq.StoreId) && (status == null || p.Status.ToLower().Equals(status.ToLower()))
                    && (startDate == null || p.Date >= startDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999)) && (endDate == null || p.Date <= endDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999))
                )
                .Include(pp => pp.Customer)
                .Include(pp => pp.PurchaseDetails)
                .Include(purchase => purchase.PurchaseType)
                .OrderByDescending(pp => pp.Date)
                .ToList();
        
        return purchases.Select(pp => new PurchaseResponseListVM
        {
            Id = pp.Id,
            Status = pp.Status,
            PurchaseType = pp.PurchaseType.Name,
            Invoice = pp.Invoice,
            Customer = pp.Customer == null ? "Umum" : pp.Customer.FullName,
            Date = pp.Date,
            PurchaseTotal = pp.PurchaseDetails.Sum(pd => pd.Total),
            Note = pp.Note
        }).ToList();
    }

    public PurchaseResponseVM GetPurchaseById(string id)
    {
        var purchase = _appDbContext.Purchases.AsNoTracking()
            .Where(p => p.Id.Equals(id))
            .Include(p => p.Customer)
            .Include(p => p.PurchaseDetails).ThenInclude(purchaseDetail => purchaseDetail.Product)
            .Include(purchase => purchase.CreatedByNavigation).Include(purchase => purchase.PurchaseType)
            .Include(purchase => purchase.PurchaseDetails).ThenInclude(purchaseDetail => purchaseDetail.UnitProduct).FirstOrDefault();

        if (purchase == null) throw new NotFoundException("Transaksi pembelian tidak ditemukan");
        
        return new PurchaseResponseVM
        {
            Id = purchase.Id,
            Invoice = purchase.Invoice,
            CustomerId = purchase.CustomerId ?? "-",
            Customer = purchase.Customer == null ? "Umum" : purchase.Customer.FullName,
            Date = purchase.Date,
            CreatedAt = purchase.CreatedAt,
            CreatedBy = purchase.CreatedByNavigation.FullName,
            PurchaseType = purchase.PurchaseType.Name,
            PurchaseTypeId = purchase.PurchaseTypeId,
            Note = purchase.Note,
            Status = purchase.Status,
            Payment = purchase.Payment,
            PurchaseTotal = purchase.PurchaseDetails.Sum(pd => pd.Total),
            Money = purchase.Money,
            PurchaseDetails = purchase.PurchaseDetails.Select(pd => new PurchaseDetailResponseVM
            {
                Id = pd.Id,
                ProductId = pd.ProductId,
                ProductName = pd.Product.Name,
                Qty = pd.Qty,
                UnitPriceId = pd.UnitProductId,
                UnitPriceName = pd.UnitProduct.Name,
                Price = pd.Price ?? 0,
                Total = pd.Total
            })
        };
    }

    public async Task<PurchaseResponseListVM> PayDebt(CredentialReq credentialReq, PurchaseDebtRequestVM purchaseDebtRequestVm)
    {
        // Create Invoices
        string invoice = await CreateRandomInvoice(credentialReq.StoreId, "2");

        try
        {
            await _appDbContext.Database.BeginTransactionAsync();

            // Fetch all debt details in one query
            var debtDetails = await _appDbContext.DebtDetails
                .Where(dd => purchaseDebtRequestVm.DebtDetailIds.Contains(dd.Id)).ToListAsync();
            
            var productIds = debtDetails.Select(pd => pd.ProductId).ToList();
            var unitPriceIds = debtDetails.Select(pd => pd.UnitProductId).ToList();
            var productPrices = await _appDbContext.ProductPrices
                .Where(pp => productIds.Contains(pp.ProductId) && unitPriceIds.Contains(pp.UnitPriceId))
                .ToDictionaryAsync(pp => new { pp.ProductId, pp.UnitPriceId });

            // Fetch all products in one query
            var products = await _appDbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            var savePurchase = new Purchase
            {
                Id = Guid.NewGuid().ToString(),
                CustomerId = purchaseDebtRequestVm.CustomerId,
                Date = DateTime.Now,
                Invoice = invoice,
                CreatedAt = DateTime.Now,
                CreatedBy = credentialReq.AccountId,
                Note = purchaseDebtRequestVm.Note,
                PurchaseTypeId = "2",
                Status = "Paid",
                Payment = purchaseDebtRequestVm.Payment,
                Money = purchaseDebtRequestVm.Money,
                StoreId = credentialReq.StoreId,
                PurchaseDetails = new List<PurchaseDetail>()
            };

            foreach (var pd in debtDetails)
            {
                if (pd.IsPaid) throw new BadRequestException("Transaksi pembayaran tidak bisa diproses karena hutang sudah dibayar");
                if (productPrices.TryGetValue(new { ProductId = pd.ProductId, UnitPriceId = pd.UnitProductId  }, out var productPrice) &&
                    products.TryGetValue(pd.ProductId, out var product))
                {
                    var purchaseDetail = new PurchaseDetail
                    {
                        Id = Guid.NewGuid().ToString(),
                        Price = pd.Price,
                        Qty = pd.Count,
                        UnitProductId = pd.UnitProductId,
                        Total = pd.Count * pd.Price,
                        ProductId = pd.ProductId,
                        
                    };
                    savePurchase.PurchaseDetails.Add(purchaseDetail);

                    // Update stock in memory
                    product.Stock -= (productPrice.QtyPcs * pd.Count);
                }

                pd.IsPaid = true;
                pd.PayDate = DateTime.Now;
            }

            await _appDbContext.Purchases.AddAsync(savePurchase);
            _appDbContext.Products.UpdateRange(products.Values);
            await _appDbContext.SaveChangesAsync();
            await _appDbContext.Database.CommitTransactionAsync();

            return new PurchaseResponseListVM
            {
                Id = savePurchase.Id,
                Status = savePurchase.Status,
                PurchaseType = savePurchase.PurchaseTypeId,
                Invoice = invoice,
                Customer = savePurchase.CustomerId ?? "-",
                Date = savePurchase.Date,
                PurchaseTotal = debtDetails.Sum(p => p.Price * p.Count),
                Note = savePurchase.Note
            };
        }
        catch (Exception e)
        {
            await _appDbContext.Database.RollbackTransactionAsync();
            Console.WriteLine(e);
            throw;
        }
    }
}