using System;
using System.Collections.Generic;

namespace store_management_mono_api.Entities;

public partial class Role
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
