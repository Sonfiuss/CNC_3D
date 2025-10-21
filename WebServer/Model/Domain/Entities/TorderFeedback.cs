using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TORDER_FEEDBACK")]
public partial class TorderFeedback
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("ORDER_ID")]
    public Guid OrderId { get; set; }

    [Column("CUSTOMER_ID")]
    public Guid? CustomerId { get; set; }

    [Column("RATING")]
    public int? Rating { get; set; }

    [Column("COMMENT")]
    public string? Comment { get; set; }

    [Column("CREATED_AT")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("TorderFeedback")]
    public virtual Tcustomers? Customer { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("TorderFeedback")]
    public virtual Torders Order { get; set; } = null!;
}
