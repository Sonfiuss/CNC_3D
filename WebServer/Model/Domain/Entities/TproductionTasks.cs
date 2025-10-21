using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TPRODUCTION_TASKS")]
public partial class TproductionTasks
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("WORK_ORDER_ID")]
    public Guid WorkOrderId { get; set; }

    [Column("NAME")]
    public string Name { get; set; } = null!;

    [Column("SEQUENCE_NO")]
    public int SequenceNo { get; set; }

    [Column("STARTED_AT")]
    public DateTime? StartedAt { get; set; }

    [Column("ENDED_AT")]
    public DateTime? EndedAt { get; set; }

    [Column("OPERATOR_ID")]
    public Guid? OperatorId { get; set; }

    [Column("NOTES")]
    public string? Notes { get; set; }

    [ForeignKey("OperatorId")]
    [InverseProperty("TproductionTasks")]
    public virtual Tusers? Operator { get; set; }

    [ForeignKey("WorkOrderId")]
    [InverseProperty("TproductionTasks")]
    public virtual TproductionWorkOrders WorkOrder { get; set; } = null!;
}
