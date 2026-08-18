using edumis.Models.Events;
using edumis.Models.Events.DTO;

namespace edumis.DataAccess.IRepositories.IEvents;

public interface IEventsRepo : IRepository<EventsModel>
{
    Task<IEnumerable<EventResponseDTO>?> GetAllEvents(SearchEventsRequestDTO requestDTO);
}
