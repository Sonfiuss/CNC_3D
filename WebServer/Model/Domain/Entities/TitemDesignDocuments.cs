using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[PrimaryKey("ItemId", "DesignDocumentVersionId")]
[Table("TITEM_DESIGN_DOCUMENTS")]
public partial class TitemDesignDocuments
{
    [Key]
    [Column("ITEM_ID")]
    public Guid ItemId { get; set; }

    [Key]
    [Column("DESIGN_DOCUMENT_VERSION_ID")]
    public Guid DesignDocumentVersionId { get; set; }

    [Column("IS_PRIMARY")]
    public bool IsPrimary { get; set; }

    [ForeignKey("DesignDocumentVersionId")]
    [InverseProperty("TitemDesignDocuments")]
    public virtual TdesignDocumentVersions DesignDocumentVersion { get; set; } = null!;

    [ForeignKey("ItemId")]
    [InverseProperty("TitemDesignDocuments")]
    public virtual Titems Item { get; set; } = null!;
}
