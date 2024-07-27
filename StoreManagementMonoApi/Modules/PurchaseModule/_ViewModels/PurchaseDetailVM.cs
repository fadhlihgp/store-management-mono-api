namespace store_management_mono_api.Modules.PurchaseModule._ViewModels;

public class PurchaseDetailRequestVM
{
    public string ProductId { get; set; }
    public int Qty { get; set; }
    public string UnitPriceId { get; set; }
    public decimal Price { get; set; }
}

public class PurchaseDetailResponseVM
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public int Qty { get; set; }
    public string UnitPriceId { get; set; }
    public string UnitPriceName { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get; set; }
}