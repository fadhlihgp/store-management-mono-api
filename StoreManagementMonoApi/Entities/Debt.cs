using System;
using System.Collections.Generic;

namespace store_management_mono_api.Entities;

public partial class Debt
{
    public string Id { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

    public string StoreId { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<DebtDetail> DebtDetails { get; set; } = new List<DebtDetail>();

    public virtual Store Store { get; set; } = null!;
}
