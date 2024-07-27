using System;
using System.Collections.Generic;

namespace store_management_mono_api.Entities;

public partial class UnitProduct
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<DebtDetail> DebtDetails { get; set; } = new List<DebtDetail>();

    public virtual ICollection<ProductPrice> ProductPrices { get; set; } = new List<ProductPrice>();

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual ICollection<StockInOut> StockInOuts { get; set; } = new List<StockInOut>();
}
