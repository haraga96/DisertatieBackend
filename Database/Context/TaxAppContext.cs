using Microsoft.EntityFrameworkCore;
using Backend_Dis_App.Models;

namespace Backend_Dis_App.Database
{
    public partial class TaxAppContext : DbContext
    {
        public TaxAppContext()
        {
        }

        public TaxAppContext(DbContextOptions<TaxAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Documents> Documents { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=Haraghios2;SSL Mode=Disable;Trust Server Certificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("adminpack");

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country", "taxapp");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Documents>(entity =>
            {
                entity.ToTable("Documents", "taxapp");

                entity.HasIndex(e => e.CountryId)
                    .HasName("fki_CountryId");

                entity.HasIndex(e => e.UserId)
                    .HasName("fki_UserId");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.CountryId).HasColumnName("Country_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CountryId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserId");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "taxapp");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasColumnType("character varying");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
