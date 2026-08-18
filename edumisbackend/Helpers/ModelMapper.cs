using AutoMapper;
using edumis.Models.Circulars;
using edumis.Models.Circulars.DTO;
using edumis.Models.Events;
using edumis.Models.Events.DTO;
using edumis.Models.Global;
using edumis.Models.Global.DTO;
using edumis.Models.Masters;
using edumis.Models.Masters.DTO;
using edumis.Models.News;
using edumis.Models.News.DTO;
using edumis.Models.Tenders;
using edumis.Models.Tenders.DTO;

namespace edumisbackend.Helpers;

public class ModelMapper : Profile
{
    public ModelMapper()
    {
        CreateMap<PostsModel, PostsDTO>().ReverseMap();

        #region Circulars & Tenders mapping
        CreateMap<TendersModel, TendersDetailsResponseDTO>()
            .ForMember(dest => dest.RecordId, opt => opt.MapFrom(x => x.RowId));

        CreateMap<CircularModel, CircularsDetailResponseDTO>()
             .ForMember(dest => dest.RecordId, opt => opt.MapFrom(x => x.RowId));
        #endregion

        CreateMap<NewsModel, NewsDetailResponseDTO>()
            .ForMember(dest => dest.RecordId, opt => opt.MapFrom(x => x.RowId));

        CreateMap<EventsModel, EventResponseDTO>()
           .ForMember(dest => dest.RecordId, opt => opt.MapFrom(x => x.RowId));
    }
}
