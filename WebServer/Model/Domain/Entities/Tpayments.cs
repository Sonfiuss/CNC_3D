using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TPAYMENTS")]
public partial class Tpayments
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("ORDER_ID")]
    public Guid OrderId { get; set; }

    [Column("AMOUNT")]
    [Precision(18, 2)]
    public decimal Amount { get; set; }

    [Column("PAYMENT_METHOD")]
    public string? PaymentMethod { get; set; }

    [Column("TRANSACTION_REF")]
    public string? TransactionRef { get; set; }

    [Column("PAID_AT")]
    public DateTime? PaidAt { get; set; }

    [Column("CREATED_AT")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("Tpayments")]
    public virtual Torders Order { get; set; } = null!;
}
