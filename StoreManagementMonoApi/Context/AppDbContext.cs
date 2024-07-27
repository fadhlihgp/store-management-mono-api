using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using store_management_mono_api.Entities;

namespace store_management_mono_api.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<ConfigValue> ConfigValues { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Debt> Debts { get; set; }

    public virtual DbSet<DebtDetail> DebtDetails { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<IncomeExpense> IncomeExpenses { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Otp> Otps { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductPrice> ProductPrices { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<PurchaseDetail> PurchaseDetails { get; set; }

    public virtual DbSet<PurchaseType> PurchaseTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<StockInOut> StockInOuts { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<UnitProduct> UnitProducts { get; set; }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//         => optionsBuilder.UseNpgsql("Host=103.245.39.103;Database=KelolaWarung_db;Username=remote_user;Password=DatabaseUser123/;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.HasIndex(e => new { e.Email, e.Username, e.PhoneNumber }, "Account_pk2").IsUnique();

            entity.HasIndex(e => e.ImageId, "IX_Account_ImageId");

            entity.HasIndex(e => e.RoleId, "IX_Account_RoleId");

            entity.HasIndex(e => e.StoreId, "IX_Account_StoreId");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.ImageId).HasMaxLength(255);
            entity.Property(e => e.LastLogin).HasColumnType("timestamp without time zone");
            entity.Property(e => e.PhoneNumber).HasMaxLength(255);
            entity.Property(e => e.RoleId).HasMaxLength(255);
            entity.Property(e => e.StoreId).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(255);

            entity.HasOne(d => d.Image).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.ImageId)
                .HasConstraintName("Account_Image_Id_fk");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Account_Role_Id_fk");

            entity.HasOne(d => d.Store).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("Account_Store_Id_fk");
        });

        modelBuilder.Entity<ConfigValue>(entity =>
        {
            entity.ToTable("ConfigValue");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.HasIndex(e => e.CreatedBy, "IX_Customer_CreatedBy");

            entity.HasIndex(e => e.DeletedBy, "IX_Customer_DeletedBy");

            entity.HasIndex(e => e.EditedBy, "IX_Customer_EditedBy");

            entity.HasIndex(e => e.StoreId, "IX_Customer_StoreId");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.DeletedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.DeletedBy).HasMaxLength(255);
            entity.Property(e => e.EditedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.EditedBy).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(255);
            entity.Property(e => e.StoreId).HasMaxLength(255);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CustomerCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Customer_Account_Id_fk");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.CustomerDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("Customer_Account_Id_fk3");

            entity.HasOne(d => d.EditedByNavigation).WithMany(p => p.CustomerEditedByNavigations)
                .HasForeignKey(d => d.EditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Customer_Account_Id_fk2");

            entity.HasOne(d => d.Store).WithMany(p => p.Customers)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Customer_Store_Id_fk");
        });

        modelBuilder.Entity<Debt>(entity =>
        {
            entity.ToTable("Debt");

            entity.HasIndex(e => e.CustomerId, "IX_Debt_CustomerId");

            entity.HasIndex(e => e.StoreId, "IX_Debt_StoreId");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.CustomerId).HasMaxLength(255);
            entity.Property(e => e.StoreId).HasMaxLength(255);

            entity.HasOne(d => d.Customer).WithMany(p => p.Debts)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Debt_Customer_Id_fk");

            entity.HasOne(d => d.Store).WithMany(p => p.Debts)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Debt_Store_Id_fk");
        });

        modelBuilder.Entity<DebtDetail>(entity =>
        {
            entity.ToTable("DebtDetail");

            entity.HasIndex(e => e.CreatedBy, "IX_DebtDetail_CreatedBy");

            entity.HasIndex(e => e.DebtId, "IX_DebtDetail_DebtId");

            entity.HasIndex(e => e.EditedBy, "IX_DebtDetail_EditedBy");

            entity.HasIndex(e => e.ProductId, "IX_DebtDetail_ProductId");

            entity.HasIndex(e => e.UnitProductId, "IX_DebtDetail_UnitProductId");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.Date).HasColumnType("timestamp without time zone");
            entity.Property(e => e.DebtId).HasMaxLength(255);
            entity.Property(e => e.EditedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.EditedBy).HasMaxLength(255);
            entity.Property(e => e.PayDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.PriceTotal).HasPrecision(18, 2);
            entity.Property(e => e.ProductId).HasMaxLength(255);
            entity.Property(e => e.UnitProductId).HasMaxLength(255);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DebtDetailCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DebtDetail_Account_Id_fk2");

            entity.HasOne(d => d.Debt).WithMany(p => p.DebtDetails)
                .HasForeignKey(d => d.DebtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DebtDetail_Debt_Id_fk");

            entity.HasOne(d => d.EditedByNavigation).WithMany(p => p.DebtDetailEditedByNavigations)
                .HasForeignKey(d => d.EditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DebtDetail_Account_Id_fk");

            entity.HasOne(d => d.Product).WithMany(p => p.DebtDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DebtDetail_Product_Id_fk");

            entity.HasOne(d => d.UnitProduct).WithMany(p => p.DebtDetails)
                .HasForeignKey(d => d.UnitProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DebtDetail_UnitProduct_Id_fk");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.ToTable("Image");

            entity.Property(e => e.Id).HasMaxLength(255);
        });

        modelBuilder.Entity<IncomeExpense>(entity =>
        {
            entity.ToTable("IncomeExpense");

            entity.HasIndex(e => e.CreatedBy, "IX_IncomeExpense_CreatedBy");

            entity.HasIndex(e => e.EditedBy, "IX_IncomeExpense_EditedBy");

            entity.HasIndex(e => e.ImageId, "IX_IncomeExpense_ImageId");

            entity.HasIndex(e => e.StoreId, "IX_IncomeExpense_StoreId");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.Amount).HasPrecision(18, 3);
            entity.Property(e => e.CreatedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.Date).HasColumnType("timestamp without time zone");
            entity.Property(e => e.EditedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.EditedBy).HasMaxLength(255);
            entity.Property(e => e.ImageId).HasMaxLength(255);
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.StoreId).HasMaxLength(255);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.IncomeExpenseCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("IncomeExpense_Account_Id_fk2");

            entity.HasOne(d => d.EditedByNavigation).WithMany(p => p.IncomeExpenseEditedByNavigations)
                .HasForeignKey(d => d.EditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("IncomeExpense_Account_Id_fk");

            entity.HasOne(d => d.Image).WithMany(p => p.IncomeExpenses)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("IncomeExpense_Image_Id_fk");

            entity.HasOne(d => d.Store).WithMany(p => p.IncomeExpenses)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("IncomeExpense_Store_Id_fk");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.ToTable("Note");

            entity.HasIndex(e => e.CreatedBy, "IX_Note_CreatedBy");

            entity.HasIndex(e => e.EditedBy, "IX_Note_EditedBy");

            entity.HasIndex(e => e.StoreId, "IX_Note_StoreId");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.EditedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.EditedBy).HasMaxLength(255);
            entity.Property(e => e.StoreId).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.NoteCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("Note_Account_Id_fk2");

            entity.HasOne(d => d.EditedByNavigation).WithMany(p => p.NoteEditedByNavigations)
                .HasForeignKey(d => d.EditedBy)
                .HasConstraintName("Note_Account_Id_fk");

            entity.HasOne(d => d.Store).WithMany(p => p.Notes)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Note_Store_Id_fk");
        });

        modelBuilder.Entity<Otp>(entity =>
        {
            entity.ToTable("Otp");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.ExpiredAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Value).HasMaxLength(255);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.HasIndex(e => e.CreatedBy, "IX_Product_CreatedBy");

            entity.HasIndex(e => e.DeletedBy, "IX_Product_DeletedBy");

            entity.HasIndex(e => e.EditedBy, "IX_Product_EditedBy");

            entity.HasIndex(e => e.ImageId, "IX_Product_ImageId");

            entity.HasIndex(e => e.StoreId, "IX_Product_StoreId");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.DeletedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.DeletedBy).HasMaxLength(255);
            entity.Property(e => e.EditedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.EditedBy).HasMaxLength(255);
            entity.Property(e => e.ImageId).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.StoreId).HasMaxLength(255);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProductCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Product_Account_Id_fk2");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.ProductDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("Product_Account_Id_fk");

            entity.HasOne(d => d.EditedByNavigation).WithMany(p => p.ProductEditedByNavigations)
                .HasForeignKey(d => d.EditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Product_Account_Id_fk3");

            entity.HasOne(d => d.Image).WithMany(p => p.Products)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Product_Image_Id_fk");

            entity.HasOne(d => d.Store).WithMany(p => p.Products)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Product_Store_Id_fk");
        });

        modelBuilder.Entity<ProductPrice>(entity =>
        {
            entity.ToTable("ProductPrice");

            entity.HasIndex(e => e.ProductId, "IX_ProductPrice_ProductId");

            entity.HasIndex(e => e.UnitPriceId, "IX_ProductPrice_UnitPriceId");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.Price).HasPrecision(18, 3);
            entity.Property(e => e.ProductId).HasMaxLength(255);
            entity.Property(e => e.UnitPriceId).HasMaxLength(255);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductPrices)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProductPrice_Product_Id_fk");

            entity.HasOne(d => d.UnitPrice).WithMany(p => p.ProductPrices)
                .HasForeignKey(d => d.UnitPriceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProductPrice_UnitProduct_Id_fk");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.ToTable("Purchase");

            entity.HasIndex(e => e.CreatedBy, "IX_Purchase_CreatedBy");

            entity.HasIndex(e => e.CustomerId, "IX_Purchase_CustomerId");

            entity.HasIndex(e => e.PurchaseTypeId, "IX_Purchase_PurchaseTypeId");

            entity.HasIndex(e => e.StoreId, "IX_Purchase_StoreId");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CustomerId).HasMaxLength(255);
            entity.Property(e => e.Date).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Invoice).HasMaxLength(255);
            entity.Property(e => e.Money).HasPrecision(18, 3);
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.Payment).HasMaxLength(255);
            entity.Property(e => e.PurchaseTypeId).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(255);
            entity.Property(e => e.StoreId).HasMaxLength(255);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Purchase_Account_Id_fk");

            entity.HasOne(d => d.Customer).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("Purchase_Customer_Id_fk");

            entity.HasOne(d => d.PurchaseType).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.PurchaseTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Purchase_PurchaseType_Id_fk");

            entity.HasOne(d => d.Store).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Purchase_Store_Id_fk");
        });

        modelBuilder.Entity<PurchaseDetail>(entity =>
        {
            entity.ToTable("PurchaseDetail");

            entity.HasIndex(e => e.ProductId, "IX_PurchaseDetail_ProductId");

            entity.HasIndex(e => e.PurchaseId, "IX_PurchaseDetail_PurchaseId");

            entity.HasIndex(e => e.UnitProductId, "IX_PurchaseDetail_UnitProductId");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.ColumnName).HasColumnName("column_name");
            entity.Property(e => e.Price).HasPrecision(18, 3);
            entity.Property(e => e.ProductId).HasMaxLength(255);
            entity.Property(e => e.PurchaseId).HasMaxLength(255);
            entity.Property(e => e.Total).HasPrecision(18, 3);
            entity.Property(e => e.UnitProductId).HasMaxLength(255);

            entity.HasOne(d => d.Product).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PurchaseDetail_Product_Id_fk");

            entity.HasOne(d => d.Purchase).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PurchaseDetail_Purchase_Id_fk");

            entity.HasOne(d => d.UnitProduct).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.UnitProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PurchaseDetail_UnitProduct_Id_fk");
        });

        modelBuilder.Entity<PurchaseType>(entity =>
        {
            entity.ToTable("PurchaseType");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<StockInOut>(entity =>
        {
            entity.ToTable("StockInOut");

            entity.HasIndex(e => e.CreatedBy, "IX_StockInOut_CreatedBy");

            entity.HasIndex(e => e.EditedBy, "IX_StockInOut_EditedBy");

            entity.HasIndex(e => e.ProductId, "IX_StockInOut_ProductId");

            entity.HasIndex(e => e.SupplierId, "IX_StockInOut_SupplierId");

            entity.HasIndex(e => e.UnitProductId, "IX_StockInOut_UnitProductId");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.EditedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.EditedBy).HasMaxLength(255);
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.ProductId).HasMaxLength(255);
            entity.Property(e => e.SupplierId).HasMaxLength(255);
            entity.Property(e => e.UnitProductId).HasMaxLength(255);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.StockInOutCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("StockInOut_Account_Id_fk");

            entity.HasOne(d => d.EditedByNavigation).WithMany(p => p.StockInOutEditedByNavigations)
                .HasForeignKey(d => d.EditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("StockInOut_Account_Id_fk2");

            entity.HasOne(d => d.Product).WithMany(p => p.StockInOuts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("StockInOut_Product_Id_fk");

            entity.HasOne(d => d.Supplier).WithMany(p => p.StockInOuts)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("StockInOut_Supplier_Id_fk");

            entity.HasOne(d => d.UnitProduct).WithMany(p => p.StockInOuts)
                .HasForeignKey(d => d.UnitProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("StockInOut_UnitProduct_Id_fk");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.ToTable("Store");

            entity.HasIndex(e => e.PhoneNumber, "Store_pk2").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.BusinessType).HasMaxLength(255);
            entity.Property(e => e.LastEdited).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(255);
            entity.Property(e => e.RegisterDate).HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("Supplier");

            entity.HasIndex(e => e.StoreId, "IX_Supplier_StoreId");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(255);
            entity.Property(e => e.StoreId).HasMaxLength(255);

            entity.HasOne(d => d.Store).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Supplier_Store_Id_fk");
        });

        modelBuilder.Entity<UnitProduct>(entity =>
        {
            entity.ToTable("UnitProduct");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
