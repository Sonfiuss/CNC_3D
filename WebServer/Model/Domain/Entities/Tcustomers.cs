using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TCUSTOMERS")]
[Index("ContactId", Name = "IDX_TCUSTOMERS_CONTACT_ID")]
[Index("UserId", Name = "IDX_TCUSTOMERS_USER_ID")]
public partial class Tcustomers
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("USER_ID")]
    public Guid? UserId { get; set; }

    [Column("CONTACT_ID")]
    public Guid ContactId { get; set; }

    [Column("NOTES")]
    public string? Notes { get; set; }

    [Column("CREATED_AT")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("ContactId")]
    [InverseProperty("Tcustomers")]
    public virtual Tcontacts Contact { get; set; } = null!;

    [InverseProperty("Customer")]
    public virtual ICollection<TcustomerAddresses> TcustomerAddresses { get; set; } = new List<TcustomerAddresses>();

    [InverseProperty("Customer")]
    public virtual ICollection<TorderFeedback> TorderFeedback { get; set; } = new List<TorderFeedback>();

    [InverseProperty("Customer")]
    public virtual ICollection<Torders> Torders { get; set; } = new List<Torders>();

    [ForeignKey("UserId")]
    [InverseProperty("Tcustomers")]
    public virtual Tusers? User { get; set; }
}
