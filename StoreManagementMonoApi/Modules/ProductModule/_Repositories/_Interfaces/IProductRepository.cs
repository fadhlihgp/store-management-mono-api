using store_management_mono_api.Modules.ProductModule._ViewModels;

namespace store_management_mono_api.Modules.ProductModule._Repositories._Interfaces;

public interface IProductRepository
{
    Task<ProductDetailResponseVm> CreateProduct(string accountId, string storeId, ProductRequestVm productRequestVm);
    Task<ProductDetailResponseVm> UpdateProduct(string productId, string accountId, ProductRequestVm productRequestVm);
    Task<IEnumerable<ProductDetailResponseVm>> GetProducts(string storeId);
    Task<ProductResponseVm> GetProductById(string productId);
    Task DeleteProduct(string productId, string accountId);
}