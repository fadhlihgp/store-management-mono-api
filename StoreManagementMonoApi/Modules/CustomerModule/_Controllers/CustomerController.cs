using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using store_management_mono_api.Modules.CustomerModule._Repositories;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Modules.CustomerModule._Controllers;

[Authorize]
[ApiController]
[Route("api/v1/customer")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerRequestVm customerRequestVm)
    {
        var accountId = User.FindFirst("AccountId")?.Value;
        var storeId = User.FindFirst("StoreId")?.Value;
        var saveCustomer = await _customerRepository.CreateCustomer(accountId, storeId, customerRequestVm);
        return Created("api/v1/customer", new SingleDataResponse
        {
            Message = "Berhasil menambah data pelanggan",
            Data = saveCustomer
        });
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllCustomer()
    {
        var storeId = User.FindFirst("StoreId")?.Value;
        var customers = await _customerRepository.GetAllCustomer(storeId);
        return Ok(new MultipleDataResponse
        {
            Message = "Berhasil mendapatkan data pelanggan",
            Data = customers,
            TotalData = customers.Count()
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerById([FromRoute] string id)
    {
        var customer = await _customerRepository.GetCustomerById(id);
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil mendapatkan data pelanggan",
            Data = customer
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomerById([FromRoute] string id, [FromBody] CustomerRequestVm customerRequestVm)
    {
        var accountId = User.FindFirst("AccountId")?.Value;
        var data = await _customerRepository.UpdateCustomer(id, accountId, customerRequestVm );
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil memperbarui data pelanggan",
            Data = data
        });
    }
    
    [HttpPut("delete/{id}")]
    public async Task<IActionResult> DeleteCustomerById([FromRoute] string id)
    {
        var accountId = User.FindFirst("AccountId")?.Value;
        await _customerRepository.DeleteCustomer(id, accountId);
        return Ok(new NoDataResponse
        {
            Message = "Berhasil menghapus data pelanggan"
        });
    }
}