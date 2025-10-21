using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TPRODUCTION_WORK_ORDERS")]
public partial class TproductionWorkOrders
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("ITEM_ID")]
    public Guid ItemId { get; set; }

    [Column("QUANTITY")]
    [Precision(18, 6)]
    public decimal Quantity { get; set; }

    [Column("WAREHOUSE_ID")]
    public Guid? WarehouseId { get; set; }

    [Column("PLANNED_START_AT")]
    public DateTime? PlannedStartAt { get; set; }

    [Column("PLANNED_END_AT")]
    public DateTime? PlannedEndAt { get; set; }

    [Column("DESIGN_DOCUMENT_VERSION_ID")]
    public Guid? DesignDocumentVersionId { get; set; }

    [Column("META", TypeName = "jsonb")]
    public string Meta { get; set; } = null!;

    [Column("CREATED_AT")]
    public DateTime CreatedAt { get; set; }

    [Column("CREATED_BY")]
    public Guid? CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TproductionWorkOrders")]
    public virtual Tusers? CreatedByNavigation { get; set; }

    [ForeignKey("DesignDocumentVersionId")]
    [InverseProperty("TproductionWorkOrders")]
    public virtual TdesignDocumentVersions? DesignDocumentVersion { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("TproductionWorkOrders")]
    public virtual Titems Item { get; set; } = null!;

    [InverseProperty("WorkOrder")]
    public virtual ICollection<TproductionConsumptions> TproductionConsumptions { get; set; } = new List<TproductionConsumptions>();

    [InverseProperty("WorkOrder")]
    public virtual ICollection<TproductionDefects> TproductionDefects { get; set; } = new List<TproductionDefects>();

    [InverseProperty("WorkOrder")]
    public virtual ICollection<TproductionLots> TproductionLots { get; set; } = new List<TproductionLots>();

    [InverseProperty("WorkOrder")]
    public virtual ICollection<TproductionOutputs> TproductionOutputs { get; set; } = new List<TproductionOutputs>();

    [InverseProperty("WorkOrder")]
    public virtual ICollection<TproductionTasks> TproductionTasks { get; set; } = new List<TproductionTasks>();

    [ForeignKey("WarehouseId")]
    [InverseProperty("TproductionWorkOrders")]
    public virtual Twarehouses? Warehouse { get; set; }
}
