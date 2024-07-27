using System;
using System.Collections.Generic;

namespace store_management_mono_api.Entities;

public partial class Supplier
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string StoreId { get; set; } = null!;

    public virtual ICollection<StockInOut> StockInOuts { get; set; } = new List<StockInOut>();

    public virtual Store Store { get; set; } = null!;
}
