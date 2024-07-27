using Microsoft.EntityFrameworkCore;
using store_management_mono_api.Context;
using store_management_mono_api.Entities;
using store_management_mono_api.Exceptions;
using store_management_mono_api.Modules.ImageModule._Repositories._Interfaces;
using store_management_mono_api.Modules.ProductModule._Repositories._Interfaces;
using store_management_mono_api.Modules.ProductModule._ViewModels;

namespace store_management_mono_api.Modules.ProductModule._Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    private readonly IImageRepository _imageRepository;

    public ProductRepository(AppDbContext context, IImageRepository imageRepository)
    {
        _context = context;
        _imageRepository = imageRepository;
    }

    public async Task<ProductDetailResponseVm> CreateProduct(string accountId, string storeId, ProductRequestVm productRequestVm)
    {
        if (productRequestVm.Barcode != null)
        {
            var findBarcode = await _context.Products.Where(p =>
                p.Barcode != null && p.Barcode.Equals(productRequestVm.Barcode) && p.StoreId.Equals(storeId)).FirstOrDefaultAsync();
            if (findBarcode != null) throw new BadRequestException("Tidak boleh terdapat 2 barcode yang sama");
        }

        if (productRequestVm.ProductPrices == null || !productRequestVm.ProductPrices.Any())
        {
            throw new BadRequestException("Produk minimal terdapat 1 harga");
        }
        
        var productId = Guid.NewGuid().ToString();
        Product saveProduct = new Product
        {
            Id = productId,
            Name = productRequestVm.Name,
            Description = productRequestVm.Description,
            Stock = productRequestVm.Stock,
            Barcode = productRequestVm.Barcode,
            ImageId = productRequestVm.ImageId,
            CreatedAt = DateTime.Now,
            CreatedBy = accountId,
            EditedAt = DateTime.Now,
            EditedBy = accountId,
            StoreId = storeId,
            ProductPrices = productRequestVm.ProductPrices.Select(p => new ProductPrice
            {
                Id = Guid.NewGuid().ToString(),
                // ProductId = productId,
                Price = p.Price,
                UnitPriceId = p.UnitPriceId,
                QtyPcs = p.QtyPcs,
            }).ToList(),
        };

        try
        {
            var entityEntry = await _context.Products.AddAsync(saveProduct);
            var response = entityEntry.Entity;
            await _context.SaveChangesAsync();

            return new ProductDetailResponseVm
            {
                Id = response.Id,
                Name = response.Name,
                Description = response.Description,
                Stock = response.Stock,
                Barcode = response.Barcode,
                ImageId = response.ImageId,
                ImageUrl = response.Image?.PhotoUrl,
                CreatedAt = response.CreatedAt,
                // CreatedBy = response.CreatedByNavigation.FullName,
                EditedAt = response.EditedAt,
                // EditedBy = response.EditedByNavigation.FullName
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public async Task<ProductDetailResponseVm> UpdateProduct(string productId, string accountId, ProductRequestVm productRequestVm)
    {
        var findProduct = await _context.Products.FindAsync(productId);
        if (findProduct == null) throw new NotFoundException("Produk tidak ditemukan");

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                if (productRequestVm.ImageId != null)
                {
                    // If there is new imageId, will delete image data
                    if (!string.IsNullOrEmpty(findProduct.ImageId))
                    {
                        await _imageRepository.DeleteImage(findProduct.ImageId);
                    }
                    findProduct.ImageId = productRequestVm.ImageId;
                }

                // Delete all product prices
                var findProductPrices = await _context.ProductPrices
                    .Where(pp => pp.ProductId.Equals(productId))
                    .ToListAsync();
                _context.ProductPrices.RemoveRange(findProductPrices);

                // update product price
                findProduct.Name = productRequestVm.Name;
                findProduct.Barcode = productRequestVm.Barcode;
                findProduct.Description = productRequestVm.Description;
                findProduct.Stock = productRequestVm.Stock;
                findProduct.EditedAt = DateTime.Now;
                findProduct.EditedBy = accountId;

                // Add new product price
                findProduct.ProductPrices = productRequestVm.ProductPrices.Select(pp => new ProductPrice
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductId = findProduct.Id,
                    QtyPcs = pp.QtyPcs,
                    Price = pp.Price,
                    UnitPriceId = pp.UnitPriceId
                }).ToList();

                // Update product
                var entityEntry = _context.Products.Update(findProduct);
                await _context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();

                // Make Response
                var response = entityEntry.Entity;
                return new ProductDetailResponseVm
                {
                    Id = response.Id,
                    Name = response.Name,
                    Description = response.Description,
                    Stock = response.Stock,
                    Barcode = response.Barcode,
                    ImageId = response.ImageId,
                    ImageUrl = response.Image?.PhotoUrl,
                    CreatedAt = response.CreatedAt,
                    // CreatedBy = response.CreatedByNavigation.FullName,
                    EditedAt = response.EditedAt,
                    // EditedBy = response.EditedByNavigation.FullName
                };
            }
            catch (Exception e)
            {
                // Rollback transactions if failed
                await transaction.RollbackAsync();
                Console.WriteLine(e);
                throw;
            }
        }
}


    public async Task<IEnumerable<ProductDetailResponseVm>> GetProducts(string storeId)
    {
        var products = await _context.Products
            .Where(p => p.StoreId.Equals(storeId) && !p.IsDeleted)
            .Include(p => p.EditedByNavigation)
            .Include(p => p.CreatedByNavigation)
            .Include(p => p.Image)
            .Include(p => p.ProductPrices.OrderBy(pp => pp.UnitPriceId)).ThenInclude(pp => pp.UnitPrice)
            .OrderBy(p => p.Name)
            .ToListAsync();
        return products.Select(p => new ProductDetailResponseVm
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Stock = p.Stock,
            Barcode = p.Barcode,
            Price = p.ProductPrices.Select(pp => pp.Price).FirstOrDefault(),
            UnitProductId = p.ProductPrices.Select(pp => pp.UnitPrice.Id).FirstOrDefault(),
            Unit = p.ProductPrices.Select(pp => pp.UnitPrice.Name).FirstOrDefault(),
            ImageId = p.ImageId,
            ImageUrl = p.Image?.PhotoUrl,
            CreatedAt = p.CreatedAt,
            CreatedBy = p.CreatedByNavigation.FullName,
            EditedAt = p.EditedAt,
            EditedBy = p.EditedByNavigation.FullName,
            ProductPrices = p.ProductPrices.Select(pp => new ProductPriceResponseVm
            {
                Id = pp.Id,
                Price = pp.Price,
                UnitPriceId = pp.UnitPriceId,
                UnitPrice = pp.UnitPrice.Name,
                QtyPcs = pp.QtyPcs
            })
        });
    }

    public async Task<ProductResponseVm> GetProductById(string productId)
    {
        var product = await _context.Products
            .Where(p => p.Id.Equals(productId) && !p.IsDeleted)
            .Include(p => p.EditedByNavigation)
            .Include(p => p.CreatedByNavigation)
            .Include(p => p.Image)
            .Include(p => p.ProductPrices.OrderBy(pp => pp.UnitPriceId))
            .ThenInclude(pp => pp.UnitPrice)
            .FirstOrDefaultAsync();

        if (product == null) throw new NotFoundException("Produk tidak ditemukan");

        return new ProductResponseVm
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Stock = product.Stock,
            Barcode = product.Barcode,
            ImageUrl = product.Image?.PhotoUrl,
            CreatedAt = product.CreatedAt,
            CreatedBy = product.CreatedByNavigation.FullName,
            EditedAt = product.EditedAt,
            EditedBy = product.EditedByNavigation.FullName,
            ProductPrices = product.ProductPrices.Select(pp => new ProductPriceResponseVm
            {
                Id = pp.Id,
                Price = pp.Price,
                UnitPriceId = pp.UnitPriceId,
                UnitPrice = pp.UnitPrice.Name,
                QtyPcs = pp.QtyPcs
            })
        };
    }

    public async Task DeleteProduct(string productId, string accountId)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null) throw new NotFoundException("Produk tidak ditemukan");

        try
        {
            product.IsDeleted = true;
            product.DeletedAt = DateTime.Now;
            product.DeletedBy = accountId;

            _context.Update(product);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}