using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TPRODUCTION_LOTS")]
[Index("ProductionDate", Name = "IDX_TPRODUCTION_LOTS_PRODUCTION_DATE")]
[Index("LotNumber", Name = "UQ_TPRODUCTION_LOTS_LOT_NUMBER", IsUnique = true)]
public partial class TproductionLots
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("LOT_NUMBER")]
    public string LotNumber { get; set; } = null!;

    [Column("WORK_ORDER_ID")]
    public Guid WorkOrderId { get; set; }

    [Column("ITEM_ID")]
    public Guid ItemId { get; set; }

    [Column("PRODUCTION_DATE")]
    public DateOnly ProductionDate { get; set; }

    [Column("EXPIRATION_DATE")]
    public DateOnly? ExpirationDate { get; set; }

    [Column("META", TypeName = "jsonb")]
    public string? Meta { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("TproductionLots")]
    public virtual Titems Item { get; set; } = null!;

    [InverseProperty("Lot")]
    public virtual ICollection<TinventoryBalances> TinventoryBalances { get; set; } = new List<TinventoryBalances>();

    [InverseProperty("Lot")]
    public virtual ICollection<TinventoryTransactions> TinventoryTransactions { get; set; } = new List<TinventoryTransactions>();

    [InverseProperty("Lot")]
    public virtual ICollection<TproductionOutputs> TproductionOutputs { get; set; } = new List<TproductionOutputs>();

    [InverseProperty("Lot")]
    public virtual ICollection<TshipmentItemLots> TshipmentItemLots { get; set; } = new List<TshipmentItemLots>();

    [ForeignKey("WorkOrderId")]
    [InverseProperty("TproductionLots")]
    public virtual TproductionWorkOrders WorkOrder { get; set; } = null!;
}
