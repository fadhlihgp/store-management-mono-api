using Microsoft.EntityFrameworkCore;
using store_management_mono_api.Context;
using store_management_mono_api.Entities;
using store_management_mono_api.Exceptions;
using store_management_mono_api.Modules.StoreModule._Repositories._Interfaces;
using store_management_mono_api.Modules.StoreModule._ViewModels;

namespace store_management_mono_api.Modules.StoreModule._Repositories;

public class StoreRepository : IStoreRepository
{
    private AppDbContext _context;

    public StoreRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateStore(StoreRequestVm storeRequestVm)
    {
        var findStoreByPhone = await _context.Stores.Where(s => s.PhoneNumber != null && s.PhoneNumber.Equals(storeRequestVm.PhoneNumber))
            .FirstOrDefaultAsync();
        if (findStoreByPhone != null)
            throw new BadRequestException("No ponsel sudah didaftarkan, silahkan cari yang lain");

        Store saveStore = new Store
        {
            Id = Guid.NewGuid().ToString(),
            Name = storeRequestVm.Name,
            Address = storeRequestVm.Address,
            PhoneNumber = storeRequestVm.PhoneNumber,
            RegisterDate = DateTime.Now,
            BusinessType = storeRequestVm.BusinessType,
            EstablishDate = storeRequestVm.EstablishDate,
            IsDeleted = false,
            LastEdited = DateTime.Now,
        };

        await _context.Stores.AddAsync(saveStore);
        await _context.SaveChangesAsync();
    }

    public async Task EditStore(string storeId, StoreRequestVm storeRequestVm)
    {
        var findStoreByPhone = await _context.Stores.Where(s => s.PhoneNumber != null && s.PhoneNumber.Equals(storeRequestVm.PhoneNumber) && !s.Id.Equals(storeId))
            .FirstOrDefaultAsync();
        if (findStoreByPhone != null)
            throw new BadRequestException("No ponsel sudah didaftarkan, silahkan cari yang lain");

        var saveStore = await GetById(storeId);
        saveStore.PhoneNumber = storeRequestVm.PhoneNumber;
        saveStore.Address = storeRequestVm.Address;
        saveStore.Name = storeRequestVm.Name;
        saveStore.LastEdited = DateTime.Now;
        saveStore.EstablishDate = storeRequestVm.EstablishDate;
        saveStore.BusinessType = storeRequestVm.BusinessType;

        try
        {
            _context.Stores.Update(saveStore);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<StoreResponseVm>> GetAllStore()
    {
        var stores = await _context.Stores.Where(s => !s.IsDeleted).ToListAsync();

        return stores.Select(s => new StoreResponseVm
        {
            Id = s.Id,
            Name = s.Name,
            Address = s.Address,
            PhoneNumber = s.PhoneNumber,
            RegisterDate = s.RegisterDate,
            BusinessType = s.BusinessType,
            EstablishDate = s.EstablishDate,
            LastEdited = s.LastEdited
        });
    }

    public async Task<StoreResponseVm> GetStoreById(string storeId)
    {
        var store = await GetById(storeId);
        return new StoreResponseVm
        {
            Id = store.Id,
            Name = store.Name,
            Address = store.Address,
            PhoneNumber = store.PhoneNumber,
            RegisterDate = store.RegisterDate,
            BusinessType = store.BusinessType,
            EstablishDate = store.EstablishDate,
            LastEdited = store.LastEdited
        };
    }

    public void DeleteStore(string storeId)
    {
        var findById = _context.Stores.Find(storeId);
        if (findById == null) throw new NotFoundException("Toko tidak ditemukan");

        findById.IsDeleted = true;
        _context.SaveChanges();
    }

    private async Task<Store> GetById(string storeId)
    {
        var findById = await _context.Stores.FindAsync(storeId);
        if (findById == null) throw new NotFoundException("Toko tidak ditemukan");
        return findById;
    }
}