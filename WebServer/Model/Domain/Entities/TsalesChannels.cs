using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServer.WebServer.Model.Domain.Entities;

[Table("TSALES_CHANNELS")]
[Index("Name", Name = "UQ_TSALES_CHANNELS_NAME", IsUnique = true)]
public partial class TsalesChannels
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("NAME")]
    public string Name { get; set; } = null!;

    [Column("PLATFORM_URL")]
    public string? PlatformUrl { get; set; }

    [Column("META", TypeName = "jsonb")]
    public string? Meta { get; set; }

    [Column("CREATED_AT")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("SalesChannel")]
    public virtual ICollection<Torders> Torders { get; set; } = new List<Torders>();

    [InverseProperty("SalesChannel")]
    public virtual ICollection<TproductPrices> TproductPrices { get; set; } = new List<TproductPrices>();
}
