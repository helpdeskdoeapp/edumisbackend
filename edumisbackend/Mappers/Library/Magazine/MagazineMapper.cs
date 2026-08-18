using AutoMapper;
using edumis.Models.Library.Magazine;
using edumis.Models.Library.Magazine.DTO;

namespace edumisbackend.Mappers.Library.Magazine;

public class MagazineMapper : Profile
{
    public MagazineMapper()
    {
        CreateMap<MagazineRequestDTO, MagazineModel>();
        CreateMap<MagazineProcurementTransactionRequestDTO, MagazineProcurementTransactionModel>();
        CreateMap<MagazineProcurementUpdateRequestDTO, MagazineProcurementTransactionModel>();

       
    }
}
