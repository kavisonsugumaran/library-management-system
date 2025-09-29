namespace Library.Application.DTOs
{
    public record MemberCreateDto(string FullName, string Email);
    public record MemberUpdateDto(string FullName, string Email);
    public record MemberReadDto(int Id, string FullName, string Email, DateTime JoinedOn);
}
