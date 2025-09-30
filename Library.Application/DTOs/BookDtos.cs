namespace Library.Application.DTOs
{
    public class BookCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Isbn { get; set; } = string.Empty;
        public DateTime? PublishedOn { get; set; }
        public int AuthorId { get; set; }
        public int TotalCopies { get; set; }
    }

    public class BookUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Isbn { get; set; } = string.Empty;
        public DateTime? PublishedOn { get; set; }
        public int AuthorId { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
    }

    public class BookReadDto
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Isbn { get; init; } = string.Empty;
        public DateTime? PublishedOn { get; init; }
        public int AuthorId { get; init; }
        public string AuthorName { get; init; } = string.Empty;
        public int TotalCopies { get; init; }
        public int AvailableCopies { get; init; }
    }
}
