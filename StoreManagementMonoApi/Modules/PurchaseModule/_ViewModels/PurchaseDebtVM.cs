namespace store_management_mono_api.Modules.PurchaseModule._ViewModels;

public class PurchaseDebtRequestVM
{
    public string CustomerId { get; set; }
    public string Payment { get; set; }
    public string? Note { get; set; }
    public decimal Money { get; set; }
    public IEnumerable<string> DebtDetailIds { get; set; }
    // public IEnumerable<PurchaseDebtDetailRequestVM> PurchaseDebtDetails { get; set; }
}

public class PurchaseDebtDetailRequestVM 
{
    public string DebtDetailId { get; set; } = null!;
    public string ProductId { get; set; }
    public int Qty { get; set; }
    public string UnitPriceId { get; set; }
    public decimal Price { get; set; }
}