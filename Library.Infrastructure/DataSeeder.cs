using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Library.Infrastructure
{
    public class DataSeeder
    {
        public static async Task EnsureMigratedAsync(IServiceProvider sp, ILogger logger)
        {
            using var scope = sp.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            if ((await ctx.Database.GetPendingMigrationsAsync()).Any())
            {
                logger.LogInformation("Applying migrations...");
                await ctx.Database.MigrateAsync();
                logger.LogInformation("Migrations applied.");
            }
        }

        public static async Task EnsureSeedDataAsync(IServiceProvider sp, ILogger logger)
        {
            using var scope = sp.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Authors/Books are already seeded via HasData, but in case DB is empty:
            if (!await ctx.Authors.AnyAsync())
            {
                var a1 = new Author { FirstName = "George", LastName = "Orwell", DateOfBirth = new DateTime(1903, 6, 25) };
                var a2 = new Author { FirstName = "Jane", LastName = "Austen", DateOfBirth = new DateTime(1775, 12, 16) };
                await ctx.Authors.AddRangeAsync(a1, a2);
                await ctx.SaveChangesAsync();

                var b1 = new Book { Title = "1984", Isbn = "9780451524935", AuthorId = a1.Id, TotalCopies = 5, AvailableCopies = 5, PublishedOn = new DateTime(1949, 6, 8) };
                var b2 = new Book { Title = "Pride and Prejudice", Isbn = "9780141439518", AuthorId = a2.Id, TotalCopies = 3, AvailableCopies = 3, PublishedOn = new DateTime(1813, 1, 28) };
                await ctx.Books.AddRangeAsync(b1, b2);
                await ctx.SaveChangesAsync();
            }

            var member = await ctx.Members.OrderBy(m => m.Id).FirstOrDefaultAsync();
            if (member is null)
            {
                member = new Member
                {
                    FullName = "Alice Johnson",
                    Email = "alice@example.com",
                    JoinedOn = DateTime.UtcNow
                };
                await ctx.Members.AddAsync(member);
                await ctx.SaveChangesAsync(); // <-- get member.Id
            }

            // Pick an existing Book (first by Id)
            var book = await ctx.Books.OrderBy(b => b.Id).FirstOrDefaultAsync();
            if (book is null)
            {
                // Fallback: create one if none exists
                var author = await ctx.Authors.OrderBy(a => a.Id).FirstAsync();
                book = new Book
                {
                    Title = "Sample Book",
                    Isbn = "0000000000001",
                    AuthorId = author.Id,
                    TotalCopies = 2,
                    AvailableCopies = 2,
                    PublishedOn = new DateTime(2000, 1, 1)
                };
                await ctx.Books.AddAsync(book);
                await ctx.SaveChangesAsync();
            }

            // Only add a sample Loan if there are none
            if (!await ctx.Loans.AnyAsync())
            {
                await ctx.Loans.AddAsync(new Loan
                {
                    // no explicit Id; let identity generate it
                    BookId = book.Id,
                    MemberId = member.Id,        // <-- now guaranteed to exist
                    LoanDate = DateTime.UtcNow.AddDays(-2),
                    DueDate = DateTime.UtcNow.AddDays(12),
                    Status = LoanStatus.Active
                });
                await ctx.SaveChangesAsync();
            }

            await ctx.SaveChangesAsync();
        }
    }
}
