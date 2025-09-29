namespace Library.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Isbn { get; set; } = default!;
        public DateTime? PublishedOn { get; set; }
        public int AuthorId { get; set; }
        public Author? Author { get; set; }

        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }

        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}
