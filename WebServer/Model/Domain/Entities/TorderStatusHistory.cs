using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TORDER_STATUS_HISTORY")]
public partial class TorderStatusHistory
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("ORDER_ID")]
    public Guid OrderId { get; set; }

    [Column("CHANGED_AT")]
    public DateTime ChangedAt { get; set; }

    [Column("CHANGED_BY")]
    public Guid? ChangedBy { get; set; }

    [ForeignKey("ChangedBy")]
    [InverseProperty("TorderStatusHistory")]
    public virtual Tusers? ChangedByNavigation { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("TorderStatusHistory")]
    public virtual Torders Order { get; set; } = null!;
}
