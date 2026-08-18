using AutoMapper;
using edumis.Models.Masters;
using edumis.Models.Masters.DTO;

namespace edumisbackend.Mappers.Masters;

public class AcademicClassesMapper : Profile
{
    public AcademicClassesMapper()
    {
        CreateMap<AcademicClassesRequestDTO, AcademicClassesModel>();
        CreateMap<AcademicClassesModel, AcademicClassesResponseDTO>()
            .ForMember(dest => dest.RecordId, opt => opt.MapFrom(x => x.RowId));
    }
}
