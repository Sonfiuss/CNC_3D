using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TSHIPMENT_ITEM_LOTS")]
[Index("LotId", Name = "IDX_TSHIPMENT_ITEM_LOTS_LOT_ID")]
public partial class TshipmentItemLots
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("SHIPMENT_ITEM_ID")]
    public Guid ShipmentItemId { get; set; }

    [Column("LOT_ID")]
    public Guid LotId { get; set; }

    [Column("QUANTITY")]
    [Precision(18, 6)]
    public decimal Quantity { get; set; }

    [ForeignKey("LotId")]
    [InverseProperty("TshipmentItemLots")]
    public virtual TproductionLots Lot { get; set; } = null!;

    [ForeignKey("ShipmentItemId")]
    [InverseProperty("TshipmentItemLots")]
    public virtual TshipmentItems ShipmentItem { get; set; } = null!;
}
