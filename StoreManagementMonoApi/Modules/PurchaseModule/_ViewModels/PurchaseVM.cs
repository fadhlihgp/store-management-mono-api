namespace store_management_mono_api.Modules.PurchaseModule._ViewModels;

public class PurchaseRequestVM
{
    public string? CustomerId { get; set; }
    public DateTime Date { get; set; }
    public string PurchaseTypeId { get; set; }
    public string? Note { get; set; }
    public string? Payment { get; set; }
    public decimal Money { get; set; }
    public IEnumerable<PurchaseDetailRequestVM> PurchaseDetails { get; set; }
}

public class PurchaseResponseListVM
{
    public string Id { get; set; }
    public string Status { get; set; }
    public string PurchaseType { get; set; }
    public string Invoice { get; set; }
    public string Customer { get; set; }
    public DateTime Date { get; set; }
    public decimal PurchaseTotal { get; set; }
    public string? Note { get; set; }
}

public class PurchaseResponseVM
{
    public string Id { get; set; }
    public string Invoice { get; set; }
    public string CustomerId { get; set; }
    public string Customer { get; set; }
    public DateTime Date { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string PurchaseType { get; set; }
    public string PurchaseTypeId { get; set; }
    public string Note { get; set; }
    public string Status { get; set; }
    public decimal PurchaseTotal { get; set; }
    public decimal Money { get; set; }
    public string Payment { get; set; }
    public IEnumerable<PurchaseDetailResponseVM> PurchaseDetails { get; set; }
}