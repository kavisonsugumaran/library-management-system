namespace Library.Application.DTOs
{
    public record BookCreateDto(string Title, string Isbn, DateTime? PublishedOn, int AuthorId, int TotalCopies);
    public record BookUpdateDto(string Title, string Isbn, DateTime? PublishedOn, int AuthorId, int TotalCopies, int AvailableCopies);

    public class BookReadDto
    {
        public int Id { get; init; }
        public string Title { get; init; } = default!;
        public string Isbn { get; init; } = default!;
        public DateTime? PublishedOn { get; init; }
        public int AuthorId { get; init; }
        public string AuthorName { get; init; } = default!;
        public int TotalCopies { get; init; }
        public int AvailableCopies { get; init; }
    }
    //public record BookReadDto(int Id, string Title, string Isbn, DateTime? PublishedOn, int AuthorId, string AuthorName, int TotalCopies, int AvailableCopies);

}
