using Microsoft.EntityFrameworkCore;
using store_management_mono_api.Context;
using store_management_mono_api.Entities;
using store_management_mono_api.Exceptions;
using store_management_mono_api.Modules.AccountModule._Interfaces;

namespace store_management_mono_api.Modules.AccountModule;

public class AccountRepository : IAccountRepository
{
    private AppDbContext _appDbContext;

    public AccountRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task CreateAccount(string roleId, string storeId, AccountRequestVm requestVm)
    {
        var findEmail = await _appDbContext.Accounts.Where(a => a.Email.Equals(requestVm.Email)).FirstOrDefaultAsync();
        if (findEmail != null) throw new BadRequestException("Email sudah digunakan, silakan cari email lain");

        var findUsername = await _appDbContext.Accounts.Where(a => a.Username.Equals(requestVm.Username))
            .FirstOrDefaultAsync();
        if (findUsername != null)
            throw new BadRequestException("Username sudah digunakan, silahkan cari username lain");
        
        Entities.Account saveAccount = new Entities.Account();
        saveAccount.Id = Guid.NewGuid().ToString();
        saveAccount.Email = requestVm.Email;
        saveAccount.Username = requestVm.Username;
        saveAccount.Password = BCrypt.Net.BCrypt.HashPassword(requestVm.Password);
        saveAccount.FullName = requestVm.FullName;
        saveAccount.IsActive = requestVm.IsActive;
        saveAccount.PhoneNumber = requestVm.PhoneNumber;
        saveAccount.IsDeleted = false;
        saveAccount.RoleId = requestVm.RoleId;
        saveAccount.StoreId = roleId.Equals("1") ? requestVm.StoreId : storeId;

        try
        {
            await _appDbContext.Accounts.AddAsync(saveAccount);
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task EditAccount(string accountId, AccountRequestVm requestVm)
    {
        var findEmail = await _appDbContext.Accounts.Where(a => a.Email.Equals(requestVm.Email) && !a.Id.Equals(accountId)).FirstOrDefaultAsync();
        if (findEmail != null) throw new BadRequestException("Email sudah digunakan, silakan cari email lain");

        var findUsername = await _appDbContext.Accounts.Where(a => a.Username.Equals(requestVm.Username) && !a.Id.Equals(accountId))
            .FirstOrDefaultAsync();
        if (findUsername != null)
            throw new BadRequestException("Username sudah digunakan, silahkan cari username lain");

        Account? editAccount = await _appDbContext.Accounts.FindAsync(accountId);
        if (editAccount == null) throw new NotFoundException("Akun tidak ditemukan");

        editAccount.Email = requestVm.Email;
        editAccount.Username = requestVm.Username;
        editAccount.Password = requestVm.Password != null && !requestVm.Password.Equals("")
            ? BCrypt.Net.BCrypt.HashPassword(requestVm.Password)
            : editAccount.Password;
        editAccount.FullName = requestVm.FullName;
        editAccount.IsActive = requestVm.IsActive;
        editAccount.PhoneNumber = requestVm.PhoneNumber;
        editAccount.RoleId = requestVm.RoleId ?? editAccount.RoleId;
        editAccount.StoreId = requestVm.StoreId ?? editAccount.StoreId;

        try
        {
            _appDbContext.Accounts.Update(editAccount);
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<AccountResponseVm>> GetAllAccounts(string roleId, string storeId)
    {
        List<Account>? listAsync;
        if (roleId.Equals("1"))
        {
            listAsync = await _appDbContext.Accounts.Where(a => !a.IsDeleted)
                .Include(account => account.Store).Include(account => account.Image)
                .Include(account => account.Role).ToListAsync();
        }
        else
        {
            listAsync = await _appDbContext.Accounts.Where(a => a.StoreId != null && a.StoreId.Equals(storeId) && !a.IsDeleted)
                .Include(account => account.Store).Include(account => account.Image).Include(account => account.Role).ToListAsync();
        }

        return listAsync.Select(a => new AccountResponseVm
        {
            Id = a.Id,
            Email = a.Email,
            Username = a.Username,
            FullName = a.FullName,
            PhoneNumber = a.PhoneNumber,
            LastLogin = a.LastLogin,
            IsActive = a.IsActive,
            StoreId = a.StoreId,
            StoreName = a.Store?.Name,
            ImageId = a.ImageId,
            ImageUrl = a.Image?.PhotoUrl,
            RoleId = a.RoleId,
            RoleName = a.Role.Name
        });
    }

    public async Task<AccountResponseVm> GetAccountById(string accountId)
    {
        var findAsync = await _appDbContext.Accounts.Include(a => a.Store)
            .Include(a => a.Image)
            .Include(a => a.Role)
            .Where(a => a.Id.Equals(accountId)).FirstOrDefaultAsync();
        if (findAsync == null) throw new NotFoundException("Akun tidak ditemukan");
        return new AccountResponseVm
        {
            Id = findAsync.Id,
            Email = findAsync.Email,
            Username = findAsync.Username,
            FullName = findAsync.FullName,
            PhoneNumber = findAsync.PhoneNumber,
            LastLogin = findAsync.LastLogin,
            IsActive = findAsync.IsActive,
            StoreId = findAsync.StoreId,
            StoreName = findAsync.Store?.Name,
            ImageId = findAsync.ImageId,
            ImageUrl = findAsync.Image?.PhotoUrl,
            RoleId = findAsync.RoleId,
            RoleName = findAsync.Role.Name
        };
    }

    public async Task DeleteAccount(string accountId)
    {
        var findAsync = await _appDbContext.Accounts.FindAsync(accountId);
        if (findAsync == null) throw new NotFoundException("Akun tidak ditemukan");
        try
        {
            findAsync.IsDeleted = true;
            _appDbContext.Update(findAsync);
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task ChangePassword(string accountId, ChangePasswordVm changePasswordVm)
    {
        var findAsync = await _appDbContext.Accounts.FindAsync(accountId);
        if (findAsync == null) throw new NotFoundException("Akun tidak ditemukan");
        bool isOldPassValid = BCrypt.Net.BCrypt.Verify(changePasswordVm.OldPassword, findAsync.Password);
        if (!isOldPassValid) throw new BadRequestException("Password lama tidak sesuai!");

        try
        {
            findAsync.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordVm.NewPassword);
            _appDbContext.Accounts.Update(findAsync);
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}