using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TAUDIT_LOG")]
[Index("EntityType", "EntityId", "CreatedAt", Name = "IDX_TAUDIT_LOG_ENTITY", IsDescending = new[] { false, false, true })]
public partial class TauditLog
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("ENTITY_TYPE")]
    public string EntityType { get; set; } = null!;

    [Column("ENTITY_ID")]
    public Guid EntityId { get; set; }

    [Column("ACTION")]
    public string Action { get; set; } = null!;

    [Column("USER_ID")]
    public Guid? UserId { get; set; }

    [Column("BEFORE_DATA", TypeName = "jsonb")]
    public string? BeforeData { get; set; }

    [Column("AFTER_DATA", TypeName = "jsonb")]
    public string? AfterData { get; set; }

    [Column("CREATED_AT")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("TauditLog")]
    public virtual Tusers? User { get; set; }
}
