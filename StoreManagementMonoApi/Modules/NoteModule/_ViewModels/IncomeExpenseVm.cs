namespace store_management_mono_api.Modules.NoteModule._ViewModels;

public class IncomeExpenseRequestVm
{
    public bool Type { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }
    public IFormFile? Image { get; set; }
}

public class IncomeExpenseResponseVm
{
    public string Id { get; set; }
    public bool Type { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }
    public string? ImageUrl { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? EditedBy { get; set; }
    public DateTime EditedAt { get; set; }
}