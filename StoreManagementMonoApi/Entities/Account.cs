using System;
using System.Collections.Generic;

namespace store_management_mono_api.Entities;

public partial class Account
{
    public string Id { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string Password { get; set; } = null!;

    public DateTime? LastLogin { get; set; }

    public bool IsDeleted { get; set; }

    public bool? IsActive { get; set; }

    public string? StoreId { get; set; }

    public string? ImageId { get; set; }

    public string RoleId { get; set; } = null!;

    public virtual ICollection<Customer> CustomerCreatedByNavigations { get; set; } = new List<Customer>();

    public virtual ICollection<Customer> CustomerDeletedByNavigations { get; set; } = new List<Customer>();

    public virtual ICollection<Customer> CustomerEditedByNavigations { get; set; } = new List<Customer>();

    public virtual ICollection<DebtDetail> DebtDetailCreatedByNavigations { get; set; } = new List<DebtDetail>();

    public virtual ICollection<DebtDetail> DebtDetailEditedByNavigations { get; set; } = new List<DebtDetail>();

    public virtual Image? Image { get; set; }

    public virtual ICollection<IncomeExpense> IncomeExpenseCreatedByNavigations { get; set; } = new List<IncomeExpense>();

    public virtual ICollection<IncomeExpense> IncomeExpenseEditedByNavigations { get; set; } = new List<IncomeExpense>();

    public virtual ICollection<Note> NoteCreatedByNavigations { get; set; } = new List<Note>();

    public virtual ICollection<Note> NoteEditedByNavigations { get; set; } = new List<Note>();

    public virtual ICollection<Product> ProductCreatedByNavigations { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductDeletedByNavigations { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductEditedByNavigations { get; set; } = new List<Product>();

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<StockInOut> StockInOutCreatedByNavigations { get; set; } = new List<StockInOut>();

    public virtual ICollection<StockInOut> StockInOutEditedByNavigations { get; set; } = new List<StockInOut>();

    public virtual Store? Store { get; set; }
}
