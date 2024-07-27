using System;
using System.Collections.Generic;

namespace store_management_mono_api.Entities;

public partial class ProductPrice
{
    public string Id { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public decimal Price { get; set; }

    public string UnitPriceId { get; set; } = null!;

    public int QtyPcs { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual UnitProduct UnitPrice { get; set; } = null!;
}
