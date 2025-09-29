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
            //CreateMap<Book, BookReadDto>()
            //    .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
            //    .ForCtorParam("Title", opt => opt.MapFrom(src => src.Title))
            //    .ForCtorParam("Isbn", opt => opt.MapFrom(src => src.Isbn))
            //    .ForCtorParam("PublishedOn", opt => opt.MapFrom(src => src.PublishedOn))
            //    .ForCtorParam("AuthorId", opt => opt.MapFrom(src => src.AuthorId))
            //    .ForCtorParam("AuthorName", opt => opt.MapFrom(src => src.Author!.FirstName + " " + src.Author!.LastName))
            //    .ForCtorParam("TotalCopies", opt => opt.MapFrom(src => src.TotalCopies))
            //    .ForCtorParam("AvailableCopies", opt => opt.MapFrom(src => src.AvailableCopies));
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
