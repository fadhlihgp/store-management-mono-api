using System;
using System.Collections.Generic;

namespace store_management_mono_api.Entities;

public partial class Store
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public DateTime RegisterDate { get; set; }

    public string? BusinessType { get; set; }

    public DateTime? EstablishDate { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime LastEdited { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Debt> Debts { get; set; } = new List<Debt>();

    public virtual ICollection<IncomeExpense> IncomeExpenses { get; set; } = new List<IncomeExpense>();

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}
