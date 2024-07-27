using System;
using System.Collections.Generic;

namespace store_management_mono_api.Entities;

public partial class IncomeExpense
{
    public string Id { get; set; } = null!;

    public bool Type { get; set; }

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    public string? Note { get; set; }

    public string? ImageId { get; set; }

    public string StoreId { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime EditedAt { get; set; }

    public string EditedBy { get; set; } = null!;

    public virtual Account? CreatedByNavigation { get; set; }

    public virtual Account EditedByNavigation { get; set; } = null!;

    public virtual Image? Image { get; set; }

    public virtual Store Store { get; set; } = null!;
}
