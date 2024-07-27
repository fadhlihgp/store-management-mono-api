namespace store_management_mono_api.Modules.NoteModule._ViewModels;

public class DebtDetailRequestVm
{
    public string? CustomerId { get; set; }
    public DateTime Date { get; set; }
    public string ProductId { get; set; } = null!;
    public int Count { get; set; }
    public string UnitProductId { get; set; } = null!;
    public decimal Price { get; set; }
    // public decimal PriceTotal { get; set; }
    public string? Note { get; set; }
}

public class DebtDetailResponseVm
{
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public int Count { get; set; }
    public string UnitProductId { get; set; }
    public string UnitProductName { get; set; }
    public decimal Price { get; set; }
    public decimal PriceTotal { get; set; }
    public string? Note { get; set; }
    public bool IsPaid { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime EditedAt { get; set; }
    public string EditedBy { get; set; }
    public string DebtId { get; set; }
}