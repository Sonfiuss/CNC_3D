using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TSHIPMENTS")]
[Index("OrderId", Name = "IDX_TSHIPMENTS_ORDER_ID")]
public partial class Tshipments
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("ORDER_ID")]
    public Guid OrderId { get; set; }

    [Column("WAREHOUSE_ID")]
    public Guid? WarehouseId { get; set; }

    [Column("SHIPPED_AT")]
    public DateTime? ShippedAt { get; set; }

    [Column("CARRIER")]
    public string? Carrier { get; set; }

    [Column("TRACKING_NUMBER")]
    public string? TrackingNumber { get; set; }

    [Column("CREATED_AT")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("Tshipments")]
    public virtual Torders Order { get; set; } = null!;

    [InverseProperty("Shipment")]
    public virtual ICollection<TshipmentItems> TshipmentItems { get; set; } = new List<TshipmentItems>();

    [ForeignKey("WarehouseId")]
    [InverseProperty("Tshipments")]
    public virtual Twarehouses? Warehouse { get; set; }
}
