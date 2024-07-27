using store_management_mono_api.Entities;

namespace store_management_mono_api.Modules.CustomerModule._Repositories;

public interface ICustomerRepository
{
    Task<Customer> CreateCustomer(string accountId, string storeId, CustomerRequestVm customerRequestVm);
    Task<IEnumerable<CustomerResponseVm>> GetAllCustomer(string storeId);
    Task<CustomerResponseVm> GetCustomerById(string customerId);
    Task<Customer> UpdateCustomer(string customerId, string accountId,
        CustomerRequestVm customerRequestVm);
    Task DeleteCustomer(string customerId, string accountId);
}