using AutoMapper;
using edumis.Models.Library.Books;
using edumis.Models.Library.Books.DTO;

namespace edumisbackend.Mappers.Library.Books;

public class BookDetailsMapper : Profile
{
    public BookDetailsMapper()
    {
        //CreateMap<BookCatalogueModel, BookCatalogueDetailsDTO>()
        //    .ForMember(dest => dest.ConditionDesc, opt => opt.MapFrom(src => src.ConditionNavigation.ConditionDesc))
        //    .ForMember(dest => dest.StatusDesc, opt => opt.MapFrom(src => src.StatusNavigation.StatusDesc));
        CreateMap<BookRequestDTO, BookDetailsModel>();
    }
}
