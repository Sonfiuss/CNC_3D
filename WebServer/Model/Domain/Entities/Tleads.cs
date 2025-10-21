using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TLEADS")]
public partial class Tleads
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("SOURCE")]
    public string? Source { get; set; }

    [Column("CONTACT_ID")]
    public Guid ContactId { get; set; }

    [Column("STATUS")]
    public string? Status { get; set; }

    [Column("META", TypeName = "jsonb")]
    public string? Meta { get; set; }

    [Column("CREATED_AT")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("ContactId")]
    [InverseProperty("Tleads")]
    public virtual Tcontacts Contact { get; set; } = null!;
}
