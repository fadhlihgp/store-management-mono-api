namespace store_management_mono_api.Modules.NoteModule._ViewModels;

public class DebtRequestVm
{
    public string CustomerId { get; set; }
    public IEnumerable<DebtDetailRequestVm> DebtDetails { get; set; }
}

// public class DebtRequestOneVm
// {
//     public string CustomerId { get; set; }
//     public DebtDetailRequestVm DebtDetail { get; set; }
// }

public class DebtResponseListVm
{
    public string Id { get; set; }
    public string CustomerName { get; set; }
    public bool IsPaidOff { get; set; }
    public decimal DebtAmount { get; set; }
}

public class DebtResponseVm
{
    public string Id { get; set; }
    public decimal PriceTotal { get; set; }
    public CustomerResponseVm Customer { get; set; }
    public IEnumerable<DebtDetailResponseVm> DebtDetails { get; set; }
}