using Microsoft.EntityFrameworkCore;
using store_management_mono_api.Context;
using store_management_mono_api.Entities;
using store_management_mono_api.Exceptions;

namespace store_management_mono_api.Modules.CustomerModule._Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _appDbContext;

    public CustomerRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Customer> CreateCustomer(string accountId, string storeId, CustomerRequestVm customerRequestVm)
    {
        try
        {
            var customerSave = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                FullName = customerRequestVm.FullName,
                PhoneNumber = customerRequestVm.PhoneNumber,
                Address = customerRequestVm.Address,
                Email = customerRequestVm.Email,
                StoreId = storeId,
                CreatedAt = DateTime.Now,
                CreatedBy = accountId,
                EditedAt = DateTime.Now,
                EditedBy = accountId,
            };

            var entityEntry = await _appDbContext.Customers.AddAsync(customerSave);
            await _appDbContext.SaveChangesAsync();
            return entityEntry.Entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<CustomerResponseVm>> GetAllCustomer(string storeId)
    {
        var customers = await _appDbContext.Customers.Where(c => c.StoreId.Equals(storeId) && !c.IsDeleted)
            .Include(c => c.EditedByNavigation)
            .Include(c => c.CreatedByNavigation)
            .OrderBy(c => c.FullName)
            .ToListAsync();

        return customers.Select(c => new CustomerResponseVm
        {
            Id = c.Id,
            FullName = c.FullName,
            PhoneNumber = c.PhoneNumber,
            Address = c.Address,
            Email = c.Email,
            CreatedAt = c.CreatedAt,
            CreatedBy = c.CreatedByNavigation.FullName,
            EditedAt = c.EditedAt,
            EditedBy = c.EditedByNavigation.FullName
        });
    }

    public async Task<CustomerResponseVm> GetCustomerById(string customerId)
    {
        var customerById = await _appDbContext.Customers.Where(c => c.Id.Equals(customerId) && !c.IsDeleted)
            .Include(c => c.CreatedByNavigation)
            .Include(c => c.EditedByNavigation)
            .FirstOrDefaultAsync();
        
        if (customerById == null) throw new NotFoundException("Pelanggan tidak ditemukan");
        return new CustomerResponseVm
        {
            Id = customerById.Id,
            FullName = customerById.FullName,
            PhoneNumber = customerById.PhoneNumber,
            Address = customerById.Address,
            Email = customerById.Email,
            CreatedAt = customerById.CreatedAt,
            CreatedBy = customerById.CreatedByNavigation.FullName,
            EditedAt = customerById.EditedAt,
            EditedBy = customerById.EditedByNavigation.FullName
        };
    }

    public async Task<Customer> UpdateCustomer(string customerId, string accountId, CustomerRequestVm customerRequestVm)
    {
        var customerById = await _appDbContext.Customers.FindAsync(customerId);
        if (customerById == null) throw new NotFoundException("Pelanggan tidak ditemukan");
        
        try
        {
            customerById.FullName = customerRequestVm.FullName;
            customerById.Email = customerRequestVm.Email;
            customerById.Address = customerRequestVm.Address;
            customerById.PhoneNumber = customerRequestVm.PhoneNumber;
            customerById.EditedAt = DateTime.Now;
            customerById.EditedBy = accountId;

            var entityEntry = _appDbContext.Update(customerById);
            await _appDbContext.SaveChangesAsync();
            return entityEntry.Entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteCustomer(string customerId, string accountId)
    {
        var customer = await _appDbContext.Customers.FindAsync(customerId);
        if (customer == null) throw new NotFoundException("Pelanggan tidak ditemukan");
        
        try
        {
            customer.IsDeleted = true;
            customer.DeletedAt = DateTime.Now;
            customer.DeletedBy = accountId;
            _appDbContext.Customers.Update(customer);
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}