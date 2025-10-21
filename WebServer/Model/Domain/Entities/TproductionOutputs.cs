using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TPRODUCTION_OUTPUTS")]
public partial class TproductionOutputs
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("WORK_ORDER_ID")]
    public Guid WorkOrderId { get; set; }

    [Column("ITEM_ID")]
    public Guid ItemId { get; set; }

    [Column("LOT_ID")]
    public Guid LotId { get; set; }

    [Column("WAREHOUSE_ID")]
    public Guid? WarehouseId { get; set; }

    [Column("QUANTITY_PRODUCED")]
    [Precision(18, 6)]
    public decimal QuantityProduced { get; set; }

    [Column("OCCURRED_AT")]
    public DateTime OccurredAt { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("TproductionOutputs")]
    public virtual Titems Item { get; set; } = null!;

    [ForeignKey("LotId")]
    [InverseProperty("TproductionOutputs")]
    public virtual TproductionLots Lot { get; set; } = null!;

    [ForeignKey("WarehouseId")]
    [InverseProperty("TproductionOutputs")]
    public virtual Twarehouses? Warehouse { get; set; }

    [ForeignKey("WorkOrderId")]
    [InverseProperty("TproductionOutputs")]
    public virtual TproductionWorkOrders WorkOrder { get; set; } = null!;
}
