using Microsoft.EntityFrameworkCore;
using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Data
{
    /// <summary>
    /// Database context for Smart Library
    /// Configures Entity Framework Core with MySQL
    /// </summary>
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Faculty> Faculty { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Fine> Fines { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure inheritance for User hierarchy (TPH - Table Per Hierarchy)
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<Student>("Student")
                .HasValue<Faculty>("Faculty");

            // Configure Book entity
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ISBN).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Author).HasMaxLength(100);
                entity.HasIndex(e => e.ISBN).IsUnique();
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure Student entity
            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.StudentId).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.StudentId).IsUnique();
            });

            // Configure Faculty entity
            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.Property(e => e.EmployeeId).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.EmployeeId).IsUnique();
            });

            // Configure Loan entity
            modelBuilder.Entity<Loan>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Loans)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Book)
                    .WithMany(b => b.Loans)
                    .HasForeignKey(e => e.BookId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Status)
                    .HasConversion<string>();
            });

            // Configure Fine entity
            modelBuilder.Entity<Fine>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasOne(e => e.Loan)
                    .WithOne(l => l.Fine)
                    .HasForeignKey<Fine>(e => e.LoanId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.Status)
                    .HasConversion<string>();
            });

            // Configure Reservation entity
            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Reservations)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Book)
                    .WithMany(b => b.Reservations)
                    .HasForeignKey(e => e.BookId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Status)
                    .HasConversion<string>();
            });

            // Configure Catalog entity
            modelBuilder.Entity<Catalog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Code).IsUnique();
            });
        }
    }
}
