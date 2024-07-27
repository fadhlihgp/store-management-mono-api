using System;
using System.Collections.Generic;

namespace store_management_mono_api.Entities;

public partial class Customer
{
    public string Id { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string StoreId { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime EditedAt { get; set; }

    public string EditedBy { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Account CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Debt> Debts { get; set; } = new List<Debt>();

    public virtual Account? DeletedByNavigation { get; set; }

    public virtual Account EditedByNavigation { get; set; } = null!;

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual Store Store { get; set; } = null!;
}
