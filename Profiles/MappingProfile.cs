namespace library_management.Profiles;
using AutoMapper;
using library_management.DTOs;
using library_management.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<Member, MemberDto>();
        CreateMap<Borrowing, BorrowingDto>()
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name));
    }
}
