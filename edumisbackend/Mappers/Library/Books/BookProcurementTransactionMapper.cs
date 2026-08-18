using AutoMapper;
using edumis.Models.Library.Books;
using edumis.Models.Library.Books.DTO;

namespace edumisbackend.Mappers.Library.Books;

public class BookProcurementTransactionMapper : Profile
{
    public BookProcurementTransactionMapper()
    {
        CreateMap<BookProcurementTransactionRequestDTO, ProcurementTransactionModel>();
        CreateMap<BookProcurementUpdateRequestDTO, ProcurementTransactionModel>()
            .ForMember(dest => dest.TransactionId, opt => opt.Ignore())
            .ForMember(dest => dest.BookDetailsNavigation, opt => opt.Ignore());
        //CreateMap<BookProcurementTransactionModel, BookProcurementTransactionDetailsDTO>()
        //    .ForMember(dest => dest.StatusDesc, opt => opt.MapFrom(src => src.StatusNavigation.StatusDesc));
    }
}
