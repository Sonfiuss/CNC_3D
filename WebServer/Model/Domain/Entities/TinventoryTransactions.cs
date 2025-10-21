using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TINVENTORY_TRANSACTIONS")]
[Index("ItemId", "OccurredAt", Name = "IDX_TINVENTORY_TRANSACTIONS_ITEM_TIME", IsDescending = new[] { false, true })]
public partial class TinventoryTransactions
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("WAREHOUSE_ID")]
    public Guid WarehouseId { get; set; }

    [Column("ITEM_ID")]
    public Guid ItemId { get; set; }

    [Column("LOT_ID")]
    public Guid? LotId { get; set; }

    [Column("QTY_CHANGE")]
    [Precision(18, 6)]
    public decimal QtyChange { get; set; }

    [Column("REASON")]
    public string Reason { get; set; } = null!;

    [Column("REFERENCE_TYPE")]
    public string? ReferenceType { get; set; }

    [Column("REFERENCE_ID")]
    public Guid? ReferenceId { get; set; }

    [Column("OCCURRED_AT")]
    public DateTime OccurredAt { get; set; }

    [Column("CREATED_BY")]
    public Guid? CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TinventoryTransactions")]
    public virtual Tusers? CreatedByNavigation { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("TinventoryTransactions")]
    public virtual Titems Item { get; set; } = null!;

    [ForeignKey("LotId")]
    [InverseProperty("TinventoryTransactions")]
    public virtual TproductionLots? Lot { get; set; }

    [ForeignKey("WarehouseId")]
    [InverseProperty("TinventoryTransactions")]
    public virtual Twarehouses Warehouse { get; set; } = null!;
}
