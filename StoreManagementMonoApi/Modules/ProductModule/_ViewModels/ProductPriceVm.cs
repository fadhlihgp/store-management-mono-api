namespace store_management_mono_api.Modules.ProductModule._ViewModels;

public class ProductPriceRequestVm
{
    public decimal Price { get; set; }
    public string UnitPriceId { get; set; } = null!;
    public int QtyPcs { get; set; }
}

public class ProductPriceResponseVm
{
    public string Id { get; set; }
    public decimal Price { get; set; }
    public string UnitPriceId { get; set; }
    public string UnitPrice { get; set; }
    public int QtyPcs { get; set; }
}
