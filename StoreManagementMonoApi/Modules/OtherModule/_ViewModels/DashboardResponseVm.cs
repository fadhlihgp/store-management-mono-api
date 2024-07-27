namespace store_management_mono_api.Modules.OtherModule._ViewModels;

public class DashboardResponseVm
{
    public List<Stat> Stats { get; set; }
    public object TransactionTotals { get; set; }
}

public class Stat
{
    public string Message { get; set; }
    public object Value { get; set; }
}