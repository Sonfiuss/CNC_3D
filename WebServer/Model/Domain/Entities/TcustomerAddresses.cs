using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[PrimaryKey("CustomerId", "AddressId")]
[Table("TCUSTOMER_ADDRESSES")]
public partial class TcustomerAddresses
{
    [Key]
    [Column("CUSTOMER_ID")]
    public Guid CustomerId { get; set; }

    [Key]
    [Column("ADDRESS_ID")]
    public Guid AddressId { get; set; }

    [Column("IS_DEFAULT")]
    public bool? IsDefault { get; set; }

    [ForeignKey("AddressId")]
    [InverseProperty("TcustomerAddresses")]
    public virtual Taddresses Address { get; set; } = null!;

    [ForeignKey("CustomerId")]
    [InverseProperty("TcustomerAddresses")]
    public virtual Tcustomers Customer { get; set; } = null!;
}
