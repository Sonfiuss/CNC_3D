using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TBOMS")]
[Index("ParentItemId", Name = "IDX_TBOMS_PARENT_ITEM_ID")]
public partial class Tboms
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("PARENT_ITEM_ID")]
    public Guid ParentItemId { get; set; }

    [Column("CHILD_ITEM_ID")]
    public Guid ChildItemId { get; set; }

    [Column("DESIGN_DOCUMENT_VERSION_ID")]
    public Guid? DesignDocumentVersionId { get; set; }

    [Column("QUANTITY")]
    [Precision(18, 6)]
    public decimal Quantity { get; set; }

    [Column("UOM_ID")]
    public Guid UomId { get; set; }

    [Column("SCRAP_FACTOR")]
    [Precision(6, 4)]
    public decimal ScrapFactor { get; set; }

    [Column("VERSION")]
    public int Version { get; set; }

    [Column("EFFECTIVE_FROM")]
    public DateTime EffectiveFrom { get; set; }

    [Column("EFFECTIVE_TO")]
    public DateTime? EffectiveTo { get; set; }

    [ForeignKey("ChildItemId")]
    [InverseProperty("TbomsChildItem")]
    public virtual Titems ChildItem { get; set; } = null!;

    [ForeignKey("DesignDocumentVersionId")]
    [InverseProperty("Tboms")]
    public virtual TdesignDocumentVersions? DesignDocumentVersion { get; set; }

    [ForeignKey("ParentItemId")]
    [InverseProperty("TbomsParentItem")]
    public virtual Titems ParentItem { get; set; } = null!;

    [ForeignKey("UomId")]
    [InverseProperty("Tboms")]
    public virtual Tuoms Uom { get; set; } = null!;
}
