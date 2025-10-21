using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TORDERS")]
[Index("CustomerId", Name = "IDX_TORDERS_CUSTOMER_ID")]
[Index("OrderDate", Name = "IDX_TORDERS_ORDER_DATE", AllDescending = true)]
[Index("OrderNumber", Name = "UQ_TORDERS_ORDER_NUMBER", IsUnique = true)]
public partial class Torders
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("ORDER_NUMBER")]
    public string OrderNumber { get; set; } = null!;

    [Column("CUSTOMER_ID")]
    public Guid? CustomerId { get; set; }

    [Column("SALES_CHANNEL_ID")]
    public Guid? SalesChannelId { get; set; }

    [Column("ORDER_DATE")]
    public DateTime OrderDate { get; set; }

    [Column("REQUIRED_BY")]
    public DateTime? RequiredBy { get; set; }

    [Column("BILLING_ADDRESS_ID")]
    public Guid? BillingAddressId { get; set; }

    [Column("SHIPPING_ADDRESS_ID")]
    public Guid? ShippingAddressId { get; set; }

    [Column("CURRENCY")]
    public string Currency { get; set; } = null!;

    [Column("TOTAL_AMOUNT")]
    [Precision(18, 2)]
    public decimal TotalAmount { get; set; }

    [Column("NOTES")]
    public string? Notes { get; set; }

    [Column("META", TypeName = "jsonb")]
    public string Meta { get; set; } = null!;

    [Column("CREATED_AT")]
    public DateTime CreatedAt { get; set; }

    [Column("UPDATED_AT")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("BillingAddressId")]
    [InverseProperty("TordersBillingAddress")]
    public virtual Taddresses? BillingAddress { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Torders")]
    public virtual Tcustomers? Customer { get; set; }

    [ForeignKey("SalesChannelId")]
    [InverseProperty("Torders")]
    public virtual TsalesChannels? SalesChannel { get; set; }

    [ForeignKey("ShippingAddressId")]
    [InverseProperty("TordersShippingAddress")]
    public virtual Taddresses? ShippingAddress { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<TorderFeedback> TorderFeedback { get; set; } = new List<TorderFeedback>();

    [InverseProperty("Order")]
    public virtual ICollection<TorderItems> TorderItems { get; set; } = new List<TorderItems>();

    [InverseProperty("Order")]
    public virtual ICollection<TorderStatusHistory> TorderStatusHistory { get; set; } = new List<TorderStatusHistory>();

    [InverseProperty("Order")]
    public virtual ICollection<Tpayments> Tpayments { get; set; } = new List<Tpayments>();

    [InverseProperty("Order")]
    public virtual ICollection<Tshipments> Tshipments { get; set; } = new List<Tshipments>();
}
