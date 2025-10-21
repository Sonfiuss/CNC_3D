using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TROLES")]
[Index("Name", Name = "UQ_TROLES_NAME", IsUnique = true)]
public partial class Troles
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("NAME")]
    public string Name { get; set; } = null!;

    [Column("DESCRIPTION")]
    public string? Description { get; set; }

    [Column("CREATED_AT")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Role")]
    public virtual ICollection<Tpermissions> Permission { get; set; } = new List<Tpermissions>();

    [ForeignKey("RoleId")]
    [InverseProperty("Role")]
    public virtual ICollection<Tusers> User { get; set; } = new List<Tusers>();
}
