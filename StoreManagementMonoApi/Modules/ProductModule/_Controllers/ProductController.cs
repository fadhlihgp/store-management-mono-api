using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using store_management_mono_api.Modules.ProductModule._Repositories._Interfaces;
using store_management_mono_api.Modules.ProductModule._ViewModels;
using store_management_mono_api.ViewModels;

namespace store_management_mono_api.Modules.ProductModule._Controllers;

[Authorize]
[ApiController]
[Route("api/v1/product")]
public class ProductController : ControllerBase
{
    private IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductRequestVm productRequestVm)
    {
        var accountId = User.FindFirst("AccountId")?.Value;
        var storeId = User.FindFirst("StoreId")?.Value;
        var productDetailResponseVm = await _productRepository.CreateProduct(accountId, storeId, productRequestVm);
        return Created("api/v1/product", new
        {
            Message = "Berhasil menambah data produk",
            Data = productDetailResponseVm
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var storeId = User.FindFirst("StoreId")?.Value;
        var data = await _productRepository.GetProducts(storeId);
        return Ok(new MultipleDataResponse
        {
            Message = "Berhasil mengambil data produk",
            Data = data,
            TotalData = data.Count()
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductById([FromRoute] string id, [FromBody] ProductRequestVm productRequestVm)
    {
        var accountId = User.FindFirst("AccountId")?.Value;
        var updateProduct = await _productRepository.UpdateProduct(id, accountId, productRequestVm);
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil memperbarui data produk",
            Data = updateProduct
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById([FromRoute] string id)
    {
        var data = await _productRepository.GetProductById(id);
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil mengambil data produk",
            Data = data
        });
    }

    [HttpPut("delete/{id}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] string id)
    {
        var accountId = User.FindFirst("AccountId")?.Value;
        await _productRepository.DeleteProduct(id, accountId);
        return Ok(new NoDataResponse
        {
            Message = "Berhasil menghapus produk"
        });
    }
}