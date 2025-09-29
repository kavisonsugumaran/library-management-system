namespace Library.Application.DTOs
{
    public record AuthorCreateDto(string FirstName, string LastName, DateTime? DateOfBirth);
    public record AuthorUpdateDto(string FirstName, string LastName, DateTime? DateOfBirth);
    public record AuthorReadDto(int Id, string FirstName, string LastName, DateTime? DateOfBirth);
}
