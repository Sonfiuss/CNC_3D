using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TDESIGN_DOCUMENT_VERSIONS")]
[Index("DocumentId", "Version", Name = "UQ_TDESIGN_DOCUMENT_VERSIONS_DOC_VER", IsUnique = true)]
public partial class TdesignDocumentVersions
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("DOCUMENT_ID")]
    public Guid DocumentId { get; set; }

    [Column("VERSION")]
    public int Version { get; set; }

    [Column("FILE_URI")]
    public string FileUri { get; set; } = null!;

    [Column("FILE_HASH")]
    public string? FileHash { get; set; }

    [Column("META", TypeName = "jsonb")]
    public string Meta { get; set; } = null!;

    [Column("CREATED_AT")]
    public DateTime? CreatedAt { get; set; }

    [Column("CREATED_BY")]
    public Guid? CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TdesignDocumentVersions")]
    public virtual Tusers? CreatedByNavigation { get; set; }

    [ForeignKey("DocumentId")]
    [InverseProperty("TdesignDocumentVersions")]
    public virtual TdesignDocuments Document { get; set; } = null!;

    [InverseProperty("DesignDocumentVersion")]
    public virtual ICollection<Tboms> Tboms { get; set; } = new List<Tboms>();

    [InverseProperty("DesignDocumentVersion")]
    public virtual ICollection<TitemDesignDocuments> TitemDesignDocuments { get; set; } = new List<TitemDesignDocuments>();

    [InverseProperty("DesignDocumentVersion")]
    public virtual ICollection<TproductionWorkOrders> TproductionWorkOrders { get; set; } = new List<TproductionWorkOrders>();
}
