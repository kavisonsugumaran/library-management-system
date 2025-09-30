using Library.Domain.Enums;

namespace Library.Application.DTOs
{
    public class LoanCreateDto
    {
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public class LoanReturnDto
    {
        public DateTime? ReturnDate { get; set; }
    }

    public class LoanReadDto
    {
        public int Id { get; init; }
        public int BookId { get; init; }
        public string BookTitle { get; init; } = string.Empty;
        public int MemberId { get; init; }
        public string MemberName { get; init; } = string.Empty;
        public DateTime LoanDate { get; init; }
        public DateTime? DueDate { get; init; }
        public DateTime? ReturnDate { get; init; }
        public int Status { get; init; }
    }
}
