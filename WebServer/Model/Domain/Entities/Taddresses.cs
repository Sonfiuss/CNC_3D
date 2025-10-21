using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TADDRESSES")]
public partial class Taddresses
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("LINE1")]
    public string Line1 { get; set; } = null!;

    [Column("LINE2")]
    public string? Line2 { get; set; }

    [Column("CITY")]
    public string? City { get; set; }

    [Column("STATE")]
    public string? State { get; set; }

    [Column("POSTAL_CODE")]
    public string? PostalCode { get; set; }

    [Column("COUNTRY_CODE")]
    public string CountryCode { get; set; } = null!;

    [Column("META", TypeName = "jsonb")]
    public string? Meta { get; set; }

    [Column("CREATED_AT")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Address")]
    public virtual ICollection<TcustomerAddresses> TcustomerAddresses { get; set; } = new List<TcustomerAddresses>();

    [InverseProperty("BillingAddress")]
    public virtual ICollection<Torders> TordersBillingAddress { get; set; } = new List<Torders>();

    [InverseProperty("ShippingAddress")]
    public virtual ICollection<Torders> TordersShippingAddress { get; set; } = new List<Torders>();

    [InverseProperty("Address")]
    public virtual ICollection<Twarehouses> Twarehouses { get; set; } = new List<Twarehouses>();
}
