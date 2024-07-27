using System;
using System.Collections.Generic;

namespace store_management_mono_api.Entities;

public partial class Image
{
    public string Id { get; set; } = null!;

    public string PhotoUrl { get; set; } = null!;

    public string? PublicId { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<IncomeExpense> IncomeExpenses { get; set; } = new List<IncomeExpense>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
