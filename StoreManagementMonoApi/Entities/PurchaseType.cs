using System;
using System.Collections.Generic;

namespace store_management_mono_api.Entities;

public partial class PurchaseType
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
