using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TUOMS")]
[Index("Code", Name = "UQ_TUOMS_CODE", IsUnique = true)]
public partial class Tuoms
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("CODE")]
    public string Code { get; set; } = null!;

    [Column("NAME")]
    public string Name { get; set; } = null!;

    [InverseProperty("Uom")]
    public virtual ICollection<Tboms> Tboms { get; set; } = new List<Tboms>();

    [InverseProperty("Uom")]
    public virtual ICollection<Titems> Titems { get; set; } = new List<Titems>();
}
