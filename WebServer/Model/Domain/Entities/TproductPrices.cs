using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TPRODUCT_PRICES")]
[Index("ItemId", "SalesChannelId", "ValidFrom", Name = "IDX_TPRODUCT_PRICES_ITEM_TIME", IsDescending = new[] { false, false, true })]
public partial class TproductPrices
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("ITEM_ID")]
    public Guid ItemId { get; set; }

    [Column("SALES_CHANNEL_ID")]
    public Guid? SalesChannelId { get; set; }

    [Column("CURRENCY")]
    public string Currency { get; set; } = null!;

    [Column("PRICE")]
    [Precision(18, 2)]
    public decimal Price { get; set; }

    [Column("VALID_FROM")]
    public DateTime ValidFrom { get; set; }

    [Column("VALID_TO")]
    public DateTime? ValidTo { get; set; }

    [Column("CREATED_AT")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("TproductPrices")]
    public virtual Titems Item { get; set; } = null!;

    [ForeignKey("SalesChannelId")]
    [InverseProperty("TproductPrices")]
    public virtual TsalesChannels? SalesChannel { get; set; }
}
