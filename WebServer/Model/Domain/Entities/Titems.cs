using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TITEMS")]
[Index("CategoryId", Name = "IDX_TITEMS_CATEGORY_ID")]
[Index("ParentId", Name = "IDX_TITEMS_PARENT_ID")]
[Index("Sku", Name = "UQ_TITEMS_SKU", IsUnique = true)]
public partial class Titems
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("PARENT_ID")]
    public Guid? ParentId { get; set; }

    [Column("SKU")]
    public string Sku { get; set; } = null!;

    [Column("NAME")]
    public string Name { get; set; } = null!;

    [Column("CATEGORY_ID")]
    public Guid? CategoryId { get; set; }

    [Column("UOM_ID")]
    public Guid UomId { get; set; }

    [Column("STATUS")]
    public string Status { get; set; } = null!;

    [Column("SPECS", TypeName = "jsonb")]
    public string Specs { get; set; } = null!;

    [Column("META", TypeName = "jsonb")]
    public string Meta { get; set; } = null!;

    [Column("CREATED_AT")]
    public DateTime? CreatedAt { get; set; }

    [Column("UPDATED_AT")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Titems")]
    public virtual TproductCategories? Category { get; set; }

    [InverseProperty("Parent")]
    public virtual ICollection<Titems> InverseParent { get; set; } = new List<Titems>();

    [ForeignKey("ParentId")]
    [InverseProperty("InverseParent")]
    public virtual Titems? Parent { get; set; }

    [InverseProperty("ChildItem")]
    public virtual ICollection<Tboms> TbomsChildItem { get; set; } = new List<Tboms>();

    [InverseProperty("ParentItem")]
    public virtual ICollection<Tboms> TbomsParentItem { get; set; } = new List<Tboms>();

    [InverseProperty("Item")]
    public virtual ICollection<TinventoryBalances> TinventoryBalances { get; set; } = new List<TinventoryBalances>();

    [InverseProperty("Item")]
    public virtual ICollection<TinventoryTransactions> TinventoryTransactions { get; set; } = new List<TinventoryTransactions>();

    [InverseProperty("Item")]
    public virtual ICollection<TitemDesignDocuments> TitemDesignDocuments { get; set; } = new List<TitemDesignDocuments>();

    [InverseProperty("Item")]
    public virtual ICollection<TorderItems> TorderItems { get; set; } = new List<TorderItems>();

    [InverseProperty("Item")]
    public virtual ICollection<TproductPrices> TproductPrices { get; set; } = new List<TproductPrices>();

    [InverseProperty("Item")]
    public virtual ICollection<TproductionConsumptions> TproductionConsumptions { get; set; } = new List<TproductionConsumptions>();

    [InverseProperty("Item")]
    public virtual ICollection<TproductionLots> TproductionLots { get; set; } = new List<TproductionLots>();

    [InverseProperty("Item")]
    public virtual ICollection<TproductionOutputs> TproductionOutputs { get; set; } = new List<TproductionOutputs>();

    [InverseProperty("Item")]
    public virtual ICollection<TproductionWorkOrders> TproductionWorkOrders { get; set; } = new List<TproductionWorkOrders>();

    [ForeignKey("UomId")]
    [InverseProperty("Titems")]
    public virtual Tuoms Uom { get; set; } = null!;
}
