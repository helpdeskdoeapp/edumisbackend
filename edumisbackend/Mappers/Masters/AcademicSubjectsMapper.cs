using AutoMapper;
using edumis.Models.Masters.DTO;
using edumis.Models.Masters;

namespace edumisbackend.Mappers.Masters;

public class AcademicSubjectsMapper : Profile
{
    public AcademicSubjectsMapper()
    {
        CreateMap<AcademicSubjectsRequestDTO, AcademicSubjectsModel>();
        CreateMap<AcademicSubjectsModel, AcademicSubjectsResponseDTO>()
            .ForMember(dest => dest.RecordId, opt => opt.MapFrom(x => x.RowId));
    }
}
