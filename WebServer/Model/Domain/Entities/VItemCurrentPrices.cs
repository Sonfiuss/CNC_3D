using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Keyless]
public partial class VItemCurrentPrices
{
    [Column("ITEM_ID")]
    public Guid? ItemId { get; set; }

    [Column("SALES_CHANNEL_ID")]
    public Guid? SalesChannelId { get; set; }

    [Column("PRICE")]
    [Precision(18, 2)]
    public decimal? Price { get; set; }

    [Column("CURRENCY")]
    public string? Currency { get; set; }

    [Column("VALID_FROM")]
    public DateTime? ValidFrom { get; set; }

    [Column("VALID_TO")]
    public DateTime? ValidTo { get; set; }
}
