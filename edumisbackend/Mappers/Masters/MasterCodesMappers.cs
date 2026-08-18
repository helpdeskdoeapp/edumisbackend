using AutoMapper;
using edumis.Models.Global;
using edumis.Models.Global.DTO;

namespace edumisbackend.Mappers.Masters;

public class MasterCodesMappers : Profile
{
    public MasterCodesMappers()
    {
        CreateMap<CodesModel, MasterCodeDetailsResponseDTO>()
              .ForMember(dest => dest.SubCodes,
                opt => opt.MapFrom(src => src.CodeValuesList)); ;

        CreateMap<CodeValuesModel, MasterCodeValueDetailsDTO>()
            .ForMember(dest => dest.SubCode, src => src.MapFrom(x => x.CodeValue))
            .ForMember(dest => dest.SubCodeDescription, src => src.MapFrom(x => x.CodeValDescription));           
    }
}
