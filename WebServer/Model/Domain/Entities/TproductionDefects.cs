using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TPRODUCTION_DEFECTS")]
public partial class TproductionDefects
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("WORK_ORDER_ID")]
    public Guid WorkOrderId { get; set; }

    [Column("DEFECT_TYPE")]
    public string DefectType { get; set; } = null!;

    [Column("QUANTITY")]
    [Precision(18, 6)]
    public decimal Quantity { get; set; }

    [Column("DESCRIPTION")]
    public string? Description { get; set; }

    [Column("OCCURRED_AT")]
    public DateTime OccurredAt { get; set; }

    [ForeignKey("WorkOrderId")]
    [InverseProperty("TproductionDefects")]
    public virtual TproductionWorkOrders WorkOrder { get; set; } = null!;
}
