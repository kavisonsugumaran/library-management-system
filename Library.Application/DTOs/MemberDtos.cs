namespace Library.Application.DTOs
{
    public class MemberCreateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class MemberUpdateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class MemberReadDto
    {
        public int Id { get; init; }
        public string FullName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public DateTime JoinedOn { get; init; }
    }
}
