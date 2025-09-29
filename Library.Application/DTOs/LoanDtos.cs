using Library.Domain.Enums;

namespace Library.Application.DTOs
{
    public record LoanCreateDto(int BookId, int MemberId, DateTime? DueDate);
    public record LoanReturnDto(DateTime ReturnDate);
    public record LoanReadDto(int Id, int BookId, string BookTitle, int MemberId, string MemberName, DateTime LoanDate, DateTime DueDate, DateTime? ReturnDate, LoanStatus Status);
}
