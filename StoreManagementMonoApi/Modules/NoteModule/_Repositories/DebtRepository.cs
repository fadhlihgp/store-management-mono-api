using Microsoft.EntityFrameworkCore;
using store_management_mono_api.Context;
using store_management_mono_api.Entities;
using store_management_mono_api.Exceptions;
using store_management_mono_api.Modules.NoteModule._Repositories._Interfaces;
using store_management_mono_api.Modules.NoteModule._ViewModels;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Modules.NoteModule._Repositories;

public class DebtRepository : IDebtRepository
{
    private readonly AppDbContext _appDbContext;

    public DebtRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task CreateDebt(CredentialReq credentialReq, DebtRequestVm debtRequestVm)
    {
        var findDebt = await _appDbContext.Debts.Where(d => d.CustomerId.Equals(debtRequestVm.CustomerId))
            .FirstOrDefaultAsync();
        try
        {
            using var transaction = _appDbContext.Database.BeginTransactionAsync();
            // If findDebt null create new debt
            if (findDebt == null)
            {
                var saveDebt = new Debt
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerId = debtRequestVm.CustomerId,
                    StoreId = credentialReq.StoreId,
                    DebtDetails = debtRequestVm.DebtDetails.Select(dd => new DebtDetail
                    {
                        Id = Guid.NewGuid().ToString(),
                        Date = dd.Date,
                        ProductId = dd.ProductId,
                        Count = dd.Count,
                        UnitProductId = dd.UnitProductId,
                        Price = dd.Price,
                        PriceTotal = dd.Price * dd.Count,
                        Note = dd.Note,
                        IsPaid = false,
                        CreatedAt = DateTime.Now,
                        CreatedBy = credentialReq.AccountId,
                        EditedAt = DateTime.Now,
                        EditedBy = credentialReq.AccountId,
                    }).ToList()
                };

                await _appDbContext.Debts.AddAsync(saveDebt);
            }
            // Else, update for add debt details
            else
            {
                findDebt.DebtDetails = debtRequestVm.DebtDetails.Select(dd => new DebtDetail
                {
                    Id = Guid.NewGuid().ToString(),
                    Date = dd.Date,
                    ProductId = dd.ProductId,
                    Count = dd.Count,
                    UnitProductId = dd.UnitProductId,
                    Price = dd.Price,
                    PriceTotal = dd.Price * dd.Count,
                    Note = dd.Note,
                    IsPaid = false,
                    CreatedAt = DateTime.Now,
                    CreatedBy = credentialReq.AccountId,
                    EditedAt = DateTime.Now,
                    EditedBy = credentialReq.AccountId,
                }).ToList();

                _appDbContext.Debts.Update(findDebt);
            }

            await _appDbContext.SaveChangesAsync();
            await _appDbContext.Database.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            await _appDbContext.Database.RollbackTransactionAsync();
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task CreateDebtOne(CredentialReq credentialReq, DebtDetailRequestVm debtRequestOneVm)
    {
        var findDebt = await _appDbContext.Debts.Where(d => d.CustomerId.Equals(debtRequestOneVm.CustomerId))
            .FirstOrDefaultAsync();
        
        try
        {
            if (findDebt == null)
            {
                findDebt = new Debt
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerId = debtRequestOneVm.CustomerId,
                    StoreId = credentialReq.StoreId,
                };
                _appDbContext.Debts.Add(findDebt);
            }

            var saveDebtDetail = new DebtDetail
            {
                Id = Guid.NewGuid().ToString(),
                Date = debtRequestOneVm.Date,
                ProductId = debtRequestOneVm.ProductId,
                Count = debtRequestOneVm.Count,
                UnitProductId = debtRequestOneVm.UnitProductId,
                Price = debtRequestOneVm.Price,
                PriceTotal = debtRequestOneVm.Price * debtRequestOneVm.Count,
                Note = debtRequestOneVm.Note,
                IsPaid = false,
                CreatedAt = DateTime.Now,
                CreatedBy = credentialReq.AccountId,
                EditedAt = DateTime.Now,
                EditedBy = credentialReq.AccountId,
                DebtId = findDebt.Id,
            };
            _appDbContext.DebtDetails.Add(saveDebtDetail);

            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<DebtResponseListVm>> GetDebts(string storeId)
    {
        try
        {
            var customersWithPaidDebtTotal = await _appDbContext.Debts
                .Where(d => d.StoreId == storeId)
                .Select(g => new
                {
                    g.Id,
                    g.Customer,
                    PriceTotal = g.DebtDetails.Where(dd => !dd.IsPaid).Sum(dd => dd.PriceTotal)
                })
                .ToListAsync();

            return customersWithPaidDebtTotal.Select(dr => new DebtResponseListVm
            {
                Id = dr.Id,
                CustomerName = dr.Customer.FullName,
                IsPaidOff = dr.PriceTotal <= 0,
                DebtAmount = dr.PriceTotal
            }).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<DebtResponseVm> GetDebtById(string debtId)
    {
        var getDebt = await _appDbContext.Debts.Where(d => d.Id.Equals(debtId))
            .Include(d => d.Customer).ThenInclude(customer => customer.CreatedByNavigation)
            .Include(debt => debt.Customer).ThenInclude(customer => customer.EditedByNavigation)
            .Include(d => d.DebtDetails).ThenInclude(debtDetail => debtDetail.Product)
            .Include(debt => debt.DebtDetails).ThenInclude(debtDetail => debtDetail.UnitProduct)
            .Include(debt => debt.DebtDetails).ThenInclude(debtDetail => debtDetail.CreatedByNavigation)
            .Include(debt => debt.DebtDetails).ThenInclude(debtDetail => debtDetail.EditedByNavigation)
            .Select(d => new
            {
                d.Id,
                d.Customer,
                d.CustomerId,
                d.DebtDetails,
                PriceTotal = d.DebtDetails.Where(dd => !dd.IsPaid).Sum(dd => dd.PriceTotal)
            })
            .FirstOrDefaultAsync();
        if (getDebt == null) throw new NotFoundException("Data hutang tidak ditemukan");
        
        try
        {
            return new DebtResponseVm
            {
                Id = getDebt.Id,
                PriceTotal = getDebt.PriceTotal,
                Customer = new CustomerResponseVm
                {
                    Id = getDebt.CustomerId,
                    FullName = getDebt.Customer.FullName,
                    PhoneNumber = getDebt.Customer.PhoneNumber,
                    Address = getDebt.Customer.Address,
                    Email = getDebt.Customer.Email,
                    CreatedAt = getDebt.Customer.CreatedAt,
                    CreatedBy = getDebt.Customer.CreatedByNavigation.FullName,
                    EditedAt = getDebt.Customer.EditedAt,
                    EditedBy = getDebt.Customer.EditedByNavigation.FullName
                },
                DebtDetails = getDebt.DebtDetails.OrderByDescending(dd => dd.Date).Select(dd => new DebtDetailResponseVm
                {
                    Id = dd.Id,
                    Date = dd.Date,
                    ProductId = dd.ProductId,
                    ProductName = dd.Product.Name,
                    Count = dd.Count,
                    UnitProductId = dd.UnitProductId,
                    UnitProductName = dd.UnitProduct.Name,
                    Price = dd.Price,
                    PriceTotal = dd.PriceTotal,
                    Note = dd.Note,
                    IsPaid = dd.IsPaid,
                    CreatedAt = dd.CreatedAt,
                    CreatedBy = dd.CreatedByNavigation.FullName,
                    EditedAt = dd.EditedAt,
                    EditedBy = dd.EditedByNavigation.FullName,
                    DebtId = dd.DebtId,
                    PayDate = dd.PayDate
                })
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<DebtDetailResponseVm> GetDebtDetailById(string debtDetailId)
    {
        var debtDetail = await _appDbContext.DebtDetails.Where(dd => dd.Id.Equals(debtDetailId))
            .Include(debtDetail => debtDetail.Product).Include(debtDetail => debtDetail.UnitProduct)
            .Include(debtDetail => debtDetail.CreatedByNavigation).Include(debtDetail => debtDetail.EditedByNavigation).FirstOrDefaultAsync();
        if (debtDetail == null) throw new NotFoundException("Data detail hutang tidak ditemukan");

        return new DebtDetailResponseVm
        {
            Id = debtDetail.Id,
            Date = debtDetail.Date,
            ProductId = debtDetail.ProductId,
            ProductName = debtDetail.Product.Name,
            Count = debtDetail.Count,
            UnitProductId = debtDetail.UnitProductId,
            UnitProductName = debtDetail.UnitProduct.Name,
            Price = debtDetail.Price,
            PriceTotal = debtDetail.PriceTotal,
            Note = debtDetail.Note,
            IsPaid = debtDetail.IsPaid,
            CreatedAt = debtDetail.CreatedAt,
            CreatedBy = debtDetail.CreatedByNavigation.FullName,
            EditedAt = debtDetail.EditedAt,
            EditedBy = debtDetail.EditedByNavigation.FullName,
            DebtId = debtDetail.DebtId,
            PayDate = debtDetail.PayDate
        };
    }

    public async Task UpdateDebtDetails(string debtDetailId, CredentialReq credentialReq, DebtDetailRequestVm detailRequestVm)
    {
        var debtDetailById = await _appDbContext.DebtDetails.FindAsync(debtDetailId);
        if (debtDetailById == null) throw new NotFoundException("Data detail hutang tidak ditemukan");
        try
        {
            debtDetailById.Count = detailRequestVm.Count;
            debtDetailById.Date = detailRequestVm.Date;
            debtDetailById.Note = detailRequestVm.Note;
            debtDetailById.Price = detailRequestVm.Price;
            debtDetailById.PriceTotal = detailRequestVm.Count * detailRequestVm.Price;
            debtDetailById.ProductId = detailRequestVm.ProductId;
            debtDetailById.UnitProductId = detailRequestVm.UnitProductId;
            debtDetailById.EditedBy = credentialReq.AccountId;
            debtDetailById.EditedAt = DateTime.Now;

            _appDbContext.Update(debtDetailById);
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteDebtDetails(string debtDetailId)
    {
        var debtDetailById = await _appDbContext.DebtDetails.FindAsync(debtDetailId);
        if (debtDetailById == null) throw new NotFoundException("Data detail hutang tidak ditemukan");
        try
        {
            _appDbContext.Remove(debtDetailById);
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}