using System;
using System.Collections.Generic;

namespace store_management_mono_api.Entities;

public partial class StockInOut
{
    public string Id { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public int Amount { get; set; }

    public string UnitProductId { get; set; } = null!;

    public bool Status { get; set; }

    public string? Note { get; set; }

    public string? SupplierId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime EditedAt { get; set; }

    public string EditedBy { get; set; } = null!;

    public virtual Account CreatedByNavigation { get; set; } = null!;

    public virtual Account EditedByNavigation { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Supplier? Supplier { get; set; }

    public virtual UnitProduct UnitProduct { get; set; } = null!;
}
