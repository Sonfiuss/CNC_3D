using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TORDER_ITEMS")]
[Index("OrderId", Name = "IDX_TORDER_ITEMS_ORDER_ID")]
public partial class TorderItems
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("ORDER_ID")]
    public Guid OrderId { get; set; }

    [Column("ITEM_ID")]
    public Guid ItemId { get; set; }

    [Column("QUANTITY")]
    [Precision(18, 6)]
    public decimal Quantity { get; set; }

    [Column("UNIT_PRICE")]
    [Precision(18, 2)]
    public decimal UnitPrice { get; set; }

    [Column("DISCOUNT")]
    [Precision(18, 2)]
    public decimal Discount { get; set; }

    [Column("TAX_AMOUNT")]
    [Precision(18, 2)]
    public decimal TaxAmount { get; set; }

    [Column("LINE_TOTAL")]
    [Precision(18, 2)]
    public decimal LineTotal { get; set; }

    [Column("META", TypeName = "jsonb")]
    public string Meta { get; set; } = null!;

    [ForeignKey("ItemId")]
    [InverseProperty("TorderItems")]
    public virtual Titems Item { get; set; } = null!;

    [ForeignKey("OrderId")]
    [InverseProperty("TorderItems")]
    public virtual Torders Order { get; set; } = null!;

    [InverseProperty("OrderItem")]
    public virtual ICollection<TshipmentItems> TshipmentItems { get; set; } = new List<TshipmentItems>();
}
