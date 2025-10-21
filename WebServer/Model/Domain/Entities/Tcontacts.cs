using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TCONTACTS")]
[Index("Email", Name = "IDX_TCONTACTS_EMAIL")]
[Index("Phone", Name = "IDX_TCONTACTS_PHONE")]
public partial class Tcontacts
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("FULL_NAME")]
    public string FullName { get; set; } = null!;

    [Column("EMAIL", TypeName = "citext")]
    public string? Email { get; set; }

    [Column("PHONE")]
    public string? Phone { get; set; }

    [Column("META", TypeName = "jsonb")]
    public string? Meta { get; set; }

    [Column("CREATED_AT")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Contact")]
    public virtual ICollection<Tcustomers> Tcustomers { get; set; } = new List<Tcustomers>();

    [InverseProperty("Contact")]
    public virtual ICollection<Tleads> Tleads { get; set; } = new List<Tleads>();
}
