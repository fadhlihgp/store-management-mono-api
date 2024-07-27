using System;
using System.Collections.Generic;

namespace store_management_mono_api.Entities;

public partial class PurchaseDetail
{
    public string Id { get; set; } = null!;

    public decimal? Price { get; set; }

    public int Qty { get; set; }

    public string UnitProductId { get; set; } = null!;

    public int? ColumnName { get; set; }

    public decimal Total { get; set; }

    public string ProductId { get; set; } = null!;

    public string PurchaseId { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Purchase Purchase { get; set; } = null!;

    public virtual UnitProduct UnitProduct { get; set; } = null!;
}
