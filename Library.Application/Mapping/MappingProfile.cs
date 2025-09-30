using AutoMapper;
using Library.Application.DTOs;
using Library.Domain.Entities;

namespace Library.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Author, AuthorReadDto>();
            CreateMap<AuthorCreateDto, Author>();
            CreateMap<AuthorUpdateDto, Author>();

            CreateMap<Book, BookReadDto>()
                .ForMember(d => d.AuthorName, m => m.MapFrom(s => s.Author!.FirstName + " " + s.Author!.LastName));
            CreateMap<BookCreateDto, Book>()
                .ForMember(d => d.AvailableCopies, m => m.MapFrom(s => s.TotalCopies));
            CreateMap<BookUpdateDto, Book>();

            CreateMap<Member, MemberReadDto>();
            CreateMap<MemberCreateDto, Member>();
            CreateMap<MemberUpdateDto, Member>();

            CreateMap<Loan, LoanReadDto>()
                .ForMember(d => d.BookTitle, m => m.MapFrom(s => s.Book!.Title))
                .ForMember(d => d.MemberName, m => m.MapFrom(s => s.Member!.FullName));
        }
    }
}
