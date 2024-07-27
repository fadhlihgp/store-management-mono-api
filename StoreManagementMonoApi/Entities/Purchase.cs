using System;
using System.Collections.Generic;

namespace store_management_mono_api.Entities;

public partial class Purchase
{
    public string Id { get; set; } = null!;

    public string? CustomerId { get; set; }

    public DateTime Date { get; set; }

    public string Invoice { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? Note { get; set; }

    public string PurchaseTypeId { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? Payment { get; set; }

    public decimal Money { get; set; }

    public string StoreId { get; set; } = null!;

    public virtual Account CreatedByNavigation { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual PurchaseType PurchaseType { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
