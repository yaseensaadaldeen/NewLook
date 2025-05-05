using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NEWLOOK.Models.NewLook
{
    public partial class NewLookContext : DbContext
    {
        public NewLookContext()
        {
        }

        public NewLookContext(DbContextOptions<NewLookContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BookDet> BookDets { get; set; } = null!;
        public virtual DbSet<BookHdr> BookHdrs { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<MstService> MstServices { get; set; } = null!;
        public virtual DbSet<MstServiceImage> MstServiceImages { get; set; } = null!;
        public virtual DbSet<MstUser> MstUsers { get; set; } = null!;
        public virtual DbSet<ServiceType> ServiceTypes { get; set; } = null!;
        public virtual DbSet<Team> Teams { get; set; } = null!;
        public virtual DbSet<TeamType> TeamTypes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-H3FPO2I\\SQLEXPRESS;Database=NewLook;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookDet>(entity =>
            {
                entity.ToTable("book_det");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.InvNo)
                    .HasMaxLength(50)
                    .HasColumnName("inv_no");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.ServTypeId).HasColumnName("serv_type_id");

                entity.HasOne(d => d.InvNoNavigation)
                    .WithMany(p => p.BookDets)
                    .HasForeignKey(d => d.InvNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__book_det__inv_no__5070F446");

                entity.HasOne(d => d.ServType)
                    .WithMany(p => p.BookDets)
                    .HasForeignKey(d => d.ServTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__book_det__serv_t__5165187F");
            });

            modelBuilder.Entity<BookHdr>(entity =>
            {
                entity.HasKey(e => e.InvNo)
                    .HasName("PK__book_hdr__A874B48A2AA38CBC");

                entity.ToTable("book_hdr");

                entity.Property(e => e.InvNo)
                    .HasMaxLength(50)
                    .HasColumnName("inv_no");

                entity.Property(e => e.BookDate)
                    .HasColumnType("datetime")
                    .HasColumnName("book_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedDt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_dt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustmId).HasColumnName("custm_id");

                entity.Property(e => e.Discount).HasColumnName("discount");

                entity.Property(e => e.TotalPrice).HasColumnName("total_price");

                entity.HasOne(d => d.Custm)
                    .WithMany(p => p.BookHdrs)
                    .HasForeignKey(d => d.CustmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__book_hdr__custm___4CA06362");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
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

            modelBuilder.Entity<MstService>(entity =>
            {
                entity.ToTable("mst_Service");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.SerDesc)
                    .HasMaxLength(500)
                    .HasColumnName("ser_desc");

                entity.Property(e => e.SerName)
                    .HasMaxLength(50)
                    .HasColumnName("ser_name");

                entity.Property(e => e.TeamId).HasColumnName("team_id");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.MstServices)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__mst_Servi__team___3E52440B");
            });

            modelBuilder.Entity<MstServiceImage>(entity =>
            {
                entity.ToTable("mst_Service_images");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active)
                    .HasMaxLength(2)
                    .HasColumnName("active")
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.ImageLink)
                    .HasMaxLength(50)
                    .HasColumnName("image_link");

                entity.Property(e => e.SerId).HasColumnName("ser_id");

                entity.HasOne(d => d.Ser)
                    .WithMany(p => p.MstServiceImages)
                    .HasForeignKey(d => d.SerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__mst_Servi__ser_i__412EB0B6");
            });

            modelBuilder.Entity<MstUser>(entity =>
            {
                entity.ToTable("mst_User");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active)
                    .HasMaxLength(2)
                    .HasColumnName("active")
                    .HasDefaultValueSql("('Y')");

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
                entity.ToTable("Service_types");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active)
                    .HasMaxLength(2)
                    .HasColumnName("active")
                    .HasDefaultValueSql("('Y')");

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

                entity.HasOne(d => d.MstSer)
                    .WithMany(p => p.ServiceTypes)
                    .HasForeignKey(d => d.MstSerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Service_t__mst_s__45F365D3");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("Team");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("active")
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.Birth)
                    .HasColumnType("date")
                    .HasColumnName("birth");

                entity.Property(e => e.CountryWork)
                    .HasMaxLength(50)
                    .HasColumnName("country_work");

                entity.Property(e => e.EmpName)
                    .HasMaxLength(50)
                    .HasColumnName("emp_name");

                entity.Property(e => e.EmpPhoneNo)
                    .HasMaxLength(50)
                    .HasColumnName("emp_phone_no");

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

                entity.Property(e => e.TmTypeId).HasColumnName("tm_type_id");

                entity.HasOne(d => d.TmType)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.TmTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Team__tm_type_id__3A81B327");
            });

            modelBuilder.Entity<TeamType>(entity =>
            {
                entity.ToTable("Team_Types");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TmTypeName)
                    .HasMaxLength(50)
                    .HasColumnName("tm_type_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
