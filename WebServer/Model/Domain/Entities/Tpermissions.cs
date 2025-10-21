using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TPERMISSIONS")]
[Index("Resource", "Action", Name = "UQ_TPERMISSIONS_RESOURCE_ACTION", IsUnique = true)]
public partial class Tpermissions
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("RESOURCE")]
    public string Resource { get; set; } = null!;

    [Column("ACTION")]
    public string Action { get; set; } = null!;

    [Column("DESCRIPTION")]
    public string? Description { get; set; }

    [ForeignKey("PermissionId")]
    [InverseProperty("Permission")]
    public virtual ICollection<Troles> Role { get; set; } = new List<Troles>();
}
