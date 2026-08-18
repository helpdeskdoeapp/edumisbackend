using AutoMapper;
using edumis.Models.MISC;
using edumis.Models.MISC.DTO;

namespace edumisbackend.Mappers.MISC;

public class SwachhBharatImagesMapper : Profile
{
    public SwachhBharatImagesMapper()
    {
        CreateMap<SwachhBharatImagesModel, SwachhBharatImagesResponseDTO>();
    }
}
