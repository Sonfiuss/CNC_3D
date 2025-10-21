using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TPRODUCT_CATEGORIES")]
[Index("ParentId", Name = "IDX_TPRODUCT_CATEGORIES_PARENT_ID")]
[Index("Slug", Name = "UQ_TPRODUCT_CATEGORIES_SLUG", IsUnique = true)]
public partial class TproductCategories
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("NAME")]
    public string Name { get; set; } = null!;

    [Column("SLUG")]
    public string? Slug { get; set; }

    [Column("PARENT_ID")]
    public Guid? ParentId { get; set; }

    [Column("META", TypeName = "jsonb")]
    public string? Meta { get; set; }

    [Column("CREATED_AT")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Parent")]
    public virtual ICollection<TproductCategories> InverseParent { get; set; } = new List<TproductCategories>();

    [ForeignKey("ParentId")]
    [InverseProperty("InverseParent")]
    public virtual TproductCategories? Parent { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Titems> Titems { get; set; } = new List<Titems>();
}
