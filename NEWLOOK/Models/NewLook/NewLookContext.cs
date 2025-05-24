using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NEWLOOK.Models.NewLook;

public partial class NewLookContext : DbContext
{
    public NewLookContext()
    {
    }

    public NewLookContext(DbContextOptions<NewLookContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BookDet> BookDets { get; set; }

    public virtual DbSet<BookHdr> BookHdrs { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Gallery> Galleries { get; set; }

    public virtual DbSet<MstService> MstServices { get; set; }

    public virtual DbSet<MstServiceImage> MstServiceImages { get; set; }

    public virtual DbSet<MstUser> MstUsers { get; set; }

    public virtual DbSet<ServiceType> ServiceTypes { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=VM0D7D1F9\\SQLSERVER;Database=NewLook;Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookDet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__book_det__3214EC271D605764");

            entity.ToTable("book_det");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.InvNo)
                .HasMaxLength(50)
                .HasColumnName("inv_no");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ServTypeId).HasColumnName("serv_type_id");

            entity.HasOne(d => d.InvNoNavigation).WithMany(p => p.BookDets)
                .HasForeignKey(d => d.InvNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__book_det__inv_no__5165187F");

            entity.HasOne(d => d.ServType).WithMany(p => p.BookDets)
                .HasForeignKey(d => d.ServTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__book_det__serv_t__52593CB8");
        });

        modelBuilder.Entity<BookHdr>(entity =>
        {
            entity.HasKey(e => e.InvNo).HasName("PK__book_hdr__A874B48A93108C9A");

            entity.ToTable("book_hdr");

            entity.Property(e => e.InvNo)
                .HasMaxLength(50)
                .HasColumnName("inv_no");
            entity.Property(e => e.BookDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("book_date");
            entity.Property(e => e.CreatedDt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_dt");
            entity.Property(e => e.CustmId).HasColumnName("custm_id");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.TotalPrice).HasColumnName("total_price");

            entity.HasOne(d => d.Custm).WithMany(p => p.BookHdrs)
                .HasForeignKey(d => d.CustmId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__book_hdr__custm___4D94879B");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC270A002CFD");

            entity.ToTable("Customer");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BirthDt)
                .HasColumnType("datetime")
                .HasColumnName("birth_dt");
            entity.Property(e => e.CustmAddress)
                .HasMaxLength(100)
                .HasColumnName("custm_Address");
            entity.Property(e => e.CustmEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("custm_email");
            entity.Property(e => e.CustmName)
                .HasMaxLength(100)
                .HasColumnName("custm_name");
            entity.Property(e => e.CustmPhone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("custm_phone");
        });

        modelBuilder.Entity<Gallery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__gallery__3214EC271445B7FA");

            entity.ToTable("gallery");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ImageLink)
                .HasMaxLength(100)
                .HasColumnName("image_link");
        });

        modelBuilder.Entity<MstService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__mst_Serv__3214EC276F112E71");

            entity.ToTable("mst_Service");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.SerDesc)
                .HasMaxLength(500)
                .HasColumnName("ser_desc");
            entity.Property(e => e.SerName)
                .HasMaxLength(50)
                .HasColumnName("ser_name");
            entity.Property(e => e.ServiceIconImage)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("service_icon_image");
        });

        modelBuilder.Entity<MstServiceImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__mst_Serv__3214EC270E174308");

            entity.ToTable("mst_Service_images");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasMaxLength(2)
                .HasDefaultValue("Y")
                .HasColumnName("active");
            entity.Property(e => e.ImageLink)
                .HasMaxLength(50)
                .HasColumnName("image_link");
            entity.Property(e => e.SerId).HasColumnName("ser_id");

            entity.HasOne(d => d.Ser).WithMany(p => p.MstServiceImages)
                .HasForeignKey(d => d.SerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mst_Servi__ser_i__4222D4EF");
        });

        modelBuilder.Entity<MstUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__mst_User__3214EC27C8106A8F");

            entity.ToTable("mst_User");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasMaxLength(2)
                .HasDefaultValue("Y")
                .HasColumnName("active");
            entity.Property(e => e.Lvl)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lvl");
            entity.Property(e => e.Pswd)
                .HasMaxLength(100)
                .HasColumnName("pswd");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("user_name");
        });

        modelBuilder.Entity<ServiceType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Service___3214EC27A081A981");

            entity.ToTable("Service_types");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasMaxLength(2)
                .HasDefaultValue("Y")
                .HasColumnName("active");
            entity.Property(e => e.MstSerId).HasColumnName("mst_ser_id");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.SerTime)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ser_time");
            entity.Property(e => e.SerTypeDesc)
                .HasMaxLength(500)
                .HasColumnName("ser_type_desc");
            entity.Property(e => e.SerTypeName)
                .HasMaxLength(100)
                .HasColumnName("ser_type_name");

            entity.HasOne(d => d.MstSer).WithMany(p => p.ServiceTypes)
                .HasForeignKey(d => d.MstSerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Service_t__mst_s__46E78A0C");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Team__3214EC273F1F07C4");

            entity.ToTable("Team");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasDefaultValue("Y")
                .HasColumnName("active");
            entity.Property(e => e.CountryWork)
                .HasMaxLength(50)
                .HasColumnName("country_work");
            entity.Property(e => e.EmpName)
                .HasMaxLength(50)
                .HasColumnName("emp_name");
            entity.Property(e => e.EmpSkills)
                .HasMaxLength(50)
                .HasColumnName("emp_skills");
            entity.Property(e => e.Experiances)
                .HasMaxLength(100)
                .HasColumnName("experiances");
            entity.Property(e => e.ImageLink)
                .HasMaxLength(100)
                .HasColumnName("image_link");
            entity.Property(e => e.Languages)
                .HasMaxLength(50)
                .HasColumnName("languages");
            entity.Property(e => e.Nationality)
                .HasMaxLength(50)
                .HasColumnName("nationality");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
