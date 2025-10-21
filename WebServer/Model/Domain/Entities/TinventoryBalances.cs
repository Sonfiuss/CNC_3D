using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[PrimaryKey("WarehouseId", "ItemId", "LotId")]
[Table("TINVENTORY_BALANCES")]
[Index("UpdatedAt", Name = "IDX_TINVENTORY_BALANCES_UPDATED_AT", AllDescending = true)]
public partial class TinventoryBalances
{
    [Key]
    [Column("WAREHOUSE_ID")]
    public Guid WarehouseId { get; set; }

    [Key]
    [Column("ITEM_ID")]
    public Guid ItemId { get; set; }

    [Key]
    [Column("LOT_ID")]
    public Guid LotId { get; set; }

    [Column("QUANTITY_ON_HAND")]
    [Precision(18, 6)]
    public decimal QuantityOnHand { get; set; }

    [Column("QUANTITY_RESERVED")]
    [Precision(18, 6)]
    public decimal QuantityReserved { get; set; }

    [Column("UPDATED_AT")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("TinventoryBalances")]
    public virtual Titems Item { get; set; } = null!;

    [ForeignKey("LotId")]
    [InverseProperty("TinventoryBalances")]
    public virtual TproductionLots Lot { get; set; } = null!;

    [ForeignKey("WarehouseId")]
    [InverseProperty("TinventoryBalances")]
    public virtual Twarehouses Warehouse { get; set; } = null!;
}
