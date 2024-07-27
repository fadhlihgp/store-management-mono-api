namespace store_management_mono_api.Modules.ProductModule._ViewModels;

public class ProductRequestVm
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int Stock { get; set; }
    public string? Barcode { get; set; }
    public string? ImageId { get; set; }
    public IEnumerable<ProductPriceRequestVm> ProductPrices { get; set; }
}

public class ProductResponseVm
{
    public string Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int Stock { get; set; }
    public string? Barcode { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime EditedAt { get; set; }
    public string EditedBy { get; set; }
    public IEnumerable<ProductPriceResponseVm> ProductPrices { get; set; }
}

public class ProductDetailResponseVm
{
    public string Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public string? UnitProductId { get; set; }
    public string? Unit { get; set; }
    public string? Barcode { get; set; }
    public string? ImageId { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime EditedAt { get; set; }
    public string EditedBy { get; set; }
    public IEnumerable<ProductPriceResponseVm>? ProductPrices { get; set; }
}