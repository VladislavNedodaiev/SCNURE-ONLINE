using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SCNURE_BACKEND.Data.Entities;

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

        public virtual DbSet<Achievement> Achievements { get; set; }
        public virtual DbSet<Canvase> Canvases { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Participant> Participants { get; set; }
        public virtual DbSet<Reward> Rewards { get; set; }
        public virtual DbSet<Entities.Startup> Startups { get; set; }
        public virtual DbSet<TeamMember> TeamMembers { get; set; }
        public virtual DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Achievement>(entity =>
            {
                entity.HasKey(e => e.AchievementId)
                    .HasName("PRIMARY");

                entity.Property(e => e.AchievementId)
                    .HasColumnName("achievement_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Photo)
                    .HasColumnName("photo")
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasColumnName("text")
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasColumnType("varchar(64)");
            });

            modelBuilder.Entity<Canvase>(entity =>
            {
                entity.HasKey(e => e.StartupId)
                    .HasName("PRIMARY");

                entity.Property(e => e.StartupId)
                    .HasColumnName("startup_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AnfairAdvantage)
                    .HasColumnName("anfair_advantage")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.Channels)
                    .HasColumnName("channels")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.CostStructure)
                    .HasColumnName("cost_structure")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.CustomerSegments)
                    .HasColumnName("customer_segments")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.Kpi)
                    .HasColumnName("kpi")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.Problem)
                    .HasColumnName("problem")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.RevenueStreams)
                    .HasColumnName("revenue_streams")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.Solution)
                    .HasColumnName("solution")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Uvp)
                    .HasColumnName("uvp")
                    .HasColumnType("varchar(500)");

                entity.HasOne(d => d.Startup)
                    .WithOne(p => p.Canvases)
                    .HasForeignKey<Canvase>(d => d.StartupId)
                    .HasConstraintName("Canvases_ibfk_1");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.CommentId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.StartupId)
                    .HasName("startup_id");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id");

                entity.Property(e => e.CommentId)
                    .HasColumnName("comment_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PostDate)
                    .HasColumnName("post_date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.StartupId)
                    .HasColumnName("startup_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasColumnName("text")
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Startup)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.StartupId)
                    .HasConstraintName("Comments_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Comments_ibfk_2");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.EventId)
                    .HasName("PRIMARY");

                entity.Property(e => e.EventId)
                    .HasColumnName("event_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.FinishDate)
                    .HasColumnName("finish_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Hash)
                    .HasColumnName("hash")
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.Photo)
                    .IsRequired()
                    .HasColumnName("photo")
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasKey(e => new { e.StartupId, e.UserId })
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id");

                entity.Property(e => e.StartupId)
                    .HasColumnName("startup_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasColumnType("enum('-1','0','1')")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Startup)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.StartupId)
                    .HasConstraintName("Likes_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Likes_ibfk_2");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotificationId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id");

                entity.Property(e => e.NotificationId)
                    .HasColumnName("notification_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PostDate)
                    .HasColumnName("post_date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasColumnName("text")
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ViewDate)
                    .HasColumnName("view_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Notifications_ibfk_1");
            });

            modelBuilder.Entity<Participant>(entity =>
            {
                entity.HasKey(e => e.ParticipantId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.EventId)
                    .HasName("event_id");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id");

                entity.Property(e => e.ParticipantId)
                    .HasColumnName("participant_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EventId)
                    .HasColumnName("event_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Visits)
                    .HasColumnName("visits")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("Participants_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Participants_ibfk_2");
            });

            modelBuilder.Entity<Reward>(entity =>
            {
                entity.HasKey(e => e.RewardId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.AchievementId)
                    .HasName("achievement_id");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id");

                entity.Property(e => e.RewardId)
                    .HasColumnName("reward_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AchievementId)
                    .HasColumnName("achievement_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PostDate)
                    .HasColumnName("post_date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Achievement)
                    .WithMany(p => p.Rewards)
                    .HasForeignKey(d => d.AchievementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Rewards_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Rewards)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Rewards_ibfk_2");
            });

            modelBuilder.Entity<Entities.Startup>(entity =>
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
                    .HasColumnType("varchar(256)");

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

            modelBuilder.Entity<TeamMember>(entity =>
            {
                entity.HasKey(e => new { e.StartupId, e.UserId })
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id");

                entity.Property(e => e.StartupId)
                    .HasColumnName("startup_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EditAccess)
                    .HasColumnName("edit_access")
                    .HasColumnType("bit")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasColumnName("role")
                    .HasColumnType("varchar(32)");

                entity.HasOne(d => d.Startup)
                    .WithMany(p => p.TeamMembers)
                    .HasForeignKey(d => d.StartupId)
                    .HasConstraintName("TeamMembers_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TeamMembers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("TeamMembers_ibfk_2");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Admin)
                    .HasColumnName("admin")
                    //.HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Ban)
                    .HasColumnName("ban")
                    //.HasColumnType("tinyint(1)")
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
					.HasColumnType("tinyint(1)");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnName("password_hash")
                    .HasColumnType("binary(64)");

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasColumnName("password_salt")
                    .HasColumnType("binary(64)");

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
                    //.HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ShowEmail)
                    .HasColumnName("show_email")
                    //.HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ShowPhone)
                    .HasColumnName("show_phone")
                    //.HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Verification)
                    .HasColumnName("verification")
                    .HasColumnType("varchar(256)");

				entity.Property(e => e.Gender)
					.IsRequired(false)
					.HasColumnName("gender")
					.HasColumnType("enum");
            });
        }
    }
}
