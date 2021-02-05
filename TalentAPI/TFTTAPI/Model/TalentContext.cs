using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TFTTAPI.Model
{
    public partial class TalentContext : DbContext
    {
        public TalentContext()
        {
        }

        public TalentContext(DbContextOptions<TalentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Talent> Talents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseMySql("mysqlconnectionstring", x => x.ServerVersion("8.0.20-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Talent>(entity =>
            {
                entity.ToTable("talent");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Bio)
                    .HasColumnType("varchar(4096)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Name)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Profile)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Reknown)
                    .HasColumnType("varchar(4096)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ShortName)
                    .HasColumnType("varchar(2048)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
