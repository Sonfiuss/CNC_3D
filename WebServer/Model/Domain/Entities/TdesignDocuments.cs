using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TDESIGN_DOCUMENTS")]
[Index("Code", Name = "UQ_TDESIGN_DOCUMENTS_CODE", IsUnique = true)]
public partial class TdesignDocuments
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("CODE")]
    public string Code { get; set; } = null!;

    [Column("NAME")]
    public string Name { get; set; } = null!;

    [Column("DESCRIPTION")]
    public string? Description { get; set; }

    [Column("CREATED_AT")]
    public DateTime? CreatedAt { get; set; }

    [Column("CREATED_BY")]
    public Guid? CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TdesignDocuments")]
    public virtual Tusers? CreatedByNavigation { get; set; }

    [InverseProperty("Document")]
    public virtual ICollection<TdesignDocumentVersions> TdesignDocumentVersions { get; set; } = new List<TdesignDocumentVersions>();
}
