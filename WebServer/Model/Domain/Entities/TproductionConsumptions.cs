using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TPRODUCTION_CONSUMPTIONS")]
public partial class TproductionConsumptions
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("WORK_ORDER_ID")]
    public Guid WorkOrderId { get; set; }

    [Column("ITEM_ID")]
    public Guid ItemId { get; set; }

    [Column("WAREHOUSE_ID")]
    public Guid? WarehouseId { get; set; }

    [Column("QUANTITY_USED")]
    [Precision(18, 6)]
    public decimal QuantityUsed { get; set; }

    [Column("OCCURRED_AT")]
    public DateTime OccurredAt { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("TproductionConsumptions")]
    public virtual Titems Item { get; set; } = null!;

    [ForeignKey("WarehouseId")]
    [InverseProperty("TproductionConsumptions")]
    public virtual Twarehouses? Warehouse { get; set; }

    [ForeignKey("WorkOrderId")]
    [InverseProperty("TproductionConsumptions")]
    public virtual TproductionWorkOrders WorkOrder { get; set; } = null!;
}
