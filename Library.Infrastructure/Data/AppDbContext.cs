using Library.Domain.Entities;
using Library.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Member> Members => Set<Member>();
        public DbSet<Loan> Loans => Set<Loan>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(e =>
            {
                e.Property(p => p.FirstName).HasMaxLength(100).IsRequired();
                e.Property(p => p.LastName).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<Book>(e =>
            {
                e.Property(p => p.Title).HasMaxLength(200).IsRequired();
                e.Property(p => p.Isbn).HasMaxLength(13).IsRequired();
                e.HasIndex(p => p.Isbn).IsUnique();
                e.HasOne(p => p.Author)
                 .WithMany(a => a.Books)
                 .HasForeignKey(p => p.AuthorId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Member>(e =>
            {
                e.Property(p => p.FullName).HasMaxLength(150).IsRequired();
                e.Property(p => p.Email).HasMaxLength(200).IsRequired();
                e.HasIndex(p => p.Email).IsUnique();
            });

            modelBuilder.Entity<Loan>(e =>
            {
                e.Property(p => p.Status).HasConversion<int>();
                e.HasOne(l => l.Book).WithMany(b => b.Loans).HasForeignKey(l => l.BookId);
                e.HasOne(l => l.Member).WithMany(m => m.Loans).HasForeignKey(l => l.MemberId);
            });

            // seed basic data (optional)
            var a1 = new Author { Id = 1, FirstName = "George", LastName = "Orwell", DateOfBirth = new DateTime(1903, 6, 25) };
            var a2 = new Author { Id = 2, FirstName = "Jane", LastName = "Austen", DateOfBirth = new DateTime(1775, 12, 16) };
            modelBuilder.Entity<Author>().HasData(a1, a2);

            var b1 = new Book { Id = 1, Title = "1984", Isbn = "9780451524935", AuthorId = 1, TotalCopies = 5, AvailableCopies = 5, PublishedOn = new DateTime(1949, 6, 8) };
            var b2 = new Book { Id = 2, Title = "Pride and Prejudice", Isbn = "9780141439518", AuthorId = 2, TotalCopies = 3, AvailableCopies = 3, PublishedOn = new DateTime(1813, 1, 28) };
            modelBuilder.Entity<Book>().HasData(b1, b2);

            //var m1 = new Member { Id = 1, FullName = "Alice Johnson", Email = "alice@example.com", JoinedOn = DateTime.UtcNow };
            //modelBuilder.Entity<Member>().HasData(m1);

            //modelBuilder.Entity<Loan>().HasData(new Loan
            //{
            //    Id = 1,
            //    BookId = 1,
            //    MemberId = 1,
            //    LoanDate = DateTime.UtcNow.AddDays(-2),
            //    DueDate = DateTime.UtcNow.AddDays(12),
            //    Status = LoanStatus.Active
            //});
        }
    }
}
