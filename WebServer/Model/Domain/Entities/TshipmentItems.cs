using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TSHIPMENT_ITEMS")]
[Index("ShipmentId", Name = "IDX_TSHIPMENT_ITEMS_SHIPMENT_ID")]
public partial class TshipmentItems
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("SHIPMENT_ID")]
    public Guid ShipmentId { get; set; }

    [Column("ORDER_ITEM_ID")]
    public Guid OrderItemId { get; set; }

    [Column("QUANTITY")]
    [Precision(18, 6)]
    public decimal Quantity { get; set; }

    [ForeignKey("OrderItemId")]
    [InverseProperty("TshipmentItems")]
    public virtual TorderItems OrderItem { get; set; } = null!;

    [ForeignKey("ShipmentId")]
    [InverseProperty("TshipmentItems")]
    public virtual Tshipments Shipment { get; set; } = null!;

    [InverseProperty("ShipmentItem")]
    public virtual ICollection<TshipmentItemLots> TshipmentItemLots { get; set; } = new List<TshipmentItemLots>();
}
