using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebServer.WebServer.Model.Domain.Entities;

namespace WebServer.WebServer.Model.Domain;

public partial class DomainDbContext : DbContext
{
    public DomainDbContext()
    {
    }

    public DomainDbContext(DbContextOptions<DomainDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Taddresses> Taddresses { get; set; }

    public virtual DbSet<TauditLog> TauditLog { get; set; }

    public virtual DbSet<Tboms> Tboms { get; set; }

    public virtual DbSet<Tcontacts> Tcontacts { get; set; }

    public virtual DbSet<TcustomerAddresses> TcustomerAddresses { get; set; }

    public virtual DbSet<Tcustomers> Tcustomers { get; set; }

    public virtual DbSet<TdesignDocumentVersions> TdesignDocumentVersions { get; set; }

    public virtual DbSet<TdesignDocuments> TdesignDocuments { get; set; }

    public virtual DbSet<TinventoryBalances> TinventoryBalances { get; set; }

    public virtual DbSet<TinventoryTransactions> TinventoryTransactions { get; set; }

    public virtual DbSet<TitemDesignDocuments> TitemDesignDocuments { get; set; }

    public virtual DbSet<Titems> Titems { get; set; }

    public virtual DbSet<Tleads> Tleads { get; set; }

    public virtual DbSet<TorderFeedback> TorderFeedback { get; set; }

    public virtual DbSet<TorderItems> TorderItems { get; set; }

    public virtual DbSet<TorderStatusHistory> TorderStatusHistory { get; set; }

    public virtual DbSet<Torders> Torders { get; set; }

    public virtual DbSet<Tpayments> Tpayments { get; set; }

    public virtual DbSet<Tpermissions> Tpermissions { get; set; }

    public virtual DbSet<TproductCategories> TproductCategories { get; set; }

    public virtual DbSet<TproductPrices> TproductPrices { get; set; }

    public virtual DbSet<TproductionConsumptions> TproductionConsumptions { get; set; }

    public virtual DbSet<TproductionDefects> TproductionDefects { get; set; }

    public virtual DbSet<TproductionLots> TproductionLots { get; set; }

    public virtual DbSet<TproductionOutputs> TproductionOutputs { get; set; }

    public virtual DbSet<TproductionTasks> TproductionTasks { get; set; }

    public virtual DbSet<TproductionWorkOrders> TproductionWorkOrders { get; set; }

    public virtual DbSet<Troles> Troles { get; set; }

    public virtual DbSet<TsalesChannels> TsalesChannels { get; set; }

    public virtual DbSet<TshipmentItemLots> TshipmentItemLots { get; set; }

    public virtual DbSet<TshipmentItems> TshipmentItems { get; set; }

    public virtual DbSet<Tshipments> Tshipments { get; set; }

    public virtual DbSet<Tuoms> Tuoms { get; set; }

    public virtual DbSet<Tusers> Tusers { get; set; }

    public virtual DbSet<Twarehouses> Twarehouses { get; set; }

    public virtual DbSet<VItemCurrentPrices> VItemCurrentPrices { get; set; }

    // Configuration is provided via DI in Program.cs; avoid hardcoding here.
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("address_type_enum", new[] { "BILLING", "SHIPPING", "WAREHOUSE", "OTHER" })
            .HasPostgresEnum("defect_severity_enum", new[] { "MINOR", "MAJOR", "CRITICAL" })
            .HasPostgresEnum("item_type_enum", new[] { "PRODUCT", "COMPONENT", "RAW_MATERIAL", "HARDWARE", "PRODUCT_TEMPLATE" })
            .HasPostgresEnum("order_status_enum", new[] { "DRAFT", "PENDING", "CONFIRMED", "IN_PRODUCTION", "SHIPPED", "DELIVERED", "CANCELLED", "RETURNED" })
            .HasPostgresEnum("payment_status_enum", new[] { "PENDING", "PAID", "PARTIALLY_PAID", "REFUNDED", "FAILED" })
            .HasPostgresEnum("production_status_enum", new[] { "PLANNED", "IN_PROGRESS", "COMPLETED", "CANCELLED" })
            .HasPostgresEnum("shipment_status_enum", new[] { "PICKING", "PACKED", "SHIPPED", "DELIVERED", "CANCELLED" })
            .HasPostgresExtension("citext")
            .HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Taddresses>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TADDRESSES_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CountryCode).HasDefaultValueSql("'VN'::text");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Meta).HasDefaultValueSql("'{}'::jsonb");
        });

        modelBuilder.Entity<TauditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TAUDIT_LOG_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.User).WithMany(p => p.TauditLog)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TAUDIT_LOG_USER_ID");
        });

        modelBuilder.Entity<Tboms>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TBOMS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.EffectiveFrom).HasDefaultValueSql("now()");
            entity.Property(e => e.Version).HasDefaultValue(1);

            entity.HasOne(d => d.ChildItem).WithMany(p => p.TbomsChildItem)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TBOMS_CHILD_ITEM_ID");

            entity.HasOne(d => d.DesignDocumentVersion).WithMany(p => p.Tboms)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TBOMS_DESIGN_DOCUMENT_VERSION_ID");

            entity.HasOne(d => d.ParentItem).WithMany(p => p.TbomsParentItem).HasConstraintName("FK_TBOMS_PARENT_ITEM_ID");

            entity.HasOne(d => d.Uom).WithMany(p => p.Tboms)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TBOMS_UOM_ID");
        });

        modelBuilder.Entity<Tcontacts>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TCONTACTS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Meta).HasDefaultValueSql("'{}'::jsonb");
        });

        modelBuilder.Entity<TcustomerAddresses>(entity =>
        {
            entity.Property(e => e.IsDefault).HasDefaultValue(false);

            entity.HasOne(d => d.Address).WithMany(p => p.TcustomerAddresses).HasConstraintName("FK_TCUSTOMER_ADDRESSES_ADDRESS_ID");

            entity.HasOne(d => d.Customer).WithMany(p => p.TcustomerAddresses).HasConstraintName("FK_TCUSTOMER_ADDRESSES_CUSTOMER_ID");
        });

        modelBuilder.Entity<Tcustomers>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TCUSTOMERS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Contact).WithMany(p => p.Tcustomers)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TCUSTOMERS_CONTACT_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Tcustomers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TCUSTOMERS_USER_ID");
        });

        modelBuilder.Entity<TdesignDocumentVersions>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TDESIGN_DOCUMENT_VERSIONS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Meta).HasDefaultValueSql("'{}'::jsonb");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TdesignDocumentVersions)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TDESIGN_DOCUMENT_VERSIONS_CREATED_BY");

            entity.HasOne(d => d.Document).WithMany(p => p.TdesignDocumentVersions).HasConstraintName("FK_TDESIGN_DOCUMENT_VERSIONS_DOCUMENT_ID");
        });

        modelBuilder.Entity<TdesignDocuments>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TDESIGN_DOCUMENTS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TdesignDocuments)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TDESIGN_DOCUMENTS_CREATED_BY");
        });

        modelBuilder.Entity<TinventoryBalances>(entity =>
        {
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Item).WithMany(p => p.TinventoryBalances).HasConstraintName("FK_TINVENTORY_BALANCES_ITEM_ID");

            entity.HasOne(d => d.Lot).WithMany(p => p.TinventoryBalances).HasConstraintName("FK_TINVENTORY_BALANCES_LOT_ID");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.TinventoryBalances).HasConstraintName("FK_TINVENTORY_BALANCES_WAREHOUSE_ID");
        });

        modelBuilder.Entity<TinventoryTransactions>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TINVENTORY_TRANSACTIONS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.OccurredAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TinventoryTransactions)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TINVENTORY_TRANSACTIONS_CREATED_BY");

            entity.HasOne(d => d.Item).WithMany(p => p.TinventoryTransactions)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TINVENTORY_TRANSACTIONS_ITEM_ID");

            entity.HasOne(d => d.Lot).WithMany(p => p.TinventoryTransactions)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TINVENTORY_TRANSACTIONS_LOT_ID");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.TinventoryTransactions)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TINVENTORY_TRANSACTIONS_WAREHOUSE_ID");
        });

        modelBuilder.Entity<TitemDesignDocuments>(entity =>
        {
            entity.Property(e => e.IsPrimary).HasDefaultValue(false);

            entity.HasOne(d => d.DesignDocumentVersion).WithMany(p => p.TitemDesignDocuments).HasConstraintName("FK_TITEM_DESIGN_DOCUMENTS_DESIGN_DOC_VER_ID");

            entity.HasOne(d => d.Item).WithMany(p => p.TitemDesignDocuments).HasConstraintName("FK_TITEM_DESIGN_DOCUMENTS_ITEM_ID");
        });

        modelBuilder.Entity<Titems>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TITEMS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Meta).HasDefaultValueSql("'{}'::jsonb");
            entity.Property(e => e.Specs).HasDefaultValueSql("'{}'::jsonb");
            entity.Property(e => e.Status).HasDefaultValueSql("'ACTIVE'::text");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Category).WithMany(p => p.Titems)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TITEMS_CATEGORY_ID");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TITEMS_PARENT_ID");

            entity.HasOne(d => d.Uom).WithMany(p => p.Titems)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TITEMS_UOM_ID");
        });

        modelBuilder.Entity<Tleads>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TLEADS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Meta).HasDefaultValueSql("'{}'::jsonb");

            entity.HasOne(d => d.Contact).WithMany(p => p.Tleads)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TLEADS_CONTACT_ID");
        });

        modelBuilder.Entity<TorderFeedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TORDER_FEEDBACK_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Customer).WithMany(p => p.TorderFeedback)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TORDER_FEEDBACK_CUSTOMER_ID");

            entity.HasOne(d => d.Order).WithMany(p => p.TorderFeedback).HasConstraintName("FK_TORDER_FEEDBACK_ORDER_ID");
        });

        modelBuilder.Entity<TorderItems>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TORDER_ITEMS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Meta).HasDefaultValueSql("'{}'::jsonb");

            entity.HasOne(d => d.Item).WithMany(p => p.TorderItems)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TORDER_ITEMS_ITEM_ID");

            entity.HasOne(d => d.Order).WithMany(p => p.TorderItems).HasConstraintName("FK_TORDER_ITEMS_ORDER_ID");
        });

        modelBuilder.Entity<TorderStatusHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TORDER_STATUS_HISTORY_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.ChangedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.TorderStatusHistory)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TORDER_STATUS_HISTORY_CHANGED_BY");

            entity.HasOne(d => d.Order).WithMany(p => p.TorderStatusHistory).HasConstraintName("FK_TORDER_STATUS_HISTORY_ORDER_ID");
        });

        modelBuilder.Entity<Torders>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TORDERS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Currency).HasDefaultValueSql("'VND'::text");
            entity.Property(e => e.Meta).HasDefaultValueSql("'{}'::jsonb");
            entity.Property(e => e.OrderDate).HasDefaultValueSql("now()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.BillingAddress).WithMany(p => p.TordersBillingAddress)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TORDERS_BILLING_ADDRESS_ID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Torders)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TORDERS_CUSTOMER_ID");

            entity.HasOne(d => d.SalesChannel).WithMany(p => p.Torders)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TORDERS_SALES_CHANNEL_ID");

            entity.HasOne(d => d.ShippingAddress).WithMany(p => p.TordersShippingAddress)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TORDERS_SHIPPING_ADDRESS_ID");
        });

        modelBuilder.Entity<Tpayments>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TPAYMENTS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Order).WithMany(p => p.Tpayments).HasConstraintName("FK_TPAYMENTS_ORDER_ID");
        });

        modelBuilder.Entity<Tpermissions>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TPERMISSIONS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
        });

        modelBuilder.Entity<TproductCategories>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TPRODUCT_CATEGORIES_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Meta).HasDefaultValueSql("'{}'::jsonb");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TPRODUCT_CATEGORIES_PARENT_ID");
        });

        modelBuilder.Entity<TproductPrices>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TPRODUCT_PRICES_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Currency).HasDefaultValueSql("'VND'::text");
            entity.Property(e => e.ValidFrom).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Item).WithMany(p => p.TproductPrices).HasConstraintName("FK_TPRODUCT_PRICES_ITEM_ID");

            entity.HasOne(d => d.SalesChannel).WithMany(p => p.TproductPrices)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TPRODUCT_PRICES_SALES_CHANNEL_ID");
        });

        modelBuilder.Entity<TproductionConsumptions>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TPRODUCTION_CONSUMPTIONS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.OccurredAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Item).WithMany(p => p.TproductionConsumptions)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TPRODUCTION_CONSUMPTIONS_ITEM_ID");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.TproductionConsumptions)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TPRODUCTION_CONSUMPTIONS_WAREHOUSE_ID");

            entity.HasOne(d => d.WorkOrder).WithMany(p => p.TproductionConsumptions).HasConstraintName("FK_TPRODUCTION_CONSUMPTIONS_WORK_ORDER_ID");
        });

        modelBuilder.Entity<TproductionDefects>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TPRODUCTION_DEFECTS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.OccurredAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Quantity).HasDefaultValueSql("1");

            entity.HasOne(d => d.WorkOrder).WithMany(p => p.TproductionDefects).HasConstraintName("FK_TPRODUCTION_DEFECTS_WORK_ORDER_ID");
        });

        modelBuilder.Entity<TproductionLots>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TPRODUCTION_LOTS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Meta).HasDefaultValueSql("'{}'::jsonb");
            entity.Property(e => e.ProductionDate).HasDefaultValueSql("CURRENT_DATE");

            entity.HasOne(d => d.Item).WithMany(p => p.TproductionLots)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TPRODUCTION_LOTS_ITEM_ID");

            entity.HasOne(d => d.WorkOrder).WithMany(p => p.TproductionLots)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TPRODUCTION_LOTS_WORK_ORDER_ID");
        });

        modelBuilder.Entity<TproductionOutputs>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TPRODUCTION_OUTPUTS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.OccurredAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Item).WithMany(p => p.TproductionOutputs)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TPRODUCTION_OUTPUTS_ITEM_ID");

            entity.HasOne(d => d.Lot).WithMany(p => p.TproductionOutputs)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TPRODUCTION_OUTPUTS_LOT_ID");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.TproductionOutputs)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TPRODUCTION_OUTPUTS_WAREHOUSE_ID");

            entity.HasOne(d => d.WorkOrder).WithMany(p => p.TproductionOutputs).HasConstraintName("FK_TPRODUCTION_OUTPUTS_WORK_ORDER_ID");
        });

        modelBuilder.Entity<TproductionTasks>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TPRODUCTION_TASKS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");

            entity.HasOne(d => d.Operator).WithMany(p => p.TproductionTasks)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TPRODUCTION_TASKS_OPERATOR_ID");

            entity.HasOne(d => d.WorkOrder).WithMany(p => p.TproductionTasks).HasConstraintName("FK_TPRODUCTION_TASKS_WORK_ORDER_ID");
        });

        modelBuilder.Entity<TproductionWorkOrders>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TPRODUCTION_WORK_ORDERS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Meta).HasDefaultValueSql("'{}'::jsonb");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TproductionWorkOrders)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TPRODUCTION_WORK_ORDERS_CREATED_BY");

            entity.HasOne(d => d.DesignDocumentVersion).WithMany(p => p.TproductionWorkOrders)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TPRODUCTION_WORK_ORDERS_DESIGN_DOC_VER_ID");

            entity.HasOne(d => d.Item).WithMany(p => p.TproductionWorkOrders)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TPRODUCTION_WORK_ORDERS_ITEM_ID");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.TproductionWorkOrders)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TPRODUCTION_WORK_ORDERS_WAREHOUSE_ID");
        });

        modelBuilder.Entity<Troles>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TROLES_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

            entity.HasMany(d => d.Permission).WithMany(p => p.Role)
                .UsingEntity<Dictionary<string, object>>(
                    "TrolePermissions",
                    r => r.HasOne<Tpermissions>().WithMany()
                        .HasForeignKey("PermissionId")
                        .HasConstraintName("FK_TROLE_PERMISSIONS_PERMISSION_ID"),
                    l => l.HasOne<Troles>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_TROLE_PERMISSIONS_ROLE_ID"),
                    j =>
                    {
                        j.HasKey("RoleId", "PermissionId");
                        j.ToTable("TROLE_PERMISSIONS");
                        j.HasIndex(new[] { "PermissionId" }, "IDX_TROLE_PERMISSIONS_PERMISSION_ID");
                        j.HasIndex(new[] { "RoleId" }, "IDX_TROLE_PERMISSIONS_ROLE_ID");
                        j.IndexerProperty<Guid>("RoleId").HasColumnName("ROLE_ID");
                        j.IndexerProperty<Guid>("PermissionId").HasColumnName("PERMISSION_ID");
                    });
        });

        modelBuilder.Entity<TsalesChannels>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TSALES_CHANNELS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Meta).HasDefaultValueSql("'{}'::jsonb");
        });

        modelBuilder.Entity<TshipmentItemLots>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TSHIPMENT_ITEM_LOTS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");

            entity.HasOne(d => d.Lot).WithMany(p => p.TshipmentItemLots)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TSHIPMENT_ITEM_LOTS_LOT_ID");

            entity.HasOne(d => d.ShipmentItem).WithMany(p => p.TshipmentItemLots).HasConstraintName("FK_TSHIPMENT_ITEM_LOTS_SHIPMENT_ITEM_ID");
        });

        modelBuilder.Entity<TshipmentItems>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TSHIPMENT_ITEMS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");

            entity.HasOne(d => d.OrderItem).WithMany(p => p.TshipmentItems)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TSHIPMENT_ITEMS_ORDER_ITEM_ID");

            entity.HasOne(d => d.Shipment).WithMany(p => p.TshipmentItems).HasConstraintName("FK_TSHIPMENT_ITEMS_SHIPMENT_ID");
        });

        modelBuilder.Entity<Tshipments>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TSHIPMENTS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Order).WithMany(p => p.Tshipments).HasConstraintName("FK_TSHIPMENTS_ORDER_ID");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Tshipments)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TSHIPMENTS_WAREHOUSE_ID");
        });

        modelBuilder.Entity<Tuoms>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TUOMS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
        });

        modelBuilder.Entity<Tusers>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TUSERS_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Meta).HasDefaultValueSql("'{}'::jsonb");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasMany(d => d.Role).WithMany(p => p.User)
                .UsingEntity<Dictionary<string, object>>(
                    "TuserRoles",
                    r => r.HasOne<Troles>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_TUSER_ROLES_ROLE_ID"),
                    l => l.HasOne<Tusers>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_TUSER_ROLES_USER_ID"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("TUSER_ROLES");
                        j.HasIndex(new[] { "RoleId" }, "IDX_TUSER_ROLES_ROLE_ID");
                        j.HasIndex(new[] { "UserId" }, "IDX_TUSER_ROLES_USER_ID");
                        j.IndexerProperty<Guid>("UserId").HasColumnName("USER_ID");
                        j.IndexerProperty<Guid>("RoleId").HasColumnName("ROLE_ID");
                    });
        });

        modelBuilder.Entity<Twarehouses>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TWAREHOUSES_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Meta).HasDefaultValueSql("'{}'::jsonb");

            entity.HasOne(d => d.Address).WithMany(p => p.Twarehouses)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TWAREHOUSES_ADDRESS_ID");
        });

        modelBuilder.Entity<VItemCurrentPrices>(entity =>
        {
            entity.ToView("V_ITEM_CURRENT_PRICES");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
