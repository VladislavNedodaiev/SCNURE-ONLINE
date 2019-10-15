using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SCNURE_BACKEND.Data
{
    public partial class SCContext : DbContext
    {
        public SCContext()
        {
        }

        public SCContext(DbContextOptions<SCContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Startups> Startups { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseMySql("");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Startups>(entity =>
            {
                entity.HasKey(e => e.StartupId)
                    .HasName("PRIMARY");

                entity.Property(e => e.StartupId)
                    .HasColumnName("startup_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("text");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.FoundationYear)
                    .HasColumnName("foundation_year")
                    .HasColumnType("smallint(6)");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasColumnType("varchar(13)");

                entity.Property(e => e.Photo)
                    .HasColumnName("photo")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PublicationDate)
                    .HasColumnName("publication_date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Website)
                    .HasColumnName("website")
                    .HasColumnType("varchar(256)");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Admin)
                    .HasColumnName("admin")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Ban)
                    .HasColumnName("ban")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasColumnType("date");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("text");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Membership)
                    .HasColumnName("membership")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasColumnType("varchar(13)");

                entity.Property(e => e.Photo)
                    .HasColumnName("photo")
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.RegisterDate)
                    .HasColumnName("register_date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.SecondName)
                    .HasColumnName("second_name")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.ShowBirthday)
                    .HasColumnName("show_birthday")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ShowEmail)
                    .HasColumnName("show_email")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ShowPhone)
                    .HasColumnName("show_phone")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Verification)
                    .HasColumnName("verification")
                    .HasColumnType("varchar(256)");
            });
        }
    }
}
