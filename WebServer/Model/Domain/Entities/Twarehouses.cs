using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TWAREHOUSES")]
[Index("Code", Name = "UQ_TWAREHOUSES_CODE", IsUnique = true)]
public partial class Twarehouses
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("CODE")]
    public string Code { get; set; } = null!;

    [Column("NAME")]
    public string Name { get; set; } = null!;

    [Column("ADDRESS_ID")]
    public Guid? AddressId { get; set; }

    [Column("META", TypeName = "jsonb")]
    public string Meta { get; set; } = null!;

    [Column("CREATED_AT")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("AddressId")]
    [InverseProperty("Twarehouses")]
    public virtual Taddresses? Address { get; set; }

    [InverseProperty("Warehouse")]
    public virtual ICollection<TinventoryBalances> TinventoryBalances { get; set; } = new List<TinventoryBalances>();

    [InverseProperty("Warehouse")]
    public virtual ICollection<TinventoryTransactions> TinventoryTransactions { get; set; } = new List<TinventoryTransactions>();

    [InverseProperty("Warehouse")]
    public virtual ICollection<TproductionConsumptions> TproductionConsumptions { get; set; } = new List<TproductionConsumptions>();

    [InverseProperty("Warehouse")]
    public virtual ICollection<TproductionOutputs> TproductionOutputs { get; set; } = new List<TproductionOutputs>();

    [InverseProperty("Warehouse")]
    public virtual ICollection<TproductionWorkOrders> TproductionWorkOrders { get; set; } = new List<TproductionWorkOrders>();

    [InverseProperty("Warehouse")]
    public virtual ICollection<Tshipments> Tshipments { get; set; } = new List<Tshipments>();
}
