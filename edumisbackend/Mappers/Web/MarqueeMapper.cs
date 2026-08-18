using AutoMapper;
using edumis.Models.Web;
using edumis.Models.Web.DTO;

namespace edumisbackend.Mappers.Web;

public class MarqueeMapper : Profile
{
    public MarqueeMapper()
    {
        CreateMap<MarqueeRequestDetailsDTO, MarqueeDetailsModels>();
        CreateMap<MarqueeDetailsModels, MarqueeDetailsResponseDTO>()
            .ForMember(dest => dest.RecordId, opt => opt.MapFrom(x => x.RowId));
    }
}
