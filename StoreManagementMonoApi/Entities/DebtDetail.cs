using System;
using System.Collections.Generic;

namespace store_management_mono_api.Entities;

public partial class DebtDetail
{
    public string Id { get; set; } = null!;

    public DateTime Date { get; set; }

    public string ProductId { get; set; } = null!;

    public int Count { get; set; }

    public string UnitProductId { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal PriceTotal { get; set; }

    public string? Note { get; set; }

    public bool IsPaid { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime EditedAt { get; set; }

    public string EditedBy { get; set; } = null!;

    public string DebtId { get; set; } = null!;

    public DateTime? PayDate { get; set; }

    public virtual Account CreatedByNavigation { get; set; } = null!;

    public virtual Debt Debt { get; set; } = null!;

    public virtual Account EditedByNavigation { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual UnitProduct UnitProduct { get; set; } = null!;
}
