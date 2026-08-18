using AutoMapper;
using edumis.Models.Library.Newspaper;
using edumis.Models.Library.Newspaper.DTO;

namespace edumisbackend.Mappers.Library.NewsPaper;

public class NewspaperDetailsMapper  : Profile
{
    public NewspaperDetailsMapper()
    {
        CreateMap<NewspaperRequestDTO, NewspaperModel>();
    }
}
