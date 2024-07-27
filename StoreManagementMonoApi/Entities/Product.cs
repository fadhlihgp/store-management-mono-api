using System;
using System.Collections.Generic;

namespace store_management_mono_api.Entities;

public partial class Product
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Stock { get; set; }

    public string? Barcode { get; set; }

    public string? ImageId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime EditedAt { get; set; }

    public string EditedBy { get; set; } = null!;

    public DateTime? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public string StoreId { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public virtual Account CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<DebtDetail> DebtDetails { get; set; } = new List<DebtDetail>();

    public virtual Account? DeletedByNavigation { get; set; }

    public virtual Account EditedByNavigation { get; set; } = null!;

    public virtual Image? Image { get; set; }

    public virtual ICollection<ProductPrice> ProductPrices { get; set; } = new List<ProductPrice>();

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual ICollection<StockInOut> StockInOuts { get; set; } = new List<StockInOut>();

    public virtual Store Store { get; set; } = null!;
}
