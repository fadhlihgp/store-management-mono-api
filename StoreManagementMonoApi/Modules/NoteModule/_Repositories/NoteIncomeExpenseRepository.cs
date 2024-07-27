using Microsoft.EntityFrameworkCore;
using store_management_mono_api.Context;
using store_management_mono_api.Entities;
using store_management_mono_api.Exceptions;
using store_management_mono_api.Modules.ImageModule._Repositories._Interfaces;
using store_management_mono_api.Modules.NoteModule._Repositories._Interfaces;
using store_management_mono_api.Modules.NoteModule._ViewModels;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Modules.NoteModule._Repositories;

public class NoteIncomeExpenseRepository : INoteIncomeExpenseRepository
{

    private readonly AppDbContext _appDbContext;
    private readonly IImageRepository _imageRepository;

    public NoteIncomeExpenseRepository(AppDbContext appDbContext, IImageRepository imageRepository)
    {
        _appDbContext = appDbContext;
        _imageRepository = imageRepository;
    }

    public async Task<IncomeExpenseResponseVm> CreateIncomeExpense(CredentialReq credentialReq, IncomeExpenseRequestVm incomeExpenseRequestVm)
    {
        try
        {
            using (var transaction = await _appDbContext.Database.BeginTransactionAsync())
            {
                IncomeExpense saveIncomeExpense = new IncomeExpense();
                if (incomeExpenseRequestVm.Image != null && incomeExpenseRequestVm.Image.Length > 0)
                {
                    var saveImage = await _imageRepository.CreateImage(incomeExpenseRequestVm.Image);
                    saveIncomeExpense.ImageId = saveImage?.Id;
                }

                saveIncomeExpense.Id = Guid.NewGuid().ToString();
                saveIncomeExpense.Amount = incomeExpenseRequestVm.Amount;
                saveIncomeExpense.Date = incomeExpenseRequestVm.Date;
                saveIncomeExpense.Note = incomeExpenseRequestVm.Note;
                saveIncomeExpense.StoreId = credentialReq.StoreId;
                saveIncomeExpense.CreatedAt = DateTime.Now;
                saveIncomeExpense.CreatedBy = credentialReq.AccountId;
                saveIncomeExpense.EditedAt = DateTime.Now;
                saveIncomeExpense.EditedBy = credentialReq.AccountId;
                saveIncomeExpense.Type = incomeExpenseRequestVm.Type;

                var result = await _appDbContext.AddAsync(saveIncomeExpense);
                await _appDbContext.SaveChangesAsync();
                await _appDbContext.Database.CommitTransactionAsync();
                var response = result.Entity;
                return new IncomeExpenseResponseVm
                {
                    Id = response.Id,
                    Type = response.Type,
                    Date = response.Date,
                    Amount = response.Amount,
                    Note = response.Note,
                    ImageUrl = response.ImageId,
                    CreatedBy = response.CreatedBy,
                    CreatedAt = response.CreatedAt,
                    EditedBy = response.EditedBy,
                    EditedAt = response.EditedAt
                };
            }
        }
        catch (Exception e)
        {
            await _appDbContext.Database.RollbackTransactionAsync();
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<IncomeExpenseResponseVm>> GetIncomeExpenseList(string storeId)
    {
        var results = await _appDbContext.IncomeExpenses.Where(i => i.StoreId.Equals(storeId))
            .Include(i => i.EditedByNavigation)
            .Include(i => i.CreatedByNavigation)
            .Include(i => i.Image)
            .OrderByDescending(i => i.Date)
            .ToListAsync();

        return results.Select(rs => new IncomeExpenseResponseVm
        {
            Id = rs.Id,
            Type = rs.Type,
            Date = rs.Date,
            Amount = rs.Amount,
            Note = rs.Note,
            ImageUrl = rs.Image?.PhotoUrl,
            CreatedBy = rs.CreatedByNavigation.FullName,
            CreatedAt = rs.CreatedAt,
            EditedBy = rs.EditedByNavigation.FullName,
            EditedAt = rs.EditedAt
        });
    }

    public async Task<IncomeExpenseResponseVm> GetIncomeExpenseDetail(string id)
    {
        var incomeExpenseId = await _appDbContext.IncomeExpenses.Where(i => i.Id.Equals(id))
            .Include(i => i.EditedByNavigation)
            .Include(i => i.CreatedByNavigation)
            .Include(i => i.Image)
            .FirstOrDefaultAsync();
        if (incomeExpenseId == null) throw new NotFoundException("Data pengeluaran/pemasukan tidak ditemukan");
        return new IncomeExpenseResponseVm
        {
            Id = incomeExpenseId.Id,
            Type = incomeExpenseId.Type,
            Date = incomeExpenseId.Date,
            Amount = incomeExpenseId.Amount,
            Note = incomeExpenseId.Note,
            ImageUrl = incomeExpenseId.Image?.PhotoUrl,
            CreatedBy = incomeExpenseId.CreatedByNavigation.FullName,
            CreatedAt = incomeExpenseId.CreatedAt,
            EditedBy = incomeExpenseId.EditedByNavigation.FullName,
            EditedAt = incomeExpenseId.EditedAt
        };
    }

    public async Task<IncomeExpenseResponseVm> UpdateIncomeExpense(CredentialReq credentialReq, string id, IncomeExpenseRequestVm incomeExpenseRequestVm)
    {
        var updateIncomeExpense = await _appDbContext.IncomeExpenses.FindAsync(id);
        if (updateIncomeExpense == null) throw new NotFoundException("Data pemasukan/pengeluaran tidak ditemukan");
        try
        {
            using (var transaction = await _appDbContext.Database.BeginTransactionAsync())
            {
                if (incomeExpenseRequestVm.Image != null && incomeExpenseRequestVm.Image.Length > 0)
                {
                    if (updateIncomeExpense.ImageId != null)
                    {
                        await _imageRepository.DeleteImage(updateIncomeExpense.ImageId);
                    }

                    var image = await _imageRepository.CreateImage(incomeExpenseRequestVm.Image);
                    updateIncomeExpense.ImageId = image.Id;
                }

                updateIncomeExpense.Type = incomeExpenseRequestVm.Type;
                updateIncomeExpense.Amount = incomeExpenseRequestVm.Amount;
                updateIncomeExpense.EditedAt = DateTime.Now;
                updateIncomeExpense.EditedBy = credentialReq.AccountId;
                updateIncomeExpense.Note = incomeExpenseRequestVm.Note;

                var entityEntry = _appDbContext.IncomeExpenses.Update(updateIncomeExpense);
                await _appDbContext.SaveChangesAsync();
                await _appDbContext.Database.CommitTransactionAsync();
                return new IncomeExpenseResponseVm
                {
                    Id = entityEntry.Entity.Id,
                    Type = entityEntry.Entity.Type,
                    Date = entityEntry.Entity.Date,
                    Amount = entityEntry.Entity.Amount,
                    Note = entityEntry.Entity.Note,
                    ImageUrl = entityEntry.Entity.ImageId,
                    CreatedBy = entityEntry.Entity.CreatedBy,
                    CreatedAt = entityEntry.Entity.CreatedAt,
                    EditedBy = entityEntry.Entity.EditedBy,
                    EditedAt = entityEntry.Entity.EditedAt
                };
            }
        }
        catch (Exception e)
        {
            await _appDbContext.Database.RollbackTransactionAsync();
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteIncomeExpense(string id)
    {
        var incomeExpenseId = await _appDbContext.IncomeExpenses.Where(i => i.Id.Equals(id))
            .Include(i => i.EditedByNavigation)
            .Include(i => i.CreatedByNavigation)
            .Include(i => i.Image)
            .FirstOrDefaultAsync();
        if (incomeExpenseId == null) throw new NotFoundException("Data pengeluaran/pemasukan tidak ditemukan");
        try
        {
            using (var transaction = await _appDbContext.Database.BeginTransactionAsync())
            {
                _appDbContext.IncomeExpenses.Remove(incomeExpenseId);
                if (incomeExpenseId.ImageId != null)
                {
                    await _imageRepository.DeleteImage(incomeExpenseId.ImageId);
                }

                await _appDbContext.SaveChangesAsync();
                await _appDbContext.Database.CommitTransactionAsync();
            }
        }
        catch (Exception e)
        {
            await _appDbContext.Database.RollbackTransactionAsync();
            Console.WriteLine(e);
            throw;
        }
    }
}