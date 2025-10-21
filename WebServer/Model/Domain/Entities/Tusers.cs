using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TUSERS")]
[Index("Email", Name = "UQ_TUSERS_EMAIL", IsUnique = true)]
[Index("Username", Name = "UQ_TUSERS_USERNAME", IsUnique = true)]
public partial class Tusers
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("USERNAME")]
    public string Username { get; set; } = null!;

    [Column("EMAIL", TypeName = "citext")]
    public string? Email { get; set; }

    [Column("PASSWORD_HASH")]
    public string? PasswordHash { get; set; }

    [Column("IS_ACTIVE")]
    public bool? IsActive { get; set; }

    [Column("META", TypeName = "jsonb")]
    public string? Meta { get; set; }

    [Column("CREATED_AT")]
    public DateTime? CreatedAt { get; set; }

    [Column("UPDATED_AT")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<TauditLog> TauditLog { get; set; } = new List<TauditLog>();

    [InverseProperty("User")]
    public virtual ICollection<Tcustomers> Tcustomers { get; set; } = new List<Tcustomers>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TdesignDocumentVersions> TdesignDocumentVersions { get; set; } = new List<TdesignDocumentVersions>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TdesignDocuments> TdesignDocuments { get; set; } = new List<TdesignDocuments>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TinventoryTransactions> TinventoryTransactions { get; set; } = new List<TinventoryTransactions>();

    [InverseProperty("ChangedByNavigation")]
    public virtual ICollection<TorderStatusHistory> TorderStatusHistory { get; set; } = new List<TorderStatusHistory>();

    [InverseProperty("Operator")]
    public virtual ICollection<TproductionTasks> TproductionTasks { get; set; } = new List<TproductionTasks>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TproductionWorkOrders> TproductionWorkOrders { get; set; } = new List<TproductionWorkOrders>();

    [ForeignKey("UserId")]
    [InverseProperty("User")]
    public virtual ICollection<Troles> Role { get; set; } = new List<Troles>();
}
