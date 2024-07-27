using System;
using System.Collections.Generic;

namespace store_management_mono_api.Entities;

public partial class Note
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string StoreId { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime EditedAt { get; set; }

    public string? EditedBy { get; set; }

    public virtual Account? CreatedByNavigation { get; set; }

    public virtual Account? EditedByNavigation { get; set; }

    public virtual Store Store { get; set; } = null!;
}
